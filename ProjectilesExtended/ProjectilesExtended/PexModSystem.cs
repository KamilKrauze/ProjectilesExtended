using System;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;

namespace ProjectilesExtended;

public class PexModSystem : ModSystem
{
    private static readonly string MOD_NAME = "Projectiles Extended";
    private static string ModID { get; set; } = string.Empty;

    // Called on server and client
    // Useful for registering block/entity classes on both sides
    public override void Start(ICoreAPI api)
    {
        ModID = Mod.Info.ModID;
        
        Mod.Logger.Notification($"Loading {MOD_NAME} {api.Side}");
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        Mod.Logger.Notification($"Loading {MOD_NAME} {api.Side}");
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        Mod.Logger.Notification($"Loading {MOD_NAME} {api.Side}");
        api.RegisterEntityBehaviorClass($"{ModID}:ProjectileSounds", typeof(ProjectileSounds));
    }
}