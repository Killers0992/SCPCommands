using CommandSystem;
using GameCore;
using Mirror;
using System;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class LobbyLockCommand : ICommand
{
	public string Command { get; } = "lobbylock";
	public string[] Aliases { get; } = new string[]
	{
		"llock",
		"ll"
	};
	public string Description { get; } = "Roundlock.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.RoundEvents, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RoundEvents;
			return false;
		}
		RoundStart.LobbyLock = !RoundStart.LobbyLock;
		response = "Lobby lock " + (RoundStart.LobbyLock ? "enabled!" : "disabled!");
		return true;
	}
}
