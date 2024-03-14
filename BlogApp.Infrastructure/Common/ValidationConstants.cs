﻿namespace BlogApp.Infrastructure.Common
{
    public static class ValidationConstants
    {
        //change the values if needed later

        //category
        public const int CategoryNameMin = 3;
        public const int CategoryNameMax = 40;

        //tag
        public const int TagNameMin = 3;
        public const int TagNameMax= 30;

        //post
        public const int PostTitleMin = 5;
        public const int PostTitleMax = 50;

        public const int PostShortDescriptionMin = 10;
        public const int PostShortDescriptionMax = 150;

        public const int PostContentMin = 10;
        public const int PostContentMax = 500;

        //comment
        public const int CommentContentMin = 1;
        public const int CommentContentMax = 100;

        //date format
        public const string DateFormat = "dd MMM yyyy";

        //required field
        public const string RequiredError = "The {0} field is required";

        //min and max value for input fields
        public const string InputError = "The {0} field should be between {2} and {1} characters";
    }
}