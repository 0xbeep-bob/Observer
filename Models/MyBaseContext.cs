using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Observer.EFModels;

namespace Observer.Models
{
    /// <summary>
    /// Коннект к моей бд
    /// </summary>
    class MyBaseContext : DbContext
    {
        public MyBaseContext() : base("MyBaseConnection")
        {
        }

        public DbSet<IDBlueprint> IDBlueprints { get; set; }
        public DbSet<Price> Prices { get; set; }
    }
}
