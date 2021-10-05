using AutoFixture;
using BreakAway.Models.Contacts;
using NUnit.Framework;
using System;
using System.Linq;
using Web.Models.Contacts;
using Web.Services;

namespace Tests
{
    [TestFixture]
    public class LastNameFilterTests
    {
        private LastNameFilter _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _sut = new LastNameFilter();
            _fixture = new Fixture();
        }

        [Test]
        public void Filter_return_exception_if_Contacts_are_null()
        {
            // Arrange    
            IQueryable<ContactItem> contact = null;

            FilterViewModel lastNameModel = _fixture.Create<FilterViewModel>();

            // Act    
            // Assert 
            Assert.Throws<ArgumentNullException>(() =>
                    _sut.Filter(lastNameModel, contact));
        }

        [Test]
        public void Filter_return_exception_if_FilterViewModel_is_null()
        {
            // Arrange    
            var contact = _fixture.Build<ContactItem>().CreateMany().AsQueryable();

            FilterViewModel lastNameModel = null;

            // Act    
            // Assert 
            Assert.Throws<ArgumentNullException>(() =>
                    _sut.Filter(lastNameModel, contact));
        }

        [Test]
        public void Filter_return_filtered_list_of_contactItems()
        {
            // Arrange
            var random = new Random();
            var count = random.Next(3, 10);

            var lastNameModel = _fixture.Create<FilterViewModel>();

            var notMatchingContacts = _fixture.CreateMany<ContactItem>(count);
            var matchingContacts = _fixture.Build<ContactItem>()
                .With(i => i.LastName,
                    $"{_fixture.Create<string>()}{lastNameModel.FilterLastName}{_fixture.Create<string>()}")
                .CreateMany(count);

            var contact = notMatchingContacts.Concat(matchingContacts).AsQueryable();

            // Act  
            var result = _sut.Filter(lastNameModel, contact);

            // Assert
            Assert.That(result.ToArray(), Is.EqualTo(matchingContacts));
        }

        [Test]
        public void Filter_return_ContactItem_with_empty_title_attribute()
        {
            // Arrange    
            var contact = _fixture.Build<ContactItem>()
                .Without(s => s.LastName)
                .CreateMany().AsQueryable();

            FilterViewModel lastNameModel = _fixture.Create<FilterViewModel>();

            // Act   
            var result = _sut.Filter(lastNameModel, contact);

            // Assert 
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ShouldFilter_return_true_if_FilterLastName_is_not_null()
        {
            // Arrange   
            var lastNameModel = _fixture.Create<FilterViewModel>();

            // Act  
            var result = _sut.ShouldFilter(lastNameModel);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldFilter_return_false_if_FilterLastName_is_null()
        {
            // Arrange  
            var lastNameModel = _fixture.Build<FilterViewModel>().Without(s => s.FilterLastName).Create();

            // Act  
            var result = _sut.ShouldFilter(lastNameModel);

            // Assert 
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldFilter_return_false_if_FilterLastName_is_empty_string()
        {
            var lastNameModel = _fixture.Build<FilterViewModel>().With(s => s.FilterLastName, string.Empty).Create();
            // Act 

            var result = _sut.ShouldFilter(lastNameModel);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldFilter_return_exception_if_FilterViewModel_is_null()
        {
            // Arrange
            FilterViewModel lastNameModel = null;

            // Act 
            var result = _sut.ShouldFilter(lastNameModel);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}