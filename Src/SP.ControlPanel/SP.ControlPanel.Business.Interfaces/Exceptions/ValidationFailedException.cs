using System;
using System.Collections.Generic;

namespace SP.ControlPanel.Business.Interfaces.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public List<KeyValuePair<string, string>> FailedValidations { get; set; }
        public ValidationFailedException(List<KeyValuePair<string, string>> failedValidations) : base("One or more validations failed")
        {
            FailedValidations = failedValidations;
        }
    }
}