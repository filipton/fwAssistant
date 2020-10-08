using System;
using System.Collections.Generic;
using System.Text;

namespace fwAssistant.Commands
{
	class ActualWeather : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
			dynamic current = Modules.GetCurrentWeatherAsync().Result;

			TTS($"Na zewnątrz w Piwnicznej-Zdroju aktualnie mamy {current["weather"][0]["description"]}. " +
				$"Aktualna temperatura to: {current["main"]["temp"]}° odczuwalna jak {current["main"]["feels_like"]}°.");
		}
	}
}
