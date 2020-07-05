using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class IntercomResetCommand : ICommand
{
	public string Command { get; } = "intercom-reset";
	public string[] Aliases { get; }
	public string Description { get; } = "Intercom reset.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		bool IsSender = false;
		if (
			!sender.CheckPermission(PlayerPermissions.RoundEvents, out IsSender) ||
			!sender.CheckPermission(PlayerPermissions.FacilityManagement, out IsSender) ||
			!sender.CheckPermission(PlayerPermissions.PlayersManagement, out IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RoundEvents + ", " + PlayerPermissions.FacilityManagement + ", " + PlayerPermissions.PlayersManagement;
			return false;
		}
		if (Intercom.host.remainingCooldown <= 0f)
		{
			response = "Intercom is already ready to use.";
			return false;
		}
		Intercom.host.remainingCooldown = -1f;
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " reset the intercom cooldown.", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
		response = "Done! Intercom cooldown reset.";
		return true;
	}
}
