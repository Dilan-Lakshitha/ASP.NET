using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Mappers;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get all stocks
        [HttpGet]
        public IActionResult GetAll()
        {
            var stock = _context.Stocks.ToList().Select(s => s.ToStockDto()).ToList();
            return Ok(stock);
        }

        // Get stock by ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }
    }
}
