using CommandSystem;
using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class OfflineBanCommand : ICommand
{
	public string Command { get; } = "offlineban";
	public string[] Aliases { get; } = new string[]
	{
		"offlineban",
		"oban"
	};
	public string Description { get; } = "Offline ban.";

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
			
			bool flag10 = Misc.IsIpAddress(arguments.At(1));
			if (!flag10 && !arguments.At(1).Contains("@"))
			{
				response = "Target must be a valid UserID or IP (v4 or v6) address.";
				return false;
			}
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " banned an offline player with " + (flag10 ? "IP address" : "UserID") + arguments.At(1) + ". Ban duration: " + arguments.At(2) + ". Reason: " + ((text10 == string.Empty) ? "(none)" : text10) + ".", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			BanHandler.IssueBan(new BanDetails
			{
				OriginalName = "Unknown - offline ban",
				Id = arguments.At(1),
				IssuanceTime = TimeBehaviour.CurrentTimestamp(),
				Expires = TimeBehaviour.GetBanExpieryTime((uint)num4),
				Reason = text10,
				Issuer = (sender as CommandSender).Nickname
			}, flag10 ? BanHandler.BanType.IP : BanHandler.BanType.UserId);
			response =  (flag10 ? "IP address " : "UserID ") + arguments.At(1) + " has been banned from this server.";
			return true;
		}
		else
			response = "To run this program, type at least 3 arguments! (some parameters are missing)";
		return false;
	}
}
