// второй контекст был создан для удобства вставки новой бд при ее обновлении
// необходимо менять название таблицы "industryActivity" на "industryActivities" потому как не шарю как изменить правило именования ед.число - мн. число

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
    /// Коннект к бд ссп
    /// </summary>
    class CCPBaseContext : DbContext
    {
        public CCPBaseContext() : base("CCPBaseConnection")
        {
        }

        public DbSet<invType> invTypes { get; set; }
        public DbSet<mapSolarSystem> mapSolarSystems { get; set; }
        public DbSet<industryActivityMaterial> industryActivityMaterials { get; set; }
        public DbSet<industryActivity> industryActivities { get; set; }
        public DbSet<industryActivityProduct> industryActivityProducts { get; set; }
        public DbSet<industryBlueprint> industryBlueprints { get; set; }
        // public DbSet<invMetaGroup> invMetaGroups { get; set; }
        public DbSet<invMetaType> invMetaTypes { get; set; }
    }
}