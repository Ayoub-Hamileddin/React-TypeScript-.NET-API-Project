using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
       public StockRepository(ApplicationDBContext context)
       {
        _context = context;
       }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(c => c.Id == id);
            if (stockModel == null) return null;
            _context.Stocks.Remove(stockModel);
            await  _context.SaveChangesAsync();
            return stockModel ;
        }

        public Task<List<Stock>> GetAllAsync()
        {
          return  _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
           return await  _context.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var exsitingStock = await _context.Stocks.FirstOrDefaultAsync(c => c.Id == id);
            if (exsitingStock == null) return null;
            exsitingStock.Symbol = stockDto.Symbol;
            exsitingStock.CompanyName = stockDto.CompanyName;
            exsitingStock.Purchase = stockDto.Purchase;
            exsitingStock.Industry = stockDto.Industry;
            exsitingStock.MarketCap = stockDto.MarketCap;
            await _context.SaveChangesAsync();
            return exsitingStock;
        }
    }
}