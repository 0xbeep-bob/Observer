using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Observer.EFModels
{
    /// <summary>
    /// my class
    /// </summary>
    public class Price
    {
        [Key]
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Sell { get; set; }        
        public decimal Buy { get; set; }
    }
}
