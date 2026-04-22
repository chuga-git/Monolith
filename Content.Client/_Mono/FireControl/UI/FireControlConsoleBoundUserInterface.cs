// Copyright Rane (elijahrane@gmail.com) 2025
// All rights reserved. Relicensed under AGPL with permission

using System.Linq;
using Content.Shared._Mono.FireControl;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Map;

namespace Content.Client._Mono.FireControl.UI;

[UsedImplicitly]
public sealed class FireControlConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private FireControlWindow? _window;


    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<FireControlWindow>();

        _window.OnServerRefresh += OnRefreshServer;

        _window.Radar.OnRadarClick += (coords) =>
        {
            var netCoords = EntMan.GetNetCoordinates(coords);

            // Send empty list of weapons for cursor tracking only when not clicking
            // This allows guided missiles to follow the cursor without firing weapons
            if (!_window.Radar.IsMouseDown())
            {
                SendCursorUpdateMessage(netCoords);
            }
            else
            {
                // Normal fire message when actually clicking
                SendFireMessage(netCoords);
            }
        };

        _window.Radar.DefaultCursorShape = Control.CursorShape.Crosshair;

        // Add event handler for when weapons are selected/deselected
        _window.OnWeaponSelectionChanged += UpdateSelectedWeapons;
    }

    private void OnRefreshServer()
    {
        SendMessage(new FireControlConsoleRefreshServerMessage());
    }

    // TODO: this is rancid
    private List<NetEntity> GetSelectedWeapons(FireControlWindow window)
    {
        return window.GetSelectedWeapons() ?? [];
    }

    private void UpdateSelectedWeapons()
    {
        if (_window?.Radar is not FireControlNavControl navControl)
            return;

        // TODO: Unsuck this once UI is finalized
        navControl.UpdateSelectedWeapons(GetSelectedWeapons(_window).ToHashSet());
    }

    private void SendFireMessage(NetCoordinates coordinates)
    {
        if (_window == null)
            return;

        var selected = GetSelectedWeapons(_window);

        if (selected.Count > 0)
            SendMessage(new FireControlConsoleFireMessage(selected, coordinates));
    }

    private void SendCursorUpdateMessage(NetCoordinates coordinates)
    {
        // Send an empty weapon list to indicate this is just a cursor update, not a firing action
        SendMessage(new FireControlConsoleFireMessage(new List<NetEntity>(), coordinates));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not FireControlConsoleBoundInterfaceState castState)
            return;
        _window?.UpdateStatus(castState);
        if (_window?.Radar is FireControlNavControl navControl)
        {
            navControl.SetConsole(Owner);
            navControl.UpdateControllables(Owner, castState.FireControllables, castState.FireGroups);

            // Update selected weapons when state updates
            UpdateSelectedWeapons();
        }
    }
}
