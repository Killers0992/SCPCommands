using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class UnlockCommand : ICommand
{
	public string Command { get; } = "unlock";
	public string[] Aliases { get; } = new string[]
	{
		"ul"
	};
	public string Description { get; } = "Unlock door in facility.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.FacilityManagement;
			return false;
		}
		if (arguments.Count == 0)
		{
			response = "Usage: unlock [doorname]";
			return false;
		}
		else
			//ProcessDoorQuery(sender, "OPEN", arguments.At(0)); TODO
			return true;
	}
}
