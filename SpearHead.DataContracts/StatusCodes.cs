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
    public enum StatusCodes
    {
        [EnumMember]
        Sucess = 1,
        [EnumMember]
        Error,
        [EnumMember]
        BadRequest
    }
}
