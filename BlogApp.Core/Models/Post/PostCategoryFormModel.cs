﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    public class PostCategoryFormModel : CategoryViewModel
    {
        public bool IsSelected { get; set; }
    }
}
