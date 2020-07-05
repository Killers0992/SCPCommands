using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Extensions
{
    public static ReferenceHub GetHub(string userID)
    {
        switch (userID)
        {
            case "SERVER CONSOLE":
                return ReferenceHub.HostHub;
            case "GAME CONSOLE":
                return ReferenceHub.HostHub;
            default:
                return ReferenceHub.GetAllHubs().Where(rh => rh.Value.characterClassManager.UserId == userID).FirstOrDefault().Value;
        }
    }
}

