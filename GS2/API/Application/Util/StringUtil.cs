namespace API.Application.Util
{
    public class StringUtil
    {
        public static bool boolean(String str)
        {
            if (str == "s" || str == "S")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
