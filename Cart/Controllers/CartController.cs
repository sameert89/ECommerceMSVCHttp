using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cart.Models;

namespace Cart.Controllers
{
    [ApiController]
    [Route("cart-api/v1/")]
    public class CartController : ControllerBase
    {
        private static Dictionary<string, OrderModel> orders;
        private int lastOrderId;
        public CartController()
        {
            orders = [];
            lastOrderId = 0;
        }

        [HttpGet("{id}")]
        public ActionResult<OrderModel> GetOrder(string id)
        {
            if (orders.TryGetValue(id, out OrderModel order))
            {
                Console.Write(orders.Count);
                return Ok(order);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public ActionResult<string> Add([FromBody] ProductDetails request)
        {
            Console.WriteLine("request recieved");
            Console.WriteLine(request);
            string orderID = request.OrderId;

            if (orders.ContainsKey(orderID))
            {
                orders[orderID].ItemsInOrder.Add(request);
            }
            else
            {
                var newOrderId = (++lastOrderId).ToString();
                orders[newOrderId] = new OrderModel(request);
            }

            return Ok(orderID);
        }
    }
}
