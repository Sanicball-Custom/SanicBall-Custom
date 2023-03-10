using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Sanicball.Editor
{
    public class TextureArray2DCreator : EditorWindow
    {
        [MenuItem("Window/SanicBall/Texture2D Array Creator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(TextureArray2DCreator));
        }

        public Texture2D[] textures;
        public int size = 5;
        public int depth = 1;
        public bool mipmap = false;
        public bool linear = false;
        public bool readWrite = false;
        public string savePath = "Assets/Textures/";
        public string name = "TextureArray2D";

        void OnGUI()
        {
            GUILayout.Label("Texture Array Creator", EditorStyles.boldLabel);
            depth = EditorGUILayout.IntField("Depth", depth);
            mipmap = EditorGUILayout.Toggle("Mipmap", mipmap);
            linear = EditorGUILayout.Toggle("Linear", linear);
            readWrite = EditorGUILayout.Toggle("Read/Write", readWrite);
            name = EditorGUILayout.TextField("Name", name);
            savePath = EditorGUILayout.TextField("Folder Path", savePath);

            EditorGUILayout.LabelField("Texutres:");
            size = EditorGUILayout.DelayedIntField("Number of textures", size);

            if (textures == null)
                textures = new Texture2D[size];
            else if (size != textures.Length)
                System.Array.Resize(ref textures, size);

            for (int i = 0; i < size; i++)
            {
                textures[i] = (Texture2D)EditorGUILayout.ObjectField("Texture " + i, textures[i],
                    typeof(Texture2D), false);

            }

            if (GUILayout.Button("Create"))
            {
                var textureArr =
                    new Texture2DArray(textures[0].width, textures[0].height, size, textures[0].format, mipmap, linear);
                textureArr.filterMode = textures[0].filterMode;
                textureArr.wrapMode = textures[0].wrapMode;
                textureArr.anisoLevel = textures[0].anisoLevel;
                //textureArr.hideFlags = HideFlags.HideAndDontSave;
                textureArr.name = name;

                for (int i = 0; i < size; i++)
                {
                    Graphics.CopyTexture(textures[i], 0, 0, textureArr, i, 0);
                }

                textureArr.Apply();
                AssetDatabase.CreateAsset(textureArr, savePath + name + ".asset");

            }
        }
    }
}