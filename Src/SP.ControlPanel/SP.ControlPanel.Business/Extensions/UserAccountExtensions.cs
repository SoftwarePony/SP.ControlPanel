using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SP.ControlPanel.Business.Interfaces.Enums;
using SP.ControlPanel.Business.Interfaces.Model;

namespace SP.ControlPanel.Business.Extensions
{
    public static class UserAccountExtensions
    {
        public static List<KeyValuePair<string, string>> Validate(this IUserAccount account)
        {
            List<KeyValuePair<string, string>> failedValidations = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrWhiteSpace(account.Name))
            {
                failedValidations.Add(new KeyValuePair<string, string>("Name", "The name is required"));
            }

            if (string.IsNullOrWhiteSpace(account.Email))
            {
                failedValidations.Add(new KeyValuePair<string, string>("Email", "The email is required"));
            }

            if (!new EmailAddressAttribute().IsValid(account.Email))
            {
                failedValidations.Add(new KeyValuePair<string, string>("Email", "The email is not valid"));
            }

            if (account.PersonType == PersonTypes.Individual && string.IsNullOrWhiteSpace(account.LastName))
            {
                failedValidations.Add(new KeyValuePair<string, string>("LastName", "The last name is required"));
            }

            return failedValidations;
        }
    }
}