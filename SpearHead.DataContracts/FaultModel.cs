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
    public class FaultModel
    {
        [DataMember]
        public Exception Exception { get; set; }

        [DataMember]
        public string Message { get; set; }

    }
}
