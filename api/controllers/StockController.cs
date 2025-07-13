using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers
{
    [Route("/api/stock")]
    [ApiController]  //Web API => json
    public class StockController : ControllerBase // ControllerBase is class for Rest Api

    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObject query) //IActionResult  it's a type of result like 404 NotFound()
        {
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(c => c.ToStockDto());
            return Ok(stocks);
        }


        [HttpGet("{id}")]
        public async  Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await  _stockRepo.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();

            }
            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto StockDto)
        {
             if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stockModel = StockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
             return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {   
             if(!ModelState.IsValid){
                return BadRequest(ModelState);
               }

           var stockModel= await _stockRepo.UpdateAsync(id, updateDto);
            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }

        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel =await  _stockRepo.DeleteAsync(id);
            if (stockModel == null) return NotFound();
            return NoContent();
        }
    }
}