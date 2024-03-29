﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    public class CommentLike
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
