using SocialMediaApp.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Models
{
    public class OperationResult<T>
    {
        public T Payload { get; set; }
        public bool IsError { get; private set; }
        public List<Error> Errors { get; set; } = new List<Error>();


        public void AddError(ErrorCodes code, string message)
        {
            HandleError(code, message);
        }

        public void AddUnknownError(string message)
        {
            HandleError(ErrorCodes.UnknownError, message);
        }

        public void ResetIsErrorFlag()
        {
            IsError = false;
        }

        #region Private methods

        private void HandleError(ErrorCodes code, string message)
        {
            Errors.Add(new Error { ErrorCode = code, ErrorMessage = message });
            IsError = true;
        }
        #endregion
    }
}
