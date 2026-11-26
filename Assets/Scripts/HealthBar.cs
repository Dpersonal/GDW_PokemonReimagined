using System;
using System.Collections;
using Unity.VisualScripting;
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

    // ---------- Health Bar Animator ----------
    private Coroutine healthAnimator;
    [SerializeField] private float duration = 0.5f;
    private float targetHealth;
    public bool FinishedAnimation { get; private set; }
    public void SetHealthAnimated(float newHealth)
    {
        if (newHealth < 0f) newHealth = 0f;
        if (newHealth > 1f) newHealth = 1f;
        if (HealthPercentage == 0f || newHealth == HealthPercentage) return;
        targetHealth = newHealth;
        if (healthAnimator != null) StopCoroutine(healthAnimator);
        healthAnimator = StartCoroutine(HealthAnimator(newHealth));
    }

    public void CancelAnimation()
    {
        if (healthAnimator != null) StopCoroutine(healthAnimator);
        HealthPercentage = targetHealth;
        FinishedAnimation = true;
    }

    private IEnumerator HealthAnimator(float newHealth) 
    {
        FinishedAnimation = false;
        float oldHealth = HealthPercentage;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            HealthPercentage = Mathf.Lerp(oldHealth, newHealth, t);
            yield return null;
        }
        HealthPercentage = targetHealth;
        healthAnimator = null;
        FinishedAnimation = true;
        yield break;
    }
    // -----------------------------------------

    // Only for debug purposes, unnecessary for final version
    public void OnPointerClick(PointerEventData eventData)
    {
        SetHealthAnimated(UnityEngine.Random.value);
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
