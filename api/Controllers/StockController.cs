using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Mappers;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.EntityFrameworkCore;


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
        public async Task<IActionResult> GetAll()
        {
            var stock =  await _context.Stocks.ToListAsync();
            var stockDto = stock.Select(s => s.ToStockDto()).ToList();
            return Ok(stock);
        }

        // Get stock by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stock = stockDto.ToStockFromCreateDTO();
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),new {id = stock.Id},stock.ToStockDto());
        }

         [HttpPut]
         [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x=> x.Id == id);
            
            if(stockModel == null){
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;


            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route ("{id}")]
        public async Task<IActionResult> Delete ([FromRoute] int id){
            var stockModel =await _context.Stocks.FirstOrDefaultAsync(x=>x.Id == id);

            if(stockModel == null){
                return NotFound();
            }

            _context.Stocks.Remove(stockModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
