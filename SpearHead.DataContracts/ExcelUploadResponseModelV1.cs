using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.DataContracts
{
    public sealed class ExcelUploadResponseModelV1<T> where T : class
    {
        [DataMember(Order = 1)]
        public StatusCodes HttpStatusCode { get; private set; }

        [DataMember(Order = 2)]
        public List<ErrorMessageModelV1<T>> ErrorMessages { get; private set; }

        public ExcelUploadResponseModelV1(List<ErrorMessageModelV1<T>> errorModels)
        {
            Initilize(errorModels);
        }

        private void Initilize(List<ErrorMessageModelV1<T>> errorModels)
        {
            if (errorModels == null)
            {
                this.HttpStatusCode = StatusCodes.Sucess;
                return;
            }
            if (errorModels.Count == 0)
            {
                this.HttpStatusCode = StatusCodes.Sucess;
                return;
            }
            ErrorMessages = errorModels;
            HttpStatusCode = StatusCodes.BadRequest;
        }
    }
}
