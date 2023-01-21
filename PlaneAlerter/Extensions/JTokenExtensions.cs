using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlaneAlerter.Extensions
{
	internal static class JTokenExtensions
	{
		public static T RequiredValue<T>(this JToken? token, params string[] keys) where T : class
		{
			if (token == null)
				throw new JsonReaderException($"Token null, unable to get key {string.Join('/', keys)}");

			var foundKey = token.TryOptionalValue<T>(out var value, keys);

			if (!foundKey)
			{
				if (keys.Length > 1)
				{
					throw new JsonReaderException(
						$"None of the keys ({string.Join(", ", keys)}) exist{(!string.IsNullOrEmpty(token.Path) ? $" as a child of {token.Path}" : "")}");
				}
				else
				{
					throw new JsonReaderException(
						$"The key {keys.First()} does not exist{(!string.IsNullOrEmpty(token.Path) ? $" as a child of {token.Path}" : "")}");
				}
			}

			return value!;
		}

		public static T RequiredValueStruct<T>(this JToken? token, params string[] keys) where T : struct
		{
			if (token == null)
				throw new JsonReaderException($"Token null, unable to get key {string.Join('/', keys)}");

			var value = token.OptionalValueStruct<T>(keys);

			if (!value.HasValue)
			{
				if (keys.Length > 1)
				{
					throw new JsonReaderException(
						$"None of the keys ({string.Join(", ", keys)}) exist{(!string.IsNullOrEmpty(token.Path) ? $" as a child of {token.Path}" : "")}");
				}
				else
				{
					throw new JsonReaderException(
						$"The key {keys.First()} does not exist{(!string.IsNullOrEmpty(token.Path) ? $" as a child of {token.Path}" : "")}");
				}
			}

			return value.Value;
		}

		public static T? OptionalValueStruct<T>(this JToken? token, params string[] keys) where T : struct
		{
			if (keys.Length == 0)
				throw new ArgumentException("No keys provided");

			if (token == null)
				throw new JsonReaderException($"Token null, unable to get key {string.Join('/', keys)}");

			foreach (var key in keys)
			{
				var keyToken = token.SelectToken(key);

				if (keyToken == null)
					continue;
				
				return keyToken.ToObject<T>();
			}
			
			return null;
		}
		
		public static T? OptionalValue<T>(this JToken? token, params string[] keys) where T : class
		{
			return TryOptionalValue(token, out T? value, keys) ? value : null;
		}

		public static bool TryOptionalValue<T>(this JToken? token, out T? value, params string[] keys) where T : class
		{
			if (keys.Length == 0)
				throw new ArgumentException("No keys provided");

			if (token == null)
				throw new JsonReaderException($"Token null, unable to get key {string.Join('/', keys)}");

			foreach (var key in keys)
			{
				var keyToken = token.SelectToken(key);

				if (keyToken == null)
					continue;

				value = keyToken.ToObject<T>();
				return true;
			}

			value = null;
			return false;
		}
	}
}
