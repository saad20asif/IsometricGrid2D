using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScrollWheel : MonoBehaviour
{
    [SerializeField]OrientationSo OrientationSo;
    public Orientation CurrentOrientation;
    public float ScrollThreshold = 0.5f; // Adjust this threshold as needed
    public static Action OrientationFlipped;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) >= ScrollThreshold)
        {
            if (scroll > 0)
            {
                // Scroll up, change to Vertical orientation
                ChangeOrientation(Orientation.Vertical);
            }
            else if (scroll < 0)
            {
                // Scroll down, change to Horizontal orientation
                ChangeOrientation(Orientation.Horizontal);
            }
        }
    }

    private void ChangeOrientation(Orientation newOrientation)
    {
        if (CurrentOrientation != newOrientation)
        {
            CurrentOrientation = newOrientation;
            OrientationSo.CurrentOrientation = newOrientation;
            if(OrientationFlipped != null)
                OrientationFlipped();
        }
    }
}
