using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSplitScreen
{
    public class SplitScreenController2D : SplitScreenController
    {
        public override void Initialize(SplitScreenManager manager, Material split)
        {
            base.Initialize(manager, split);

            // camera orientation
            worldUp = -Vector3.forward;
            cameraDistanceVector = worldUp * SplitScreenManager.CameraDistance;
        }

        protected override void CreateRenderTextures(int width, int height)
        {
            splitRT = new RenderTexture(SplitScreenManager.ScreenWidth, SplitScreenManager.ScreenHeight, 0, RenderTextureFormat.ARGB32);
        }

        public override bool RegisterCamera(int playerIndex, PlayerCamera camera)
        {
            if (base.RegisterCamera(playerIndex, camera))
            {
                playerCameraComponent[playerIndex - 1].orthographic = true;
                playerCameraComponent[playerIndex - 1].orthographicSize = splitScreenCamera.orthographicSize;
                return true;
            }

            return false;
        }

        public override void Update(float cameraDistance, Vector3 player1Position, Vector3 player2Position, float splitRatio, bool singlePlayer, ref bool splitScreenLocked, ref float splitAngle)
        {
            Vector2 player1Position2D = player1Position;
            Vector2 player2Position2D = player2Position;
            Vector2 midPoint = Vector2.Lerp(player1Position2D, player2Position2D, 0.5f);

            if (!singlePlayer)
            {
                // get current values of relevant shit from manager
                bool lockSplitScreen = splitManager.LockSplitScreen;
                bool lockSplitAngle = splitManager.LockSplitAngle;
                bool overrideSplitAngle = splitManager.OverrideSplitAngle;
                float mainCameraOrthoSize = splitScreenCamera.orthographicSize;

                // calculate minimum distance required for split screen
                Vector2 playerPositionDifference = (player1Position2D - player2Position2D);

                playerPositionDifference.Normalize();
                minimumSplitScreenDistance = playerPositionDifference.magnitude * mainCameraOrthoSize * splitManager.MinimumSplitScreenDistanceMultiplier;

                Vector2 playerPositionDifferenceA = playerPositionDifference;
                playerPositionDifferenceA.x *= aspectRatio;

                float normalSplitScreenDistance = playerPositionDifferenceA.magnitude * mainCameraOrthoSize;

                float playersDistance = Vector2.Distance(player1Position2D, player2Position2D);
                float transitionRatio = lockSplitScreen ? 1 : Mathf.InverseLerp(minimumSplitScreenDistance, normalSplitScreenDistance, playersDistance);
                float extendedTransitionRatio = lockSplitScreen ? 1 : Mathf.InverseLerp(minimumSplitScreenDistance, normalSplitScreenDistance * 2.5f, playersDistance);

                bool shouldBeSplit = playersDistance > minimumSplitScreenDistance || lockSplitScreen || lockSplitAngle;
                SplitScreenState = SplitScreenState != SplitScreenState.Disabled ?
                    (enableSplitScreen && shouldBeSplit ? SplitScreenState.Split : SplitScreenState.Single) : SplitScreenState.Disabled;

                if (SplitScreenState == SplitScreenState.Split)
                {
                    // set main camera position
                    splitScreenCamera.transform.position = cameraDistanceVector;

                    // calculate values for smooth transition in and out of split screen
                    if (!lockSplitScreen)
                    {
                        transitionAspectRatio = Mathf.Lerp(1, aspectRatio, transitionRatio);
                        transitionSplitRatio = Mathf.Lerp(0, splitRatio, transitionRatio);
                        smoothTransitionRatio = Mathf.Lerp(.75f, 1, transitionRatio);
                    }
                    playerCamera[PLAYER_1_INDEX].TransitionRatio = extendedTransitionRatio * smoothTransitionRatio;
                    playerCamera[PLAYER_2_INDEX].TransitionRatio = extendedTransitionRatio * smoothTransitionRatio;

                    // calculate split line offset and angle
                    if (!lockSplitScreen && !lockSplitAngle)
                    {
                        centerOffset = new Vector2(playerPositionDifference.y, -playerPositionDifference.x);
                        centerOffset = centerOffset.normalized * transitionSplitRatio * 0.5f;
                        splitAngle = Vector3.SignedAngle(playerPositionDifference, -Vector3.right, worldUp);
                        SplitLine.Rotation = Quaternion.Euler(0, 0, splitAngle);
                    }
                    else if (lockSplitAngle)
                    {
                        if (overrideSplitAngle && !lockSplitScreen)
                        {
                            splitAngle = splitAngle % 360;
                            splitAngle += splitAngle < 0 ? 360 : 0;
                            SplitLine.Rotation = Quaternion.Euler(0, 0, splitAngle);
                        }

                        Vector3 newDif = Quaternion.AngleAxis(splitAngle, worldUp) * Vector3.right;
                        playerPositionDifference = new Vector2(-newDif.x, newDif.y);

                        centerOffset = new Vector2(playerPositionDifference.y, -playerPositionDifference.x);
                        centerOffset = centerOffset.normalized * transitionSplitRatio * 0.5f;
                    }

                    if (!lockSplitScreen || !splitScreenLocked)
                    {
                        // split line position
                        Vector3 splitPos = new Vector2(Mathf.Lerp(-screenX, screenX, (1 + centerOffset.y) / 2), -Mathf.Lerp(-screenY, screenY, (1 + centerOffset.x) / 2));
                        SplitLine.Position = splitPos;

                        // move player cameras
                        lastCam1Distance = playerCameraComponent[PLAYER_1_INDEX].orthographicSize * (1 - ((1 + transitionSplitRatio) / 2));
                        lastCam2Distance = playerCameraComponent[PLAYER_2_INDEX].orthographicSize * ((1 + transitionSplitRatio) / 2);
                    }

                    Vector2 p1Dif, p2Dif;

                    // calculate player position 
                    if (lockSplitScreen || overrideSplitAngle || lockSplitAngle)
                    {
                        Vector2 iplayer1Position = new Vector2(playerPositionDifference.x, playerPositionDifference.y);
                        Vector2 iplayer2Position = Vector2.zero;
                        Vector2 imidPoint = Vector2.Lerp(iplayer1Position, iplayer2Position, 0.5f);
                        p1Dif = (iplayer1Position - imidPoint).normalized;
                        p2Dif = (iplayer2Position - imidPoint).normalized;
                    }
                    else
                    {
                        p1Dif = playerPositionDifference.normalized;
                        p2Dif = playerPositionDifference.normalized * -1;
                    }

                    // calculate offsets
                    if (!lockSplitScreen || !splitScreenLocked)
                    {
                        lastPlayer1CameraPositionOffset = new Vector3(p1Dif.x * transitionAspectRatio * smoothTransitionRatio, p1Dif.y * smoothTransitionRatio);
                        lastPlayer2CameraPositionOffset = new Vector3(p2Dif.x * transitionAspectRatio * smoothTransitionRatio, p2Dif.y * smoothTransitionRatio);
                    }

                    // update camera position
                    playerCamera[PLAYER_1_INDEX].UpdatePosition(player1Position2D - ((Vector2)lastPlayer1CameraPositionOffset * lastCam1Distance), worldUp, cameraDistance, lockSplitScreen);
                    playerCamera[PLAYER_2_INDEX].UpdatePosition(player2Position2D - ((Vector2)lastPlayer2CameraPositionOffset * lastCam2Distance), worldUp, cameraDistance, lockSplitScreen);

                    // set split screen locked
                    if (lockSplitScreen && !splitScreenLocked)
                    {
                        splitScreenLocked = true;
                    }
                }
            }

            // main camera position
            splitScreenCamera.transform.position = new Vector3(midPoint.x, midPoint.y, Mathf.Lerp(player1Position.z, player2Position.z, .5f));
            splitScreenCamera.transform.position += cameraDistanceVector;
        }
    }
}
