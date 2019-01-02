using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Observer.EFModels
{
    class mapSolarSystem
    {
        [Key]
        public int solarSystemID { get; set; }
        public string solarSystemName { get; set; }
        public decimal security { get; set; }
    }
}
