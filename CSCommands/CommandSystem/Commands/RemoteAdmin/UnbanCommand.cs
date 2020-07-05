using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class UnbanCommand : ICommand
{
	public string Command { get; } = "unban";
	public string[] Aliases { get; }
	public string Description { get; } = "Unban.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.LongTermBanning, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.LongTermBanning;
			return false;
		}
		if (arguments.Count != 3)
		{
			response = "Usage: unban <id/ip> <userId/ipAddress>";
			return false;
		}
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " ran the unban " + arguments.At(1) + " command on " + arguments.At(2) + ".", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
		switch (arguments.At(1))
		{
			case "id":
			case "playerid":
			case "player":
				BanHandler.RemoveBan(arguments.At(2), BanHandler.BanType.UserId);
				response = "Done!";
				return true;
			case "ip":
			case "address":
				BanHandler.RemoveBan(arguments.At(2), BanHandler.BanType.IP);
				response = "Done!";
				return true;
			default:
				response = "Usage: unban <id/ip> <userId/ipAddress>";
				return false;
		}
	}
}
