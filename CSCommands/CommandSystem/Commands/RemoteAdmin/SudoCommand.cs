using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SudoCommand : ICommand
{
	public string Command { get; } = "sudo";
	public string[] Aliases { get; } = new string[]
	{
		"rcon"
	};
	public string Description { get; } = "Sudo/rcon.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.ServerConsoleCommands, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.ServerConsoleCommands;
			return false;
		}
		if (arguments.Count < 2)
		{
			response = "To run this program, type at least 1 argument! (some parameters are missing)";
			return false;
		}
		if (arguments.At(1).StartsWith("!") && !ServerStatic.RolesConfig.GetBool("allow_central_server_commands_as_ServerConsoleCommands"))
		{
			response = "Running central server commands in Remote Admin is disabled in RA config file!";
			return false;
		}
		string text15 = arguments.Aggregate("", (string current, string arg) => current + arg + " ");
		text15 = text15.Substring(0, text15.Length - 1);
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " executed command as server console: " + text15 + " player.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
		ServerConsole.EnterCommand(text15, out ConsoleColor _, sender);
		response = "Command \"" + text15 + "\" executed in server console!";
		return true;
	}
}
