using BY.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BY.Data.DAO
{
    public class CourtViewModel
    {
        public Court Court { get; set; } = new Court();

        [BindProperty] // This is now okay since it's a view model
        public IFormFile ImageFile { get; set; }
    }
}
