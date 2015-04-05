using Dapper;

namespace MicroOrmDemo.DataLayer {
    // used for SimpleCRUD
    [Table("Contacts")]
    public class Contact {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }

        //Additional properties not in database
        //[Editable(false)]
        //public bool IsNew {
        //    get {
        //        return this.Id == default(int);
        //    }
        //}
    }
}
