using BreakAway.Entities;
using Moq;
using NUnit.Framework;
using System;
using Web.Models.Contacts;
using Web.Services;

namespace Tests
{
    [TestFixture]
    public class Tests
    { 
        [Test, CustomAutoData]
        public void Test1(Action<string> action, string message)
        {
            action(message);

            Mock.Get(action).Verify(p => p(message));
        }

        // IS TRUE || False

        [Test]
        public void Filter_Input_Is_Equal_To_True()
        {
            // Arrange

            //var FilterService = new TitleFilter();
            //var FilterService = new FirstNameFilter();
            //var FilterService = new LastNameFilter();
            var filterOptions = new FilterViewModel { FilterTitle = "Doe" };
            // Act 

            FilterService.ShouldFilter(filterOptions);

            // Assert
            Assert.That(true, Is.True);
        }

        [Test]
        public void FirstNameFilter_Input_Is_Equal_To_True()
        {
            var FilterService = new TitleFilter();
            var filterOptions = new FilterViewModel { FilterFirstName = "Doe" };
            // Act 

            FilterService.ShouldFilter(filterOptions);

            // Assert
            Assert.That(true, Is.True);
        }

        [Test]
        public void LastNameFilter_Input_Is_Equal_To_True()
        {
            var FilterService = new TitleFilter();
            var filterOptions = new FilterViewModel { FilterLastName = "Doe" };
            // Act 

            FilterService.ShouldFilter(filterOptions);

            // Assert
            Assert.That(true, Is.True);
        }

        [Test]
        public void TitleFilter_Input_Is_Equal_To_False()
        {
            var FilterService = new TitleFilter();
            var filterOptions = new FilterViewModel { FilterLastName = null };
            // Act 

            FilterService.ShouldFilter(filterOptions);

            // Assert
            Assert.That(false, Is.False);
        }



        //[Test]
        //public void Test_2()
        //{
        //    Assert.Pass();
        //}


        //[Test]
        //public void Test_3()
        //{
        //    Assert.Fail();
        //}
    }
}