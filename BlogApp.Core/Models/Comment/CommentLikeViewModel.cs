using BlogApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Comment
{
    public class CommentLikeViewModel
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string UserId { get; set; }
    }
}
