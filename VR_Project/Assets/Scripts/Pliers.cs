using UnityEngine;

public class Pliers : MonoBehaviour {

    private Animator animator;
    private float oldVal;
    private float newVal;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start () {
        oldVal = 0f;
        newVal = 0f;
    }

    public bool Cutting
    {
        get { return oldVal < newVal && newVal > 0.9; }
    }

    public void SetClosedValue(float val)
    {
        oldVal = newVal;
        if (animator.runtimeAnimatorController != null)
            animator.Play("Pinch", 0, val);
        newVal = val;
    }
}
