using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.DataContracts
{
    [Serializable]
    [DataContract]
    public class ErrorMessageModel
    {
        [DataMember(Order = 1)]
        public int Row { get; private set; }
        [DataMember(Order = 2)]
        public List<string> ErrorMessagees { get; set; }

        public ErrorMessageModel(int row)
        {
            Row = row;
            ErrorMessagees = new List<string>();
        }

    }
}
