using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BreakAway.Entities;
using BreakAway.Models.Contacts;
using Web.Abstractions;
using Web.Services;

namespace BreakAway.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IRepository _repository;
        //private readonly IndexViewModel _indexViewModel; 
        private readonly IContactFilter _contactFilter;
        private readonly IContactService _contactModel;



        public ContactsController(IRepository repository, IContactFilter contactFilter, IContactService contactModel)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (contactFilter == null)
            {
                throw new ArgumentNullException("contactFilter");
            }
            _repository = repository;
            _contactFilter = contactFilter;
            _contactModel = contactModel;
        }
        // GET: Contacts        
 
        [HttpGet]
        public ActionResult Index(string message, IContactFilter something)
        {
             
           
            ViewBag.message = message;


            IndexViewModel viewModel = _contactModel.getModel(); 



            //viewModel.Contacts = _contactFilter.Title(searchTitle, viewModel);

            //viewModel.Contacts = _contactFilter.FirstName(searchFirstName, viewModel);
            
            //viewModel.Contacts = _contactFilter.LastName(searchLastName, viewModel);
 
            return View(viewModel);

        }


        public ActionResult Edit(int? id, string message)
        {

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            if (id == null)
            {
                return RedirectToAction("Index", "Contacts");
            }

            var contact = _repository.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contacts", new { message = "Sorry the Id: '" + id + "' does not exist " });
            }

            var viewModel = new EditViewModel
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Title = contact.Title,
                Addresses = GetAddresses(contact.Id)
            };

            return View(viewModel);
        }

        private IList<AddressModel> GetAddresses(int id)
        {
            return (from address in _repository.Addresses
                    where address.ContactId == id
                    select new AddressModel
                    {
                        Id = address.Id,
                        CountryRegion = address.CountryRegion,
                        PostalCode = address.PostalCode,
                        Street1 = address.Mail.Street1,
                        Street2 = address.Mail.Street2,
                        City = address.Mail.City,
                        StateProvince = address.Mail.StateProvince,
                        AddressType = address.AddressType
                    }
                    ).ToArray();

        }

        // POST: /Contacts/Edit/{Id}
        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var contact = _repository.Contacts.FirstOrDefault(p => p.Id == model.Id);

            var address = (from addressItem in _repository.Addresses
                           where addressItem.ContactId == contact.Id
                           select addressItem).ToArray();

            if (contact == null)
            {
                return RedirectToAction("Index", "Contacts", new { message = "Contact was not found" });
            }

            contact.Id = model.Id;
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.LastName = model.LastName;
            contact.Title = model.Title;
            contact.ModifiedDate = DateTime.Now;

            if (model.Addresses == null)
            {
                for (var i = 0; i < address.Count(); i++)
                {
                    _repository.Addresses.Delete(address[i]);
                }
            }
            else
            {

                for (var i = 0; i < model.Addresses.Count(); i++)
                {
                    var targetAddress = address.FirstOrDefault(s => s.Id == model.Addresses[i].Id);

                    if (targetAddress != null)
                    {
                        targetAddress.Mail.Street1 = model.Addresses[i].Street1;
                        targetAddress.Mail.Street2 = model.Addresses[i].Street2;
                        targetAddress.Mail.City = model.Addresses[i].City;
                        targetAddress.Mail.StateProvince = model.Addresses[i].StateProvince;
                        targetAddress.CountryRegion = model.Addresses[i].CountryRegion;
                        targetAddress.PostalCode = model.Addresses[i].PostalCode;
                        targetAddress.AddressType = model.Addresses[i].AddressType;
                    }

                    if (model.Addresses[i].Id == 0)
                    {
                        var newAddress = new Address
                        {
                            ContactId = model.Id,
                            CountryRegion = model.Addresses[i].CountryRegion,
                            PostalCode = model.Addresses[i].PostalCode,
                            AddressType = model.Addresses[i].AddressType,
                            Mail = new Mail
                            {
                                Street1 = model.Addresses[i].Street1,
                                Street2 = model.Addresses[i].Street2,
                                City = model.Addresses[i].City,
                                StateProvince = model.Addresses[i].StateProvince,
                            },
                            ModifiedDate = DateTime.Now
                        };
                        _repository.Addresses.Add(newAddress);
                    }
                }
                for (var i = 0; i < address.Count(); i++)
                {
                    var targetModel = model.Addresses.FirstOrDefault(s => s.Id == address[i].Id);

                    if (targetModel == null)
                    {
                        _repository.Addresses.Delete(address[i]);
                    }
                }
            }
            _repository.Save();

            return RedirectToAction("Edit", "Contacts", new { id = contact.Id, message = "Edits have been Successful" });
        }

        [HttpGet]
        public ActionResult Add(string message)
        {

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }

            var model = new AddViewModel();

            return View(model);
        }

        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult Add(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var contact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Title = model.Title,
                ModifiedDate = DateTime.Now,
                AddDate = DateTime.Now,
                //Addresses = new Address

                //Addresses = _repository.Activities.FirstOrDefault(p => p.Id == model.PrimaryActivityId)
            };

            _repository.Contacts.Add(contact);
            _repository.Save();

            return RedirectToAction("index", "Contacts", new { message = "Contact has been added successfully" });
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Contacts", new { message = "You cannot Delete" });
            }

            var contact = _repository.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact != null)
            {
                var desiredCustomer = (from customer in _repository.Customers
                                       where customer.Id == contact.Id
                                       select customer).FirstOrDefault();
                if (desiredCustomer != null)
                {
                    var desiredReservation = (from reservation in _repository.Reservations
                                              where reservation.CustomerId == desiredCustomer.Id
                                              select reservation).ToArray();

                    foreach (var reservation in desiredReservation)
                    {

                        var desiredPayment = (from payment in _repository.Payments
                                              where reservation.Id == payment.ReservationId
                                              select payment).ToArray();

                        if (desiredPayment != null)
                        {
                            foreach (var payment in desiredPayment)
                            {
                                _repository.Payments.Delete(payment);
                            }
                        }


                        _repository.Reservations.Delete(reservation);
                    }
                }


                var desiredAddress = (from address in _repository.Addresses
                                      where address.ContactId == contact.Id
                                      select address).ToArray();

                if (desiredAddress != null)
                {
                    foreach (var address in desiredAddress)
                    {
                        _repository.Addresses.Delete(address);
                    }


                    var desiredLodge = (from lodge in _repository.Lodgings
                                        where lodge.ContactId == contact.Id || lodge.LocationId.Equals(null)
                                        select lodge).ToArray();

                    if (desiredLodge != null || desiredLodge.Length == 0)
                    {

                        foreach (var lodge in desiredLodge)
                        {
                            var desiredEvent = (from events in _repository.Events
                                                where lodge.Id == events.LodgingId && lodge.LocationId == events.LocationId
                                                select events).ToArray();

                            foreach (var @event in desiredEvent)
                            {

                                //var desiredEventActivities = (from eventActivities in _repository.Events where @event.Id == eventActivities.Activities select eventActivities).ToArray();

                                _repository.Events.Delete(@event);
                            }

                            _repository.Lodgings.Delete(lodge);
                        }
                    }


                }

                if (desiredCustomer != null)
                {
                    _repository.Customers.Delete(desiredCustomer);
                }


                _repository.Contacts.Delete(contact);

                _repository.Save();
                return RedirectToAction("Index", "Contacts");
            }
            return View();
        }
    }
}