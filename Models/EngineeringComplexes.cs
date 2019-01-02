using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Models
{
    /// <summary>
    /// структура описывающая бонусы Инженерных комплексов
    /// </summary>
    struct EngineeringComplexes
    {
        public string Name { get; set; }
        public int MEBonus { get; set; }
        public int TEBonus { get; set; }
        public decimal TaxBonus { get; set; }                
    }
}
