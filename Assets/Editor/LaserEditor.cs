using UnityEditor;


[CustomEditor(typeof(Laser))]
public class LaserEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Laser laser = target as Laser;

        if (laser.toggleMechanism)
        {
            laser.toggleInterval = EditorGUILayout.Slider("Toggle Interval", laser.toggleInterval, 0.0f, 2.0f);
        }
        
    }

}
