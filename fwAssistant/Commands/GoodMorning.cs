using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace fwAssistant.Commands
{
	class GoodMorning : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
            dynamic current = Modules.GetCurrentWeatherAsync().Result;
            dynamic currentDay = Modules.GetDailyWeatherAsync(0).Result;

            DateTime sunrise = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(currentDay["sunrise"])).ToLocalTime();
            DateTime sunset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(currentDay["sunset"])).ToLocalTime();

            string wind_spd = current["wind"]["speed"];

            TTS($"Dzień dobry {Program.Username}! " +
                $"Aktualna godzina to {DateTime.Now.ToString("HH:mm", CultureInfo.GetCultureInfo("pl-PL"))}. " +
                $"Dzisiaj jest {DateTime.Now.ToString("dddd dd MMMM", CultureInfo.GetCultureInfo("pl-PL"))}. " +
                $"Na zewnątrz w Piwnicznej-Zdroju aktualnie mamy {current["weather"][0]["description"]}. " +
                $"Aktualna temperatura to: {current["main"]["temp"]}° odczuwalna jak {current["main"]["feels_like"]}°. " +
                $"Dzisiaj minimalna temperatura będzie wynosiła {currentDay["temp"]["min"]}° a maksymalna {currentDay["temp"]["max"]}°. " +
                $"Wiatr wieje z szybkością około {wind_spd.Replace(".", ",")} m/s! " +
                $"Słońce dzisiaj wschodzi o {sunrise:HH:mm} a zachodzi o {sunset:HH:mm}. " +
                $"Po południu będą {currentDay["weather"][0]["description"]} a temperatura będzie wynosić około {currentDay["temp"]["day"]}°. " +
                $"Miłego dnia!");
        }
	}
}