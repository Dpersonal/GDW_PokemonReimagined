using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonCollider : MonoBehaviour
{
    //so apparently we cant do it directly to the buttons
    //so i've devised a monstrosity of an idea where i just shove them to where the buttons are, but in the actual object space
    //there is 100% a better way to do this

    //this is a bad way to do it. dont do this. anyways this cooresponds to whatever move button it is, so like top left is 1, top right is 2, etc
    [SerializeField] int moveButtonOrder;
    private void OnMouseEnter()
    {
        Debug.Log("a message goes here");
        GameObject.FindFirstObjectByType<UIManager>().UpdateMoveDescriptionText(moveButtonOrder);
    }
    private void OnMouseExit()
    {
        Debug.Log("it's always negative one so its actually much easier");
        GameObject.FindFirstObjectByType<UIManager>().UpdateMoveDescriptionText(-1);
    }
}
