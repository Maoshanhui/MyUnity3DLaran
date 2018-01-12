using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets._3rdPlugins.Scripts.Editor.Tools
{
    public class CharactorCombineEditorWindow : EditorWindow
    {
        [MenuItem("工具/美术工具/角色合成工具")]

        public static void ShowCharactorCombineEditorWindow()
        {
            var win = GetWindow<CharactorCombineEditorWindow>("角色合成工具");
            win.Show();
        }

        [SerializeField]
        private GameObject body;
        [SerializeField]
        private GameObject face;
        [SerializeField]
        private GameObject hair;

        [SerializeField]
        private GameObject weaponLeft;
        [SerializeField]
        private GameObject weaponRight;


        protected void OnGUI()
        {
            GUILayout.Label("角色的各部分prefab", EditorStyles.boldLabel);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("身体", EditorStyles.boldLabel);   
            body = EditorGUILayout.ObjectField(body, typeof(GameObject)) as GameObject;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("脸部", EditorStyles.boldLabel);  
            face = EditorGUILayout.ObjectField(face, typeof(GameObject)) as GameObject;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("头发", EditorStyles.boldLabel);  
            hair = EditorGUILayout.ObjectField(hair, typeof(GameObject)) as GameObject;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("左手武器", EditorStyles.boldLabel);
            weaponLeft = EditorGUILayout.ObjectField(weaponLeft, typeof(GameObject)) as GameObject;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("右手武器", EditorStyles.boldLabel);
            weaponRight = EditorGUILayout.ObjectField(weaponRight, typeof(GameObject)) as GameObject;
            GUILayout.EndHorizontal();

            if (GUILayout.Button("合成"))
            {
                if (body == null)
                {
                    EditorUtility.DisplayDialog("提示", "身体 prefab 不能为空", "确定");
                    return;
                }

                if (face == null)
                {
                    EditorUtility.DisplayDialog("提示", "脸部 prefab 不能为空", "确定");
                    return;
                }

                if (hair == null)
                {
                    EditorUtility.DisplayDialog("提示", "头发 prefab 不能为空", "确定");
                    return;
                }

                // 创建身体模型
                GameObject bodyInst = GameObject.Instantiate(body, null);
                ModelBindPoints headBindPoint = bodyInst.GetComponent<ModelBindPoints>();
                if (headBindPoint.pointList != null && headBindPoint.pointList.Count >= 9)
                {
                    Transform headTrans = headBindPoint.pointList[9];

                    Transform weaponLeftTrans = headBindPoint.pointList[7];

                    Transform weaponRightTrans = headBindPoint.pointList[8];


                    if (headTrans == null || weaponLeftTrans == null || weaponRightTrans == null)
                    {
                        EditorUtility.DisplayDialog("提示", "身体 prefab 没有找到相关绑定点，请检查模型绑定点", "确定");
                    }
                    else
                    {
                        // 脸部和头发挂在头部
                        GameObject faceInst = GameObject.Instantiate(face, headTrans);
                        GameObject hairInst = GameObject.Instantiate(hair, headTrans);

                        if (weaponLeft != null && weaponLeftTrans != null)
                        {
                            GameObject weaponLeftInst = GameObject.Instantiate(weaponLeft, weaponLeftTrans); 
                        }

                        if (weaponRight != null && weaponRightTrans != null)
                        {
                            GameObject weaponRightInst = GameObject.Instantiate(weaponRight, weaponRightTrans); 
                        }

                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "身体 prefab 没有找到头部绑定点", "确定");
                }
            }

        }

    }

    
}