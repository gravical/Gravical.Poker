using System;

namespace Gravical.Poker.Core
{
    public static class Guards
    {
        public static void ArgumentSuccess(bool condition, string argument, string message = null)
        {
            if (!condition) throw new ArgumentException(message ?? "Invalid argument", argument);
        }

        public static void ArgumentNotNull(object value, string name)
        {
            if (value == null) throw new ArgumentNullException(name);
        }

        public static void ArgumentNotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty", name);
        }

        public static void ArgumentHasExactSize(byte[] value, int size, string name)
        {
            if (value == null || value.Length != size) throw new ArgumentException($"Value must be exactly {size} bytes", name);
        }

        public static byte[] ArgumentBase64HasExactSize(string value, int size, string name)
        {
            ArgumentNotNullOrEmpty(value, name);
            try
            {
                var binary = Convert.FromBase64String(value);
                ArgumentHasExactSize(binary, size, name);
                return binary;
            }
            catch (FormatException)
            {
                throw new ArgumentException("Value is not valid base64", name);
            }
        }

        public static T ArgumentEnum<T>(string value, string name)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {
                throw new ArgumentException($"Value is not a valid {typeof(T).Name} enumeration", name);
            }
        }

        public static void OperationSuccess(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
    }
}