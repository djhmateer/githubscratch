using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroOrmDemo.DataLayer.Tests {
    [TestClass]
    public class ContactRepositoryTests {
        [TestMethod]
        public void Get_all_should_return_6_results() {
            // arrange
            IContactRepository repository = CreateRepository();

            // act
            var contacts = repository.GetAll();

            // assert
            contacts.Should().NotBeNull();
            contacts.Count.Should().Be(6);
        }

        static int id;

        [TestMethod]
        public void Insert_should_assign_identity_to_new_entity() {
            // arrange
            IContactRepository repository = CreateRepository();
            var contact = new Contact {
                FirstName = "Joe",
                LastName = "Blow",
                Email = "joe.blow@gmail.com",
                Company = "Microsoft",
                Title = "Developer"
            };
            Address address = new Address {
                AddressType = "Home",
                StreetAddress = "123 Main Street",
                City = "Baltimore",
                StateId = 1,
                PostalCode = "22222"
            };
            contact.Addresses.Add(address);

            // act
            repository.Add(contact);
            //repository.Save(contact);

            // assert
            contact.Id.Should().NotBe(0, "because Identity should have been assigned by database.");
            Console.WriteLine("New ID: " + contact.Id);
            id = contact.Id;
        }

        [TestMethod]
        public void Find_should_retrieve_existing_entity() {
            // arrange
            IContactRepository repository = CreateRepository();

            // act
            var contact = repository.Find(id);
            //var contact = repository.GetFullContact(id);

            // assert
            contact.Should().NotBeNull();
            contact.Id.Should().Be(id);
            contact.FirstName.Should().Be("Joe");
            contact.LastName.Should().Be("Blow");
            contact.Email.Should().Be("joe.blow@gmail.com");
            contact.Company.Should().Be("Microsoft");
            contact.Title.Should().Be("Developer");

            //contact.Addresses.Count.Should().Be(1);
            //contact.Addresses.First().StreetAddress.Should().Be("123 Main Street");
        }

        [TestMethod]
        public void Modify_should_update_existing_entity() {
            // arrange
            IContactRepository repository = CreateRepository();

            // act
            var contact = repository.Find(id);
            //var contact = repository.GetFullContact(id);
            contact.FirstName = "Bob";
            //contact.Addresses[0].StreetAddress = "456 Main Street";
            repository.Update(contact);
            //repository.Save(contact);

            // create a new repository for verification purposes
            IContactRepository repository2 = CreateRepository();
            var modifiedContact = repository2.Find(id);
            //var modifiedContact = repository2.GetFullContact(id);

            // assert
            modifiedContact.FirstName.Should().Be("Bob");
            //modifiedContact.Addresses.First().StreetAddress.Should().Be("456 Main Street");
        }

        [TestMethod]
        public void Delete_should_remove_entity() {
            // arrange
            IContactRepository repository = CreateRepository();

            // act
            repository.Remove(id);

            // create a new repository for verification purposes
            IContactRepository repository2 = CreateRepository();
            var deletedEntity = repository2.Find(id);

            // assert
            deletedEntity.Should().BeNull();
        }

        private IContactRepository CreateRepository()
        {
            return new ContactRepository();
        }
    }
}
