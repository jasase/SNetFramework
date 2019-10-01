using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Contracts.Services.DataAccess.EntityDescriptions
{
    public class ValidationResult
    {
        public string Message { get; private set; }
        public bool IsValid { get; private set; }

        private ValidationResult(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }

        public static ValidationResult Valid()
        {
            return new ValidationResult(string.Empty, true);
        }

        public static ValidationResult Invalid(string message)
        {
            return new ValidationResult(message, false);
        }
    }
}
