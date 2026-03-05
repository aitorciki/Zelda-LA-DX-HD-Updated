using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Things;
using ProjectZ.InGame.Map;

namespace ProjectZ.InGame.Things
{
    public class CameraField
    {
        public List<GameObject> GameObjectList = new List<GameObject>();
        public List<ObjCameraField> CameraFieldList = new List<ObjCameraField>();

        public Vector2 CameraFieldCoords;

        public void AddToList(ObjCameraField fieldCenter)
        {
            CameraFieldList.Add(fieldCenter);
        }

        public void SetClosestCoords()
        {
            // If the list is empty, there is no camera coords to obtain.
            if (CameraFieldList.Count <= 0)
            {
                CameraFieldCoords = Vector2.Zero;
                return;
            }
            // Get Link's position to determine the closest camera.
            Vector2 playerPos = MapManager.ObjLink.CenterPosition.Position;
            ObjCameraField closestCam = null;
            float closestDist = float.MaxValue;

            // Loop through all the camera objects found.
            foreach (ObjCameraField fieldCenter in CameraFieldList)
            {
                // If there are multiple camera objects, find the closest camera to Link.
                float dist = Vector2.Distance(playerPos, fieldCenter.EntityPosition.Position);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestCam = fieldCenter;
                }
            }
            // If the closest camera is null, there is no camera coords to obtain.
            if (closestCam == null)
            {
                CameraFieldCoords = Vector2.Zero;
                return;
            }
            // Return the coordinates of the closest camera.
            CameraFieldCoords = new Vector2(closestCam.EntityPosition.X, closestCam.EntityPosition.Y);
        }

        public void FindClosestCoords()
        {
            // Always reset. If we fail to find anything, coords become Zero (not stale).
            CameraFieldList.Clear();
            CameraFieldCoords = Vector2.Zero;

            var map = MapManager.ObjLink?.Map;
            var objects = map?.Objects;
            if (objects == null)
                return;

            var player = MapManager.ObjLink.CenterPosition.Position;
            var objList = objects.GetObjects((int)player.X - 160, (int)player.Y - 100, 320, 200);

            // Loop through the game objects.
            foreach (var gameObject in objList)
                if (gameObject is ObjCameraField camField)
                    CameraFieldList.Add(camField);

            // Set the closest camera to Link.
            SetClosestCoords();
        }

        public void ClearList()
        {
            // Clear the camera field object list and clear properties.
            CameraFieldList.Clear();
            CameraFieldCoords = Vector2.Zero;
        }
    }
}
