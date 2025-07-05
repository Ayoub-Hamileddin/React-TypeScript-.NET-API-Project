using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll() //IActionResult  it's a type of result like 404 NotFound()
        {
            var stocks = _context.Stocks.ToList();
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();

            }
            return Ok(stock);
        }
    }
}