using CommandSystem;
using Mirror;
using System;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ServerEventCommand : ICommand
{
	public string Command { get; } = "server_event";
	public string[] Aliases { get; }
	public string Description { get; } = "Execute server event.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (arguments.Count == 1)
		{
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced a server event: " + arguments.At(0), ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			GameObject gameObject10 = GameObject.Find("Host");
			MTFRespawn component6 = gameObject10.GetComponent<MTFRespawn>();
			AlphaWarheadController component7 = gameObject10.GetComponent<AlphaWarheadController>();
			bool flag15 = true;
			bool IsSender = false;
			switch (arguments.At(0).ToUpper())
			{
				case "FORCE_CI_RESPAWN":
					if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out IsSender) || !IsSender)
					{
						response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RespawnEvents;
						return false;
					}
					component6.nextWaveIsCI = true;
					component6.timeToNextRespawn = 0.1f;
					break;
				case "FORCE_MTF_RESPAWN":
					if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out IsSender) || !IsSender)
					{
						response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RespawnEvents;
						return false;
					}
					component6.nextWaveIsCI = false;
					component6.timeToNextRespawn = 0.1f;
					break;
				case "DETONATION_START":
					if (!sender.CheckPermission(PlayerPermissions.WarheadEvents, out IsSender) || !IsSender)
					{
						response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.WarheadEvents;
						return false;
					}
					component7.InstantPrepare();
					component7.StartDetonation();
					break;
				case "DETONATION_CANCEL":
					if (!sender.CheckPermission(PlayerPermissions.WarheadEvents, out IsSender) || !IsSender)
					{
						response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.WarheadEvents;
						return false;
					}
					component7.CancelDetonation();
					break;
				case "DETONATION_INSTANT":
					if (!sender.CheckPermission(PlayerPermissions.WarheadEvents, out IsSender) || !IsSender)
					{
						response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.WarheadEvents;
						return false;
					}
					component7.InstantPrepare();
					component7.StartDetonation();
					component7.NetworktimeToDetonation = 5f;
					break;
				case "TERMINATE_UNCONN":
					if (!sender.CheckPermission(PlayerPermissions.RoundEvents, out IsSender) || !IsSender)
					{
						response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RoundEvents;
						return false;
					}
					foreach (NetworkConnection value3 in NetworkServer.connections.Values)
					{
						if (GameCore.Console.FindConnectedRoot(value3) == null)
						{
							value3.Disconnect();
							value3.Dispose();
						}
					}
					break;
				case "ROUND_RESTART":
				case "ROUNDRESTART":
				case "RR":
				case "RESTART":
					{
						if (!sender.CheckPermission(PlayerPermissions.RoundEvents, out IsSender) || !IsSender)
						{
							response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RoundEvents;
							return false;
						}
						PlayerStats component8 = PlayerManager.localPlayer.GetComponent<PlayerStats>();
						if (component8.isServer)
							component8.Roundrestart();
						break;
					}
				default:
					flag15 = false;
					break;
			}
			if (flag15)
			{
				response = "Started event: " + arguments.At(0).ToUpper();
				return true;
			}
			else
			{
				response = "Incorrect event! (Doesn't exist)";
				return false;
			}
		}
		else
			response = "To run this program, type at least 2 arguments! (some parameters are missing)";
		return false;
	}
}
