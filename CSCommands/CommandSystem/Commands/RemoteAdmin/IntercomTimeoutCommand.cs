using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class IntercomTimeoutCommand : ICommand
{
	public string Command { get; } = "intercom-timeout";
	public string[] Aliases { get; }
	public string Description { get; } = "Intercom timeout.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		bool IsSender = false;
		if (!sender.CheckPermission(PlayerPermissions.KickingAndShortTermBanning, out IsSender) ||
			!sender.CheckPermission(PlayerPermissions.BanningUpToDay, out IsSender) ||
			!sender.CheckPermission(PlayerPermissions.LongTermBanning, out IsSender) ||
			!sender.CheckPermission(PlayerPermissions.RoundEvents, out IsSender) ||
			!sender.CheckPermission(PlayerPermissions.FacilityManagement, out IsSender) ||
			!sender.CheckPermission(PlayerPermissions.PlayersManagement, out IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.KickingAndShortTermBanning + ", " + PlayerPermissions.BanningUpToDay + ", " + PlayerPermissions.LongTermBanning + ", " + PlayerPermissions.RoundEvents + ", " + PlayerPermissions.FacilityManagement + ", " + PlayerPermissions.PlayersManagement;
			return false;
		}
		if (!Intercom.host.speaking)
		{
			response = "Intercom is not being used.";
			return false;
		}
		if (Math.Abs(Intercom.host.speechRemainingTime - -77f) < 0.1f)
		{
			response = "Intercom is being used by player with bypass mode enabled.";
			return false;
		}
		Intercom.host.speechRemainingTime = -1f;
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " timeouted the intercom speaker.", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
		response = "Done! Intercom speaker timeouted.";
		return true;
	}
}
