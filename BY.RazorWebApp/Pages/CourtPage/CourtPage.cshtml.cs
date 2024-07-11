using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BY.RazorWebApp.Pages.CourtPage
{
    public class CourtPageModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness = new CourtBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public Court Court { get; set; } = default;
        public List<Court> Courts { get; set; } = new List<Court>();


        private readonly IHostEnvironment _environment;
        public CourtPageModel(IHostEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
        }

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
            if (FileUpload != null)
            {
                try
                {
                    var fileName = FileUpload.FileName.Split('.');
                    var newFileName = $"{fileName[0]}{DateTime.Now.ToString("yyyyMMddHHmmss")}.{fileName[fileName.Length - 1]}";
                    var path = Path.Combine(_environment.ContentRootPath, "wwwroot/image", newFileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                    }
                    Court.Image = $"{Request.Scheme}://{Request.Host}/image/{newFileName}";
                }
                catch (Exception ex)
                {
                    var k = ex.Message;
                }
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

        // Update court data
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (FileUpload != null)
            {
                try
                {
                    if (Court.Image != null)
                    {
                        var oldFileName = Court.Image.Split("/");
                        var oldPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "image", oldFileName[oldFileName.Length - 1]);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    var fileName = FileUpload.FileName.Split('.');
                    var newFileName = $"{fileName[0]}{DateTime.Now.ToString("yyyyMMddHHmmss")}.{fileName[fileName.Length - 1]}";
                    var path = Path.Combine(_environment.ContentRootPath, "wwwroot/image", newFileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                    }
                    Court.Image = $"{Request.Scheme}://{Request.Host}/image/{newFileName}";
                }
                catch (Exception ex)
                {
                    var k = ex.Message;
                }
            }
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
