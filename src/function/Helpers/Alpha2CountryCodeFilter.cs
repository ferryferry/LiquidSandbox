using Bia.Countries.Iso3166;

namespace MasterData.Repositories.Helpers
{
    public static class Alpha2CountryCodeFilter
    {
        /// <summary>
        /// This function is to convert a string I.e. "NL" to "Netherlands'
        /// </summary>
        /// <param name="alpha2CountryCode">The alpha 2 letter code of the country (ISO-3166-1 Standard)</param>
        /// <returns>The country name based on the ISO-3166-1 standard</returns>
        public static string CountryCodeToName(string alpha2CountryCode)
        {
            if(string.IsNullOrWhiteSpace(alpha2CountryCode) || string.IsNullOrEmpty(alpha2CountryCode))
                return "";
            
            // If is Greece (EL) is used in the EU to identify Greece.
            if(alpha2CountryCode.Equals("EL")){
                return Countries.GetCountryByAlpha2("GR").ShortName;
            }

            // Get a country by ISO-3166-1 Alpha2 code.
            var foundCountry = Countries.GetCountryByAlpha2(alpha2CountryCode);

            return foundCountry == null ? "" : foundCountry.ShortName;
        }
    }
}