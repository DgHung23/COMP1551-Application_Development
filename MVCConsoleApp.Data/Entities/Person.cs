namespace MVCConsoleApp.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public int PersonRoleId { get; set; }

        // Filled by joins when listing; not stored in People.
        public string? PersonRoleName { get; set; }

        public void Display(Person person)
        {
            Console.WriteLine($"Id: {person.Id}, FullName: {person.FullName}, Telephone: {person.Telephone}, Email: {person.Email}, Role: {person.PersonRoleName}");
        }
    }
}
