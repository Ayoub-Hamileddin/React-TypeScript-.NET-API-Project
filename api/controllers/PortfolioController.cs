using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extension;
using api.Interfaces;
using api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IUserPortfolio _userPortfolio;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo,IUserPortfolio userPortfolio)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _userPortfolio = userPortfolio;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _userPortfolio.GetUserPortfolio(user);
            return Ok(userPortfolio);
        }
    }
}