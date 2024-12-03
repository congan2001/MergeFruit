using UnityEngine;
using UnityEngine.UI;

public class Tilt : MonoBehaviour
{
    public float tiltForce;
    public float tiltDuration;
    public float tiltLeftRemaining, tiltRightRemaining;
    public Text tiltLeftNumber, tiltRightNumber;
    public bool tiltLeftCondition = true, tiltRightCondition = true;
    public Vector2 tiltLeft = new Vector2(-1, 0);
    public Vector2 tiltRight = new Vector2(1, 0);
    private Vector2 originalGravity;

    public void Start()
    {
        originalGravity = Physics2D.gravity;
    }
    public void TiltLeft()
    {
        if (tiltLeftCondition)
        {
            tiltLeftRemaining--;
            tiltLeftNumber.text = $"{tiltLeftRemaining}";
            Physics2D.gravity = tiltLeft.normalized * tiltForce;
            Invoke("ResetGravity", tiltDuration);
        }
        if (tiltLeftRemaining == 0)
        {
            tiltLeftCondition = false;
        }
    }
    public void TiltRight()
    {
        if (tiltRightCondition)
        {
            tiltRightRemaining--;
            tiltRightNumber.text = $"{tiltRightRemaining}";
            Physics2D.gravity = tiltRight.normalized * tiltForce;
            Invoke("ResetGravity", tiltDuration);
        }
        if (tiltRightRemaining == 0)
        {
            tiltRightCondition = false;
        }
    }
    public void ResetGravity()
    {
        Physics2D.gravity = originalGravity;
    }
}
