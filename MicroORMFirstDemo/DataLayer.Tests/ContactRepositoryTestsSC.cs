using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace MicroOrmDemo.DataLayer.Tests {
    [TestFixture]
    public class ContactRepositoryTestsSC{
        ContactRepositorySC repo;

        [SetUp]
        public void Setup(){
            repo = new ContactRepositorySC();
        }

        [Test]
        public void _005_Get_a_single_should_return_only_1() {
            var contact = repo.Get(1);

            contact.Should().NotBeNull();
            Assert.AreEqual("Michael", contact.FirstName);
        }

        [Test]
        public void _01_Get_list_should_return_6_results() {

            var contacts = repo.GetList().ToList();

            contacts.Should().NotBeNull();
            contacts.Count().Should().Be(6);
        }

        static int id;

        [Test]
        public void _02_Insert_should_assign_identity_to_new_entity() {

            var contact = new Contact {
                FirstName = "Joe",
                LastName = "Blow",
                Email = "joe.blow@gmail.com",
                Company = "Microsoft",
                Title = "Developer"
            };

            contact = repo.Insert(contact);
            
            contact.Id.Should().NotBe(0, "because Identity should have been assigned by database.");
            Console.WriteLine("New ID: " + contact.Id);
            id = contact.Id;
        }

        [Test]
        public void _03_Find_should_retrieve_existing_entity() {

            // act
            var contact = repo.Find(id);
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
            // act
            var contact = repo.Find(id);
            //var contact = repository.GetFullContact(id);
            contact.FirstName = "Bob";
            //contact.Addresses[0].StreetAddress = "456 Main Street";
            repo.Update(contact);
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
            repo.Delete(id);

            // create a new repository for verification purposes
            var repository2 = new ContactRepository();

            var deletedEntity = repository2.Find(id);

            // assert
            deletedEntity.Should().BeNull();
        }
    }
}
