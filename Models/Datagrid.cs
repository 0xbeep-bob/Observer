using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Observer.Models
{
    /// <summary>
    /// Класс описывающий Datagrid
    /// </summary>
    class Datagrid
    {
        public int ItemID { get; set; }
        public int MaterialItemID { get; set; }
        public string MaterialItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Sell { get; set; }
        public decimal Buy { get; set; }
    }
}
