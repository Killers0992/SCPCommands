using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ForcestartCommand : ICommand
{
	public string Command { get; } = "forcestart";
	public string[] Aliases { get; } = new string[]
	{
		"fc"
	};
	public string Description { get; } = "Forcestart round.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.RoundEvents, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RoundEvents;
			return false;
		}
		bool flag14 = CharacterClassManager.ForceRoundStart();
		if (flag14)
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced round start.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
		response = (flag14 ? "Done! Forced round start." : "Failed to force start.");
		return true;
	}
}
