using CommandSystem;
using GameCore;
using System;
using System.Globalization;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class RoundTimeCommand : ICommand
{
	public string Command { get; } = "roundtime";
	public string[] Aliases { get; } = new string[]
	{
		"rt",
		"rtime"
	};
	public string Description { get; } = "Roundtime.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.RoundEvents, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RoundEvents;
			return false;
		}
		if (arguments.Count == 0)
		{
			response = "Usage: cassie [text]";
			return false;
		}
		if (RoundStart.RoundLenght.Ticks == 0L)
			response = "The round has not yet started!";
		else
			response = "Round time: " + RoundStart.RoundLenght.ToString("hh\\:mm\\:ss\\.fff", CultureInfo.InvariantCulture);
		return true;
	}
}
