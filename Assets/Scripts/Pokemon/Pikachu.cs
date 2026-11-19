using System.Collections.Generic;

public sealed class Pikachu : Pokemon
{

    public override PokeStats Stats => new PokeStats
    (
        type1: PokeType.Electric,
        type2: PokeType.None,
        maxHP: 100,
        name: "Pikachu"
    );

}