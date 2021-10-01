using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Contacts
{

    public interface IFilterViewModel
    {
         string FilterTitle { get; set; }
         string FilterFirstName { get; set; }
         string FilterLastName { get; set; }
    }
    public class FilterViewModel
    {
        public string FilterTitle { get; set; }
        public string FilterFirstName { get; set; }
        public string FilterLastName { get; set; }
    }
}