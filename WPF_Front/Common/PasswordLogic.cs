namespace WPF_Front.Common
{
    internal static class PasswordLogic
    {
        private static int step => 15;

        /// <summary>
        /// Transforme string with basic encryption
        /// </summary>
        /// <param name="StringToEncode"></param>
        /// <returns></returns>
        public static string Encode(string StringToEncode)
        {
            string encodedString = string.Empty;
            foreach (char achar in StringToEncode)
            {
                var caractInt = Convert.ToInt64(achar);
                encodedString += Convert.ToChar(caractInt + step);
            }

            byte[] toEncodeAsBytes = Encoding.UTF32.GetBytes(Invert(encodedString));
            string base64String = Convert.ToBase64String(toEncodeAsBytes);

            return base64String;
        }

        /// <summary>
        /// Decode string
        /// </summary>
        /// <param name="encodedLine"></param>
        /// <returns></returns>
        public static string Decode(string encodedString)
        {
            byte[] encodedDataAsBytes = null;
            if (string.IsNullOrWhiteSpace(encodedString)) return null;
            try
            {
                encodedDataAsBytes = Convert.FromBase64String(encodedString);
            }
            catch (FormatException)
            {
                return null;
            }
            var decodedString = Invert(Encoding.UTF32.GetString(encodedDataAsBytes));

            string decodedBase64String = string.Empty;
            foreach (char achar in decodedString)
            {
                var caractInt = Convert.ToInt64(achar);
                decodedBase64String += Convert.ToChar(caractInt - step);
            }
            return decodedBase64String;
        }

        /// <summary>
        /// Shuffle the key value to prevent simple Base64 convertion
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static string Invert(string origin)
        {
            string resultString;
            if (origin.Length % 2 == 0)
            {
                var cut = origin.Substring(0, origin.Length / 2);
                resultString = origin.Substring((origin.Length / 2)) + cut;
            }
            else
            {
                var center = origin[(origin.Length / 2)];
                var cut = origin.Substring(0, origin.Length / 2);
                resultString = origin.Substring((origin.Length / 2) + 1) + center + cut;
            }
            return resultString;
        }

    }
}
