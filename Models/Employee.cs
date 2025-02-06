using SQLite;

namespace SQLite_ORM_Test
{

    public class Employee
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NT_ID { get; set; }
        public string? Password { get; set; }
    }
}
