using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface IStringFormatterService
	{
		/// <summary>
		/// Parse a custom format string to replace property names with values
		/// </summary>
		/// <returns>Parsed string</returns>
		string Format(string format, Aircraft aircraft, Condition condition);
	}

	internal class StringFormatterService : IStringFormatterService
	{
		private readonly IVrsService _vrsService;
		private readonly IVrsEnumService _vrsEnumService;

		public StringFormatterService(IVrsService vrsService, IVrsEnumService vrsEnumService)
		{
			_vrsService = vrsService;
			_vrsEnumService = vrsEnumService;
		}

		/// <summary>
		/// Parse a custom format string to replace property names with values
		/// </summary>
		/// <returns>Parsed string</returns>
		public string Format(string format, Aircraft aircraft, Condition condition)
		{
			var variables = new Dictionary<string, string>
			{
				{ "ConditionName", condition.Name },
				{
					"RcvrName",
					_vrsService.Receivers.ContainsKey(aircraft.GetProperty("Rcvr"))
						? _vrsService.Receivers[aircraft.GetProperty("Rcvr")]
						: ""
				},
				{ "Date", DateTime.Now.ToString("d") },
				{ "Time", DateTime.Now.ToString("t") },
			};

			//Iterate variables
			foreach (var varKey in variables.Keys)
			{
				//Check if content contains keyword
				if (format.ToLower().Contains(@"[" + varKey.ToLower() + @"]"))
				{
					//Replace keyword with value
					format = Regex.Replace(format, @"\[" + varKey + @"\]", variables[varKey], RegexOptions.IgnoreCase);
				}
			}

			//Iterate properties
			foreach (var info in Core.VrsPropertyData.Values)
			{
				//Check if content contains keyword
				if (!format.ToLower().Contains(@"[" + info[2].ToLower() + @"]"))
					continue;

				//Replace keyword with value
				var value = aircraft.GetProperty(info[2]) ?? "";

				//If enum, replace with string value
				if (_vrsEnumService.TryToString(info[2], value, out var convertedValue))
				{
					value = convertedValue;
				}

				format = Regex.Replace(format, @"\[" + info[2] + @"\]", value, RegexOptions.IgnoreCase);
			}

			return format;
		}
	}
}
