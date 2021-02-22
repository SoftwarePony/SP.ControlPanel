using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SP.ControlPanel.Api.Models;
using SP.ControlPanel.Business.Interfaces.Enums;
using SP.ControlPanel.Business.Interfaces.Model;

namespace SP.ControlPanel.Api.ViewModels
{
    public class AccountVM
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public PersonTypes PersonType { get; set; }


        public IUserAccount ToUserAccount()
        {
            UserAccount userAccount = new UserAccount();

            userAccount.PersonType = PersonType;
            userAccount.Email = Email;
            userAccount.Name = Name;
            userAccount.LastName = LastName;

            return userAccount;
        }

        public List<KeyValuePair<string, string>> IsValid()
        {
            List<KeyValuePair<string, string>> failedValidations = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrWhiteSpace(Password))
            {
                failedValidations.Add(new KeyValuePair<string, string>("Password", "The password can not be empty"));
            }

            if (Password.Length < 6)
            {
                failedValidations.Add(new KeyValuePair<string, string>("Password", "The password must be at least 6 characters long"));
            }

            return failedValidations;
        }
    }
}