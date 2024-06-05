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
            public List<Court> courts { get; set; } = new List<Court>();
        /*
            public void OnGet()
            {
                courts = this.GetCourt();
            
            }
        */
        /*
            public IActionResult OnPost()
            {
                this.SaveCourt();
                return RedirectToPage();
            }

            public void OnDelete()
            {
            }
        */

        public Court EditedCourt { get; set; }

        // Thêm một hàm mới để tải dữ liệu Court cần chỉnh sửa
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

        /*
         private void SaveCourt()
        {
            var courtResult = _courtBusiness.Save(this.Court);

            if (courtResult != null)
            {
                this.Message = courtResult.Result.Message;

            }
            else
            {
                this.Message = "Error system";
            }
        }
         */
    }
}
