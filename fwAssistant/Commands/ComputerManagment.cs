using System;
using System.Collections.Generic;
using System.Text;

namespace fwAssistant.Commands
{
	class ComputerManagment : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
			if(cmd.ToLower().StartsWith("włącz ", StringComparison.CurrentCulture))
			{

			}
			else if (cmd.ToLower().StartsWith("wyłącz ", StringComparison.CurrentCulture))
			{

			}
		}
	}
}