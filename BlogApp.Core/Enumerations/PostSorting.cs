using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Enumerations
{
    public enum PostSorting
    {
        [Description("None")]
        None = 0,
        [Description("Newest")]
        Newest = 1,
        [Description("Oldest")]
        Oldest = 2,
        [Description("Title Ascending")]
        TitleAscending = 3,
        [Description("Title Descending")]
        TitleDescending = 4
    }
}
