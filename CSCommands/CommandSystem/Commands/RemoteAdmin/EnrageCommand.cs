using CommandSystem;
using PlayableScps;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class EnrageCommand : ICommand
{
	public string Command { get; } = "enrage";
	public string[] Aliases { get; }
	public string Description { get; } = "Enrages scp 096.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.Broadcasting;
			return false;
		}
		Scp096 scp;
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (hub.scpsController && (scp = (hub.scpsController.CurrentScp as Scp096)) != null)
		{
			scp.Windup(force: true);
			response = "Setting 096 into rage mode...";
			return true;

		}
		response = "You must play as scp 096";
		return false;
	}
}
