namespace Esentis.Ieemdb.Persistence.Helpers
{
  using System.Text;

  public static class StringExtensions
    {
        /// <summary>
        ///  This method normalizes string to avoid inconsistencies.
        /// </summary>
        /// <param name="value">Text that should be normalized</param>
        /// <returns>Returns text normalized.</returns>
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
