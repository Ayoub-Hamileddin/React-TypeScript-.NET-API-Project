using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment stockModel)
        {
            await _context.Comments.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Comment?> DeleteAsync(int commentId)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (commentModel == null) return null;
            _context.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllCommentAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var excitingComment = await _context.Comments.FindAsync(id);
            if (excitingComment == null) return null;
            excitingComment.Content = comment.Content;
            excitingComment.Title = comment.Title;
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}