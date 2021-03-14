using System.Text;

namespace Ieemdb.Persistence.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        ///  This method normalizes string to avoid inconsistencies.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NormalizeSearch(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var builder = new StringBuilder(value);

            return builder.ToString()
                .Normalize(NormalizationForm.FormC)
                .Trim()
                .ToUpperInvariant();
        }
    }
}
