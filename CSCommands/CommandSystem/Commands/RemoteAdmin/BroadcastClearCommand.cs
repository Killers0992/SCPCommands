using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class BroadcastClearCommand : ICommand
{
	public string Command { get; } = "bcclear";
	public string[] Aliases { get; } = new string[]
	{
		"cl",
		"clearbc",
		"clearalert",
		"alertclear"
	};
	public string Description { get; } = "Broadcast clear.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.Broadcasting, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.Broadcasting;
			return false;
		}
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " ran the cleared all broadcasts.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging); 
		PlayerManager.localPlayer.GetComponent<Broadcast>().RpcClearElements();
		response = "All broadcasts cleared.";
		return true;
	}
}
