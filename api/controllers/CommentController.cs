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
        public CommentController(ICommentRepository commentRepo)
        {
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
    }
}