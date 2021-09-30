using BreakAway.Entities;
using BreakAway.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Services
{
    public class ContactService : IContactService
    {
        private readonly IRepository _repository;

        public ContactService(IRepository repository) {
            if (repository == null) {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
        }


        public IndexViewModel getModel() {
            IndexViewModel test = new IndexViewModel
            {
                Contacts = (from contact in _repository.Contacts
                            select new ContactItem
                            {
                                Id = contact.Id,
                                FirstName = contact.FirstName,
                                LastName = contact.LastName,
                                Title = contact.Title,
                                //Addresses = contact.Addresses.Where(s => s.Id == contact.Id),
                            }).ToArray()
            };
            return test; 
        }

    }

    public interface IContactService {
        IndexViewModel getModel();
    }
}