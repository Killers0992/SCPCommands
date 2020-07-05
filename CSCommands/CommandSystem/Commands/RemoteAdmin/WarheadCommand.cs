using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class WarheadCommand : ICommand
{
	public string Command { get; } = "warhead";
	public string[] Aliases { get; }
	public string Description { get; } = "Warhead.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.WarheadEvents, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.WarheadEvents;
			return false;
		}
		if (arguments.Count == 0)
		{
			response = "Usage: warhead <status/detonate/instant/cancel/enable/disable>";
			return false;
		}
		switch (arguments.At(1))
		{
			case "status":
				if (AlphaWarheadController.Host.detonated || Math.Abs(AlphaWarheadController.Host.timeToDetonation) < 0.001f)
					response = "Warhead has been detonated.";
				else if (AlphaWarheadController.Host.inProgress)
				{
					response = "Detonation is in progress.";
				}
				else if (!AlphaWarheadOutsitePanel.nukeside.enabled)
				{
					response = "Warhead is disabled.";
				}
				else if (AlphaWarheadController.Host.timeToDetonation > AlphaWarheadController.Host.RealDetonationTime())
				{
					response = "Warhead is restarting.";
				}
				else
				{
					response = "Warhead is ready to detonation.";
				}
				return true;
			case "detonate":
				AlphaWarheadController.Host.StartDetonation();
				response = "Detonation sequence started.";
				return true;
			case "instant":
				AlphaWarheadController.Host.InstantPrepare();
				AlphaWarheadController.Host.StartDetonation();
				AlphaWarheadController.Host.NetworktimeToDetonation = 5f;
				response = "Instant detonation started.";
				return true;
			case "cancel":
				AlphaWarheadController.Host.CancelDetonation(null);
				response = "Detonation has been canceled.";
				return true;
			case "enable":
				AlphaWarheadOutsitePanel.nukeside.Networkenabled = true;
				response = "Warhead has been enabled.";
				return true;
			case "disable":
				AlphaWarheadOutsitePanel.nukeside.Networkenabled = false;
				response = "Warhead has been disabled.";
				return true;
			default:
				response = "WARHEAD: Unknown subcommand.";
				return false;
		}
	}
}
