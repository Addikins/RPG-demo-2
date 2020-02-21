using UnityEditor;
using UnityEngine;

public class ToolTest : EditorWindow {
    [MenuItem ("Window/Tester")]
    public static void ShowWindow () {
        GetWindow<ToolTest> ("Tester");
    }
    private void OnGUI () {
        GUILayout.Label ("Configure animations!", EditorStyles.boldLabel);

        if (GUILayout.Button ("Configure")) {
            foreach (GameObject animation in Selection.gameObjects)
            {

                Animator animator = animation.GetComponent<Animator>();
                if (animator != null)
                {
                }
            }
        }
    }
}