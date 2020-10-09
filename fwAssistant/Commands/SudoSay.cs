using System;
using System.Collections.Generic;
using System.Text;

namespace fwAssistant.Commands
{
	class SudoSay : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
			string msg = ReplaceCmdPrefix(cmd, kvCmd.Key);
			TTS(msg, true);
		}
	}
}