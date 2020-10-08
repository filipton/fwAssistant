using System;
using System.Collections.Generic;
using System.Text;

namespace fwAssistant.Commands
{
	class InteractiveSudoSay : Command
	{
		public string Text = string.Empty;

		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
			if (!_Interactivity)
			{
				TTSWT("Tak, co mam powiedzieć?");
				RegisterInteractivity(kvCmd);
			}
			else if(_Interactivity && Text != string.Empty)
			{
				if (cmd.ToLower() == "oczywiście że tak")
				{
					TTS(Text);
					Text = string.Empty;
					UnRegisterInteractivity();
				}
			}
			else if(_Interactivity && Text == string.Empty)
			{
				Text = cmd;
				TTSWT("Czy napewno chcesz abym to powiedziala?");
				RegisterInteractivity(kvCmd);
			}
		}
	}
}
