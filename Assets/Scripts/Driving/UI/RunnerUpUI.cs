using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The runner up UI is designed to display the racers
/// immediately behind this racer using a simple, 
/// easy to read UI
/// </summary>
public class RunnerUpUI : DrivingModule
{
    #region Private Properties
    private float CanvasWidthToHeight => canvas.pixelRect.width / canvas.pixelRect.height;
    private float RunnerUpSpaceHeight => distanceRange.length;
    // Compute the width of the runner up space behind the car
    // based on the ratio of the canvas width to height
    private float RunnerUpSpaceWidth => RunnerUpSpaceHeight * CanvasWidthToHeight;
    // Maximum distance that can exist between this racer and any indicated runners up
    private float MaxRunnerUpDistance
    {
        get
        {
            float w = RunnerUpSpaceWidth / 2f;
            float h = RunnerUpSpaceHeight;
            return Mathf.Sqrt(w * w + h * h);
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the canvas that will contain the ui " +
        "for each runner up")]
    private Canvas canvas;
    [SerializeField]
    [Tooltip("Direct parent of the runner up icons")]
    private Transform runnerUpIconParent;
    [SerializeField]
    [Tooltip("Icon prefab to use for each runner up icon")]
    private RunnerUpIcon runnerUpIconPrefab;
    [SerializeField]
    [Tooltip("Min-max range of the ui. Racers closer than the min " +
        "and farther than the max do not show up as runner ups to this driver")]
    private FloatRange distanceRange;
    [SerializeField]
    [Tooltip("The minimum possible size of the runner up icon")]
    private float minimumIconSize = 0.1f;
    #endregion

