namespace MVCConsoleApp.Data.Entities
{
    public class PersonRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public void Display(PersonRole role)
        {
            Console.WriteLine($"Id: {role.Id}, Name: {role.Name}, Description: {role.Description}");
        }
    }
}
