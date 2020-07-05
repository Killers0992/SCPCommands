using CommandSystem;
using RemoteAdmin;
using System;
using System.Linq;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class KillCommand : ICommand
{
	public string Command { get; } = "kill";
	public string[] Aliases { get; }
	public string Description { get; } = "Kill.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.PlayersManagement, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.PlayersManagement;
			return false;
		}
		if (arguments.Count == 1)
		{
			if (!int.TryParse(arguments.At(0), out int id4))
			{
				response = "Invalid player id!";
				return false;
			}
			GameObject gameObject = PlayerManager.players.FirstOrDefault((GameObject pl) => pl.GetComponent<QueryProcessor>().PlayerId == id4);
			if (gameObject == null)
            {
				response = "Invalid player id!";
				return false;
			}
			else if (ReferenceHub.GetHub(gameObject).playerStats.HurtPlayer(new PlayerStats.HitInfo(-1f, "ADMIN", DamageTypes.Wall, 0), gameObject))
			{
				response = "Player has been killed!";
				return true;
			}
			else
			{
				response = "Kill failed!";
				return false;
			}
		}
		else
			response = "Please specify the PlayerId!";
		return false;
	}
}
