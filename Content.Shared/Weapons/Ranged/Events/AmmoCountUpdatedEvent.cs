namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
/// Raised on a gun when its ammo count updates.
/// </summary>
[ByRefEvent]
public readonly record struct AmmoCountUpdatedEvent;
