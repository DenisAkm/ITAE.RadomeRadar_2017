using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Apparat
{
    public class CameraManager
    {
        #region Singleton Pattern
        private static CameraManager instance = null;
        public static CameraManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CameraManager();
                }
                return instance;
            }
        }
        #endregion
        public static float zfarPlane = 1000f;
        public static float znearPlane = 0.01f;
        public static float aspectRatio = (float)DeviceManager.Instance.form.Width / (float)DeviceManager.Instance.form.Height;

        #region Constructor
        private CameraManager()
        {
            OrbitPanCamera ocp = new OrbitPanCamera();

            //OrbitCamera oc = new OrbitCamera();
            EgoCamera ecX = new EgoCamera();
            EgoCamera ecY = new EgoCamera();
            EgoCamera ecZ = new EgoCamera();

            cameras.Add(ocp);

            //cameras.Add(oc);
            cameras.Add(ecX);
            cameras.Add(ecY);
            cameras.Add(ecZ);

            currentIndex = 0;
            currentCamera = cameras[currentIndex];
        }
        #endregion

        List<Camera> cameras = new List<Camera>();

        public Camera currentCamera;
        int currentIndex;

        public Matrix ViewPerspective
        {
            get
            {
                if (currentCamera is EgoCamera)
                {
                    return ((EgoCamera)currentCamera).ViewPerspective;
                }
                else
                {
                    return currentCamera.ViewPerspective;
                }
            }
        }

        public string CycleCameras()
        {
            int numCameras = cameras.Count;
            currentIndex = currentIndex + 1;
            if (currentIndex == numCameras)
                currentIndex = 0;
            currentCamera = cameras[currentIndex];
            return currentCamera.ToString();
        }
        public string CameraAt(int i)
        {
            currentIndex = i;
            currentCamera = cameras[currentIndex];
            return currentCamera.ToString();
        }
        public Camera returnCamera(int i)
        {
            return cameras[i];
        }
        public Matrix View
        {
            get
            {
                if (currentCamera is EgoCamera)
                {
                    return ((EgoCamera)currentCamera).View;
                }
                else
                {
                    return currentCamera.View;
                }
            }
        }
        public void SetView(Vector3 eye, Vector3 target)
        {
            if (currentCamera != null)
                currentCamera.SetView(eye, target, new Vector3(0, 1, 0));
        }
        public void GetView(out Vector3 eye, out Vector3 target)
        {
            eye = currentCamera.eye;
            target = currentCamera.target;
        }
    }
}
