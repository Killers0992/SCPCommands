using CommandSystem;
using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SetGroupCommand : ICommand
{
	public string Command { get; } = "setgroup";
	public string[] Aliases { get; }
	public string Description { get; } = "Setgroup.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.SetGroup, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.SetGroup;
			return false;
		}
		if (!ConfigFile.ServerConfig.GetBool("online_mode", true))
        {
			response = "This command requires the server to operate in online mode!";
			return false;
		}
		else if (arguments.Count >= 3)
		{
			ServerLogs.AddLog(ServerLogs.Modules.Permissions, sender.LogName + " ran the setgroup command (new group: " + arguments.At(2) + " min) on " + arguments.At(1) + " players.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			StandardizedQueryModel1(sender, query[0], query[1], query[2], out failures, out successes, out error, out replySent);
			if (!replySent)
			{
				if (failures == 0)
					response = "Done! The request affected " + successes + " player(s)!";
				else
					response = "The proccess has occured an issue! Failures: " + failures + "\nLast error log:\n" + error;
				return true;
			}
			response = "";
			return false;
		}
		else
		{
			response = "To run this program, type at least 3 arguments! (some parameters are missing)";
			return false;
		}
	}
}
