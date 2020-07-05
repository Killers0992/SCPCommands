using CommandSystem;
using Mirror;
using System;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ShowTagCommand : ICommand
{
	public string Command { get; } = "showtag";
	public string[] Aliases { get; }
	public string Description { get; } = "Show tag.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (hub != ReferenceHub.HostHub)
		{
			hub.serverRoles.HiddenBadge = null;
			hub.serverRoles.GlobalHidden = false;
			hub.serverRoles.RpcResetFixed();
			hub.serverRoles.RefreshPermissions(disp: true);
			response = "Local tag refreshed!";
			return true;
		}
		response = "";
		return false;
	}
}
