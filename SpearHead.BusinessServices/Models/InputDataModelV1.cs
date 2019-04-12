using SpearHead.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.BusinessServices.Models
{
    public class InputDataModelV1 : IModelIdentifier
    {
        public string Packet { get; set; }
        public string Product { get; set; }
        public int Row { get; set; }
    }
}
