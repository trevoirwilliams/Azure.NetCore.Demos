using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesWeb.NetCore.EF.Demo.Data;

namespace RazorPagesWeb.NetCore.EF.Demo.Pages.Persons
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesWeb.NetCore.EF.Demo.Data.AppDbContext _context;

        public IndexModel(RazorPagesWeb.NetCore.EF.Demo.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Person> Person { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Persons != null)
            {
                Person = await _context.Persons.ToListAsync();
            }
        }
    }
}
