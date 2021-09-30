using BreakAway.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Abstractions
{

    public interface IContactFilter
    {
        ContactItem[] Title(string searchLastName, IndexViewModel model);
        ContactItem[] FirstName(string searchLastName, IndexViewModel model);
        ContactItem[] LastName(string searchLastName, IndexViewModel model);
    }
    public class ContactFilter : IContactFilter
    {

        public ContactItem[] FirstName(string searchFirstName, IndexViewModel model)
        {
            if (!string.IsNullOrEmpty(searchFirstName) && model.Contacts != null)
            {
                return model.Contacts.Where(s => s.FirstName.ToUpper().Contains(searchFirstName.ToUpper())).ToArray();
            }
            else {
                return model.Contacts;
            }
        }

        public ContactItem[] LastName(string searchLastName, IndexViewModel model)
        {
            if (!string.IsNullOrEmpty(searchLastName) && model.Contacts != null )
            {
                return model.Contacts.Where(s => s.LastName.ToUpper().Contains(searchLastName.ToUpper())).ToArray();
            }
            else
            {
                return model.Contacts;
            }
        }

        public ContactItem[] Title(string searchTitle, IndexViewModel model)
        {
            if (!string.IsNullOrEmpty(searchTitle) && model.Contacts != null)
            {
                return model.Contacts.Where(s => !(string.IsNullOrEmpty(s.Title))
                                                                    && s.Title.ToUpper().Contains(searchTitle.ToUpper())).ToArray();
            }
            else
            {
                return model.Contacts;
            }
        }
    }
}