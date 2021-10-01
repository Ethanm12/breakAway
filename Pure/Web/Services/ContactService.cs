using BreakAway.Entities;
using BreakAway.Models.Contacts;
using System;
using System.Linq;
using Web.Models.Contacts;

namespace Web.Services
{
    public interface IContactService
    {
        ContactItem[] GetContactItems(FilterViewModel filterOptions);
        IndexViewModel getModel();
    }

    public class ContactService : IContactService
    {
        private readonly IRepository _repository;
        private readonly IContactFilter[] _contactFilter;

        public ContactService(IRepository repository, IContactFilter[] contactFilter)
        {
            if (repository == null)
            {
                throw new ArgumentException("repository");
            }
            if (contactFilter == null)
            {
                throw new ArgumentException("contactFilter");
            }
            _repository = repository;
            _contactFilter = contactFilter;
        }

        public ContactItem[] GetContactItems(FilterViewModel filterOptions)
        {

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

        public IndexViewModel getModel()
        {

            IndexViewModel returnModel = new IndexViewModel
            {
                Contacts = (from contact in _repository.Contacts
                            select new ContactItem
                            {
                                Id = contact.Id,
                                FirstName = contact.FirstName,
                                LastName = contact.LastName,
                                Title = contact.Title,
                            }).ToArray()
            };

            return returnModel;
        }

    }

}