using UnityEngine;

public abstract class AquaFaultBase : MonoBehaviour
{
    public enum FaultState { Inactive, Active, Resolved, Failed }
    public enum Severity { Low, Medium, High, Critical }

    [Header("Reference")]
    [SerializeField] protected WakeCycleManager wakeCycle;

    [Header("Runtime")]
    public FaultState state = FaultState.Inactive;
    public Severity severity = Severity.Low;
    public int stress = 0;

    [Header("Rules")]
    [Tooltip("each year stress is added.")]
    public int stressPerYear = 1;

    [Tooltip("If stress reaches this value, the fault fails.")]
    public int failAtStress = 5;

    protected virtual void OnEnable()
    {
        if (wakeCycle == null)
        {
            Debug.LogError($"{name}: wakecycle.");
            enabled = false;
            return;
        }

        wakeCycle.OnYearChange += HandleYearChange;
    }

    protected virtual void OnDisable()
    {
        if (wakeCycle != null)
            wakeCycle.OnYearChange -= HandleYearChange;
    }

    public virtual void Activate()
    {
        state = FaultState.Active;
        stress = 0;
        severity = Severity.Low;
        Debug.Log($"{name}: Activated");
    }

    public virtual void Deactivate()
    {
        state = FaultState.Inactive;
        Debug.Log($"{name}: Deactivated");
    }

    private void HandleYearChange(int newYear)
    {
        if (state != FaultState.Active) return;

        OnNewYear(newYear);
        RecomputeSeverity();

        if (stress >= failAtStress)
        {
            state = FaultState.Failed;
            Debug.Log($"{name}: FAILED on year {newYear} (stress={stress})");
        }
        else
        {
            Debug.Log($"{name}: Year {newYear} progressed (stress={stress}, severity={severity})");
        }
    }

    protected virtual void OnNewYear(int newYear)
    {
        stress += stressPerYear;
    }

    protected virtual void RecomputeSeverity()
    {
        severity =
            stress >= 4 ? Severity.Critical :
            stress >= 3 ? Severity.High :
            stress >= 2 ? Severity.Medium :
            Severity.Low;
    }

    public string StatusString()
        => $"{name}: state={state}, severity={severity}, stress={stress}";
}