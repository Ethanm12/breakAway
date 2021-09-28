using BreakAway.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BreakAway.Models.Contacts
{
    public class AddViewModel
    {

        [MaxLength(50), Required]
        public string FirstName { get; set; }
    
        [MaxLength(50), Required]
        public string LastName { get; set; }
        [MaxLength(50), Required]
        public string Title { get; set; }

        public IList<Address> Addresses { get; set; }
    }
}