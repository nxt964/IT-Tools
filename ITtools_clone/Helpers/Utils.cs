namespace ITtools_clone.Helpers
{
    public class Utils
    {
        public static string Slugify(string input)
        {
            return input.ToLower().Replace(" ", "-");
        }
        public static string Unslugify(string input)
        {
            return string.Join(" ", input.Split('-')
                .Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }

    }
}