    #region Private Fields
    private DrivingManager[] otherDrivers = new DrivingManager[0];
    private RunnerUpIcon[] icons = new RunnerUpIcon[0];
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Listen for drivers being registered and deregistered
        manager.DriverRegisteredEvent.AddListener(OnDriverRegistryChanged);
        manager.DriverDeregisteredEvent.AddListener(OnDriverRegistryChanged);
    }
    private void Update()
    {
        // IMPORTANT
        // This will not work over the network because the number of drivers is volatile!
        // They might leave the game, resulting in null reference exceptions.
        // This also won't work if another racer joins a game... except they cannot join a race in progress
        // So the only issue would be that you could not see the correct runner up UI in the lobby
        // while people are joining because the icons does not resize and neither does the 
        // number of drivers

        for (int i = 0; i < otherDrivers.Length; i++)
        {
            // Get the other driver's runner up coordinates
            Vector3 runnerUpCoordinates = RunnerUpCoordinates(otherDrivers[i]);
            bool display = RunnerUpCoordinateIsDisplayable(runnerUpCoordinates);

            // If we display the icon then move it to the right place
            if (display)
            {
                // Compute the canvas position of the icon
                Vector2 anchor = CanvasCoordinate(runnerUpCoordinates);
                icons[i].SetAnchoredPosition(anchor);

                // Compute the scale of the icon
                icons[i].transform.localScale = Vector3.one * RunnerUpIconSize(runnerUpCoordinates);
            }

            // Display the icon if this runner up can be displayed
            icons[i].gameObject.SetActive(display);
        }
    }
    // Draw the space for the runners up
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        DrivingManager manager = GetComponentInParent<DrivingManager>();

        if (manager)
        {
            float maxX = RunnerUpSpaceWidth / 2f;
            Vector3 topMiddle = manager.rigidbody.position - manager.heading * distanceRange.min;
            Vector3 topLeft = manager.rigidbody.position - manager.right * maxX - manager.heading * distanceRange.min;
            Vector3 topRight = manager.rigidbody.position + manager.right * maxX - manager.heading * distanceRange.min;
            Vector3 bottomLeft = topLeft - manager.heading * distanceRange.max;
            Vector3 bottomRight = topRight - manager.heading * distanceRange.max;

            Gizmos.DrawLine(topMiddle, bottomLeft);
            Gizmos.DrawLine(topMiddle, bottomRight);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topLeft, bottomLeft);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomLeft, bottomRight);
        }
    }
    #endregion

    #region Event Listeners
    private void OnDriverRegistryChanged(DrivingManager newDriver)
    {
        // Destroy any existing icons
        foreach(RunnerUpIcon icon in icons)
        {
            Destroy(icon.gameObject);
        }

        // Create a list with all drivers besides this one
        otherDrivers = DriverRegistry.Registry
            .Where(driver => driver != manager)
            .ToArray();

        // Create a new array to hold the icons
        icons = new RunnerUpIcon[otherDrivers.Length];

        // Instantiate an icon for each driver
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i] = Instantiate(runnerUpIconPrefab, runnerUpIconParent);
            icons[i].DisplayDriver(otherDrivers[i]);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Coordinates of the other driver
    /// relative to the forward-right-up direction of this driver
    /// </summary>
    /// <returns></returns>
    private Vector3 RunnerUpCoordinates(DrivingManager otherDriver)
    {
        Vector3 toThem = otherDriver.rigidbody.position - manager.rigidbody.position;

        // Get the vector that points to them 
        // relative to the manager's coordinate system
        return new Vector3(
            Vector3.Dot(toThem, manager.right),
            Vector3.Dot(toThem, manager.up),
            Vector3.Dot(toThem, manager.heading));
    }
    /// <summary>
    /// Determine if a runner up at the determined coordinate 
    /// should be displayed in the UI
    /// </summary>
    /// <param name="runnerUpCoordinate"></param>
    /// <returns></returns>
    private bool RunnerUpCoordinateIsDisplayable(Vector3 runnerUpCoordinate)
    {
        // Take absolute value of the coordinates
        float x = Mathf.Abs(runnerUpCoordinate.x);
        float z = -runnerUpCoordinate.z;
        return x < (RunnerUpSpaceWidth / 2f) &&
            z > distanceRange.min &&
            z < distanceRange.max;
    }
    /// <summary>
    /// Convert a 3D runner up coordinate to its position on the 
    /// edges of the canvas, relative to an anchor point
    /// at the top-center of the canvas
    /// </summary>
    /// <param name="runnerUpCoordinate"></param>
    /// <returns></returns>
    private Vector2 CanvasCoordinate(Vector3 runnerUpCoordinate)
    {
        // Compute the x interpolator between left and right of canvas
        float xInterpolator = runnerUpCoordinate.x / (RunnerUpSpaceWidth / 2f);
        // Compute the y interpolator between top and bottom of canvas
        float yInterpolator = distanceRange.GetInterpolator(Mathf.Abs(runnerUpCoordinate.z));
        // Half the width of the canvas, used both if statement branches
        float halfCanvasWidth = canvas.pixelRect.width / 2f;

        Vector2 anchor = new Vector2(
            // Interpolate between middle and right of canvas
            // (xInterpolator may go negative, going to left side of canvas)
            Mathf.LerpUnclamped(0f, halfCanvasWidth, xInterpolator),
            // Interpolate the height based on where the runner up is
            Mathf.LerpUnclamped(0f, -canvas.pixelRect.height, yInterpolator));

        // Compute the absolute value of the anchor,
        // in preparation for a similar triangles computation
        Vector2 absAnchor = anchor.Abs();
        float myWidthToHeight = absAnchor.x / absAnchor.y;

        // If my ratio is bigger than canvas width to height ratio,
        // then the coordinate should be on the side of the screen
        if (myWidthToHeight > CanvasWidthToHeight / 2f)
        {
            float myHeightToWidth = 1f / myWidthToHeight;
            anchor.y = -(halfCanvasWidth * myHeightToWidth);
            anchor.x = halfCanvasWidth * Mathf.Sign(anchor.x);
        }
        // If my ratio is smaller than canvas width to height ratio,
        // then the coordinate should be on the bottom of the screen
        else
        {
            anchor.x = canvas.pixelRect.height * myWidthToHeight * Mathf.Sign(anchor.x);
            anchor.y = -canvas.pixelRect.height;
        }

        return anchor;
    }
    private float RunnerUpIconSize(Vector3 runnerUpCoordinates) 
    {
        float interpolator = runnerUpCoordinates.magnitude / MaxRunnerUpDistance;
        return Mathf.Lerp(1f, minimumIconSize, interpolator);
    }
    #endregion
}
