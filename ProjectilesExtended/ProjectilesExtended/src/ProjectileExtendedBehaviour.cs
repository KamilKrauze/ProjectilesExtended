using System;
using System.Collections.Generic;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace ProjectilesExtended;

public class ProjectileExtendedBehaviour : EntityBehavior
{
    private string FlyingSound { get; set; } = string.Empty;

    private long lastMs = 0;
    private long currentMs = 0;
    private ICoreClientAPI _clientApi;
    public override string PropertyName() => "pex:Sounds";
    private event EventHandler OnTickActions;

    public ProjectileExtendedBehaviour(Entity entity) : base(entity)
    {
        // OnTickActions = new Dictionary<string, Action>();
    }
    
    public override void Initialize(EntityProperties properties, JsonObject attributes)
    {
        base.Initialize(properties, attributes);
        
        _clientApi = entity.Api as ICoreClientAPI;
        FlyingSound = attributes["flying_sound"].AsString(string.Empty);
        if (FlyingSound != string.Empty)
        {
            OnTickActions += PlayFlyingSoundAtInterval;
        }
    }
    
    public override void OnGameTick(float deltaTime)
    {
        if (_clientApi == null || !entity.Alive)
            return;

        OnTickActions.Invoke(this, EventArgs.Empty);
    }
    
    private void PlayFlyingSoundAtInterval(object? caller, EventArgs e)
    {
        // Projectile must be moving and not stuck
        if (entity is ProjectileEntity pe)
        {
            if (pe.Stuck) return;
        }

        double speed = entity.SidedPos.Motion.Length();
        if (speed < 0.01) return;

        currentMs = entity.World.ElapsedMilliseconds;

        // Play sound every 120 ms
        if (currentMs >= lastMs + 120)
        {
            double dPitch = GameMath.Clamp(speed * 1.5, 0.5d, 2.0);
            float pitch = Convert.ToSingle(dPitch);
            PlaySoundAtLocation(FlyingSound, entity.Pos, pitch);
            lastMs = currentMs;
        }
    }
    
    private void PlaySoundAtLocation(in string soundAssetName, in EntityPos position, float pitch)
    {
        _clientApi.World.PlaySoundAt(new AssetLocation(soundAssetName), position.X, position.Y, position.Z, dualCallByPlayer:null, pitch: pitch);
    }
}