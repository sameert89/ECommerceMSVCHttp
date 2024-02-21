namespace Cart.Models
{
    public class ProductDetails
    {
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public ProductDetails() { }
        public ProductDetails(ProductModel prd, int qty, string oid)
        {
            Product = prd;
            Quantity = qty;
            OrderId = oid;
        }
    }
}
