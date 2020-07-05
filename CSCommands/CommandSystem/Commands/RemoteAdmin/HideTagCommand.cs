using CommandSystem;
using Mirror;
using System;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class HideTagCommand : ICommand
{
	public string Command { get; } = "hidetag";
	public string[] Aliases { get; }
	public string Description { get; } = "Hide tag.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (hub != ReferenceHub.HostHub)
		{
			if (!string.IsNullOrEmpty(hub.serverRoles.HiddenBadge))
			{
				response = "Your badge is already hidden.";
				return false;
			}
			if (string.IsNullOrEmpty(hub.serverRoles.MyText))
			{
				response = "Your don't have any badge.";
				return false;
			}
			hub.serverRoles.GlobalHidden = hub.serverRoles.GlobalSet;
			hub.serverRoles.HiddenBadge = hub.serverRoles.MyText;
			hub.serverRoles.NetworkGlobalBadge = null;
			hub.serverRoles.SetText(null);
			hub.serverRoles.SetColor(null);
			hub.serverRoles.RefreshHiddenTag();
			response = "Tag hidden!";
			return true;
		}
		response = "";
		return false;
	}
}
