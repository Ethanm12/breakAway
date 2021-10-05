using AutoFixture;
using BreakAway.Models.Contacts;
using NUnit.Framework;
using System;
using System.Linq;
using Web.Models.Contacts;
using Web.Services;

namespace Tests
{
    // Tests for filter
    [TestFixture]
    public class TitleFilterTests
    { 
        private TitleFilter _sut;
        private Fixture _fixture; 

        [SetUp]
        public void SetUp()
        { 
            _sut = new TitleFilter();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        // Filter Methods 
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

            FilterViewModel titleModel = null;
             
            // Act
            // Assert 
            Assert.Throws<ArgumentNullException>(() =>
                    _sut.Filter(titleModel, contact));
        }

        [Test]
        public void Filter_return_filtered_list_of_contactItems()
        {
            // Arrange
            var random = new Random();
            var count = random.Next(3, 10);

            var titleModel = _fixture.Create<FilterViewModel>();

            var notMatchingContacts = _fixture.CreateMany<ContactItem>(count);
            var machingContacts = _fixture.Build<ContactItem>()
                .With(i => i.Title,
                    $"{_fixture.Create<string>()}{titleModel.FilterTitle}{_fixture.Create<string>()}")
                .CreateMany(count);

            var contact = notMatchingContacts.Concat(machingContacts).AsQueryable();
 
            // Act  
            var result = _sut.Filter(titleModel, contact);

            // Assert
            Assert.That(result.ToArray(), Is.EqualTo(machingContacts));
        }

        [Test]
        public void Filter_does_not_return_ContactItem_with_empty_title_attribute()
        {
            // Arrange    
            var contact = _fixture.Build<ContactItem>()
                .Without(s => s.Title)
                .CreateMany().AsQueryable();

            FilterViewModel titleModel = _fixture.Create<FilterViewModel>();

            // Act   
            var result =  _sut.Filter(titleModel, contact);

            // Assert 
            Assert.That(result, Is.Empty);
        }

        //Bool Methods
        [Test] 
        public void ShouldFilter_return_true_if_FilterTitle_is_not_null()
        {
            // Arrange   
            var titleModel = _fixture.Create<FilterViewModel>();

            // Act  
            var result = _sut.ShouldFilter(titleModel); 

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldFilter_return_false_if_FilterTitle_is_null()
        {
            // Arrange  
            var titleModel = _fixture.Build<FilterViewModel>().Without(s => s.FilterTitle).Create();

            // Act  
            var result = _sut.ShouldFilter(titleModel);
             
            // Assert 
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void ShouldFilter_return_false_if_FilterTitle_is_empty_string()
        {
            var titleModel = _fixture.Build<FilterViewModel>().With(s => s.FilterTitle, string.Empty).Create();
            // Act 

            var result = _sut.ShouldFilter(titleModel);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldFilter_return_exception_if_FilterViewModel_is_null()
        {
            // Arrange
            FilterViewModel titleModel = null;

            // Act 
            var result = _sut.ShouldFilter(titleModel);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}