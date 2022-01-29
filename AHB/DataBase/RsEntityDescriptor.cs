namespace AHB.DataBase
{
    public class RsEntityDescriptor
    {
        public RsEntityDescriptor()
        {
            Fields = new List<RsEntityFieldDescriptor>();
        }

        public string EntityName { get; set; }
        public ICollection<RsEntityFieldDescriptor> Fields { get; set; }
    }
}