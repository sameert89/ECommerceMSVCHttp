
namespace Cart.Models
{
    public class OrderModel
    {
        public List<ProductDetails> ItemsInOrder { get; set; }
        public string CreationDate { get; set; }
        public OrderModel(ProductDetails prd)
        {
            ItemsInOrder = [prd];
            CreationDate = DateTime.Now.ToString();
        }
    }
}
