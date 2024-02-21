using Catalog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("catalog-api/v1/")]
    public class CatalogController : Controller
    {
        private static readonly List<ProductModel> _products = new List<ProductModel>
        {
            new ProductModel(1, "Laptop", "A high performance laptop", 1200.00m),
            new ProductModel(2, "Smartphone", "An advanced smartphone", 800.00m),
            new ProductModel(3, "Tablet", "A user-friendly tablet for all your needs", 600.00m),
        };

        [HttpGet]
        public ActionResult<List<ProductModel>> GetAllProducts()
        {
            return _products;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductModel> GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
    }
}
