using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    private SerializedProperty _iosAppId;

    private SerializedProperty _inAppSetting;



    private SerializedProperty _adsSettings;
    private SerializedProperty _privatePolicySetting;

    private void OnEnable()
    {
       
        _iosAppId = serializedObject.FindProperty(GameSettings.IOS_APP_ID);
        _inAppSetting = serializedObject.FindProperty(GameSettings.IN_APP_SETTING);

        _privatePolicySetting = serializedObject.FindProperty(GameSettings.PRIVATE_POLICY_SETTING);
        _adsSettings = serializedObject.FindProperty(GameSettings.ADS_SETTINGS);
    }

    public override void OnInspectorGUI()
    {
        DrawAppId();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DrawAdsSettings();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
       
        DrawInApp();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        DrawPrivatePolicy();
        EditorGUILayout.Space();
        DrawFixIfNeeded();


        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void DrawFixIfNeeded()
    {
        if (MissingSymbols())
        {
            if (GUILayout.Button("Fix Missing Symbols"))
            {
                FixMissingSymbols();
            }
        }
    }

    private void FixMissingSymbols()
    {
        var adsSettings = _adsSettings.ToObjectValue<AdsSettings>();


        HandleScriptingSymbol(BuildTargetGroup.iOS, adsSettings.iosAdmobSetting.enable, "ADMOB");
        HandleScriptingSymbol(BuildTargetGroup.Android, adsSettings.androidAdmobSetting.enable, "ADMOB");


        var inAppSetting = _inAppSetting.ToObjectValue<InAppSetting>();
        HandleScriptingSymbol(BuildTargetGroup.iOS, inAppSetting.enable, "IN_APP");
        HandleScriptingSymbol(BuildTargetGroup.Android, inAppSetting.enable, "IN_APP");


    }

    private bool MissingSymbols()
    {
        var adsSettings = _adsSettings.ToObjectValue<AdsSettings>();
        if (adsSettings.iosAdmobSetting.enable && !HaveBuildSymbol(BuildTargetGroup.iOS, "ADMOB"))
        {
            return true;
        }

        if (adsSettings.androidAdmobSetting.enable && !HaveBuildSymbol(BuildTargetGroup.Android, "ADMOB"))
        {
            return true;
        }

        var inAppSetting = _inAppSetting.ToObjectValue<InAppSetting>();
        if (inAppSetting.enable && (!HaveBuildSymbol(BuildTargetGroup.Android, "IN_APP") ||
                                    !HaveBuildSymbol(BuildTargetGroup.iOS, "IN_APP")))
            return true;


        return false;
    }

    private void DrawAdsSettings()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ads Settings", EditorStyles.boldLabel);
        _adsSettings.isExpanded = EditorGUILayout.ToggleLeft("", _adsSettings.isExpanded);
        EditorGUILayout.EndHorizontal();
        if (_adsSettings.isExpanded)
        {
            EditorGUI.indentLevel++;

          
            _adsSettings.DrawChildrenDefault(nameof(AdsSettings.iosAdmobSetting)
                , nameof(AdsSettings.androidAdmobSetting)
                ,
                nameof(AdsSettings.iosUnityAppId),
                nameof(AdsSettings.androidUnityAppId));
        

            DrawUnitySetting(_adsSettings.FindPropertyRelative(nameof(AdsSettings.iosUnityAppId)),
                _adsSettings.FindPropertyRelative(nameof(AdsSettings.androidUnityAppId)));

            DrawAdmobSetting(_adsSettings.FindPropertyRelative(nameof(AdsSettings.iosAdmobSetting)),
                _adsSettings.FindPropertyRelative(nameof(AdsSettings.androidAdmobSetting)));
  
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
    }

    
    private void DrawPrivatePolicy()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        var enableProperty = _privatePolicySetting.FindPropertyRelative(nameof(PrivatePolicySetting.enable));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Private Policy", EditorStyles.boldLabel);
        enableProperty.boolValue = EditorGUILayout.ToggleLeft("", enableProperty.boolValue);
        EditorGUILayout.EndHorizontal();



        if (enableProperty.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_privatePolicySetting.FindPropertyRelative(nameof(PrivatePolicySetting.url)));
            EditorGUI.indentLevel--;
        }


        EditorGUILayout.EndVertical();
    }


    private void DrawInApp()
    {
        var enableProperty = _inAppSetting.FindPropertyRelative(nameof(InAppSetting.enable));

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("In App", EditorStyles.boldLabel);
        var enable = EditorGUILayout.ToggleLeft("", enableProperty.boolValue);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        if (enable != enableProperty.boolValue)
        {
            enableProperty.boolValue = enable;
            if (enableProperty.boolValue)
            {
                AddBuildSymbol(BuildTargetGroup.iOS, "IN_APP");
                AddBuildSymbol(BuildTargetGroup.Android, "IN_APP");
            }
            else
            {
                RemoveBuildSymbol(BuildTargetGroup.iOS, "IN_APP");
                RemoveBuildSymbol(BuildTargetGroup.Android, "IN_APP");
            }
        }


        if (enableProperty.boolValue)
        {
            _inAppSetting.DrawChildrenDefault(nameof(InAppSetting.enable));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }


    private void DrawAdmobSetting(SerializedProperty iosAdmobSetting, SerializedProperty androidAdmobSetting)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        iosAdmobSetting.isExpanded = EditorGUILayout.Foldout(iosAdmobSetting.isExpanded, "Admob Setting");

        if (iosAdmobSetting.isExpanded)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            var iosEnableProperty = iosAdmobSetting.FindPropertyRelative(nameof(AdmobSetting.enable));
            var iosEnable = EditorGUILayout.Toggle("Ios", iosEnableProperty.boolValue);

            if (iosEnable != iosEnableProperty.boolValue)
            {
                iosEnableProperty.boolValue = iosEnable;
                HandleScriptingSymbol(BuildTargetGroup.iOS, iosEnable, "ADMOB");
            }

            if (iosEnableProperty.boolValue)
            {
                EditorGUI.indentLevel++;
                iosAdmobSetting.DrawChildrenDefault(nameof(AdmobSetting.enable));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            var androidEnableProperty = androidAdmobSetting.FindPropertyRelative(nameof(AdmobSetting.enable));
            var androidEnable = EditorGUILayout.Toggle("Android", androidEnableProperty.boolValue);

            if (androidEnable != androidEnableProperty.boolValue)
            {
                androidEnableProperty.boolValue = androidEnable;
                HandleScriptingSymbol(BuildTargetGroup.Android, androidEnable, "ADMOB");
            }

            if (androidEnableProperty.boolValue)
            {
                EditorGUI.indentLevel++;
                androidAdmobSetting.DrawChildrenDefault(nameof(AdmobSetting.enable));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawUnitySetting(SerializedProperty iosUnityId, SerializedProperty androidUnityId)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
#if UNITY_ADS
        var enable = true;
#else
        var enable = false;
#endif
        iosUnityId.isExpanded = EditorGUILayout.Foldout(iosUnityId.isExpanded, "Unity Setting");

        if (iosUnityId.isExpanded)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(!enable);

            if (!enable)
                EditorGUILayout.HelpBox("Enable Unity Ads in Services", MessageType.Info);
            EditorGUILayout.PropertyField(androidUnityId);
            EditorGUILayout.PropertyField(iosUnityId);
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
    }

    //    private void DrawAdColonySetting(SerializedProperty iosAdColonySetting, SerializedProperty androidAdColonySetting)
    //    {
    //        EditorGUILayout.BeginVertical(GUI.skin.box);
    //        iosAdColonySetting.isExpanded = EditorGUILayout.Foldout(iosAdColonySetting.isExpanded, "AdColony Setting");
    //        if (iosAdColonySetting.isExpanded)
    //        {
    //            EditorGUILayout.Space();
    //
    //            EditorGUILayout.BeginVertical(GUI.skin.box);
    //            EditorGUI.indentLevel++;
    //            var iosEnableProperty = iosAdColonySetting.FindPropertyRelative(nameof(AdColonySettings.enable));
    //
    //            var iosEnable = EditorGUILayout.Toggle("Ios", iosEnableProperty.boolValue);
    ////
    //            if (iosEnable != iosEnableProperty.boolValue)
    //            {
    //                iosEnableProperty.boolValue = iosEnable;
    //                HandleScriptingSymbol(BuildTargetGroup.iOS, iosEnable, "ADCLONY");
    //            }
    //
    ////
    //            if (iosEnableProperty.boolValue)
    //            {
    //                EditorGUI.indentLevel++;
    //                iosAdColonySetting.DrawChildrenDefault(nameof(AdColonySettings.enable));
    //
    ////            _iosAdColonySetting.NextVisible(false);
    //                EditorGUI.indentLevel--;
    //            }
    //
    //            EditorGUI.indentLevel--;
    //            EditorGUILayout.EndVertical();
    //            EditorGUILayout.BeginVertical(GUI.skin.box);
    //            EditorGUI.indentLevel++;
    //            var androidEnableProperty = androidAdColonySetting.FindPropertyRelative(nameof(AdColonySettings.enable));
    //            var androidEnable = EditorGUILayout.Toggle("Android", androidEnableProperty.boolValue);
    //
    //            if (androidEnable != androidEnableProperty.boolValue)
    //            {
    //                androidEnableProperty.boolValue = androidEnable;
    //                HandleScriptingSymbol(BuildTargetGroup.Android, androidEnable, "ADCLONY");
    //            }
    //
    //            if (androidEnableProperty.boolValue)
    //            {
    //                EditorGUI.indentLevel++;
    //                androidAdColonySetting.DrawChildrenDefault(nameof(AdColonySettings.enable));
    //                EditorGUI.indentLevel--;
    //            }
    //
    //            EditorGUI.indentLevel--;
    //            EditorGUILayout.EndVertical();
    //        }
    //
    //        EditorGUILayout.EndVertical();
    //    }


    private void DrawAppId()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("App Ids", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(_iosAppId);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private static bool HaveBuildSymbol(BuildTargetGroup group, string symbol)
    {
        var scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        var strings = scriptingDefineSymbolsForGroup.Split(';').ToList();

        return strings.Contains(symbol);
    }

    private static void AddBuildSymbol(BuildTargetGroup group, string symbol)
    {
        if (HaveBuildSymbol(group, symbol))
            return;
        var scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        var strings = scriptingDefineSymbolsForGroup.Split(';').ToList();
        strings.Add(symbol);
        var str = "";
        foreach (var s in strings)
        {
            str += s + ";";
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, str);
    }

    private static void RemoveBuildSymbol(BuildTargetGroup group, string symbol)
    {
        if (!HaveBuildSymbol(group, symbol))
            return;
        var scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        var strings = scriptingDefineSymbolsForGroup.Split(';').ToList();
        strings.Remove(symbol);
        var str = "";
        foreach (var s in strings)
        {
            str += s + ";";
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, str);
    }

    private static void HandleScriptingSymbol(BuildTargetGroup buildTargetGroup, bool enable, string key)
    {
        var scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        var strings = scriptingDefineSymbolsForGroup.Split(';').ToList();

        if (enable)
        {
            strings.Add(key);
        }
        else
        {
            strings.Remove(key);
        }


        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", strings.Distinct()));
    }
}


public static class EditorExtensions
{
    public static void DrawChildrenDefault(this SerializedProperty property,
        params string[] exceptChildren)
    {
        var exceptList = exceptChildren?.ToList() ?? new List<string>();

        property = property.Copy();

        var parentDepth = property.depth;
        if (property.NextVisible(true) && parentDepth < property.depth)
        {
            do
            {
                if (exceptList.Contains(property.name))
                    continue;
                EditorGUILayout.PropertyField(property, true);
            } while (property.NextVisible(false) && parentDepth < property.depth);
        }
    }
}

public static class MenuExtensions
{
    [MenuItem("MyGames/Clear Prefs")]
    public static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    [MenuItem("Scenes/Splash")]
    public static void SplashScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Splash.unity");
    }

    [MenuItem("Scenes/Menu")]
    public static void MenuScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    [MenuItem("Scenes/Game")]
    public static void GameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
    }

}