namespace InventoryCommon.Models
{
     class Product
    {
        Guid ID { get; set; }
        string Name { get; set; }
        string SKU { get; set; }
        decimal Price { get; set; }
        string Category { get; set; }
        uint  Quantity { get; set; }


    }
}
