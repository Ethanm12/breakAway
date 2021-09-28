using System.Collections.Generic;

namespace BreakAway.Models.Contacts
{
    public interface IEditViewModel
    {
        IList<AddressModel> Addresses { get; set; }
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string Title { get; set; }
    }
}