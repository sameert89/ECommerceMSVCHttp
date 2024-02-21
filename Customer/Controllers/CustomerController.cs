using Microsoft.AspNetCore.Mvc;
using Customer.Models;
using System.Text.Json;

namespace Customer.Controllers
{
    [ApiController]
    [Route("customer-api/v1/")]
    public class CustomerController : Controller
    {
        private static readonly CustomerModel _dummyCustomer = new();
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _httpClient = new HttpClient(clientHandler);
            _configuration = configuration;
        }
        [HttpGet]
        public ActionResult<CustomerModel> GetCustomerModel()
        {
            return _dummyCustomer;
        }
        [HttpPost]
        public void BuyProduct(int id, int qty)
        {
            var cartURI = _configuration.GetValue<string>("NeighborURIs:CartService");
            var catalogURI = _configuration.GetValue<string>("NeighborURIs:CatalogService");
            ProductModel? product = null;
            // Get the product from the Catalog
            try
            {
                HttpResponseMessage catalogResponse = _httpClient.GetAsync($"{catalogURI}/{id}").Result;

                catalogResponse.EnsureSuccessStatusCode();

                string JsonString = catalogResponse.Content.ReadAsStringAsync().Result;
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                product = JsonSerializer.Deserialize<ProductModel>(JsonString, options);
                if(product == null) {
                    throw new Exception("Unable to read product from Catalog");
                }
                Console.WriteLine(product.ToString());
                // Create a View for the Cart MSVC
                var purchasedProduct = new ProductDetails(product, qty, "0");

                Console.WriteLine(JsonSerializer.Serialize(purchasedProduct));

                // Make POST call to the cart to add items
                HttpResponseMessage cartResponse = _httpClient.PostAsJsonAsync(cartURI, purchasedProduct).Result;

                if (!cartResponse.IsSuccessStatusCode)
                {
                    var errorContent = cartResponse.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Error: {errorContent}");
                }

                cartResponse.EnsureSuccessStatusCode();

                cartResponse.Content.ReadAsStringAsync().ContinueWith(readTask =>
                {
                    Console.WriteLine($"Item Added to Order ID: {readTask.Result}");
                });

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error: ", e.Message);
            }
        }
    }
}
