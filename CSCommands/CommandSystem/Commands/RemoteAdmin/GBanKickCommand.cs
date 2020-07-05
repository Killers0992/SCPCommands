using CommandSystem;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GBanKickCommand : ICommand
{
	public string Command { get; } = "gban-kick";
	public string[] Aliases { get; }
	public string Description { get; } = "Global ban & kick.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (!hub.serverRoles.RaEverywhere && !hub.serverRoles.Staff)
		{
			response = "You don't have permissions to run this command!";
			return false;
		}
		if (arguments.Count != 2)
		{
			response = "To run this program, type exactly 1 argument! (some parameters are missing)";
			return false;
		}
		List<int> players = Misc.ProcessRaPlayersList(arguments.At(1));
		if (players == null)
		{
			response = "An unexpected problem has occurred during PlayerId array processing.";
			return false;
		}
		try
		{
			foreach (var player in players)
			{
				try
				{
					foreach (GameObject plr in PlayerManager.players)
					{
						if (player != plr.GetComponent<QueryProcessor>().PlayerId)
							continue;
						QueryProcessor.Localplayer.GetComponent<BanPlayer>().KickUser(plr, "Globally Banned", (sender as CommandSender).Nickname, true);
					}
				}
				catch (Exception ex3)
				{
					response = ex3.Message + "\nStackTrace:\n" + ex3.StackTrace;
					return false;
				}
			}
		}
		catch (Exception ex4)
		{
			response = "An unexpected problem has occurred!\nMessage: " + ex4.Message + "\nStackTrace: " + ex4.StackTrace + "\nAt: " + ex4.Source + "\nMost likely the PlayerId array was not in the correct format.";
			return false;
		}
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " globally banned and kicked " + arguments.At(1) + " player.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
		response = arguments.At(1) + " globbaly banned.";
		return true;
	}
}
