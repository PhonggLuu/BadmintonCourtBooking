using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;
using BY.Business;
using Microsoft.Identity.Client;
using Elfie.Serialization;
using System.Drawing.Printing;

namespace BY.RazorWebApp.Pages.CustomerPage
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerBusiness _business;

        public IndexModel()
        {
            _business ??= new CustomerBusiness();
        }

        public IList<Customer> Customers { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; } = default!;


        [BindProperty(SupportsGet = true)]
        public string SearchAddress { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; } = 5;

        [BindProperty(SupportsGet = true)]
        public int CountPage { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Customers = (await _business.GetAll()).Data as IList<Customer>;
            if (!string.IsNullOrEmpty(SearchName))
            {
                Customers = Customers.Where(c => c.Name.ToLower().Contains(SearchName.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(SearchAddress))
            {
                Customers = Customers.Where(c => c.Address.ToLower().Contains(SearchAddress.ToLower())).ToList();
            }
            CountPage = (int)Math.Ceiling((double)Customers.Count / PageSize);
            if (PageIndex >= 1 && PageIndex <= CountPage)
            {
                Customers = Customers.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                Customers = Customers.Take(PageSize).ToList();
                PageIndex = 1;
            }    
            //var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
