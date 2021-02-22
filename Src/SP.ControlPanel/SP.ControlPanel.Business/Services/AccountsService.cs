using System.Collections.Generic;
using System.Linq;
using SP.ControlPanel.Business.DTO;
using SP.ControlPanel.Business.Helpers;
using SP.ControlPanel.Business.Interfaces.Exceptions;
using SP.ControlPanel.Business.Interfaces.Helpers;
using SP.ControlPanel.Business.Interfaces.Model;
using SP.ControlPanel.Business.Interfaces.Services;
using SP.ControlPanel.Business.Model;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Repositories;

namespace SP.ControlPanel.Business.Services
{
    public class AccountsService : IAccountsService
    {
        private IAccountsRepository _accountsRepository;
        private IPersonsRepository _personsRepository;
        private IUnitOfWork _uow;

        public AccountsService(IUnitOfWork unitOfWork)
        {
            _accountsRepository = unitOfWork.AccountsRepository;
            _personsRepository = unitOfWork.PersonsRepository;
            _uow = unitOfWork;
        }

        public IUserAccount Create(IUserAccount account)
        {
            //Validate if email doesn't exist
            IPerson personWithEmail = _personsRepository.GetByEmail(account.Email);
            if (personWithEmail.Id != 0)
            {
                throw new EmailAlreadyInUseException($"Email '{account.Email}' is already in use");
            }

            IPerson personDb = new PersonDTO()
            {
                Email = account.Email,
                PersonTypeId = (int)account.PersonType,
                Name = account.Name,
                LastName = account.LastName
            };
            _personsRepository.Add(personDb);
            _uow.Save();

            IPerson savedPerson = _personsRepository.GetByEmail(account.Email);
            IAccount accountDb = new AccountDTO()
            {
                IsActive = true,
                AccountTypeId = (int)account.AccountType,
                IdentityProviderId = account.IdentityProviderId,
                PersonId = savedPerson.Id
            };
            _accountsRepository.Add(accountDb);
            _uow.Save();

            IAccountDetail createdAccount = _accountsRepository.GetByIdentityProviderId(account.IdentityProviderId);

            return new UserAccount(createdAccount);
        }

        public IUserAccount Update(IUserAccount account)
        {
            IAccountDetail currentPerson = _accountsRepository.GetById(account.AccountId);
            if (currentPerson.Email.ToLower() != account.Email)
            {
                //Validate that the new email doesn't exist
                IPerson newEmailPerson = _personsRepository.GetByEmail(account.Email);

                if (newEmailPerson.Id != 0)
                {
                    throw new EmailAlreadyInUseException($"Email '{account.Email}' is already in use");
                }
            }

            _personsRepository.Update(new PersonDTO()
            {
                Name = account.Name, 
                Email = account.Email, 
                LastName = account.LastName
            });
            _uow.Save();

            IAccountDetail updatedUser = _accountsRepository.GetById(account.AccountId);

            return new UserAccount(updatedUser);
        }

        public IUserAccount Delete(IUserAccount account)
        {
            _accountsRepository.DeleteById(account.AccountId);

            IAccountDetail disabledAccount = _accountsRepository.GetById(account.AccountId);

            return new UserAccount(disabledAccount);
        }

        public IUserAccount GetByGlobalId(string id)
        {
            IAccountDetail account = _accountsRepository.GetByIdentityProviderId(id);

            return new UserAccount(account);
        }

        public IUserAccount GetById(long id)
        {
            IAccountDetail account = _accountsRepository.GetById(id);

            return new UserAccount(account);
        }

        public IPaginatedResult<IUserAccount> PaginatedList(int page, int size)
        {
            Data.Interfaces.Helpers.IPaginatedResult<IAccountDetail> accountDetails = _accountsRepository.PaginatedGetAllDetails(1, 1);

            IEnumerable<IUserAccount> accountsPage = accountDetails.Items.Select(x => new UserAccount(x)).ToList();

            return new PaginatedResult<IUserAccount>(accountsPage, accountDetails.TotalItems, accountDetails.Page, accountDetails.PageSize);
        }
    }
}