using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Models.Post
{
    public class AddPostFormModel
    {
        [Required(ErrorMessage = RequiredError)]
        [StringLength(PostTitleMax, MinimumLength = PostTitleMin, ErrorMessage = InputError)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(PostContentMax, MinimumLength = PostContentMin, ErrorMessage = InputError)]
        public string Content { get; set; } = string.Empty;

        //if the short description isn't provided
        //just use the first 3-4 sentences
        //TODO: thing of a better idea
        [StringLength(PostShortDescriptionMax, MinimumLength = PostShortDescriptionMin, ErrorMessage = InputError)]
        public string ShortDescription { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public List<PostCategoryModel> Categories {  get; set; } = new List<PostCategoryModel>();

        public List<PostTagModel>? Tags { get; set; } = new List<PostTagModel>();
    }
}
