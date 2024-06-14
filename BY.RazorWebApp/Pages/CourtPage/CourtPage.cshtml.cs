using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using BY.Data.DAO;

namespace BY.RazorWebApp.Pages.CourtPage
{
    public class CourtPageModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness = new CourtBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public Court Court { get; set; } = default;
        public List<Court> courts { get; set; } = new List<Court>();
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IHostEnvironment _environment;
        public CourtPageModel(IWebHostEnvironment hostingEnvironment,IHostEnvironment environment)
        {
            _hostingEnvironment = hostingEnvironment;
            _environment = environment;
        }

        [BindProperty] // Make it bindable from the form
        public CourtViewModel CourtViewModel { get; set; } = new CourtViewModel();
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }
        public Court EditedCourt { get; set; }

       
        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            var result = await _courtBusiness.GetById(id);
            if (result.Status > 0 && result.Data != null)
            {
                EditedCourt = (Court)result.Data;
                return new JsonResult(EditedCourt); // Trả về dữ liệu Court dưới dạng JSON
            }
            else
            {
                return NotFound();
            }
        }

        public async Task OnGetAsync()
        {
            var result = await _courtBusiness.GetAllCourt();
            if (result.Status > 0 && result.Data != null)
            {
                courts = (List<Court>)result.Data;
            }
        }

        // Save the court data when the form is submitted
        public async Task<IActionResult> OnPostAsync()
        {
            /*     var check = _environment.ContentRootPath;

                 if (FileUploads != null)
                 {
                     try { 
                     foreach(var FileUpload in FileUploads)
                     {
                         var file = Path.Combine(_environment.ContentRootPath, "Images", FileUpload.FileName);
                         using(var fileStream = new FileStream(file, FileMode.Create))
                         {
                             await FileUpload.CopyToAsync(fileStream);
                         }
                     }
                     }catch(Exception ex)
                     {
                         var k = ex.Message;
                     }
                 }
               */
            if (CourtViewModel.ImageFile != null)
            {
                // Generate a unique filename
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(CourtViewModel.ImageFile.FileName);
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await CourtViewModel.ImageFile.CopyToAsync(fileStream);
                }

                // Save the image filename in the Court model
                CourtViewModel.Court.Image = "~/Images/" + uniqueFileName;
            }


            var result = await _courtBusiness.Save(Court);
            if (result.Status > 0)
            {
                Message = result.Message;
            }
            else
            {
                Message = "Error: " + result.Message;
            }

            return RedirectToPage();
        }

        // Handle the delete action
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _courtBusiness.Delete(id);
            if (result.Status > 0)
            {
                Message = result.Message;
            }
            else
            {
                Message = "Error: " + result.Message;
            }

            return RedirectToPage();
        }

        // Handle the edit action


        // Update court data
        public async Task<IActionResult> OnPostEditAsync()
        {
            var result = await _courtBusiness.Update(Court);
            if (result != null)
            {
                Message = result.Message;
            }
            else
            {
                Message = "Error: " + result.Message;
            }

            return RedirectToPage();
        }

        private List<Court> GetCourt()
        {
            var courtResult = _courtBusiness.GetAllCourt();

            if (courtResult.Status > 0 && courtResult.Result.Data != null)
            {
                var courts = (List<Court>)courtResult.Result.Data;
                return courts;
            }
            return new List<Court>();
        }

    }
}
