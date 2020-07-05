using CommandSystem;
using GameCore;
using System;
using System.Globalization;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ReloadConfigCommand : ICommand
{
	public string Command { get; } = "reloadconfig";
	public string[] Aliases { get; }
	public string Description { get; } = "Reload config.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.CheckPermission(PlayerPermissions.ServerConfigs, out bool IsSender) || !IsSender)
		{
			response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + PlayerPermissions.ServerConfigs;
			return false;
		}
		try
		{
			ConfigFile.ReloadGameConfigs();
			response = "Reloaded all configs!";
			return true;
		}
		catch (Exception arg3)
		{
			response = "Reloading configs failed: " + arg3;
			return false;
		}
	}
}
