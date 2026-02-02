using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;

namespace ProjectilesExtended;

public class PexModSystem : Vintagestory.API.Common.ModSystem
{
    private static readonly string MOD_NAME = "Projectiles Extended"; 
    
    // Called on server and client
    // Useful for registering block/entity classes on both sides
    public override void Start(ICoreAPI api)
    {
        Mod.Logger.Notification($"Loading {MOD_NAME} {api.Side}");
        
        api.RegisterEntityBehaviorClass("pex:Sounds", typeof(ProjectileExtendedBehaviour));
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("pex:hello"));
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("pex:hello"));
    }
}