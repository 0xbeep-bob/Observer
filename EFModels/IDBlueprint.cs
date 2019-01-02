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
    public class IDBlueprint
    {
        [Key]
        public int ItemID { get; set; }
        public string ItemName { get; set; }
    }
}
