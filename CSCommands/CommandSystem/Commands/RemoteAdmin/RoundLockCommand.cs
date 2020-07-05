using CommandSystem;
using Mirror;
using System;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class RoundLockCommand : ICommand
{
	public string Command { get; } = "roundlock";
	public string[] Aliases { get; } = new string[]
	{
		"rlock",
		"rl"
	};
	public string Description { get; } = "Roundlock.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.RoundEvents, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RoundEvents;
			return false;
		}
		RoundSummary.RoundLock = !RoundSummary.RoundLock;
		response = "Round lock " + (RoundSummary.RoundLock ? "enabled!" : "disabled!");
		return true;
	}
}
