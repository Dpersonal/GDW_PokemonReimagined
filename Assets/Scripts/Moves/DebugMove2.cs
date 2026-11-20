public sealed class DebugMove2 : Move
{
    public override MoveData Data => new MoveData
        (
            power: 30,
            accuracy: 100,
            powerPoints: 10,
            type: PokeType.Flying,
            name: "Debug"
        );
}