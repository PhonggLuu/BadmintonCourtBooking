using BY.Data.Base;
using BY.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BY.Data.Repository
{
    public class CourtRepository : GenericRepository<Court>
    {
        public CourtRepository() { }
        public CourtRepository(Net1704_221_2_BYContext context) => _context = context;
    }
}
