using System;
using System.Collections.Generic;
using System.Text;

namespace fwAssistant.Commands
{
	class SudoSay : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
			string msg = cmd.Remove(0, 8);
			TTS(msg, true);
		}
	}
}