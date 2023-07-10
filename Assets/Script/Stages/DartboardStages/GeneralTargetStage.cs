using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;
using System;

public class GeneralTargetStage : Stage {
    private float timeElapsedTotal, timeElapsedUser;
    private float distance, size, xAng, yAng, userViewTime, maxTime; 
    public bool userSucceeded;
    public GeneralTargetStage(float distance, float size, float xAng, float yAng, float userViewTime, float maxTime) {
        this.distance = distance;
        this.size = size;
        this.xAng = xAng;
        this.yAng = yAng;
        this.userViewTime = userViewTime;
        this.maxTime = maxTime;
    }
    public GeneralTargetStage getWithDistance(float dis) {
        return new GeneralTargetStage(dis, size, xAng, yAng, userViewTime, maxTime);
    }
    public GeneralTargetStage getWithSize(float sz) {
        return new GeneralTargetStage(distance, sz, xAng, yAng, userViewTime, maxTime);
    }
    public override void start() {
        StageStatic.GameObjects["startButton"].SetActive(false);
        StageStatic.GameObjects["instructions"].SetActive(false);
        StageStatic.GameObjects["target"].SetActive(true);
        timeElapsedTotal = 0;
        timeElapsedUser = 0;
        StageStatic.moveDartboardTo(distance, size, xAng, yAng);
    }
    public override void update() {
        timeElapsedTotal += Time.deltaTime;

        if (!StageStatic.relativeToWorld) {
            StageStatic.moveDartboardTo(distance, size, xAng, yAng);
        }

        if (StageStatic.EyeDataCol.worldPosL != new Vector3(0, 0, 0)) {
            StageStatic.GameObjects["left"].SetActive(true);
            StageStatic.GameObjects["left"].transform.position = StageStatic.EyeDataCol.worldPosL;
        }
        if (StageStatic.EyeDataCol.worldPosR != new Vector3(0, 0, 0)) {
            StageStatic.GameObjects["right"].SetActive(true);
            StageStatic.GameObjects["right"].transform.position = StageStatic.EyeDataCol.worldPosR;
        }

        int leftNum = -1;
        int rightNum = -1;
        if (StageStatic.EyeDataCol.lObjectName.Length > 6) {
            int.TryParse(StageStatic.EyeDataCol.lObjectName.Substring(6), out leftNum);
        }
        if (StageStatic.EyeDataCol.rObjectName.Length > 6) {
            int.TryParse(StageStatic.EyeDataCol.rObjectName.Substring(6), out rightNum);
        }

        if ((StageStatic.hasActiveUser && (leftNum < 1 || rightNum < 1)) || (!StageStatic.hasActiveUser && !Input.GetKey(KeyCode.A))) {
            timeElapsedUser = -0.1f;
        } else {
            timeElapsedUser += Time.deltaTime;
            if (timePassedNumber()) {
                StageStatic.stopAllAudio();
                StageStatic.Audios["" + (userViewTime - Math.Truncate(timeElapsedUser))].Play();
            }
        }
    }
    private bool timePassedNumber() {
        return (timeElapsedUser - Time.deltaTime < Math.Truncate(timeElapsedUser));
    }
    public override bool finished() {
        userSucceeded = timeElapsedUser >= userViewTime;
        return timeElapsedTotal >= maxTime || timeElapsedUser >= userViewTime;
    }
    public override void end() { }
}