﻿using Content.Server.MachineLinking.System;

namespace Content.Server._00OuterRim.Mapping;

/// <summary>
/// This handles...
/// </summary>
public sealed class AutoLinkSystem : EntitySystem
{
    [Dependency] private readonly SignalLinkerSystem _signalLinkerSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<AutoLinkTransmitterComponent, MapInitEvent>(OnAutoLinkMapInit);
    }

    private void OnAutoLinkMapInit(EntityUid uid, AutoLinkTransmitterComponent component, MapInitEvent args)
    {
        var xform = Transform(uid);

        foreach (var receiver in EntityQuery<AutoLinkReceiverComponent>())
        {
            if (receiver.AutoLinkChannel != component.AutoLinkChannel)
                continue; // Not ours.

            var rxXform = Transform(receiver.Owner);

            if (rxXform.GridUid != xform.GridUid)
                continue;

            _signalLinkerSystem.TryLinkDefaults(receiver.Owner, uid, EntityUid.Invalid);
        }
    }
}
