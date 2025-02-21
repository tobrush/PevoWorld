using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSplitScreen
{
    public class SplitScreenController3D : SplitScreenController
    {
        public override void Initialize(SplitScreenManager manager, Material split)
        {
            base.Initialize(manager, split);

            // camera orientation
            worldUp = Vector3.up;
            cameraDistanceVector = worldUp * SplitScreenManager.CameraDistance;
        }

        protected override void CreateRenderTextures(int width, int height)
        {
            splitRT = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        }

        public override bool RegisterCamera(int playerIndex, PlayerCamera camera)
        {
            if (base.RegisterCamera(playerIndex, camera))
            {
                playerCameraComponent[playerIndex - 1].orthographic = false;
                playerCameraComponent[playerIndex - 1].fieldOfView = splitScreenCamera.fieldOfView;
                playerCameraComponent[playerIndex - 1].aspect = splitScreenCamera.aspect;
                return true;
            }

            return false;
        }

        public override void Update(float cameraDistance, Vector3 player1Position, Vector3 player2Position, float splitRatio, bool singlePlayer, ref bool splitScreenLocked, ref float splitAngle)
        {
            Vector2 player1Position2D = new Vector2(player1Position.x, player1Position.z);
            Vector2 player2Position2D = new Vector2(player2Position.x, player2Position.z);
            Vector3 midPoint = Vector3.Lerp(player1Position2D, player2Position2D, 0.5f);
            float xAngle = splitManager.XAngle;

            if (!singlePlayer)
            {
                float screenSizeInWorldUnits = cameraDistance * Mathf.Tan(Mathf.Deg2Rad * splitScreenCamera.fieldOfView / 2);
                float player1ScreenSizeInWorldUnits = playerCamera[PLAYER_1_INDEX].CameraDistance * Mathf.Tan(Mathf.Deg2Rad * splitScreenCamera.fieldOfView / 2);
                float player2ScreenSizeInWorldUnits = playerCamera[PLAYER_2_INDEX].CameraDistance * Mathf.Tan(Mathf.Deg2Rad * splitScreenCamera.fieldOfView / 2);

                // get current values of relevant shit from manager
                bool lockSplitScreen = splitManager.LockSplitScreen;
                bool lockSplitAngle = splitManager.LockSplitAngle;
                bool overrideSplitAngle = splitManager.OverrideSplitAngle;

                // calculate minimum distance required for split screen
                Vector2 playerPositionDifference = (player1Position2D - player2Position2D);
                playerPositionDifference.Normalize();
                minimumSplitScreenDistance = playerPositionDifference.magnitude * screenSizeInWorldUnits * splitManager.MinimumSplitScreenDistanceMultiplier;

                Vector3 playerPositionDifferenceA = playerPositionDifference;
                playerPositionDifferenceA.x *= aspectRatio;

                float normalSplitScreenDistance = playerPositionDifferenceA.magnitude * screenSizeInWorldUnits;

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
                        splitAngle = Vector3.SignedAngle(new Vector3(playerPositionDifference.x, 0, playerPositionDifference.y), -Vector3.right, worldUp);
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
                        playerPositionDifference = new Vector2(-newDif.x, newDif.z);
                        
                        centerOffset = new Vector2(playerPositionDifference.y, -playerPositionDifference.x);
                        centerOffset = centerOffset.normalized * transitionSplitRatio * 0.5f;
                    }

                    if (!lockSplitScreen || !splitScreenLocked)
                    {
                        // split line position
                        Vector3 splitPos = new Vector2(Mathf.Lerp(-screenX, screenX, (1 + centerOffset.y) / 2), -Mathf.Lerp(-screenY, screenY, (1 + centerOffset.x) / 2));
                        SplitLine.Position = splitPos;

                        // move player cameras
                        lastCam1Distance = player1ScreenSizeInWorldUnits * (1 - ((1 + transitionSplitRatio) / 2));
                        lastCam2Distance = player2ScreenSizeInWorldUnits * ((1 + transitionSplitRatio) / 2);
                    }

                    Vector3 p1Dif, p2Dif;

                    // calculate player position 
                    if (lockSplitScreen || overrideSplitAngle || lockSplitAngle)
                    {
                        Vector2 iplayer1Position = new Vector2(playerPositionDifference.x, playerPositionDifference.y);
                        Vector2 iplayer2Position = Vector2.zero;
                        Vector2 imidPoint = Vector2.Lerp(iplayer1Position, iplayer2Position, 0.5f);
                        p1Dif = (iplayer1Position - imidPoint).normalized;
                        p2Dif = (iplayer2Position - imidPoint).normalized;
                        p1Dif.z = p1Dif.y;
                        p1Dif.y = 0;
                        p2Dif.z = p2Dif.y;
                        p2Dif.y = 0;
                    }
                    else
                    {
                        p1Dif = new Vector3(playerPositionDifference.x, 0, playerPositionDifference.y).normalized;
                        p2Dif = new Vector3(playerPositionDifference.x, 0, playerPositionDifference.y).normalized * -1;
                    }

                    // calculate offsets
                    if (!lockSplitScreen || !splitScreenLocked)
                    {
                        lastPlayer1CameraPositionOffset = new Vector3(p1Dif.x * transitionAspectRatio * smoothTransitionRatio, 0, p1Dif.z * smoothTransitionRatio);
                        lastPlayer2CameraPositionOffset = new Vector3(p2Dif.x * transitionAspectRatio * smoothTransitionRatio, 0, p2Dif.z * smoothTransitionRatio);
                    }

                    // smooth out camera Y positions for transition
                    Vector3 player1DistanceVector = playerCamera[PLAYER_1_INDEX].CameraDistance * worldUp;
                    Vector3 player2DistanceVector = playerCamera[PLAYER_2_INDEX].CameraDistance * worldUp;

                    // offset camera due to xAngle
                    Vector3 angleOffset1 = new Vector3(0, 0, player1DistanceVector.magnitude * Mathf.Tan(Mathf.Deg2Rad * (90 - xAngle)));
                    Vector3 angleOffset2 = new Vector3(0, 0, player2DistanceVector.magnitude * Mathf.Tan(Mathf.Deg2Rad * (90 - xAngle)));

                    // calculate merged camera distance
                    float yPosDif = Mathf.Lerp(player1Position.y - player2Position.y, 0, transitionRatio);
                    Vector3 heightDif = new Vector3(0, yPosDif, 0);

                    // update camera positions
                    playerCamera[PLAYER_1_INDEX].UpdatePosition(player1Position - (lastPlayer1CameraPositionOffset * lastCam1Distance) - angleOffset1 - heightDif/2, worldUp, cameraDistance, lockSplitScreen);
                    playerCamera[PLAYER_2_INDEX].UpdatePosition(player2Position - (lastPlayer2CameraPositionOffset * lastCam2Distance) - angleOffset2 + heightDif/2, worldUp, cameraDistance, lockSplitScreen);

                    // update camera rotations
                    playerCameraTransform[PLAYER_1_INDEX].rotation = Quaternion.Euler(xAngle, playerCameraTransform[PLAYER_1_INDEX].rotation.eulerAngles.y, playerCameraTransform[PLAYER_1_INDEX].rotation.eulerAngles.z);
                    playerCameraTransform[PLAYER_2_INDEX].rotation = Quaternion.Euler(xAngle, playerCameraTransform[PLAYER_2_INDEX].rotation.eulerAngles.y, playerCameraTransform[PLAYER_2_INDEX].rotation.eulerAngles.z);

                    // set split screen locked
                    if (lockSplitScreen && !splitScreenLocked)
                    {
                        splitScreenLocked = true;
                    }

                    return;
                }
            }

            // main camera rotation
            splitScreenCamera.transform.rotation = Quaternion.Euler(xAngle, splitScreenCamera.transform.rotation.eulerAngles.y, splitScreenCamera.transform.rotation.eulerAngles.z);

            // main camera position
            float zPositionOffset = cameraDistanceVector.magnitude * Mathf.Tan(Mathf.Deg2Rad * (90 - xAngle));
            splitScreenCamera.transform.position = new Vector3(midPoint.x, Mathf.Lerp(player1Position.y, player2Position.y, .5f), midPoint.y - zPositionOffset);
            splitScreenCamera.transform.position += cameraDistanceVector;
        }
    }
}
