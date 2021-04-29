/*
    A simple little editor extension to copy and paste all components
    Help from http://answers.unity3d.com/questions/541045/copy-all-components-from-one-character-to-another.html
    license: WTFPL (http://www.wtfpl.net/)
    author: aeroson
    advise: ChessMax
    editor: frekons
*/

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ComponentsCopier
{

    static List<UnityEngine.UI.Text> copiedComponents = new List<UnityEngine.UI.Text>();
    static List<GameObject> allchilds = new List<GameObject>();
    static List<GameObject> newallchilds = new List<GameObject>();
    private static void GetChildRecursive(ref List<GameObject> list, GameObject obj) {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform) {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            list.Add(child.gameObject);
            GetChildRecursive(ref list,child.gameObject);
        }
    }

    private static void GetAllComponentsInList(List<GameObject> list) {
        if (null == list)
            return;

        foreach (GameObject go in list) {
            if (null == go)
                continue;
            UnityEngine.UI.Text component;
            if (component = go.GetComponent<UnityEngine.UI.Text>())
                copiedComponents.Add(component);
        }
    }

    [MenuItem("GameObject/Copy all components %&C")]
    static void Copy()
    {
        if (UnityEditor.Selection.activeGameObject == null)
            return;

        copiedComponents.Clear(); // to prevent wrong pastes in future
        allchilds.Clear();
        newallchilds.Clear();

        GetChildRecursive(ref allchilds, UnityEditor.Selection.activeGameObject);
        GetAllComponentsInList(allchilds);
        Debug.Log("copiedComponents.Length = "+copiedComponents.Count);
    }

    [MenuItem("GameObject/Paste all components %&P")]
    static void Paste()
    {
        if (copiedComponents == null)
        {
            Debug.LogError("Nothing is copied!");
            return;
        }

        GetChildRecursive(ref newallchilds, UnityEditor.Selection.activeGameObject);
        int i = 0;
        int goIndex = 0;
        foreach (GameObject targetGameObject in newallchilds) {
            if (!targetGameObject)
                continue;

            Undo.RegisterCompleteObjectUndo(targetGameObject, targetGameObject.name + ": Paste All Components"); // sadly does not record PasteComponentValues, i guess

            targetGameObject.name = allchilds[goIndex].name;

            var targetComponent = targetGameObject.GetComponent<UnityEngine.UI.Text>();
            if (targetComponent) {
                var copiedComponent = copiedComponents[i];
                //UnityEditorInternal.ComponentUtility.CopyComponent(copiedComponent);

                if (targetComponent) // if gameObject already contains the component
                {
                    //if (UnityEditorInternal.ComponentUtility.PasteComponentValues(targetComponent))
                    try {
                        targetComponent.text = copiedComponent.text;
                        Debug.Log("Successfully pasted: " + copiedComponent.GetType() + "("+copiedComponent.text+")");
                    } catch (System.NullReferenceException) {
                        Debug.LogError("(i = " + i + ") Failed to copy: " + copiedComponent.GetType());
                    }
                }
                i++;
            }
            goIndex++;
        }
                //else // if gameObject does not contain the component
                //{
                //    if (UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGameObject))
                //    {
                //        Debug.Log("Successfully pasted: " + copiedComponent.GetType());
                //    }
                //    else
                //    {
                //        Debug.LogError("Failed to copy: " + copiedComponent.GetType());
                //    }
                //}
            //}
        //}

        copiedComponents.Clear(); // to prevent wrong pastes in future
        allchilds.Clear();
        newallchilds.Clear();
    }

}
#endif