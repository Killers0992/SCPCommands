using CommandSystem;
using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class BanCommand : ICommand
{
	public string Command { get; } = "ban";
	public string[] Aliases { get; } = new string[]
	{
		"offlineban",
		"oban"
	};
	public string Description { get; } = "Ban.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (arguments.Count >= 2)
		{
			string text10 = string.Empty;
			if (arguments.Count > 2)
				text10 = arguments.Skip(2).Aggregate((string current, string n) => current + " " + n);
			int num4 = 0;
			string time = arguments.At(2);
			try
			{
				num4 = Misc.RelativeTimeToSeconds(time, 60);
			}
			catch
			{
				response = "Invalid time: " + time;
				return false;
			}
			if (num4 < 0)
			{
				num4 = 0;
				time = "0";
			}
			bool IsSender = false;
			if ((num4 == 0 &&
				!sender.CheckPermission(PlayerPermissions.KickingAndShortTermBanning, out IsSender) ||
				!sender.CheckPermission(PlayerPermissions.BanningUpToDay, out IsSender) ||
				!sender.CheckPermission(PlayerPermissions.LongTermBanning, out IsSender) ||
				!IsSender))
			{
				response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.KickingAndShortTermBanning + ", " + PlayerPermissions.BanningUpToDay + ", " + PlayerPermissions.LongTermBanning;
				return false;
			}
			if ((num4 > 0 && num4 <= 3600 &&
				!sender.CheckPermission(PlayerPermissions.KickingAndShortTermBanning, out IsSender) ||
				!IsSender))
			{
				response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.KickingAndShortTermBanning;
				return false;
			}
			if ((num4 > 3600 && num4 <= 86400 &&
				!sender.CheckPermission(PlayerPermissions.BanningUpToDay, out IsSender) ||
				!IsSender))
			{
				response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.BanningUpToDay;
				return false;
			}
			if ((num4 > 86400 &&
				!sender.CheckPermission(PlayerPermissions.LongTermBanning, out IsSender) ||
				!IsSender))
			{
				response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.LongTermBanning;
				return false;
			}
			List<int> list2 = Misc.ProcessRaPlayersList(arguments.At(1));
			if (list2 == null)
			{
				response = "Invalid players list.";
				return false;
			}
			ushort num5 = 0;
			ushort num6 = 0;
			string text11 = string.Empty;
			foreach (int item in list2)
			{
				try
				{
					ReferenceHub hub2 = ReferenceHub.GetHub(item);
					if (hub2 == null)
					{
						num6 = (ushort)(num6 + 1);
						continue;
					}
					string combinedName = hub2.nicknameSync.CombinedName;
					if ((sender as CommandSender).FullPermissions)
						goto IL_18fa;
					byte b = hub2.serverRoles.Group?.RequiredKickPower ?? 0;
					if (b <= (sender as CommandSender).KickPower)
						goto IL_18fa;
					num6 = (ushort)(num6 + 1);
					text11 += $"You can't kick/ban {combinedName}. Required kick power: {b}, your kick power: {(sender as CommandSender).KickPower}.";
					goto end_IL_1866;
				IL_18fa:
					ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " banned player " + hub2.LoggedNameFromRefHub() + ". Ban duration: " + time + ". Reason: " + ((text10 == string.Empty) ? "(none)" : text10) + ".", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
					if (ServerStatic.PermissionsHandler.IsVerified && hub2.serverRoles.BypassStaff)
						PlayerManager.localPlayer.GetComponent<BanPlayer>().BanUser(hub2.gameObject, 0, text10, (sender as CommandSender).Nickname);
					else
					{
						if (num4 == 0 && ConfigFile.ServerConfig.GetBool("broadcast_kicks"))
							PlayerManager.localPlayer.GetComponent<Broadcast>().RpcAddElement(ConfigFile.ServerConfig.GetString("broadcast_kick_text", "%nick% has been kicked from this server.").Replace("%nick%", combinedName), ConfigFile.ServerConfig.GetUShort("broadcast_kick_duration", 5), Broadcast.BroadcastFlags.Normal);
						else if (num4 != 0 && ConfigFile.ServerConfig.GetBool("broadcast_bans", def: true))
						{
							PlayerManager.localPlayer.GetComponent<Broadcast>().RpcAddElement(ConfigFile.ServerConfig.GetString("broadcast_ban_text", "%nick% has been banned from this server.").Replace("%nick%", combinedName), ConfigFile.ServerConfig.GetUShort("broadcast_ban_duration", 5), Broadcast.BroadcastFlags.Normal);
						}
						PlayerManager.Localplayer.GetComponent<BanPlayer>().BanUser(hub2.gameObject, num4, text10, (sender as CommandSender).Nickname);
					}
					num5 = (ushort)(num5 + 1);
				end_IL_1866:;
				}
				catch (Exception ex4)
				{
					num6 = (ushort)(num6 + 1);
					Debug.Log(ex4);
					text11 += "Error occured during banning: " + ex4.Message + ".\n" + ex4.StackTrace;
				}
			}
			if (num6 == 0)
			{
				string text12 = "Banned";
				if (int.TryParse(time, out int result2))
					text12 = ((result2 > 0) ? "Banned" : "Kicked");
				response = "Done! " + text12 + " " + num5 + " player(s)!";
				return true;
			}
			else
				response = "The process has occured an issue! Failures: " + num6 + "\nLast error log:\n" + text11;
			return false;
		}
		else
			response = "To run this program, type at least 3 arguments! (some parameters are missing)";
		return false;
	}
}
