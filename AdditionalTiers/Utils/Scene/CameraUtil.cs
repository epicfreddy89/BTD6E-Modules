namespace AdditionalTiers.Utils.Scene {
    public class CameraUtil {
        public static Action<int, float> shakeCamera = (step, intensity) => {
            var camera = getCamera();
            var x = step % 12 == 2 ? intensity : step % 12 == 0 ? -intensity : 0;
            var y = step % 12 == 6 ? intensity : step % 12 == 4 ? -intensity : 0;
            var z = step % 12 == 10 ? intensity : step % 12 == 8 ? -intensity : 0;
            camera.transform.position += new Vector3(x, y, z);
        };

        public static Action resetCamera = () => {
            getCamera().transform.position = new Vector3(0, 0, 0);
            getCamera().transform.rotation = new Quaternion(60, 0, 0, 0);
        };

        public static Action<Vector3> moveCamera = (position) => {
            var camera = getCamera();
            camera.transform.position = position;
        };

        public static Action<TowerToSimulation> cameraPan = tts => {
            Vector3 curPos = tts.position;

            float newAngle = tts.tower.transform.rotation.value + 90, newX = Mathf.Cos(newAngle.ToRadians()), newZ = Mathf.Sin(newAngle.ToRadians());
            if (newAngle < 0)
                newAngle += 360;

            Vector3 addPos = new Vector3(newX, 1, newZ) / 10;

            curPos += addPos;
            CameraUtil.resetCamera();
            var camera = CameraUtil.getCamera();
            CameraUtil.moveCamera(curPos);
            camera.Rotate(-20, tts.tower.Rotation - 180, 180);
        };

        public static Func<Transform> getCamera = () => InGame.instance.sceneCamera.gameObject.transform.parent;
    }
}