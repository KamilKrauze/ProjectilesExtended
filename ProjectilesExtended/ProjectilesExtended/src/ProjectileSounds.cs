using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace ProjectilesExtended;

public class ProjectileSounds : EntityBehavior
{
    private ILoadedSound FlyingSound { get; set; }
    
    private ICoreClientAPI clientAPI;
    public override string PropertyName() => "pex:Sounds";
    public override bool ThreadSafe => true;
    private event EventHandler OnTickActions;

    public ProjectileSounds(Entity entity) : base(entity)
    {
    }
    
    public override void Initialize(EntityProperties properties, JsonObject attributes)
    {
        base.Initialize(properties, attributes);
        
        clientAPI = entity.Api as ICoreClientAPI;

        JsonObject flyingSoundCfg = attributes["flyingSound"];
        if (flyingSoundCfg == null)
        {
            return;
        }
        
        string soundLocation = flyingSoundCfg["assetLocation"].AsString(null);
        float baseVolume = flyingSoundCfg["baseVolume"].AsFloat(1.0f);
        if (soundLocation == null)
        {
            return;
        }
        
        FlyingSound = clientAPI.World.LoadSound(new SoundParams(new AssetLocation(soundLocation)));
        if (FlyingSound != null)
        {
            FlyingSound.SetLooping(true);
            FlyingSound.SetVolume(baseVolume);
            FlyingSound.Params.SoundType = EnumSoundType.Entity;
            OnTickActions += PlayFlyingSoundAtInterval;
        }
    }
    
    public override void OnGameTick(float deltaTime)
    {
        if (clientAPI == null || !entity.Alive)
            return;

        OnTickActions.Invoke(this, EventArgs.Empty);
    }
    
    private void PlayFlyingSoundAtInterval(object? caller, EventArgs args)
    {
        double speed = entity.SidedPos.Motion.LengthSq();
        if (speed > 0.1)
        {
            FlyingSound.SetPosition(entity.Pos.XYZFloat);

            if (!FlyingSound.IsPlaying)
            {
                FlyingSound.Start();
            }
        }
        else
        {
            FlyingSound.FadeOutAndStop(0.500f);
        }
    }
}