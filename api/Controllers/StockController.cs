using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Mappers;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.EntityFrameworkCore;
using api.interfaces;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context,IStockRepository stockRepository)
        {
            _stockRepo = stockRepository;
            _context = context;
        }

        // Get all stocks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stock =  await _stockRepo.GetALLAsync();
            var stockDto = stock.Select(s => s.ToStockDto());
            return Ok(stock);
        }

        // Get stock by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById),new {id = stockModel.Id},stockModel.ToStockDto());
        }

         [HttpPut]
         [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepo.UpdateAsync(id,updateDto);
            
            if(stockModel == null){
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route ("{id}")]
        public async Task<IActionResult> Delete ([FromRoute] int id){
            var stockModel =await _stockRepo.DeleteAsync(id);

            if(stockModel == null){
                return NotFound();
            }

            return NoContent();
        }
    }
}
