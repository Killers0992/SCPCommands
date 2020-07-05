using CommandSystem;
using Mirror;
using System;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class PermCommand : ICommand
{
	public string Command { get; } = "perm";
	public string[] Aliases { get; }
	public string Description { get; } = "Permission list of current group.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (hub != ReferenceHub.HostHub)
		{
			ulong permissions = hub.serverRoles.Permissions;
			string text16 = "Your permissions:";
			foreach (string allPermission in ServerStatic.PermissionsHandler.GetAllPermissions())
			{
				string text17 = ServerStatic.PermissionsHandler.IsRaPermitted(ServerStatic.PermissionsHandler.GetPermissionValue(allPermission)) ? "*" : "";
				text16 = text16 + "\n" + allPermission + text17 + " (" + ServerStatic.PermissionsHandler.GetPermissionValue(allPermission) + "): " + (ServerStatic.PermissionsHandler.IsPermitted(permissions, allPermission) ? "YES" : "NO");
			}
			response = text16;
			return true;
		}
		response = "";
		return false;
	}
}
