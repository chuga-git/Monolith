using Content.Shared.Timing;
using Content.Shared.Weapons.Ranged.Components;

namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
/// Raised directed on a gun when it cycles.
/// </summary>
public record struct GunCooldownStartEvent(StartEndTime Cooldown);
