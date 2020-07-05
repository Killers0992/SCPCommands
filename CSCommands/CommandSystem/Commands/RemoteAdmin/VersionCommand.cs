using CommandSystem;
using GameCore;
using System;
using System.Globalization;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class VersionCommand : ICommand
{
	public string Command { get; } = "version";
	public string[] Aliases { get; }
	public string Description { get; } = "Server version.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		response = "Server Version: " + CustomNetworkManager.CompatibleVersions[0] + " " + Application.buildGUID;
		return true;
	}
}
