using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackOffice.Model;
using BackOffice.DBContext;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BackOffice.CustomAttributes;

namespace BackOffice.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly BackOfficeDBContext DBContext;

        public EmployeeController(BackOfficeDBContext dBContext)
        {
            DBContext = dBContext;
        }
        
        [Cache(20)]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            // var employee = await DBContext.Employee.Include(x=>x.Department).ToListAsync();
            var employee = await DBContext.Employee.ToListAsync();
            return Ok(employee);
        }
    }
}