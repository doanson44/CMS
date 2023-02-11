namespace CMS.Core.Helpers
{
    public static class CurrencyHelper
    {
        public static string CurrencyFormatVnd(double currency)
        {
            return string.Format("{0:0,0đ}", currency);
        }
    }
}
