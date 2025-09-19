
namespace UnityEditor.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BaseButton))]
    public class BaseButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            BaseButton baseButton = (BaseButton)target;

            baseButton.percentScale = EditorGUILayout.FloatField("Percent Scale", baseButton.percentScale);
            baseButton.isSound = EditorGUILayout.Toggle("Is Sound", baseButton.isSound);

            base.OnInspectorGUI();
        }
    }
}

