using CommandSystem;
using RemoteAdmin;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class LockdownCommand : ICommand
{
	public string Command { get; } = "lockdown";
	public string[] Aliases { get; } = new string[]
	{
		"ld"
	};
	public string Description { get; } = "Lockdown.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.FacilityManagement;
			return false;
		}
		Door[] array;
		if (!QueryProcessor.Lockdown)
		{
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " enabled the lockdown.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			array = UnityEngine.Object.FindObjectsOfType<Door>();
			foreach (Door door in array)
			{
				if (!door.locked)
				{
					door.lockdown = true;
					door.UpdateLock();
				}
			}
			QueryProcessor.Lockdown = true;
			response = "Lockdown enabled!";
			return true;
		}
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " disabled the lockdown.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);;
		array = UnityEngine.Object.FindObjectsOfType<Door>();
		foreach (Door door2 in array)
		{
			if (door2.lockdown)
			{
				door2.lockdown = false;
				door2.UpdateLock();
			}
		}
		QueryProcessor.Lockdown = false;
		response = "Lockdown disabled!";
		return true;
	}
}
