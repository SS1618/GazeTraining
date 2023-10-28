using System;
using UnityEngine;

public class IPDSetupStage : Stage
{
    private float distance;

    public IPDSetupStage(float distance=10) {
        this.distance = distance;
    }
    
    public override void start() {
        StageStatic.GameObjects["startButton"].SetActive(false);
        StageStatic.GameObjects["instructions"].SetActive(false);
        StageStatic.GameObjects["target"].SetActive(true);
    }

    public override void update() {
        StageStatic.moveDartboardTo(distance, 5, 0, 0);
    }
    
    public override bool finished() {
        return Input.GetKeyDown(KeyCode.A);
    }

    public override void end() {
        // User thinks they are looking at the dartboard with their eye now
        
        // Get the local direction that the user is facing
        Vector3 dir = StageStatic.EyeDataCol.L_Direction;
        // Compare the local direction that the user is looking to the local direction that the dartboard is in
        var theta = Vector3.Angle(dir, Vector3.forward);
        theta = Math.Abs(theta) * (float)Math.PI/180; // Based on left/right eye, just modify
        // Find the final amount based on this and set the final IPD to this
        var ipd = (float)Math.Tan(theta) * distance;
        Debug.Log("Calculated IPD: " + ipd);
        StageStatic.setIPD(ipd);
    }
}
