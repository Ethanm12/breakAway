
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BreakAway.Models.Contacts
{
    public class EditViewModel : IEditViewModel
    { 
        public int Id { get; set; } 
        [MaxLength(50), Required]
        public string FirstName { get; set; } 
        [MaxLength(50), Required]
        public string LastName { get; set; }
        [MaxLength(50), Required]
        public string Title { get; set; }
        public IList<AddressModel> Addresses { get; set; } 
    }

    public class AddressModel
    {
        public int Id { get; set; } 
        
        [StringLength(50)]
        public string CountryRegion { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        [StringLength(50), Required]
        public string AddressType { get; set; }

        public int ContactId { get; set; }
        [StringLength(50)]
        public string Street1 { get; set; }

        [StringLength(50)]
        public string Street2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string StateProvince { get; set; }
    } 
}