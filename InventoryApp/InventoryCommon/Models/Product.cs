namespace InventoryCommon.Models
{
    public class Product
    {
        public Guid ID { get;  set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public uint Quantity { get; set; }


    }
}
