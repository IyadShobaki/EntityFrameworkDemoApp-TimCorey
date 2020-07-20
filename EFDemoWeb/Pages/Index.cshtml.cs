using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// Benefits of Entity Framework Core   -------------->   Tim comment
// 1. Faster development speed   --------> slow in production (bad performance)
// 2. You don't have to know SQL --------> Wrong - You should know SQL

// Finally, If you are advanced user of entity framework and you know how to optimize it
// absolutely use EF Core

// Benefits of Dapper
// 1. Faster in production -> slow in development
// 2.Easier to work with for SQL developer
// 3. Designed for loose coupling

namespace EFDemoWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PeopleContext _db;

        public IndexModel(ILogger<IndexModel> logger, PeopleContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            LoadSampleData();

            var people = _db.People
                .Include(a => a.Addresses)
                .Include(e => e.EmailAddresses)
                //.ToList()
                //.Where(x => ApprovedAge(x.Age)); // C# code - bad practice 
                .Where(x => x.Age >= 18 && x.Age <= 65)
                .ToList();
        }

        private bool ApprovedAge(int age)
        {
            return (age >= 18 && age <= 65);
        }
        private void LoadSampleData()
        {
            if (_db.People.Count() == 0)
            {
                string file = System.IO.File.ReadAllText("generated.json");
                var people = JsonSerializer.Deserialize<List<Person>>(file);
                _db.AddRange(people);
                _db.SaveChanges();
            }
        }


    }
}
