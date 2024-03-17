using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    public class PostTagFormModel : TagViewModel
    {
        public bool IsSelected { get; set; }
    }
}
