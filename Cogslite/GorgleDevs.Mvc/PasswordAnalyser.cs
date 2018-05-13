using System.Text.RegularExpressions;

namespace GorgleDevs.Mvc
{
    public static class PasswordAnalyser
    {
		public static int PasswordStrength(string password)
		{
			int strength = password.Length;

			var lowerCount = Regex.Matches(password, "[a-z]").Count;
			var upperCount = Regex.Matches(password, "[A-Z]").Count;
			var numberCount = Regex.Matches(password, "[0-9]").Count;
			var specialCount = password.Length - lowerCount - upperCount - numberCount;

			strength += lowerCount > 1 ? 3 : 0;
			strength += upperCount > 1 ? 3 : 0;
			strength += numberCount > 1 ? 5 : 0;
			strength += specialCount > 1 ? 5 : 0;

			return strength;
		}
    }
}
