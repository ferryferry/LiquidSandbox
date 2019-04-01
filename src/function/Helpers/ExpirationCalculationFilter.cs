using System;
using System.Text.RegularExpressions;

namespace MasterData.Repositories.Helpers
{
    public static class  ExpirationCalculationFilter
    {
        public static string Textilize(string expirationCalculation)
        {
            if(string.IsNullOrWhiteSpace(expirationCalculation) || string.IsNullOrEmpty(expirationCalculation))
                return null;
            
            var regex = new Regex("(\\d+)([dmwy|DMWY])");

            var matches = regex.Matches(expirationCalculation);
            if (matches == null || matches.Count == 0)
                return null;
            
            var period = new TimeSpan();
            
            foreach (Match match in matches)
            {
                var amount = Convert.ToInt32(match.Groups[1].Value);
                var unit = match.Groups[2].Value.ToUpper()[0];
                
                switch (unit)
                {
                    case 'D':
                        period += TimeSpan.FromDays(1 * amount);
                        break;
                    case 'M':
                        period += TimeSpan.FromDays(30.42D * amount);
                        break;
                    case 'W':
                        period += TimeSpan.FromDays(7 * amount);
                        break;
                    case 'Y':
                        period += TimeSpan.FromDays(365.2422D * amount);
                        break;
                }
            }
            
            return period.TotalDays.ToString();
        }
    }
}
