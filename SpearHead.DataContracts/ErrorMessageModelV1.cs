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
public sealed class ErrorMessageModelV1<T> where T : class
{
    [DataMember(Order = 1)]
    public int Row { get; private set; }
    [DataMember(Order = 2)]
    public T Content { get; set; }
    [DataMember(Order = 3)]
    public List<string> ErrorMessages { get; set; }

    public ErrorMessageModelV1(int row, T content)
    {
        Row = row;
        Content = content;
    }
}
}
