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


            _mockFilter = new Mock<IContactFilter>[] { new Mock<IContactFilter>(), new Mock<IContactFilter>(), new Mock<IContactFilter>() };
            _sut = new ContactService(_mockRepo.Object, _mockFilter.Select(s => s.Object).ToArray());
        }

        //Constructor
        [Test]
        public void Constructor_throw_ArgumentNullException_if_repository_is_null()
        {
            //Arrange 
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                    new ContactService(null, _mockFilter.Select(f => f.Object).ToArray()));

        }

        [Test]
        public void Constructor_throw_ArgumentNullException_if_ContactFilter_is_null()
        {
            //Arrange

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                    new ContactService(_mockRepo.Object, null));

        }

        // GetContactItems Method 
        [Test]
        public void GetContactItems_throw_ArgumentNullException_if_filterOptions_is_null()
        {
            //Arrange

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                    _sut.GetContactItems(null));
        }

        [Test]
        public void GetContactItems_throw_ArgumentNullException_if_repository_Contacts_is_null()
        {
            // Arrange 
            var filterOptions = _fixture.Create<FilterViewModel>();

            _mockRepo.Setup(s => s.Contacts).Returns((ITable<Contact>)null);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                    _sut.GetContactItems(filterOptions));
        }
        // Wont compile if filter is null  
        [Test]
        public void GetContactItem_return_all_ContactItems_from_repository_with_no_applied_filters()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Build<FilterViewModel>().Create();

            foreach (var filter in _mockFilter)
            {
                filter.Setup(s => s.ShouldFilter(filterOptions)).Returns(false);
            }

            _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            var result = _sut.GetContactItems(filterOptions);

            // Assert 

            Assert.IsNotNull(result);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(list.Count()));
            foreach (var item in result)
            {
                Assert.True(list.Any(i => i.IsEqual(item)));
            }
        }

        [Test]
        public void GetContactItem_return_all_ContactItems_when_contactFilter_dependancy_is_zero()
        {
            // Arrange  
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Create<FilterViewModel>();

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);

            _sut = new ContactService(_mockRepo.Object, new IContactFilter[0]);

            // Act
            var result = _sut.GetContactItems(filterOptions);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(list.Count()));
            foreach (var item in result)
            {
                Assert.True(list.Any(i => i.IsEqual(item)));
            }

        }

        [Test]
        public void GetContactItem_return_result_if_ShouldFilter_returns_false()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Create<FilterViewModel>();

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            var result = _sut.GetContactItems(filterOptions);
            // Assert 
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetContactItem_verify_Filter_is_not_called_if_ShouldFilter_returns_false()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable();
            var filterOptions = _fixture.Create<FilterViewModel>();

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list.AsTable());

            foreach (var filter in _mockFilter)
            {
                filter.Setup(s => s.ShouldFilter(filterOptions)).Returns(false);
            }

            // Act
            var result = _sut.GetContactItems(filterOptions);

            // Assert 
            foreach (var filter in _mockFilter)
            {
                filter.Verify(s => s.Filter(It.IsAny<FilterViewModel>(), It.IsAny<IQueryable<ContactItem>>()),
                    Times.Never());
            }
        }

        [Test]
        public void GetContactItem_verify_if_Filter_is_called_once()
        {
            // Arrange

            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Create<FilterViewModel>();
            var testFilterList = _fixture.CreateMany<ContactItem>().AsQueryable().AsTable();

            for (var i = 0; i < _mockFilter.Length; i++)
            {
                _mockFilter[i].Setup(s => s.ShouldFilter(filterOptions)).Returns(true);
                _mockFilter[i].Setup(s => s.Filter(filterOptions, testFilterList)).Returns(testFilterList);
            }

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            var result = _sut.GetContactItems(filterOptions);

            // Assert  
            foreach (var filter in _mockFilter)
            {
                filter.Verify(s => s.Filter(It.IsAny<FilterViewModel>(), It.IsAny<IQueryable<ContactItem>>()), Times.Once());
            }
        }

        [Test]
        public void GetContactItem_verify_if_ShouldFilter_is_called_Once()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Create<FilterViewModel>();

            for (var i = 0; i < _mockFilter.Length; i++)
            {
                _mockFilter[i].Setup(s => s.ShouldFilter(filterOptions)).Returns(true);
            }

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            var result = _sut.GetContactItems(filterOptions);

            // Assert 
            foreach (var filter in _mockFilter)
            {
                filter.Verify(s => s.ShouldFilter(filterOptions), Times.Once());
                filter.Verify(s => s.ShouldFilter(It.IsAny<FilterViewModel>()), Times.Once());
            }
        }

        [Test]
        public void GetContactItem_verify_if_Repository_Contacts_is_called_exactly_twice()
        {
            // Arrange
            var list = _fixture.CreateMany<Contact>().AsQueryable().AsTable();
            var filterOptions = _fixture.Create<FilterViewModel>();

            var mockList = _mockRepo.Setup(s => s.Contacts).Returns(list);

            // Act
            var result = _sut.GetContactItems(filterOptions);

            // Assert 
            _mockRepo.Verify(s => s.Contacts, Times.Exactly(2));
        }

        // Test filters. Returned result is expected

        [Test]
        public void GetContactItem_return_filtered_list_of_contacts()
        {
            // Arrange
            var list = _fixture.CreateMany<ContactItem>().AsQueryable();
            var listForMock = _fixture.Build<Contact>().CreateMany().AsQueryable().AsTable();

            var filterOptions = _fixture.Create<FilterViewModel>();

            _mockRepo.Setup(s => s.Contacts).Returns(listForMock);
            foreach (var filter in _mockFilter.Skip(1))
            { 
                filter.Setup(s => s.ShouldFilter(filterOptions)).Returns(false);
            }
            _mockFilter[0].Setup(s => s.ShouldFilter(filterOptions)).Returns(true);
            _mockFilter[0].Setup(s => s.Filter(It.IsAny<FilterViewModel>(), It.IsAny<IQueryable<ContactItem>>()))
                .Returns(list); 

            // Act
            var result = _sut.GetContactItems(filterOptions);

            //Assert
            Assert.That(result, Is.EqualTo(list));

        }
        // Test that each filter is invoked with the returned contactList of the previous filter 

        [Test]
        public void GetContactItem_return_multiple_filtered_list_of_contacts()
        {
            // Arrange
            var list = _fixture.CreateMany<ContactItem>().AsQueryable();

            var listForMock = _fixture.CreateMany<Contact>().AsQueryable().AsTable();

            var filterOptions = _fixture.Create<FilterViewModel>();

            _mockRepo.Setup(s => s.Contacts).Returns(listForMock);

            foreach (var filter in _mockFilter)
            {
                filter.Setup(s => s.ShouldFilter(It.IsAny<FilterViewModel>())).Returns(true);
            }

            _mockFilter[0].Setup(s => s.Filter(It.IsAny<FilterViewModel>(), It.IsAny<IQueryable<ContactItem>>()))
                .Returns(list);
            _mockFilter[1].Setup(s => s.Filter(It.IsAny<FilterViewModel>(), It.IsAny<IQueryable<ContactItem>>()))
                .Returns(list.Reverse());
            _mockFilter[2].Setup(s => s.ShouldFilter(It.IsAny<FilterViewModel>()))
                .Returns(false);
             
            // Act
            var result = _sut.GetContactItems(filterOptions);

            //Assert
            Assert.That(result, Is.EqualTo(list.Reverse())); 
        } 
    }

    public static class ContactExtension
    {
        public static bool IsEqual(this Contact contact, ContactItem contactItem)
        {
            // TODO -  add nullchecks here!!!!

            return contact.Id == contactItem.Id && contact.LastName == contactItem.LastName
                && contact.Title == contactItem.Title && contact.FirstName == contactItem.FirstName;
        }
    }
}