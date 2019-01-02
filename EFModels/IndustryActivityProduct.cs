using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Observer.EFModels
{
    public class industryActivityProduct
    {
        [Key]
        public int typeID { get; set; }
        public int activityID { get; set; }
        public int productTypeID { get; set; }
        public int quantity { get; set; }
    }
}
