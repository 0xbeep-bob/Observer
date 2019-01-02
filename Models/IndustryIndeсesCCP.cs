using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Models
{
    /// <summary>
    /// Десериализация индустриальных индексов
    /// </summary>
    class IndustryIndeсesCCP
    {
        public List<Index> cost_indices { get; set; }
        //public Dictionary<string, decimal> cost_indices { get; set; }
        public int solar_system_id { get; set; }
    }

    /// <summary>
    /// Вложенный тип для класса десериализации индустриальных индексов
    /// </summary>
    class Index
    {
        public string activity { get; set; }
        public decimal cost_index { get; set; }
    }
}
