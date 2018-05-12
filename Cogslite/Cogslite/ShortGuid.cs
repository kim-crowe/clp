using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Cogslite
{
    public static class ShortGuid
    {
		public static string ToShortGuid(this Guid guid)
		{
			return Convert.ToBase64String(guid.ToByteArray()).Replace("/", "-").Replace("+", "_").Replace("=", "");
		}

		public static Guid Parse(string guid)
		{
			if (guid.Length == 22)
				return ToGuid(guid);

			return Guid.Parse(guid);
		}

		public static Guid ToGuid(string shortGuid)
		{			
			var guid = default(Guid);
			var base64 = shortGuid.Replace("-", "/").Replace("_", "+") + "==";

			try
			{
				guid = new Guid(Convert.FromBase64String(base64));
			}
			catch (Exception ex)
			{
				throw new Exception("Bad Base64 conversion to GUID", ex);
			}

			return guid;
		}
    }

	public class ShortGuidModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
				throw new ArgumentNullException(nameof(bindingContext));

			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			if (valueProviderResult == ValueProviderResult.None)
				throw new InvalidOperationException("Unable to find value " + bindingContext.ModelName);

			var valueAsString = valueProviderResult.FirstValue;

			Guid result = Guid.Empty;

			if (!string.IsNullOrEmpty(valueAsString))
			{
				if (valueAsString.Length == 22)
					result = ShortGuid.ToGuid(valueAsString);
				else
					result = Guid.Parse(valueAsString);
			}
			
			bindingContext.Result = ModelBindingResult.Success(result);

			return Task.CompletedTask;
		}
	}

	public class ShortGuidModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(Guid)) 
				return new ShortGuidModelBinder();

			return null;
		}
	}

	public static class MvcOptionsExtensions
	{
		public static void UseShortGuids(this MvcOptions opts)
		{
			var binderToFind = opts.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));

			if (binderToFind == null) return;

			var index = opts.ModelBinderProviders.IndexOf(binderToFind);
			opts.ModelBinderProviders.Insert(index, new ShortGuidModelBinderProvider());
		}
	}
}
