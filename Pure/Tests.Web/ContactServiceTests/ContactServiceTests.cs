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

        //[OneTimeSetUp]
        //public void OneTimeSetup()
        //{
        //    _fixture = new Fixture();
        //    _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        //        .ForEach(b => _fixture.Behaviors.Remove(b));
        //    _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        //}

        [SetUp]
        public void SetUp()
        { 
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var contact = _fixture.CreateMany<Contact>(); 

            _mockRepo = new Mock<IRepository>();
            _mockRepo.Setup(s => s.Contacts);

            _mockFilter = new Mock<IContactFilter>[] { new Mock<IContactFilter>(), new Mock<IContactFilter>(), new Mock<IContactFilter>() };
            _sut = new ContactService(_mockRepo.Object, _mockFilter.Select(f=>f.Object).ToArray());
        }

        [Test]
        public void GetContactItems_throw_error_when_contacts_is_null()
        {

            
             
         //_mockRepo.Setup(s => s.Addresses).Returns(s => s._fixture.Build<Address>());
   
        }

        // Method expects from FilterViewModel being null
        // Method Expects when some of ContactItem is missing
        // Method Expects if we return a different type from repository with and without filtering 

        //Method when just calling getModel(), make it null 
        // Method return what we expect
        // Was the DataBase called
        //Was filter calleda

    }
}