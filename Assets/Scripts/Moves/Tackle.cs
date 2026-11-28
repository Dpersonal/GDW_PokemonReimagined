
public sealed class Tackle : Move
{

    public override MoveData Data => new MoveData
    (
        power: 40, 
        accuracy: 100, 
        powerPoints: 35, 
        type: PokeType.Normal,
        name: "Tackle",
        description: "Tackle your enemies!"
    );

}