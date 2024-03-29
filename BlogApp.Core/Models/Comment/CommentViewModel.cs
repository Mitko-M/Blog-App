﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Comment
{
    public class CommentViewModel
    {
        public string Content { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PostOwnerId { get; set; } = string.Empty;
        public DateTime CommentUploadDate { get; set; }
        public IEnumerable<CommentLikeViewModel> CommentsLikes { get; set; } = new List<CommentLikeViewModel>();
    }
}
