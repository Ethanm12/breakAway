using System.Collections.Generic;
using BreakAway.Entities;

namespace BreakAway.Models.Contacts
{

    public class IndexViewModel 
    {
        public ContactItem[] Contacts { get; set; }

    }

    public class ContactItem {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IList<Address> Addresses { get; set; } 
    }

}