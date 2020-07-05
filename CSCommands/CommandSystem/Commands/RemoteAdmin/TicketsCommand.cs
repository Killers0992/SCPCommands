using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class TicketCommand : ICommand
{
	public string Command { get; } = "tickets";
	public string[] Aliases { get; } = new string[]
	{
		"tix"
	};
	public string Description { get; } = "Cassie.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!PlayerManager.localPlayer.TryGetComponent(out MTFRespawn component))
        {
			response = "Respawn component doesn't exist!";
			return false;
		}
		else if (arguments.Count <= 2)
        {
			bool IsSender = false;
			switch (arguments.At(1))
			{
				case "ntf":
				case "mtf":
				case "ninetailedfox":
				case "mobiletaskforce":
					if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out IsSender) || !IsSender)
					{
						response = "NtfTix#NTF spawn tickets: !---";
						return false;
					}
					response = $"NtfTix#NTF spawn tickets: !{component.MtfRespawnTickets}";
					return true;
				case "ci":
				case "chaos":
				case "chaosinsurgency":
					if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out IsSender) || !IsSender)
					{
						response = "CiTix#CI spawn tickets: !---";
						return false;
					}
					response = $"CiTix#CI spawn tickets: !{component.ChaosRespawnTickets}";
					return true;
				case "fetch":
					if (sender.CheckPermission(PlayerPermissions.RespawnEvents, out IsSender) || IsSender)
					{
						response = $"NtfTix#NTF spawn tickets: !{component.MtfRespawnTickets}";
						response += Environment.NewLine + $"CiTix#CI spawn tickets: !{component.ChaosRespawnTickets}";
						return true;
					}
					else
                    {
						response = "NtfTix#NTF spawn tickets: !---";
						response += Environment.NewLine + "CiTix#CI spawn tickets: !---";
						return false;
                    }
			}
		}
		else
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out bool isSender) || !isSender)
			{
				response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.RespawnEvents;
				return false;
			}
			if (!int.TryParse(arguments.At(2), out int result) && arguments.At(2) != "dec" && arguments.At(2) != "inc")
			{
				response = "Wrong syntax; try: 'TICKETS [team] [amount]'.";
				return false;
			}
			switch (arguments.At(1).ToLower())
			{
				case "ntf":
				case "mtf":
				case "ninetailedfox":
				case "mobiletaskforce":
					if (arguments.At(2) == "dec")
						component.MtfRespawnTickets--;
					else if (arguments.At(2) == "inc")
					{
						component.MtfRespawnTickets++;
					}
					else
					{
						component.MtfRespawnTickets = result;
					}
					ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " set NTF spawn tickets amount to " + component.MtfRespawnTickets + ".", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
					response = $"SetNtfTix#NTF spawn tickets set to !{component.MtfRespawnTickets}";
					return true;
				case "ci":
				case "chi":
				case "chaos":
				case "chaosinsurgency":
					if (arguments.At(2) == "dec")
						component.ChaosRespawnTickets--;
					else if (arguments.At(2) == "inc")
					{
						component.ChaosRespawnTickets++;
					}
					else
					{
						component.ChaosRespawnTickets = result;
					}
					ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " set CI spawn tickets amount to " + component.ChaosRespawnTickets + ".", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
					response = $"SetCiTix#CI spawn tickets set to !{component.ChaosRespawnTickets}";
					return true;
			}
		}
		response = "";
		return false;
	}
}
