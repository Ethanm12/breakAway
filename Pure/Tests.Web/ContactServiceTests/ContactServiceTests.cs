using AutoFixture;
using BreakAway.Entities;
using BreakAway.Models.Contacts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models.Contacts;
using Web.Services;

namespace Tests
{
    [TestFixture]
    public class ContactServiceTests
    {
        private ContactService _sut;
        private Mock<IRepository> _mockRepo;
        private Mock<IContactFilter>[] _mockFilter; 
        private Fixture _fixture;  

        [SetUp]
        public void SetUp()
        { 
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var contacts = _fixture.CreateMany<Contact>().AsQueryable();
            _mockRepo = new Mock<IRepository>(); 

            //_mockRepo.Setup(p => p.Contacts).Returns(contacts.Cast(ITable<Contact>)); 

            //contacts = contacts.Select(contact => new ContactItem
            //{
            //    Id = contact.Id,
            //    FirstName = contact.FirstName,
            //    LastName = contact.LastName,
            //    Title = contact.Title,
            //}).ToArray()


            _mockFilter = new Mock<IContactFilter>[] { new Mock<IContactFilter>(), new Mock<IContactFilter>(), new Mock<IContactFilter>() };
            _sut = new ContactService(_mockRepo.Object, _mockFilter.Select(f=>f.Object).ToArray());
        }

        [Test]
        public void ContactService_throw_error_when_contacts_is_null()
        {
            //Arrange
            // This might need some new shit 
            var something = _mockRepo.Object;
            something = null;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                    new ContactService(something, _mockFilter.Select(f => f.Object).ToArray())); 

        }

        [Test]
        public void ConntactService_throw_error_when_contactFilter_injection_is_null()
        {
            //Arrange
            _mockFilter = null;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                    new ContactService(_mockRepo.Object, _mockFilter.Select(f => f.Object).ToArray())); 

        }

        // Methods

        [Test]
        public void GetContactItems_return_false_if_filterOptions_is_null()
        {
            //Arrange
            FilterViewModel filterOptions = null;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => 
                    _sut.GetContactItems(filterOptions));
        }

        //Return something is DB and Filter are Empty 

        [Test]
        public void GetContactItems_throw_exception_if_repository_is_null()
        {
            // Arrange
            //_mockRepo.Setup(s => s.GetType());
            var filterOptions = _fixture.Create<FilterViewModel>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => _sut.GetContactItems(filterOptions)); 
        }

        // Get contacts with no arguments

            //MAYBE YEET THIS
        [Test]
        public void GetModel_throwss_exception_if_repository_is_null()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Create<FilterViewModel>();
            _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            // Assert 
            Assert.That(() => _sut.GetContactItems(filterOptions), Throws.Nothing);
        }

        // COme back to this 
        [Test]
        public void GetContactItem_return_contactList_with_no_applied_filters()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Build<FilterViewModel>()
                .With(s => s.FilterTitle, string.Empty)
                .With(s => s.FilterFirstName, string.Empty)
                .With(s => s.FilterLastName, string.Empty)
                .Create();

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);
            var something = _mockRepo.Object.Contacts.ToArray();

            // Act
            var result = _sut.GetContactItems(filterOptions);

            // Assert 
            Assert.AreEqual(result, Is.EqualTo(something));
        }

        [Test]
        public void GetContactItem_return_true_if_ContactList_is_not_empty()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Build<FilterViewModel>()
                .With(s => s.FilterTitle, string.Empty)
                .With(s => s.FilterFirstName, string.Empty)
                .With(s => s.FilterLastName, string.Empty)
                .Create();

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            var result = _sut.GetContactItems(filterOptions);
            // Assert 
            Assert.That(result, Is.InstanceOf<ContactItem[]>());
        }

        [Test]
        public void GetContactItem_return_true_if_Linq_to_Database_is_called()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Build<FilterViewModel>()
                .With(s => s.FilterTitle, string.Empty)
                .With(s => s.FilterFirstName, string.Empty)
                .With(s => s.FilterLastName, string.Empty)
                .Create();

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            var result = _sut.GetContactItems(filterOptions);
            // Assert 
            //_mockRepo.Verify(result, Times.on);
        }
    }
}