﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Observer.EFModels
{
    public class invMetaType
    {
        [Key]
        public int typeID { get; set; }
        public int parentTypeID { get; set; }
        public int metaGroupID { get; set; }
    }
}
