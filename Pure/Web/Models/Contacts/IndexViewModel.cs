using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        
    }
}