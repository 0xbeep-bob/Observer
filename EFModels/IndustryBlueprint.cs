using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Observer.EFModels
{
    class industryBlueprint
    {
        [Key]
        public int typeID { get; set; }
        public int maxProductionLimit { get; set; }
    }
}
