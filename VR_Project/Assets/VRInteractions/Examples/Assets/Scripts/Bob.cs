using UnityEngine;
using System.Collections;

/// <summary>
/// Component makes object bob randomly.
/// </summary>
public class Bob : MonoBehaviour {

    private Vector3 MinPos;
    public Vector3 MaxPos;
    public float CycleTime = 2;
    private Vector3 CurrentPos;
    private Vector3 StartPos;
    float currentTime = 0;
    bool Up = true;

    void OnEnable()
    {
        //StartPos = transform.position;
        StartPos = Vector3.zero;
        MinPos = transform.localPosition;
        MaxPos = transform.localPosition + MaxPos;

        
    }

    void LerpToPosition(Vector3 _pos)
    {
        if (StartPos == Vector3.zero)
            StartPos = transform.localPosition;

        CurrentPos = Vector3.Lerp(StartPos, _pos, currentTime / (CycleTime / 2));
        //transform.localPosition = CurrentPos;
        transform.localPosition = CurrentPos;
    }

    void Update()
    {
        CurrentPos = transform.localPosition;

        if (Up == true)
        {

            LerpToPosition(MaxPos);
        }
        else
        {
            LerpToPosition(MinPos);
        }

        currentTime += Time.deltaTime;

        if (currentTime >= (CycleTime / 2))
        {
            currentTime = 0;
            Up = !Up;

            StartPos = Vector3.zero;
        }

    }
}
