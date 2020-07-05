using CommandSystem;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SpeakCommand : ICommand
{
	public string Command { get; } = "speak";
	public string[] Aliases { get; } = new string[]
	{
		"icom"
	};
	public string Description { get; } = "Set intercom speaker.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.Broadcasting, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.Broadcasting;
			return false;
		}
		ReferenceHub hub = Extensions.GetHub((sender as CommandSender).SenderId);
		if (!Intercom.AdminSpeaking)
		{
			if (Intercom.host.speaking)
			{
				response = "Intercom is being used by someone else.";
				return false;
			}
			Intercom.AdminSpeaking = true;
			Intercom.host.RequestTransmission(hub.GetComponent<Intercom>().gameObject);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " requested global voice over the intercom.", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
			response = "Done! Global voice over the intercom granted.";
			return true;
		}
		else
		{
			Intercom.AdminSpeaking = false;
			Intercom.host.RequestTransmission(null);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " ended global intercom transmission.", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
			response = "Done! Global voice over the intercom revoked.";
			return true;
		}
	}
}
