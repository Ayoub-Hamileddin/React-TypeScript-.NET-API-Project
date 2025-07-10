using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.models;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [Route("/api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _commentRepo = commentRepo;

        }


        [HttpGet]
        public async Task<IActionResult> GetAllCommentAsync()
        {
            var comments = await _commentRepo.GetAllCommentAsync();
            var commentDto = comments.Select(c => c.ToCommentDto());
            return Ok(commentDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdASync([FromRoute] int id)
        {
            var commet = await _commentRepo.GetByIdAsync(id);
            if (commet == null)
            {
                return NotFound();
            }
            return Ok(commet.ToCommentDto());
        }


        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            if (!await _stockRepo.ExcitingStock(stockId))
            {
                return BadRequest();
            }
            var commentModel = commentDto.ToCommentFromCreateDto(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return Ok(commentModel.ToCommentDto());
        }


        [HttpPut]
        [Route("{commentId}")]
        public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentDto commentDto)
        {
            var dto = commentDto.ToCommentFromUpdateDto();
            var comment = await _commentRepo.UpdateAsync(commentId, dto);
            if (comment == null) return NotFound();
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{commentId}")]
        public async Task<IActionResult> Delete([FromRoute] int commentId)
        {
            var comment = await _commentRepo.DeleteAsync(commentId);
            if (comment == null) return NotFound();
            return NoContent();
        }
    }
}