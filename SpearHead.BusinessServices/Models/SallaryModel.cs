using SpearHead.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.BusinessServices.Models
{
    public class SallaryModel : IModelIdentifier
    {
        public double No { get; set; }
        public double Age { get; set; }
        public double Sallary { get; set; }
        public int Row { get; set; }
    }
}
