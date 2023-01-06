using System;
using Newtonsoft.Json.Linq;

namespace PlaneAlerter.Extensions
{
	internal static class JTokenExtensions
	{
		public static T RequiredValue<T>(this JToken? token, params string[] keys)
		{
			if (token == null)
				throw new Exception($"Token null, unable to get key {string.Join('/', keys)}");

			var value = token.OptionalValue<T>(keys);

			if (value == null)
				throw new Exception($"None of the keys ({string.Join(", ", keys)}) exist as a child of {token.Path}");

			return value;
		}

		public static T? OptionalValue<T>(this JToken? token, params string[] keys)
		{
			if (token == null)
				throw new Exception($"Token null, unable to get key {string.Join('/', keys)}");

			var value = default(T);
			foreach (var key in keys)
			{
				var keyToken = token.SelectToken(key);

				if (keyToken == null)
					continue;

				//value = token.Value<T>(key);
				value = keyToken.ToObject<T>();

				return value;
			}

			return value;
		}
	}
}
