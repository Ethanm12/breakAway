using AutoFixture;
using BreakAway.Entities;
using BreakAway.Models.Contacts;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Web.Models.Contacts;
using Web.Services;

namespace Tests
{
    // Tests for filter
    [TestFixture]
    public class FirstNameFilterTests
    {
        private FirstNameFilter _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _sut = new FirstNameFilter();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }


        [Test]
        public void Filter_throw_exception_if_Contacts_are_null()
        {
            // Arrange    
            IQueryable<ContactItem> contact = null;

            FilterViewModel firstNameModel = _fixture.Create<FilterViewModel>();

            // Act    
            // Assert 
            Assert.Throws<ArgumentNullException>(() =>
                    _sut.Filter(firstNameModel, contact));
        }

        [Test]
        public void Filter_throw_exception_if_FilterViewModel_is_null()
        {
            // Arrange    
            var contact = _fixture.Build<ContactItem>().CreateMany().AsQueryable();

            FilterViewModel firstNameModel = null;

            // Act    
            // Assert 
            Assert.Throws<ArgumentNullException>(() =>
                    _sut.Filter(firstNameModel, contact));
        }

        [Test]
        public void Filter_return_filtered_list_of_contactItems()
        {
            // Arrange
            var random = new Random();
            var count = random.Next(3, 10);

            var firstNameModel = _fixture.Create<FilterViewModel>();

            var notMatchingContacts = _fixture.CreateMany<ContactItem>(count);
            var machingContacts = _fixture.Build<ContactItem>()
                .With(i => i.FirstName,
                    $"{_fixture.Create<string>()}{firstNameModel.FilterFirstName}{_fixture.Create<string>()}")
                .CreateMany(count);

            var contact = notMatchingContacts.Concat(machingContacts).AsQueryable();

            // Act  
            var result = _sut.Filter(firstNameModel, contact);

            // Assert
            Assert.That(result.ToArray(), Is.EqualTo(machingContacts));
        }

        [Test]
        public void Filter_return_ContactItem_with_empty_title_attribute()
        {
            // Arrange    
            var contact = _fixture.Build<ContactItem>()
                .Without(s => s.FirstName)
                .CreateMany().AsQueryable();

            FilterViewModel firstNameModel = _fixture.Create<FilterViewModel>();

            // Act   
            var result = _sut.Filter(firstNameModel, contact);

            // Assert 
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ShouldFilter_return_true_if_FilterFirstName_is_not_null()
        {
            // Arrange   
            var firstNameModel = _fixture.Create<FilterViewModel>();

            // Act  
            var result = _sut.ShouldFilter(firstNameModel);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldFilter_return_false_if_FilterFirstName_is_null()
        {
            // Arrange  
            var firstNameModel = _fixture.Build<FilterViewModel>().Without(s => s.FilterFirstName).Create();

            // Act  
            var result = _sut.ShouldFilter(firstNameModel);

            // Assert 
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldFilter_return_false_if_FilterFirstName_is_empty_string()
        {
            var firstNameModel = _fixture.Build<FilterViewModel>().With(s => s.FilterFirstName, string.Empty).Create();
            // Act 

            var result = _sut.ShouldFilter(firstNameModel);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldFilter_return_exception_if_FilterViewModel_is_null()
        {
            // Arrange
            FilterViewModel firstNameModel = null;

            // Act 
            var result = _sut.ShouldFilter(firstNameModel);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}