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
    public sealed class ExcelUploadResponseModel
    {
        [DataMember(Order = 1)]
        public StatusCodes HttpStatusCode { get; private set; }

        [DataMember(Order = 2)]
        public List<ErrorMessageModel> ErrorMessages { get; private set; }

        public ExcelUploadResponseModel(List<ErrorMessageModel> errorModels)
        {
            Initilize(errorModels);
        }

        private void Initilize(List<ErrorMessageModel> errorModels)
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
