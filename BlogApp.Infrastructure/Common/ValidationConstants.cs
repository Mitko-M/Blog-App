namespace BlogApp.Infrastructure.Common
{
    public static class ValidationConstants
    {
        //change the values if needed later

        //identity constants
        public const int FirstNameMax = 50;
        public const int LastNameMax = 50;

        public const int FirstNameMin = 3;
        public const int LastNameMin = 3;

        public const int UserNameMax = 20; 
        public const int UserNameMin = 5;

        public const int EmailMax = 60;
        public const int EmailMin = 10;

        public const int PassMax = 20;
        public const int PassMin = 5;

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
        public const int PostContentMax = 5000;

        //comment
        public const int CommentContentMin = 1;
        public const int CommentContentMax = 100;

        //date format
        public const string PostDateFormat = "dd MMM yyyy";

        //required field
        public const string RequiredError = "The {0} field is required";

        //min and max value for input fields
        public const string InputError = "The {0} field should be between {2} and {1} characters";
    }
}
