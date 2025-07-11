using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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

        public async Task<bool> ExcitingStock(int id)
        {
            return await _context.Stocks.AnyAsync(c => c.Id == id);
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
          var stocks= _context.Stocks.Include(c=>c.Comments).AsQueryable();
          // 🔍 Filtrage
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(c => c.Symbol.Contains(query.Symbol));
            }

            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(c => c.CompanyName.Contains(query.CompanyName));
            }
             // 🔃 Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(c => c.Symbol) : stocks.OrderBy(c => c.Symbol);
                }
            }
            var Skipage = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(Skipage).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
           return await  _context.Stocks.Include(c=>c.Comments).FirstOrDefaultAsync(c => c.Id == id);
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