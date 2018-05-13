using System;
using System.Net.Mail;

namespace CogsLite.Core
{
    public static class EmailValidator
    {
		public static bool IsEmailAddress(this string text)
		{			
			try
			{
				new MailAddress(text);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}			
		}
    }
}
