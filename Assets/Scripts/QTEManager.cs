using System.Threading.Tasks;
using UnityEngine;

public class QTEManager : MonoBehaviour
{ 
    public async Awaitable<float> PerformQTE() // add arguments as needed
    {
        await Task.Delay(1000); // qte happens
        return Random.value;
    }
}