namespace CMS.Core.Constants
{
    public static class CssStyleConstants
    {
        public const string Info = "info";
        public const string Success = "success";
        public const string Warning = "warning";
        public const string Danger = "danger";
        public const string Primary = "primary";

        public static string ToCssStyle(this int input)
        {
            var style = Primary;
            var value = input % 5;
            switch (value)
            {
                case 1:
                    style = Danger;
                    break;
                case 2:
                    style = Warning;
                    break;
                case 3:
                    style = Info;
                    break;
                case 4:
                    style = Success;
                    break;
                default:
                    style = Primary;
                    break;
            }

            return style;
        }
    }
}
