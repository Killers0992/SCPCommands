using CommandSystem;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class TKCommand : ICommand
{
	public string Command { get; } = "ffd";
	public string[] Aliases { get; } = new string[]
	{
		"tk",
		"tko"
	};
	public string Description { get; } = "Friendly fire detector.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (arguments.Count != 1)
		{
			response = "Usage: ffd <Player ID/status/pause/unpause>";
			return false;
		}
		bool isSender = false;
		switch(arguments.At(1).ToLower(CultureInfo.InvariantCulture))
        {
			case "status":
				if (!sender.CheckPermission(PlayerPermissions.FriendlyFireDetectorTempDisable, out isSender) || !isSender)
                {
					response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.FriendlyFireDetectorTempDisable;
					return false;
				}
				response = "Friendly fire detector is currently " + (FriendlyFireConfig.PauseDetector ? string.Empty : "**NOT** ") + "paused.";
				return true;
			case "pause":
				if (!sender.CheckPermission(PlayerPermissions.FriendlyFireDetectorTempDisable, out isSender) || !isSender)
				{
					response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.FriendlyFireDetectorTempDisable;
					return false;
				}
				if (FriendlyFireConfig.PauseDetector)
				{
					response = "Friendly fire detector is already paused.";
					return true;
				}
				FriendlyFireConfig.PauseDetector = true;
				response = "Friendly fire detector has been paused.";
				ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " has paused Friendly Fire Detector.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
				return true;
			case "unpause":
				if (!sender.CheckPermission(PlayerPermissions.FriendlyFireDetectorTempDisable, out isSender) || !isSender)
				{
					response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.FriendlyFireDetectorTempDisable;
					return false;
				}
				if (!FriendlyFireConfig.PauseDetector)
				{
					response = "Friendly fire detector is not paused.";
					return true;
				}
				FriendlyFireConfig.PauseDetector = false;
				response = "Friendly fire detector has been unpaused.";
				ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " has unpaused Friendly Fire Detector.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
				return true;
			default:
				if (!sender.CheckPermission(PlayerPermissions.PlayersManagement, out isSender) || !isSender)
				{
					response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.PlayersManagement;
					return false;
				}
				if (!int.TryParse(arguments.At(1), out int id3))
				{
					response = "Player ID must be an integer.";
					return false;
				}
				if (arguments.At(1).Contains("."))
				{
					response = "FFD command requires exact one selected player.";
					return false;
				}
				GameObject gameObject6 = PlayerManager.players.FirstOrDefault((GameObject pl) => pl.GetComponent<QueryProcessor>().PlayerId == id3);
				if (gameObject6 == null)
				{
					response = "Can't find requested player.";
					return false;
				}
				FriendlyFireHandler friendlyFireHandler = ReferenceHub.GetHub(gameObject6.gameObject).FriendlyFireHandler;
				response = $"--- Friendly Fire Detector Stats ---\nKills - Damage dealt\n\nRound: {friendlyFireHandler.Round.Kills} - {friendlyFireHandler.Round.Damage}\nLife: {friendlyFireHandler.Life.Kills} - {friendlyFireHandler.Life.Damage}\nWindow: {friendlyFireHandler.Window.Kills} - {friendlyFireHandler.Window.Damage} [Window: {FriendlyFireConfig.Window}s]\nRespawn: {friendlyFireHandler.Respawn.Kills} - {friendlyFireHandler.Respawn.Damage} [Window: {FriendlyFireConfig.RespawnWindow}s]";
				return true;
		}
	}
}
