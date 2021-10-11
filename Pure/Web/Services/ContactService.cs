using BreakAway.Entities;
using BreakAway.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models.Contacts;

namespace Web.Services
{
    public interface IContactService
    {
        ContactItem[] GetContactItems(FilterViewModel filterOptions); 
    }

    public class ContactService : IContactService
    {
        private readonly IRepository _repository;
        private readonly IContactFilter[] _contactFilter;

        public ContactService(IRepository repository, IContactFilter[] contactFilter)
        {
            // Null Exceptions set intentionally 
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (contactFilter == null)
            {
                throw new ArgumentNullException(nameof(contactFilter));
            }
            _repository = repository;
            _contactFilter = contactFilter;
        }

        public ContactItem[] GetContactItems(FilterViewModel filterOptions)
        {

            if (filterOptions == null)
            {
                throw new ArgumentNullException(nameof(filterOptions));
            }
            if (_repository.Contacts == null)
            {
                throw new ArgumentNullException(nameof(filterOptions));
            }

            var contactList = (from contact in _repository.Contacts
                                                 select new ContactItem
                                                 {
                                                     Id = contact.Id,
                                                     FirstName = contact.FirstName,
                                                     LastName = contact.LastName,
                                                     Title = contact.Title,
                                                 });
            
            foreach (var applyFilter in _contactFilter)
            { 
                if (applyFilter.ShouldFilter(filterOptions))
                {
                    contactList =  applyFilter.Filter(filterOptions, contactList);
                }
            } 
           
            return contactList.ToArray();
        }
    }

}