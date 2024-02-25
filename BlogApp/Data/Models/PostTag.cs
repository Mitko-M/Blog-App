﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Data.Models
{
    public class PostTag
    {
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        public int TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; } = null!;
    }
}
