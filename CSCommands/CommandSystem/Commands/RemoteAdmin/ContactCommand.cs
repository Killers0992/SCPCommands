using CommandSystem;
using GameCore;
using System;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ContactCommand : ICommand
{
	public string Command { get; } = "contact";
	public string[] Aliases { get; }
	public string Description { get; } = "Contact email.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		response = "Contact email address: " + ConfigFile.ServerConfig.GetString("contact_email");
		return true;
	}
}
