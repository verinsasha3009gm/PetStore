using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Library.Results
{
    public class CollectionResult<T> : BaseResult
    {
        public List<T> Data;
        public long Count;
        public CollectionResult(List<T> data,string errorMessage, int errorCode)
        {
            Data = data;
            Count = data.Count;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}
