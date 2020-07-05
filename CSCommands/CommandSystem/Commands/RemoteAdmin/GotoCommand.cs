using CommandSystem;
using RemoteAdmin;
using System;
using System.Linq;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GotoCommand : ICommand
{
	public string Command { get; } = "goto";
	public string[] Aliases { get; }
	public string Description { get; } = "Goto.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.PlayersManagement, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.PlayersManagement;
			return false;
		}
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (arguments.Count == 2)
		{
			if (hub.characterClassManager.CurClass == RoleType.Spectator || hub.characterClassManager.CurClass < RoleType.Scp173)
			{
				response = "Command is disabled when you are spectator!";
				return false;
			}
			if (!int.TryParse(arguments.At(1), out int id))
			{
				response = "Player ID must be an integer.";
				return false;
			}
			if (arguments.At(1).Contains("."))
			{
				response = "Goto command requires exact one selected player.";
				return false;
			}
			GameObject gameObject8 = PlayerManager.players.FirstOrDefault((GameObject pl) => pl.GetComponent<QueryProcessor>().PlayerId == id);
			if (gameObject8 == null)
			{
				response = "Can't find requested player.";
				return false;
			}
			if (gameObject8.GetComponent<CharacterClassManager>().CurClass == RoleType.Spectator || gameObject8.GetComponent<CharacterClassManager>().CurClass < RoleType.None)
			{
				response = "Requested player is a spectator!";
				return false;
			}
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " ran the goto command on " + arguments.At(1) + " players.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			hub.playerMovementSync.OverridePosition(gameObject8.GetComponent<PlayerMovementSync>().RealModelPosition, 0f);
			response = "Done!";
			return true;
		}
		else
			response = "To run this program, type exactly 2 arguments!";
		return false;
	}
}
