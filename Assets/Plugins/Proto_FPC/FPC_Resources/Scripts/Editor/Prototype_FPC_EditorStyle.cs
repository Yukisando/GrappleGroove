//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using UnityEditor;
using PrototypeFPC;

namespace PrototypeFPC
{
    [CustomEditor(typeof(Prototype_FPC))]
    public class Prototype_FPC_EditorStyle : Editor
    {
        //Scripts
        Perspective perspective;
        Movement movement;
        Jump jump;
        Slide slide;
        WallRun wallRun;
        Vault vault;
        Projectile projectile;
        GrapplingHook grapplingHook;
        Flashlight flashlight;
        Inspect inspect;
        GrabThrow grabThrow;
        Sway sway;

        //Tool image
        Texture toolImage;


        //----------------


        //Functions
        ///////////////

        void OnEnable() 
        {
            perspective = FindObjectOfType<Perspective>();
            movement = FindObjectOfType<Movement>();
            jump = FindObjectOfType<Jump>();
            slide = FindObjectOfType<Slide>();
            wallRun = FindObjectOfType<WallRun>();
            vault = FindObjectOfType<Vault>();
            projectile = FindObjectOfType<Projectile>();
            grapplingHook = FindObjectOfType<GrapplingHook>();
            flashlight = FindObjectOfType<Flashlight>();
            inspect = FindObjectOfType<Inspect>();
            grabThrow = FindObjectOfType<GrabThrow>();
            sway = FindObjectOfType<Sway>();

            //Tool Image
            this.toolImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Proto_FPC/FPC_Resources/Scripts/Editor/Title.png", typeof(Texture));
        }

        public override void OnInspectorGUI() 
        {
            EditorGUILayout.Space();

            //Display Tool Image
            EditorGUILayout.BeginHorizontal();
            EditorGUI.DrawRect(new Rect(0, 0, EditorGUIUtility.currentViewWidth, 102), new Color(1, 0.85f, 0.3f, 0.90f));
            GUILayout.Label(toolImage, new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter}, GUILayout.Height(80));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            var header = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            header.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("- CHARACTER ATTRIBUTES -", header, GUILayout.ExpandWidth(true));

            EditorGUILayout.Space();


            //Enable & Disable Scripts
            //----------------------

            GUILayout.BeginVertical();
            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            var leftAncher = new GUIStyle(GUI.skin.button) {alignment = TextAnchor.MiddleLeft};
            leftAncher.margin = new RectOffset(2, 18, 2, 2);

            //Perspective script
            if(perspective.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Perspective", leftAncher, GUILayout.ExpandWidth(true)))
                {
                    perspective.enabled = false;
                }
            }

            else if(!perspective.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Perspective - (Disabled)", leftAncher, GUILayout.ExpandWidth(true)))
                {
                    perspective.enabled = true;
                }
            }

            //Movement script
            if(movement.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Movement", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    movement.enabled = false;
                }
            }

            else if(!movement.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Movement - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    movement.enabled = true;
                }
            }

            //Jump script
            if(jump.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Jump", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    jump.enabled = false;
                }
            }

            else if(!jump.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Jump - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    jump.enabled = true;
                }
            }

            //Slide script
            if(slide.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Slide", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    slide.enabled = false;
                }
            }

            else if(!slide.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Slide - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    slide.enabled = true;
                }
            }

            //Wall run script
            if(wallRun.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Wall Run", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    wallRun.enabled = false;
                }
            }

            else if(!wallRun.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Wall Run - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    wallRun.enabled = true;
                }
            }

            //Vault script
            if(vault.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Vault", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    vault.enabled = false;
                }
            }

            else if(!vault.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Vault - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    vault.enabled = true;
                }
            }

            //Projectile script
            if(projectile.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Projectile", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    projectile.enabled = false;
                }
            }

            else if(!projectile.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Projectile - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    projectile.enabled = true;
                }
            }

            //Grappling hook script
            if(grapplingHook.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Grappling Hook", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    grapplingHook.enabled = false;
                }
            }

            else if(!grapplingHook.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Grappling Hook - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    grapplingHook.enabled = true;
                }
            }

            //Flashlight script
            if(flashlight.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Flashlight", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    flashlight.enabled = false;
                }
            }

            else if(!flashlight.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Flashlight - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    flashlight.enabled = true;
                }
            }

            //Inspect script
            if(inspect.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Inspect", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    inspect.enabled = false;
                }
            }

            else if(!inspect.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Inspect - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    inspect.enabled = true;
                }
            }

            //Grab and throw script
            if(grabThrow.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Grab Throw", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    grabThrow.enabled = false;
                }
            }

            else if(!grabThrow.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Grab Throw - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    grabThrow.enabled = true;
                }
            }

            //Sway script
            if(sway.enabled)
            {
                GUI.contentColor = new Color(1, 1, 1, 1);
                if(GUILayout.Button("• Sway", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    sway.enabled = false;
                }
            }

            else if(!sway.enabled)
            {
                GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
                if(GUILayout.Button("Sway - (Disabled)", leftAncher, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    sway.enabled = true;
                }
            }

            GUILayout.EndVertical();

            EditorGUILayout.Space();

            GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.6f);
            var footer = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            EditorGUILayout.LabelField("------------------", footer, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Prototype FPC v1.0", footer, GUILayout.ExpandWidth(true));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Developed By", footer, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("The Famous Mouse™", footer, GUILayout.ExpandWidth(true));
        }
    }
}
