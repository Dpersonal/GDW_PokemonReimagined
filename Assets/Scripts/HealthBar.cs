using UnityEngine;
using UnityEngine.EventSystems;

public class HealthBar : MonoBehaviour, IPointerClickHandler
{
    private Transform healthBar;

    private float _healthPercentage = 1f;
    public float HealthPercentage
    {
        get
        {
            return _healthPercentage;
        }
        set
        {
            _healthPercentage = value;
            if(_healthPercentage <  0f) _healthPercentage = 0f;
            if(_healthPercentage > 1f) _healthPercentage = 1f;
        }
    }

    // Only for debug purposes, unnecessary for final version
    public void OnPointerClick(PointerEventData eventData)
    {
        HealthPercentage = Random.value;
    }

    void Start()
    {
        healthBar = GetComponentsInChildren<Transform>()[1];
    }

    void Update()
    {
        healthBar.localScale = new Vector3(HealthPercentage, 1, 1);
        healthBar.localPosition = new Vector3((-1 + HealthPercentage)/2, 0, 0);
    }
}
