using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Sanicball.Data;
using Sanicball.Extra;

[CustomPropertyDrawer(typeof(CharacterAbility))]
public class CharacterAbilityPropertyDrawer : PropertyDrawer {
    string subAssetPath;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

        var scriptProperty = property.FindPropertyRelative("script");
        var script = scriptProperty.objectReferenceValue;
        if (script == null) return 20f;
        Type t = null;
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(a => {
            if(t == null) t = a.GetType(script.name);
        });
        float height = 20f;
        if (t != null && t.GetInterfaces().ToList().Any(i => i == typeof(IAbility) || i.GetInterface("IAbility") != null)) {
            t.GetFields().Where(f => f.GetCustomAttribute<ExposeToCharacterAttribute>() != null).ToList().ForEach(f => {
                height += 20f;
            });
        }
        return height;
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(rect, label, property);

        //Debug.Log(property.propertyType + " - " + property.propertyPath + " - " + property.serializedObject.FindProperty(property.propertyPath).name);

        //var p = property.serializedObject.FindProperty(property.propertyPath);
        //while (p.Next(true) && p.propertyPath.StartsWith("characters.Array.data[0].testAbilities")) {
        //    Debug.Log("p => " + p.name + " - " + p.propertyPath);
        //}

        var pathParts = property.propertyPath.Split(new string[] { "Array.data" }, StringSplitOptions.RemoveEmptyEntries);
        int characterIndex = -1;
        int abilityIndex = -1;
        bool assigningCharacter = false;
        pathParts.ToList().ForEach(el => {
            if (characterIndex < 0 || abilityIndex < 0) {
                if (el.Contains("[")) {
                    var index = el.Split('.')[0];
                    if (assigningCharacter) characterIndex = int.Parse(index.Substring(1, index.Length - 2));
                    else abilityIndex = int.Parse(index.Substring(1, index.Length - 2));
                }
                assigningCharacter = el.Contains("character");
            }
        });

        string baseAssetPath = "Assets/Prefabs/ActiveData/ch" + characterIndex + "_abilities/ab" + abilityIndex + "_params";

        var scriptProperty = property.FindPropertyRelative("script");
        var paramsProperty = property.FindPropertyRelative("parameters");
        var typesProperty = property.FindPropertyRelative("parameterTypes");
        var dummyProperty = property.FindPropertyRelative("dummyObjects");
        //var dummyObjects = ActiveData.characterDataInEditor[characterIndex].testAbilities[abilityIndex].dummyObjects;
        //var dummyProperty = property.serializedObject.targetObject.name;
        //Debug.Log(ActiveData.characterDataInEditor[0].testAbilities[0].dummyObjects.Length);
        //var ability = (CharacterAbility)scriptProperty.serializedObject.targetObject;

        subAssetPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject).Replace("ActiveData.prefab", "ActiveDataExtra.asset");
        //if(!EditorUtility.IsPersistent(property.serializedObject.targetObject) || subAssetPath.Length == 0) {
        //    var tempPath = "Assets/__temp/SceneData_" + EditorSceneManager.GetActiveScene().name;
        //    if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
        //    if (!File.Exists("Assets/__temp/IMPORTANT.txt")) File.WriteAllText("Assets/__temp/IMPORTANT.txt", "DO NOT DELETE THIS FOLDER ORA ANY ITS SUBFOLDERS UNLESS IT ISN'T NEEDED ANYMORE, THIS HOLDS THE DATA FOR THE UNAPPLIED ActiveData PREFABS IN THE SCENES SHOWN IN THESE FOLDERS.");
        //    subAssetPath = tempPath + "/ActiveDataExtra.asset";
        //    if (!File.Exists(subAssetPath)) AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ActiveDataExtra>(), subAssetPath);
        //}

        var script = scriptProperty.objectReferenceValue;
        rect.height = 20f;
        EditorGUI.PropertyField(rect, scriptProperty);
        var newScript = scriptProperty.objectReferenceValue;
        if (newScript != script) {
            script = newScript;

            //Debug.Log(subAssetPath);

            AssetDatabase.LoadAllAssetRepresentationsAtPath(subAssetPath).ToList().ForEach(el => {
                if(el != null) AssetDatabase.RemoveObjectFromAsset(el);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(el));
            });
            AssetDatabase.SaveAssets();

            AssetDatabase.ImportAsset(subAssetPath);

            paramsProperty.ClearArray();// = new object[0];
            typesProperty.ClearArray();// = new Type[0];
            dummyProperty.ClearArray();
            property.serializedObject.ApplyModifiedProperties();
        }
        if (script == null) return;
        rect.y += rect.height;

        Type t = null;
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(a => {
            if (t == null) t = a.GetType(script.name);
        });
        if (t != null && t.GetInterfaces().ToList().Any(i => i == typeof(IAbility) || i.GetInterface("IAbility") != null)) {
            int fieldIndex = 0;
            rect.width /= 3f;
            Type[] primitiveTypes = new Type[] { typeof(int), typeof(long), typeof(float), typeof(double), typeof(bool), typeof(string) };
            var fieldList = t.GetFields().Where(f => f.GetCustomAttribute<ExposeToCharacterAttribute>() != null).ToList();
            paramsProperty.FindPropertyRelative("Array.size").intValue = fieldList.Count;
            fieldList.ForEach(f => {
                EditorGUI.LabelField(rect, f.Name);
                rect.x += rect.width;
                rect.width *= 2f;
                int arraySize = paramsProperty.FindPropertyRelative("Array.size").intValue;
                //paramsProperty.GetArrayElementAtIndex(fieldIndex) = EditorGUI.ObjectField(rect, null, f.FieldType, false);
                if (arraySize <= fieldIndex) {
                    paramsProperty.FindPropertyRelative("Array.size").intValue = ++arraySize;
                }
                var p = paramsProperty.FindPropertyRelative("Array.data[" + fieldIndex + "]");
                if (p.objectReferenceValue == null && primitiveTypes.Contains(f.FieldType)) {
                    var abilityAsset = AssetDatabase.LoadAllAssetRepresentationsAtPath(subAssetPath).FirstOrDefault(el => el != null && el.name == f.Name);
                    //Debug.Log(abilityAsset.name + " - " + f.Name);
                    if (abilityAsset == null) {
                        string objPath = baseAssetPath + "/" + f.FieldType.Name + "_" + f.Name + ".asset";
                        var obj = TypeUtils.CreatePrimitiveParamOfType(f.FieldType, f.Name);
                        //AssetDatabase.CreateAsset(obj, objPath);
                        obj.name = "ch" + characterIndex + "_" + obj.name;
                        AssetDatabase.AddObjectToAsset(obj, subAssetPath);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
                        p.objectReferenceValue = obj;
                    } else {
                        p.objectReferenceValue = abilityAsset;
                    }
                }
                DynamicField(ref p, f, ref rect);
                property.serializedObject.ApplyModifiedProperties();
                fieldIndex++;
                rect.width /= 2f;
                rect.x -= rect.width;
                rect.y += 20f;
            });
            rect.width *= 3f;
        }
        property.serializedObject.ApplyModifiedProperties();
        //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(property.serializedObject.targetObject));

        EditorGUI.EndProperty();
    }

    private void DynamicField(ref SerializedProperty p, FieldInfo f, ref Rect rect) {
        Type t = f.FieldType;
        if (t == typeof(int)) {
            ((PrimitiveParamValue)p.objectReferenceValue).paramValue = EditorGUI.IntField(rect, p.objectReferenceValue.As<int>()).ToString();
        } else if (t == typeof(float)) {
            ((PrimitiveParamValue)p.objectReferenceValue).paramValue = EditorGUI.FloatField(rect, p.objectReferenceValue.As<float>()).ToString();
        } else if (t == typeof(double)) {
            ((PrimitiveParamValue)p.objectReferenceValue).paramValue= EditorGUI.DoubleField(rect, p.objectReferenceValue.As<double>()).ToString();
        } else if (t == typeof(long)) {
            ((PrimitiveParamValue)p.objectReferenceValue).paramValue= EditorGUI.LongField(rect, p.objectReferenceValue.As<long>()).ToString();
        } else if (t == typeof(string)) {
            if (f.GetCustomAttributes(typeof(TextAreaAttribute), false).Length > 0) {
                ((PrimitiveParamValue)p.objectReferenceValue).paramValue= EditorGUI.TextArea(rect, p.objectReferenceValue.As<string>()).ToString();
            } else {
                ((PrimitiveParamValue)p.objectReferenceValue).paramValue= EditorGUI.TextField(rect, p.objectReferenceValue.As<string>()).ToString();
            }
        } else if (t == typeof(bool)) {
            ((PrimitiveParamValue)p.objectReferenceValue).paramValue= EditorGUI.Toggle(rect, p.objectReferenceValue.As<bool>()).ToString();
        } else if (t == typeof(Vector3)) {
            p.vector3Value = EditorGUI.Vector3Field(rect, "", p.vector3Value);
        } else if (t == typeof(Vector2)) {
            p.vector2Value = EditorGUI.Vector2Field(rect, "", p.vector2Value);
        } else if (t == typeof(Vector4)) {
            p.vector4Value = EditorGUI.Vector4Field(rect, "", p.vector4Value);
        } else if (t == typeof(Quaternion)) {
            p.quaternionValue = Quaternion.Euler(EditorGUI.Vector3Field(rect, "", p.quaternionValue.eulerAngles));
        } else if (t == typeof(GameObject) || t.BaseType == typeof(ScriptableObject) || t.BaseType == typeof(UnityEngine.Object)) {
            p.objectReferenceValue = EditorGUI.ObjectField(rect, p.objectReferenceValue, t, false);
        }
    }

    [MenuItem("Assets/Remove Parameter")]
    private static void UpdateActiveDataExtra() {
        AssetDatabase.RemoveObjectFromAsset(Selection.activeObject);
    }

    [MenuItem("Assets/Remove Parameter", true)]
    private static bool UpdateActiveDataExtraValidation() {
        return Selection.activeObject is PrimitiveParamValue;
    }
}