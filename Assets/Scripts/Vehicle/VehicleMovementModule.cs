using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleMovementModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Torque applied to the wheels to get the car moving")]
    private float thrust = 300f;
    [SerializeField]
    [Tooltip("Max angle that the wheels can turn at")]
    private float maxSteerAngle = 60f;

    [SerializeField]
    [Tooltip("Wheels on the front axle")]
    private VehicleAxle frontAxle;
    [SerializeField]
    [Tooltip("Wheels on the back axle")]
    private VehicleAxle backAxle;
    [SerializeField]
    [Tooltip("Type of drive for the vehicle")]
    private VehicleDriveType driveType;

    private Rigidbody m_Rigidbody;
    private List<VehicleAxle> m_motorAxles = new List<VehicleAxle>();

    // Get the axles that drive the motor and brake
    private List<VehicleAxle> motorAxles
    {
        get
        {
            // If no axles exist in the local list, set it up
            if(m_motorAxles.Count <= 0)
            {
                if(driveType == VehicleDriveType.FrontWheelDrive)
                {
                    m_motorAxles.Add(frontAxle);
                }
                if(driveType == VehicleDriveType.BackWheelDrive)
                {
                    m_motorAxles.Add(backAxle);
                }
                if(driveType == VehicleDriveType.FourWheelDrive)
                {
                    m_motorAxles.Add(frontAxle);
                    m_motorAxles.Add(backAxle);
                }
            }
            return m_motorAxles;
        }
    }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Drive the car forward-backward by t ranging from -1 (max back up speed) to 1 (max forward speed)
    public void Drive(float t)
    {
        foreach(VehicleAxle axle in motorAxles)
        {
            axle.Motor(t * thrust);
        }
    }

    // Steer the car left to right by t ranging from -1 (farthest left) to 1 (farthest right)
    public void Steer(float t)
    {
        frontAxle.Steer(t * maxSteerAngle);
    }

    public void CleanUp()
    {
        // If the car is facing slightly upside-down, correct it to face right side up again and remove angular velocity
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            m_Rigidbody.rotation = Quaternion.AngleAxis(0f, transform.right);
            m_Rigidbody.position += Vector3.up * 5f;
        }
    }
}

public enum VehicleDriveType
{
    FrontWheelDrive,
    BackWheelDrive,
    FourWheelDrive
}
