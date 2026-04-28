using Robust.Shared.Serialization;
using Robust.Shared.Map;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Timing;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Mono.FireControl;

[Serializable, NetSerializable]
public sealed class FireControlConsoleUpdateEvent : EntityEventArgs
{
}

[Serializable, NetSerializable]
public sealed class FireControlConsoleBoundInterfaceState : BoundUserInterfaceState
{
    public bool Connected;
    public FireControllableEntry[] FireControllables;
    public Dictionary<string, List<FireControllableEntry>> FireGroups;
    public NavInterfaceState NavState;

    public FireControlConsoleBoundInterfaceState(bool connected, FireControllableEntry[] fireControllables, Dictionary<string, List<FireControllableEntry>> fireGroups, NavInterfaceState navState)
    {
        Connected = connected;
        FireControllables = fireControllables;
        FireGroups = fireGroups;
        NavState = navState;
    }
}

[Serializable, NetSerializable]
public enum FireControlConsoleUiKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class FireControlConsoleRefreshServerMessage : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class FireControlConsoleFireMessage : BoundUserInterfaceMessage
{
    public List<NetEntity> Selected;
    public NetCoordinates Coordinates;
    public FireControlConsoleFireMessage(List<NetEntity> selected, NetCoordinates coordinates)
    {
        Selected = selected;
        Coordinates = coordinates;
    }
}

/// <summary>
///
/// </summary>
[Serializable, NetSerializable]
public sealed class FireControlConsoleWeaponUpdateMessage : BoundUserInterfaceMessage
{
    public NetEntity NetEntity;
    public int Shots;
    public int Capacity;
    public StartEndTime? Cooldown;
}


/// <summary>
/// Event raised when a fire control console wants to fire weapons at specific coordinates.
/// Used for tracking cursor position.
/// </summary>
public sealed class FireControlConsoleFireEvent : EntityEventArgs
{
    /// <summary>
    /// The coordinates of the cursor/firing position
    /// </summary>
    public NetCoordinates Coordinates;

    /// <summary>
    /// The weapons selected to fire
    /// </summary>
    public List<NetEntity> Selected;

    public FireControlConsoleFireEvent(NetCoordinates coordinates, List<NetEntity> selected)
    {
        Coordinates = coordinates;
        Selected = selected;
    }
}

[Serializable, NetSerializable]
public record struct FireControllableEntry(
    NetEntity NetEntity,
    NetCoordinates Coordinates,
    string Name,
    TimeSpan NextFire,
    int? Shots = null,
    int? Capacity = null,
    bool HasManualReload = false,
    bool CanFire = false )
{
    /// <summary>
    /// The entity in question
    /// </summary>
    public NetEntity NetEntity = NetEntity;

    /// <summary>
    /// Location of the entity
    /// </summary>
    public NetCoordinates Coordinates = Coordinates;

    /// <summary>
    /// Display name of the entity
    /// </summary>
    public string Name = Name;

    /// <summary>
    /// Current ammunition count.
    /// </summary>
    public int? Shots = Shots;

    /// <summary>
    /// Ammunition capacity.
    /// </summary>
    public int? Capacity = Capacity;

    /// <summary>
    /// Whether this weapon has manual reload.
    /// </summary>
    public bool HasManualReload = HasManualReload;

    /// <summary>
    ///
    /// </summary>
    public StartEndTime FireCooldown;
}
