using CommandSystem;
using Mirror;
using System;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GlobalTagCommand : ICommand
{
	public string Command { get; } = "globaltag";
	public string[] Aliases { get; } = new string[]
	{
		"gtag"
	};
	public string Description { get; } = "Global tag.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (hub != ReferenceHub.HostHub)
		{
			if (string.IsNullOrEmpty(hub.serverRoles.PrevBadge))
			{
				response = "You don't have global tag.";
				return false;
			}
			hub.serverRoles.HiddenBadge = null;
			hub.serverRoles.GlobalHidden = false;
			hub.serverRoles.RpcResetFixed();
			hub.serverRoles.NetworkGlobalBadge = hub.serverRoles.PrevBadge;
			response = "Global tag refreshed!";
			return true;
		}
		response = "";
		return false;
	}
}
