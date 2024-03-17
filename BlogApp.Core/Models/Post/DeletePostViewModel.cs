using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    public class DeletePostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatedOn { get; set; }
    }
}
