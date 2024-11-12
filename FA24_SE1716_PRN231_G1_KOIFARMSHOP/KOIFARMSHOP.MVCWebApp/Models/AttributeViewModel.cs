namespace KOIFARMSHOP.MVCWebApp.Models
{
    public class AttributeViewModel
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string DisplayName { get; set; }
        public bool IsComparable { get; set; }
        public bool IsDeletable { get; set; }
    }

}
