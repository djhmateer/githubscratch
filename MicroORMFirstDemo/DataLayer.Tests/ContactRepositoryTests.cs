using FluentAssertions;
using NUnit.Framework;
using System;

namespace MicroOrmDemo.DataLayer.Tests {
    [TestFixture]
    public class ContactRepositoryTests {
        // Integration tests run in alphabetical order (nunit default)

        [Test]
        public void _01_Get_all_should_return_6_results() {
            var repository = new ContactRepository();

            var contacts = repository.GetAll();

            contacts.Should().NotBeNull();
            contacts.Count.Should().Be(6);
        }

        static int id;

        [Test]
        public void _02_Insert_should_assign_identity_to_new_entity() {
            // arrange
            var repository = new ContactRepository();

            var contact = new Contact {
                FirstName = "Joe",
                LastName = "Blow",
                Email = "joe.blow@gmail.com",
                Company = "Microsoft",
                Title = "Developer"
            };
            //Address address = new Address {
            //    AddressType = "Home",
            //    StreetAddress = "123 Main Street",
            //    City = "Baltimore",
            //    StateId = 1,
            //    PostalCode = "22222"
            //};
            //contact.Addresses.Add(address);

            // act
            repository.Insert(contact);
            //repository.Save(contact);

            // assert
            contact.Id.Should().NotBe(0, "because Identity should have been assigned by database.");
            Console.WriteLine("New ID: " + contact.Id);
            id = contact.Id;
        }

        [Test]
        public void _03_Find_should_retrieve_existing_entity() {
            // arrange
            var repository = new ContactRepository();

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

        [Test]
        public void _04_Modify_should_update_existing_entity() {

            var repository = new ContactRepository();

            // act
            var contact = repository.Find(id);
            //var contact = repository.GetFullContact(id);
            contact.FirstName = "Bob";
            //contact.Addresses[0].StreetAddress = "456 Main Street";
            repository.Update(contact);
            //repository.Save(contact);

            // create a new repository for verification purposes
            var repository2 = new ContactRepository();

            var modifiedContact = repository2.Find(id);
            //var modifiedContact = repository2.GetFullContact(id);

            // assert
            modifiedContact.FirstName.Should().Be("Bob");
            //modifiedContact.Addresses.First().StreetAddress.Should().Be("456 Main Street");
        }

        [Test]
        public void _05_Delete_should_remove_entity() {

            var repository = new ContactRepository();

            repository.Delete(id);

            // create a new repository for verification purposes
            var repository2 = new ContactRepository();

            var deletedEntity = repository2.Find(id);

            // assert
            deletedEntity.Should().BeNull();
        }

        // FK Tables
    }
}
