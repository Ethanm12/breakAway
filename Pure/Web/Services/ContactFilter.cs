using BreakAway.Models.Contacts;
using System;
using System.Linq;
using Web.Models.Contacts;

namespace Web.Services
{
    public interface IContactFilter
    {
        IQueryable<ContactItem> Filter(FilterViewModel filterModel, IQueryable<ContactItem> contacts);
        bool ShouldFilter(FilterViewModel filterModel);
    }

    public class TitleFilter : IContactFilter
    {
        public IQueryable<ContactItem> Filter(FilterViewModel filterModel, IQueryable<ContactItem> contacts)
        {

            return contacts.Where(s => !(string.IsNullOrEmpty(s.Title))
                                                                && s.Title.ToUpper().Contains(filterModel.FilterTitle.ToUpper()));
        }

        public bool ShouldFilter(FilterViewModel filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.FilterTitle))
            {
                return true;
            }
            return false;
        }
    }
    public class FirstNameFilter : IContactFilter
    {

        public IQueryable<ContactItem> Filter(FilterViewModel filterModel, IQueryable<ContactItem> contacts)
        {
            return contacts.Where(s => s.FirstName.ToUpper().Contains(filterModel.FilterFirstName.ToUpper()));
        }

        public bool ShouldFilter(FilterViewModel filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.FilterFirstName))
            {
                return true;
            }
            return false;
        }
    }
    public class LastNameFilter : IContactFilter
    {
        public IQueryable<ContactItem> Filter(FilterViewModel filterModel, IQueryable<ContactItem> contacts)
        {
            return contacts.Where(s => s.LastName.ToUpper().Contains(filterModel.FilterLastName.ToUpper()));
        }

        public bool ShouldFilter(FilterViewModel filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.FilterLastName))
            {
                return true;
            }
            return false;
        }
    }

}

