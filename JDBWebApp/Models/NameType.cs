namespace JDBWebApp.Models
{
    public class NameType
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public NameType() { }
        public NameType(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
