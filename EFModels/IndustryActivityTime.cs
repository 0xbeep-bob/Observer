using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Observer.EFModels
{
    public class industryActivity
    {
        [Key]
        public int typeID { get; set; }
        public int activityID { get; set; }
        public int time { get; set; }
    }
}
