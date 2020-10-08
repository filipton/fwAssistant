using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace fwAssistant.Commands
{
	class Weather : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
			int dayIndex = 0;

			if (cmd.ToLower().Contains(" jutro"))
			{
				dayIndex = 1;
			}
			else if (cmd.ToLower().Contains(" pojutrze"))
			{
				dayIndex = 2;
			}

			dynamic Day = Modules.GetDailyWeatherAsync(dayIndex).Result;

			TTS($"Na zewnątrz w {DateTime.Now.AddDays(dayIndex).ToString("dddd", CultureInfo.GetCultureInfo("pl-PL"))} będą {Day["weather"][0]["description"]}. " +
				$"Minimalna temperatura będzie wynosić {Day["temp"]["min"]}° a maksymalna {Day["temp"]["max"]}°. " +
				$"Po południu temperatura będzie wynosić około {Day["temp"]["day"]}° odczuwalna jak {Day["feels_like"]["day"]}°. ");
		}
	}
}