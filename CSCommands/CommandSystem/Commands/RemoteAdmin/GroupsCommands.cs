using CommandSystem;
using RemoteAdmin;
using System;
using System.Linq;
using System.Collections.Generic;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GroupsCommand : ICommand
{
	public string Command { get; } = "groups";
	public string[] Aliases { get; }
	public string Description { get; } = "Groups.";

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		string text9 = "Groups defined on this server:";
		Dictionary<string, UserGroup> allGroups2 = ServerStatic.PermissionsHandler.GetAllGroups();
		ServerRoles.NamedColor[] namedColors = QueryProcessor.Localplayer.GetComponent<ServerRoles>().NamedColors;
		foreach (KeyValuePair<string, UserGroup> permentry in allGroups2)
		{
			try
			{
				text9 = text9 + "\n" + permentry.Key + " (" + permentry.Value.Permissions + ") - <color=#" + namedColors.FirstOrDefault((ServerRoles.NamedColor x) => x.Name == permentry.Value.BadgeColor).ColorHex + ">" + permentry.Value.BadgeText + "</color> in color " + permentry.Value.BadgeColor;
			}
			catch
			{
				text9 = text9 + "\n" + permentry.Key + " (" + permentry.Value.Permissions + ") - " + permentry.Value.BadgeText + " in color " + permentry.Value.BadgeColor;
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.KickingAndShortTermBanning))
				text9 += " BN1";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.BanningUpToDay))
				text9 += " BN2";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.LongTermBanning))
				text9 += " BN3";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassSelf))
				text9 += " FSE";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassToSpectator))
				text9 += " FSP";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassWithoutRestrictions))
				text9 += " FWR";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.GivingItems))
				text9 += " GIV";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.WarheadEvents))
				text9 += " EWA";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.RespawnEvents))
				text9 += " ERE";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.RoundEvents))
				text9 += " ERO";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.SetGroup))
				text9 += " SGR";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.GameplayData))
				text9 += " GMD";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Overwatch))
				text9 += " OVR";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FacilityManagement))
				text9 += " FCM";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PlayersManagement))
				text9 += " PLM";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PermissionsManagement))
				text9 += " PRM";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ServerConsoleCommands))
				text9 += " SCC";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ViewHiddenBadges))
				text9 += " VHB";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ServerConfigs))
				text9 += " CFG";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Broadcasting))
				text9 += " BRC";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PlayerSensitiveDataAccess))
				text9 += " CDA";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Noclip))
				text9 += " NCP";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.AFKImmunity))
				text9 += " AFK";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.AdminChat))
				text9 += " ATC";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ViewHiddenGlobalBadges))
				text9 += " GHB";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Announcer))
				text9 += " ANN";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Effects))
				text9 += " EFF";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FriendlyFireDetectorImmunity))
				text9 += " FFI";
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FriendlyFireDetectorTempDisable))
				text9 += " FFT";
		}
		response = text9;
		return true;
	}
}
