using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class DoorListCommand : ICommand
{
	public string Command { get; } = "doorlist";
	public string[] Aliases { get; } = new string[]
	{
		"dl",
		"doors"
	};
	public string Description { get; } = "Doors.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.FacilityManagement;
			return false;
		}
		string str2 = "List of named doors in the facility:\n";
		List<string> list3 = (from item in UnityEngine.Object.FindObjectsOfType<Door>()
							  where !string.IsNullOrEmpty(item.DoorName)
							  select item.DoorName + " - " + (item.isOpen ? "<color=green>OPENED</color>" : "<color=orange>CLOSED</color>") + (item.locked ? " <color=red>[LOCKED]</color>" : "") + ((item.PermissionLevels == (Door.AccessRequirements)0) ? "" : " <color=blue>[CARD REQUIRED]</color>")).ToList();
		list3.Sort();
		str2 += list3.Aggregate((string current, string adding) => current + "\n" + adding);
		response = str2;
		return true;
	}
}
