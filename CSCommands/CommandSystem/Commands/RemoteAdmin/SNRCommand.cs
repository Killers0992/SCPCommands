using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SNRCommand : ICommand
{
	public string Command { get; } = "snr";
	public string[] Aliases { get; } = new string[]
	{
		"stopnextround"
	};
	public string Description { get; } = "Stopnextround.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.ServerConsoleCommands, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.ServerConsoleCommands;
			return false;
		}
		ServerStatic.StopNextRound = !ServerStatic.StopNextRound;
		response = "Server " + (ServerStatic.StopNextRound ? "WILL" : "WON'T") + " stop after next round.";
		return true;
	}
}
