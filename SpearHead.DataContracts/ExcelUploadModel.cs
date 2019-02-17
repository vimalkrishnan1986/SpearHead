using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.DataContracts
{
    [DataContract]
    [Serializable]
    public sealed class ExcelUploadModel
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }
        [DataMember(Order = 2)]
        public byte[] Content { get; set; }
    }
}
