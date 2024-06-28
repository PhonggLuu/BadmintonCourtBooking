using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using BY.Data.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;


namespace BY.RazorWebApp.Pages.CourtPage
{
    public class CourtPageModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness = new CourtBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public Court Court { get; set; } = default;
        public List<Court> Courts { get; set; } = new List<Court>();

      
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public CourtPageModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
        }
       

        [BindProperty] // Make it bindable from the form
        public CourtViewModel CourtViewModel { get; set; } = new CourtViewModel();
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        public Court EditedCourt { get; set; }


        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10; // Number of records per page
        public int TotalPages { get; set; }
        
        //


        [BindProperty]
        public string SearchName { get; set; } = string.Empty;
        [BindProperty]
        public string SearchAddress { get; set; } = string.Empty;
        [BindProperty]
        public string SearchSurfaceType { get; set; } = string.Empty;
        [BindProperty]
        public string SearchType { get; set; } = string.Empty;
        //

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

        public async Task OnGetAsync(int currentPage = 1, string searchName = "", string searchAddress = "", string searchSurfaceType = "", string searchType = "")
        {
            CurrentPage = currentPage;
            SearchName = searchName;
            SearchAddress = searchAddress;
            SearchSurfaceType = searchSurfaceType;
            SearchType = searchType;
           
            var courtResult = await _courtBusiness.GetAllCourt();
            if (courtResult.Status > 0 && courtResult.Data != null)
            {
                var courts = (List<Court>)courtResult.Data;
                if (!string.IsNullOrEmpty(SearchName))
                {
                    courts = courts.Where(c =>
                        (c.Name?.ToLower().StartsWith(SearchName.ToLower()) ?? false) ||
                        (c.Name?.ToLower().Contains(" " + SearchName.ToLower()) ?? false)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(SearchAddress))
                {
                    courts = courts.Where(c =>
                        (c.Address?.ToLower().StartsWith(SearchAddress.ToLower()) ?? false) ||
                        (c.Address?.ToLower().Contains(" " + SearchAddress.ToLower()) ?? false)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(SearchSurfaceType))
                {
                    courts = courts.Where(c =>
                                            (c.SurfaceType?.ToLower().StartsWith(SearchSurfaceType.ToLower()) ?? false) ||
                                            (c.SurfaceType?.ToLower().Contains(" " + SearchSurfaceType.ToLower()) ?? false)
                                        ).ToList();
                }
                if (!string.IsNullOrEmpty(SearchType))
                {
                    courts = courts.Where(c =>
                        (c.Type?.ToLower().StartsWith(SearchType.ToLower()) ?? false) ||
                        (c.Type?.ToLower().Contains(" " + SearchType.ToLower()) ?? false)
                    ).ToList();
                }

                int totalCount = courts.Count;
                TotalPages = (int)System.Math.Ceiling(totalCount / (double)PageSize);

                Courts = courts
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

            }
        }

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

            return RedirectToPage(new { currentPage = CurrentPage, searchName = SearchName, searchAddress = SearchAddress, searchSurfaceType = SearchSurfaceType, searchType = SearchType });
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var check = _environment.ContentRootPath;

            if (FileUpload != null)
            {
                try
                {

                    var file = Path.Combine(_environment.ContentRootPath, "Images", FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                    }

                }
                catch (Exception ex)
                {
                    var k = ex.Message;
                }
            }

  /*          if (CourtViewModel.ImageFile != null)
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
            }*/


            var result = await _courtBusiness.Save(CourtViewModel.Court);
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



    }
}
