using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CassieSLCommand : ICommand
{
	public string Command { get; } = "cassie_sl";
	public string[] Aliases { get; } = new string[]
	{
		"cassie_silentnoise",
		"cassie_sn",
		"cassie_silent"
	};
	public string Description { get; } = "Silent cassie.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.Announcer, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.Announcer;
			return false;
		}
		if (arguments.Count == 0)
		{
			response = "Usage: cassie_sl [text]";
			return false;
		}
		string message = string.Join(" ", arguments);
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " ran the cassie_sl command (parameters: " + message + ").", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
		PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(message, false, false);
		response = "Executed silent cassie with message " + message;
		return true;
	}
}
