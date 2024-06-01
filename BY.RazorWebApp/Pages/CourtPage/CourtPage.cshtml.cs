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

            public void OnGet()
            {
                courts = this.GetCourt();
            
            }

            public IActionResult OnPost()
            {
                this.SaveCourt();
                return RedirectToPage();
            }

            public void OnDelete()
            {
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
        }
    }
