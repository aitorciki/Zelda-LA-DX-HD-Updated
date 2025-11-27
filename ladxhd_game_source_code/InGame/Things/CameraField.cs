using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Things;
using ProjectZ.InGame.Map;

namespace ProjectZ.InGame.Things
{
    public class CameraField
    {
        public List<ObjCameraField> CameraFieldList = new List<ObjCameraField>();
        public Vector2 CameraFieldCoords;
        public bool CameraFieldLock;

        public void AddToList(ObjCameraField fieldCenter, bool cameraLock)
        {
            CameraFieldList.Add(fieldCenter);
            CameraFieldLock = cameraLock;
        }

        public void SetClosestCoords()
        {
            // If the list is empty, there is no camera coords to obtain.
            if (CameraFieldList.Count <= 0)
            {
                CameraFieldCoords = Vector2.Zero;
                return;
            }
            Vector2 playerPos = MapManager.ObjLink.EntityPosition.Position;
            ObjCameraField closestCam = null;
            float closestDist = float.MaxValue;

            // If there are multiple camera objects, find the closest camera to Link.
            foreach (ObjCameraField fieldCenter in CameraFieldList)
            {
                float dist = Vector2.Distance(playerPos, fieldCenter.EntityPosition.Position);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestCam = fieldCenter;
                }
            }
            // Return the coordinates of the closest camera.
            CameraFieldCoords = new Vector2(closestCam.EntityPosition.X, closestCam.EntityPosition.Y);
        }
        public void ClearList()
        {
            CameraFieldList.Clear();
            CameraFieldCoords = Vector2.Zero;
            CameraFieldLock = false;
        }
    }
}
