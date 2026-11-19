
public sealed class DebugMove : Move
{
    public DebugMove() : base() { }

    public override MoveData Data => new MoveData
    (
        power: 10,
        accuracy: 100,
        powerPoints: 10,
        type: PokeType.Ground,
        name: "Debug"
    );
}