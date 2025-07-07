using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers
{
    [Route("/api/stock")]
    [ApiController]  //Web API => json
    public class StockController : ControllerBase // ControllerBase is class for Rest Api

    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() //IActionResult  it's a type of result like 404 NotFound()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto=stocks.Select(c => c.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public async  Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await  _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();

            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto StockDto)
        {
            var stockModel =  StockDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stockModel }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel =await  _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null) return NotFound();
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel =await  _context.Stocks.FirstOrDefaultAsync(c => c.Id == id);
            if (stockModel == null) return NotFound();
            _context.Stocks.Remove(stockModel);
            await  _context.SaveChangesAsync();
            return NoContent();
        }
    }
}