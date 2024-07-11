using BY.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BY.Data.DAO
{
    public class CourtViewModel
    {
        public Court Court { get; set; } = new Court();

        [BindProperty]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
    }
}
