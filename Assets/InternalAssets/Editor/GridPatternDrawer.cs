namespace Assets.Editor
{
    //[CustomPropertyDrawer(typeof(GridPattern))]
    //public class GridPatternDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        EditorGUI.PrefixLabel(position, label);
    //        Rect newPosition = position;
    //        newPosition.y += 18f;

    //        SerializedProperty columns = property.FindPropertyRelative("columns");

    //        for (int j = 0; j < 4; j++)
    //        {
    //            SerializedProperty row = columns.GetArrayElementAtIndex(0).FindPropertyRelative("row");

    //            if (row.arraySize != 4)
    //            {
    //                row.arraySize = 4;
    //            }

    //            newPosition.width = position.width / 7;
    //            for (int i = 0; i < 4; i++)
    //            {
    //                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(i));
    //                newPosition.x += newPosition.width;
    //            }

    //            newPosition.x = position.x;
    //            newPosition.y += 18f;
    //        }
    //    }

    //    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //    {
    //        return 18f * 8;
    //    }
    //}
}
