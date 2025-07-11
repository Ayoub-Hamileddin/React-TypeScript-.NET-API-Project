using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage="le text du text est obligatoire")]
        [MinLength(5,ErrorMessage="le minumum est 5 lettre")]
        [MaxLength(200,ErrorMessage="le maximum est 5 lettre")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage="le text du text est obligatoire")]
        [MinLength(5,ErrorMessage="le minumum est 5 lettre")]
        [MaxLength(200,ErrorMessage="le maximum est 5 lettre")]
        public string Content { get; set; } = string.Empty;
    }
}