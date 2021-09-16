using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BreakAway.Entities;
using BreakAway.Models.Contacts;

namespace BreakAway.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IRepository _repository;

        public ContactsController(IRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
        }
        // GET: Contacts
        [HttpGet]
        public ActionResult Index(string message, string searchTitle, string searchFirstName, string searchLastName)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }

            var viewModel = new IndexViewModel();

            viewModel.Contacts = (from contact in _repository.Contacts
                                  select new ContactItem
                                  {
                                      Id = contact.Id,
                                      FirstName = contact.FirstName,
                                      LastName = contact.LastName,
                                      Title = contact.Title,
                                  }).ToArray();

            if (!string.IsNullOrEmpty(searchTitle))
            { 
                viewModel.Contacts = viewModel.Contacts.Where(s => !(string.IsNullOrEmpty(s.Title)) 
                                                                    && s.Title.ToUpper().Contains(searchTitle.ToUpper())).ToArray();
            }
            if (!string.IsNullOrEmpty(searchFirstName))
            { 
                viewModel.Contacts = viewModel.Contacts.Where(s => s.FirstName.ToUpper().Contains(searchFirstName.ToUpper())).ToArray();
            }
            if (!string.IsNullOrEmpty(searchLastName))
            {
                viewModel.Contacts = viewModel.Contacts.Where(s => s.LastName.ToUpper().Contains(searchLastName.ToUpper())).ToArray();
            }

            return View(viewModel);

        }
        [HttpGet]
        public ActionResult Edit(int id, string message) {

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }

            var contact = _repository.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contacts");
            }

            var viewModel = new EditViewModel
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Title = contact.Title
            }; 

            return View(viewModel);
        }

        // POST: /Contacts/Edit/{Id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", "Contacts", model);
            }

            var contact = _repository.Contacts.FirstOrDefault(p => p.Id == model.Id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contacts", new { message = "Contact containing id of '" + model.Id + "' was not found" });
            }
            // Add some validation
            contact.Id = model.Id;
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.Title = model.Title;
            contact.ModifiedDate = DateTime.Now;

            _repository.Save();

            return RedirectToAction("Edit", "Contacts", new { id = contact.Id, message = "Edits have been Successful" });
        }

        public ActionResult Add(string message)
        {

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }

            var model = new AddViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Add", "Contacts", new { message = "Contact not created" });
            }

            var contact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Title = model.Title,
                ModifiedDate = DateTime.Now,
                AddDate = DateTime.Now
            };

            _repository.Contacts.Add(contact);
            _repository.Save();

            return RedirectToAction("index", "Contacts", new { message = "Contact has been added successfully" });
        }

        [HttpPost]
        public ActionResult Delete(int id) {

            var contact = _repository.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact != null)
            {
                return RedirectToAction("Index", "Contacts", new { message = contact });
                //_repository.Contacts.Delete(contact);
                //_repository.Save();
            }

            return RedirectToAction("Index", "Contacts");
        }
    }
}