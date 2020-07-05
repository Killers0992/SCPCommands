using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CassieCommand : ICommand
{
	public string Command { get; } = "cassie";
	public string[] Aliases { get; }
	public string Description { get; } = "Cassie.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.Announcer, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.Announcer;
			return false;
		}
		if (arguments.Count == 0)
        {
			response = "Usage: cassie [text]";
			return false;
        }
		string message = string.Join(" ", arguments);
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " ran the cassie command (parameters: " + message + ").", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
		PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(message, false, true);
		response = "Executed cassie with message " + message;
		return true;
	}
}
