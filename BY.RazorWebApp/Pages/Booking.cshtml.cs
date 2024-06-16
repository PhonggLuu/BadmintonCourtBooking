using BY.Business;
using BY.Data.Models;
using BY.RazorWebApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class BookingModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness;

        [BindProperty]
        public string Search { get; set; } = "";
        [BindProperty]
        public string SortBy { get; set; } = "";
        [BindProperty]
        public string OrderBy { get; set; } = "";

        public List<Court> Courts { get; set; } = new List<Court>();
        public int TotalCount { get; set; } = 0;
        public int PageTotal { get; set; } = 0;
        public int CurrentPage { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public BookingModel()
        {
            _courtBusiness = new CourtBusiness();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string search = Request.Query["search"];
            string sortby = Request.Query["sortby"];
            string orderby = Request.Query["orderby"];

            Search = search ?? string.Empty;
            SortBy = sortby ?? string.Empty;
            OrderBy = orderby ?? string.Empty;
            if (!int.TryParse(Request.Query["page"], out int page))
            {
                page = 1;
            }
            if (!int.TryParse(Request.Query["pagesize"], out int pagesize))
            {
                pagesize = 10;
            }
            var result = await _courtBusiness.GetAllCourt();
            if (result != null && result.Status > 0)
            {
                if (result.Data != null)
                {
                    var query = result.Data as IEnumerable<Court>;
                    if (query != null && query.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(search))
                        {
                            search = search.Trim().ToLower();
                            query = query.Where(x => x.Name.ToLower().Contains(search) || x.Address != null && x.Address.ToLower().Contains(search));
                        }
                        var PageCount = query.Count();
                        // Determine sorting
                        if (!string.IsNullOrWhiteSpace(sortby))
                        {
                            sortby = sortby.Trim().ToLower();
                            orderby = orderby?.Trim().ToLower() ?? "asc";

                            bool isAscending = orderby.Equals("asc");

                            query = sortby switch
                            {
                                "name" => isAscending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
                                "address" => isAscending ? query.OrderBy(x => x.Address) : query.OrderByDescending(x => x.Address),
                                _ => query
                            };
                        }

                        var pagedCourts = query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToList();

                        Courts = pagedCourts;

                        TotalCount = PageCount;
                        PageTotal = (int)Math.Ceiling(PageCount / (double)pagesize);
                        CurrentPage = page;
                        PageSize = pagesize;
                    }
                }
            }
            return Page();
        }
    }
}
