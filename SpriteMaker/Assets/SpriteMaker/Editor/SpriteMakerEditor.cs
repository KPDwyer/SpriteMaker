using UnityEngine;
using UnityEditor;

namespace SpriteMaker
{
	
    public class SpriteMakerEditor : EditorWindow
    {

        TextureBuilder texBuilder;
        DrawCommandManager drawCommandManager;

        SpritePreviewEditor spritePreview;

        private Vector2 ScrollPosition;


        private bool previewFocus = false;
        private bool updateOnChange = false;
        private bool saveOnChange = false;

        private bool showFile = true;
        private bool showSpriteInfo = true;
        private bool showDrawCommands = true;


        //Files
        private SpriteMakerAsset activeAsset;
        private SpriteMakerAsset loadAsset;


        [MenuItem("Window/SpriteMaker")]
        static void Init()
        {
            SpriteMakerEditor window = (SpriteMakerEditor)EditorWindow.GetWindow(typeof(SpriteMakerEditor), false, "Sprite Maker");
            window.texBuilder = new TextureBuilder();
            window.drawCommandManager = new DrawCommandManager();
            window.Show();
        }

        void OnGUI()
        {

            //if the editor window was left open on a new Unity bootup, init doesn't get called.
            if (texBuilder == null)
            {
                texBuilder = new TextureBuilder();
            }
            if (drawCommandManager == null)
            {
                drawCommandManager = new DrawCommandManager();
            }

            //we draw file ops up top
            GUISpriteMaker();

            //we only show further GUI if there's an asset loaded.
            if (activeAsset != null)
            {
                Undo.RecordObject(activeAsset, "Spritemaker Asset changed");

                //we draw sprite info first so users can always hit preview
                GUISpriteInfo();
            


                EditorGUI.BeginChangeCheck();
                {
                    //scroll area for the draw commands
                    ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            GUIDrawCommands();
                        }
                        EditorGUILayout.EndVertical();	
                    }
                    EditorGUILayout.EndScrollView();


                    if (EditorGUI.EndChangeCheck())
                    {
                        if (updateOnChange)
                        {
                            PreviewTexture();
                        }
                        if (saveOnChange)
                        {
                            SaveAssetFile(false);
                        }
                    }
                }



            }

            if (previewFocus)
            {
                spritePreview.Focus();
                previewFocus = false;
            }



        }

        /// <summary>
        /// Shows the preview Texture.
        /// </summary>
        /// <param name="_tex">The texture to preview</param>
        private void ShowPreview(Texture2D _tex)
        {
            if (spritePreview == null)
            {
                spritePreview = (SpritePreviewEditor)EditorWindow.GetWindow(typeof(SpritePreviewEditor), false, "Sprite Preview");
                spritePreview.Show();
            }
            spritePreview.SetTexture(_tex);
        }

        /// <summary>
        /// Previews the texture.
        /// </summary>
        private void PreviewTexture()
        {
            ShowPreview(texBuilder.BuildTexture(activeAsset.DrawCommands));
        }


        /// <summary>
        /// Triggers a preview than asks the TextureBuilder to save the texture
        /// </summary>
        private void SaveTexture()
        {
            PreviewTexture();
            texBuilder.SaveTexture();
        }


        /// <summary>
        /// Saves the asset file toa  scriptable object
        /// </summary>
        /// <param name="shouldFocus">If set to <c>true</c>, will focus the asset file in the editor</param>
        private void SaveAssetFile(bool shouldFocus = false)
        {
            EditorUtility.SetDirty(activeAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            if (shouldFocus)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = activeAsset;
            }
        }



        private void GUISpriteMaker()
        {
            showFile = GUILayout.Toggle(showFile, "File Ops", EditorStyles.boldLabel);
            if (showFile)
            {
                EditorGUILayout.BeginVertical("Box");
                {
                    if (GUILayout.Button("New Asset File"))
                    {
                        SpriteMakerAsset temp = ScriptableObject.CreateInstance<SpriteMakerAsset>();
                        temp.FriendlyName = texBuilder.fileName;
                        drawCommandManager.SetupAsset(ref temp);
                        string AssetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/" + temp.FriendlyName + ".asset");
                        AssetDatabase.CreateAsset(temp, AssetPath);

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        activeAsset = temp;
                        EditorUtility.FocusProjectWindow();
                        Selection.activeObject = temp;

                    }
                    //TODO do we need manual save
                    if (GUILayout.Button("Save Asset File"))
                    {
                        
                        SaveAssetFile(true);
                    }
                    saveOnChange = GUILayout.Toggle(saveOnChange, "Auto-Save (Experimental)");

                
                    loadAsset = (SpriteMakerAsset)EditorGUILayout.ObjectField(activeAsset, typeof(SpriteMakerAsset), false);
                    if (activeAsset != loadAsset)
                    {
                        activeAsset = loadAsset;
                        drawCommandManager.UseAsset(ref activeAsset);
                        texBuilder.fileName = activeAsset.FriendlyName;
                    }
                    if (activeAsset != null)
                    {
                        EditorGUILayout.LabelField("Currently Editing: " + activeAsset.FriendlyName);
                    }

                }
                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// Draws info at the highest level for the sprite (Filename, pixel size, etc)
        /// </summary>
        private void GUISpriteInfo()
        {
            showSpriteInfo = GUILayout.Toggle(showSpriteInfo, "Sprite Info", EditorStyles.boldLabel);

            if (showSpriteInfo)
            {
                EditorGUILayout.BeginVertical("Box");
                {

                    texBuilder.fileName = activeAsset.FriendlyName = EditorGUILayout.TextField("File Name", texBuilder.fileName);
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Width");
                        texBuilder.pixelWidth = GetIntFromTextField(EditorGUILayout.TextField(texBuilder.pixelWidth.ToString()));		
                        GUILayout.Label("Height");

                        texBuilder.pixelHeight = GetIntFromTextField(EditorGUILayout.TextField(texBuilder.pixelHeight.ToString()));
                    }
                    EditorGUILayout.EndHorizontal();

                    updateOnChange = GUILayout.Toggle(updateOnChange, "Auto-Preview (Use Low Image Sizes)");

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Preview Texture"))
                        {
                            PreviewTexture();
                            previewFocus = true;
                        }


                        if (GUILayout.Button("Save Texture"))
                        {
                            SaveTexture();
                            previewFocus = true;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// sets up the GUI for drawCommandManager to draw the commands
        /// </summary>
        private void GUIDrawCommands()
        {
            showDrawCommands = GUILayout.Toggle(showDrawCommands, "DrawCommands", EditorStyles.boldLabel);

            if (showDrawCommands)
            {
                EditorGUILayout.BeginVertical("Box");
                {


                    drawCommandManager.DrawControls(ref activeAsset);



                }
                EditorGUILayout.EndVertical();
            }
        }


        int temp = 0;

        /// <summary>
        /// Helper function for textfields using Ints
        /// </summary>
        private int GetIntFromTextField(string _text)
        {
            temp = 0;
            if (int.TryParse(_text, out temp))
            {
                temp = Mathf.Abs(temp); 	//No negatives
            }
            else
            {
                temp = 0;
            }
            return temp;
        }


    }
}