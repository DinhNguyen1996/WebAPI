using APIWebManagement.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIWebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ImportsController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("ReadFileExcel")]
        public async Task<IActionResult> ReadFileExcel([FromForm] IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                throw new Exception("formfile is empty");
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Not Support file extension");
            }

            try
            {
                var lstPeople = new List<People>();
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream, cancellationToken);

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                            throw new Exception("File excel empty");

                        // get number of rows and columns in the sheet
                        int rows = worksheet.Dimension.Rows;
                        int columns = worksheet.Dimension.Columns;

                        // loop through the worksheet rows and columns
                        for (int i = 2; i <= rows; i++)
                        {
                            lstPeople.Add(new People
                            {
                                STT = worksheet.Cells[rows, 1].Value.ToString().Trim(),
                                Name = worksheet.Cells[rows, 2].Value.ToString().Trim(),
                                Active = worksheet.Cells[rows, 3].Value.ToString().Trim(),
                            });
                        }
                    }
                }

                return Ok(new { People = lstPeople });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile formFile, CancellationToken cancellationToken)
        {
            try
            {
                if (formFile == null || formFile.Length <= 0)
                {
                    throw new Exception("formfile is empty");
                }
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Not Support file extension");
                }

                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream, cancellationToken);

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var excelPackage = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                            throw new Exception("File excel empty");

                        string rootPath = _hostingEnvironment.ContentRootPath + "\\Excel";
                        //create rootPath and check
                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }

                        //the path of the file
                        string fileName = formFile.FileName;
                        FileInfo file = new FileInfo(Path.Combine(rootPath, fileName));

                        //Write the file to the disk
                        FileInfo fi = new FileInfo(file.ToString());
                        excelPackage.SaveAs(fi);

                        //delete file
                        //IFileProvider physicalFileProvider = new PhysicalFileProvider(rootPath);
                        //DeleteFiles(physicalFileProvider);
                    }
                }

                return Ok(new MessageResponse("Import excel sucessfully"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Export")]
        public async Task<IActionResult> Export(CancellationToken cancellationToken)
        {
            try
            {
                string folder = "E:\\";
                string excelName = $"People-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                string downloadUrl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, excelName);
                FileInfo file = new FileInfo(Path.Combine(folder, excelName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(folder, excelName));
                }

                await Task.Yield();

                var list = new List<People>()
                {
                    new People { STT = "1",Name = "catcher", Active = "Co" },
                    new People { STT = "2",Name = "huhu", Active = "Co" },
                    new People { STT = "3",Name = "hihi", Active = "Khong" },
                };

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(file))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(list, true);
                    BindingFormatForExcel(workSheet, list);
                    package.Save();
                }
                return Ok(new MessageResponse("Export file successfull"));

            }
            catch (Exception)
            {
                throw;
            }


        }

        private void BindingFormatForExcel(ExcelWorksheet worksheet, List<People> listItems)
        {
            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 10;
            // Tự động xuống hàng khi text quá dài
            worksheet.Cells.Style.WrapText = true;
            // Tạo header
            worksheet.Cells[1, 1].Value = "STT";
            worksheet.Cells[1, 2].Value = "Họ và tên";
            worksheet.Cells[1, 3].Value = "Hoạt động";
        }

        private void DeleteFiles(IFileProvider physicalFileProvider)
        {
            if (physicalFileProvider is PhysicalFileProvider)
            {
                var directory = physicalFileProvider.GetDirectoryContents(string.Empty);
                foreach (var file in directory)
                {
                    if (!file.IsDirectory)
                    {
                        var fileInfo = new System.IO.FileInfo(file.PhysicalPath);
                        fileInfo.Delete();

                    }
                }
            }
        }

        public class People
        {
            public string STT { get; set; }
            public string Name { get; set; }
            public string Active { get; set; }
        }
    }
}
