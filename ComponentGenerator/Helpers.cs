namespace ComponentGenerator
{
    internal static class Helpers
    {

        internal static string CapitalizeFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }
        internal static string ToSnakeCase(string source)
        {
            return source.Replace('.', '_');
        }

        internal static string GetLifeTimeSyntax(string lifetime)
        {
            switch (lifetime)
            {
                case "0":
                    return "Singleton";
                case "1":
                    return "Transient";
                case "2":
                    return "Scoped";
                default:
                    return string.Empty;
            }
        }
    }
}