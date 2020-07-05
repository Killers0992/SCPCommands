using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class BroadcastCommand : ICommand
{
	public string Command { get; } = "broadcast";
	public string[] Aliases { get; } = new string[]
	{
		"bc",
		"alert",
		"broadcastmono",
		"bcmono",
		"alertmono"
	};
	public string Description { get; } = "Broadcast.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.Broadcasting, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.Broadcasting;
			return false;
		}
		if (arguments.Count < 2)
		{
			response = "Usage: broadcast [seconds] [text]";
			return false;
		}
		if (!ushort.TryParse(arguments.At(0), out ushort duration) || duration < 1)
        {
			response = "Usage: broadcast [seconds] [text]";
			return false;
		}
		string message = string.Join(" ", arguments.Skip(1));
		ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " ran the broadcast command (duration: " + duration + " seconds) with text \"" + message + "\".", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
		PlayerManager.localPlayer.GetComponent<Broadcast>().RpcAddElement(message, duration, Broadcast.BroadcastFlags.Normal); //mono???
		response = "Broadcast sent.";
		return true;
	}
}
