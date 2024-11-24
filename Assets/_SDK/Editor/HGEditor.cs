#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class HGInfor
{
    public string keystorePath = "";
    public string keyaliasName = "";
    public string keyaliasPass = "";
    public string keystorePass = "";
    public int screenOrientation = 0;
    public string iconPath, smallIconPath, HGLogo, logoBGColor;
    public bool useCustomKeystore;
    public int scriptingImplementation;
    public bool devolopBuild;
}


public class HGEditor : EditorWindow
{

    static GUILayoutOption[] guiImages = new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(80) };

    static GUILayoutOption[] guiTitleImages = new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(80) };
    static GUILayoutOption[] guiMargin = new GUILayoutOption[] { GUILayout.Width(10), GUILayout.Height(80) };
    static GUILayoutOption[] guiRow = new GUILayoutOption[] { GUILayout.Width(1000), GUILayout.Height(5) };
    static GUILayoutOption[] guiButton = new GUILayoutOption[] { GUILayout.Width(100) };
    static GUILayoutOption[] guiField = new GUILayoutOption[] { GUILayout.Width(500) };
    static string companyName, productName, productID, versionName;
    static int versionCode;
    static string resIcon, resSmallIcon, resHGIcon;
    static string keystorePath, keyaliasName, keyaliasPass, keystorePass;
    static bool useCustomKeystore;
    static bool devolopBuild;
    //static ScreenOrientation screenOrientation;
    static Sprite iconSprite, smallIconSprite, higameIconSprite;
    static Color loadingColor = Color.black;
    static ScriptingImplementation buildType;


    const string hg_ic_sp = "hg_ic_sp";
    const string hg_ic_sm_sp = "hg_ic_sm_sp";
    const string hg_ld_ic_sp = "hg_ld_ic_sp";

    static void SetConfig(bool reload)
    {

        //PlayerSettings.companyName = companyName;
        //PlayerSettings.productName = productName;
        //PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, productID);
        //PlayerSettings.bundleVersion = versionName;
        //PlayerSettings.Android.bundleVersionCode = versionCode;

        //var jsonTextFile = Resources.Load<TextAsset>("editor_offline_data");
        //var config = JsonUtility.FromJson<HGInfor>(jsonTextFile.text);

        var jsonTextFile = Resources.Load<TextAsset>("editor_offline_data");
        var config = JsonUtility.FromJson<HGInfor>(jsonTextFile.text);
        config.keystorePath = keystorePath;
        config.keystorePass = keystorePass;
        config.keyaliasName = keyaliasName;
        config.keyaliasPass = keyaliasPass;
        config.devolopBuild = devolopBuild;
        //config.screenOrientation = (int)screenOrientation;
        config.scriptingImplementation = (int)buildType;
        config.useCustomKeystore = useCustomKeystore;
        config.logoBGColor = loadingColor.r + "," + loadingColor.g + "," + loadingColor.b;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        WriteToFile(config, reload);
    }
    private void OnEnable()
    {
        GetConfigs();
    }
    private void OnLostFocus()
    {
        SetConfig(true);
    }
    private void OnDisable()
    {
        SetConfig(true);
    }
    static void GetConfigs()
    {
        //companyName = PlayerSettings.companyName;
        //productName = PlayerSettings.productName;

        //productID = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
        //versionName = PlayerSettings.bundleVersion;
        //versionCode = PlayerSettings.Android.bundleVersionCode;

        var jsonTextFile = Resources.Load<TextAsset>("editor_offline_data");
        var config = JsonUtility.FromJson<HGInfor>(jsonTextFile.text);
        useCustomKeystore = config.useCustomKeystore;
        keystorePath = config.keystorePath;
        keystorePass = config.keystorePass;
        keyaliasName = config.keyaliasName;
        keyaliasPass = config.keyaliasPass;

        devolopBuild = config.devolopBuild;
        string[] split = config.logoBGColor.Split(",");
        loadingColor = new Color(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
        buildType = (ScriptingImplementation)config.scriptingImplementation;
        //screenOrientation = (ScreenOrientation)config.screenOrientation;



        iconSprite = Resources.Load<Sprite>(config.iconPath);
        smallIconSprite = Resources.Load<Sprite>(config.smallIconPath);
        higameIconSprite = Resources.Load<Sprite>(config.HGLogo);
    }
    static void WriteToFile(HGInfor content, bool reload)
    {
        var res = JsonUtility.ToJson(content);
        string filePath = "Assets/_SDK/Resources/editor_offline_data.json";
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.Write(res);
        outStream.Close();
        if (reload) AssetDatabase.Refresh();
    }
    //void InstantiateScreenOrientation(ScreenOrientation val)
    //{
    //    screenOrientation = val;
    //}
    void InstantiateScriptingImplementation(ScriptingImplementation val)
    {
        buildType = val;
    }


    [MenuItem("HIGAME/1: Editor", false, 2)]

    public static void OpenEditor()
    {

        const int width = 520;
        const int height = 200;

        var x = (Screen.currentResolution.width) / 2;
        var y = (Screen.currentResolution.height) / 2;
        GetConfigs();
        GetWindow<HGEditor>().minSize = new Vector2(width, height);
    }
    [MenuItem("HIGAME/2: Check Config", false, 2)]
    public static void CheckConfig()
    {
        OpenConfigFie();
    }
    //[MenuItem("HIGAME/3: Import HiGame Package", false, 2)]
    public static void ImportAllAssets()
    {
        var unityPackagesPath = Path.Combine(Application.dataPath, "_SDK/Packages");
        string[] unityPackages = Directory.GetFiles(unityPackagesPath);
        if (unityPackages.Length == 0) return;
        for (int j = 0; j < unityPackages.Length; j++)
        {
            if (unityPackages[j].Contains("external-dependency"))
            {
                var temp = unityPackages[j];
                unityPackages[j] = unityPackages[0];
                unityPackages[0] = temp;
                break;
            }
        }
        int i = 0;
        AssetDatabase.importPackageCompleted += (s) =>
        {
            i++;
            if (i >= unityPackages.Length)
            {
                Debug.Log("Assets have been imported");
                return;
            }
            AssetDatabase.ImportPackage(unityPackages[i], true);
        };
        AssetDatabase.importPackageFailed += (s, err) =>
        {
            Debug.Log(s + err);
        };
        AssetDatabase.ImportPackage(unityPackages[i], true);
    }

    void OnGUI()
    {

        //companyName = EditorGUILayout.TextField("Company Name", companyName, guiField);
        //productName = EditorGUILayout.TextField("Product Name", productName, guiField);
        //productID = EditorGUILayout.TextField("Product ID", productID, guiField);


        //versionName = EditorGUILayout.TextField("Version Name", versionName, guiField);
        //versionCode = EditorGUILayout.IntField("Version Code", versionCode, guiField);
        GUILayout.Label("", EditorStyles.boldLabel, guiRow);
        //GUILayout.Label("Icon Settings", EditorStyles.boldLabel);
        //GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Height(4));
        //EditorGUILayout.BeginHorizontal();
        //GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(150));
        //EditorGUILayout.BeginVertical();
        //GUILayout.Label("Normal(x512)", EditorStyles.label, GUILayout.Width(80), GUILayout.Height(15));
        //GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Height(2));
        //iconSprite = (Sprite)EditorGUILayout.ObjectField(iconSprite, typeof(Sprite), true, guiImages);
        //if (iconSprite != null && iconSprite.texture.width != 512) iconSprite = null;
        //if (iconSprite != null&& !iconSprite.texture.isReadable)
        //{
        //     Debug.LogError("<b>ENABLE <color=#00FF36> Read/Write</color></b> " + iconSprite.name);
        //}
        //EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginVertical();
        //GUILayout.Label("Small Ic(x64)", EditorStyles.label, GUILayout.Width(80), GUILayout.Height(15));
        //GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Height(2));
        //smallIconSprite = (Sprite)EditorGUILayout.ObjectField(smallIconSprite, typeof(Sprite), true, guiImages);
        //if (smallIconSprite != null && smallIconSprite.texture.width != 64) smallIconSprite = null;

        //EditorGUILayout.EndVertical();
        //GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(15));
        //EditorGUILayout.BeginVertical();
        //GUILayout.Label("Loading Icon", EditorStyles.label, GUILayout.Width(80), GUILayout.Height(15));
        //GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Height(2));
        //higameIconSprite = (Sprite)EditorGUILayout.ObjectField(higameIconSprite, typeof(Sprite), true, guiImages);
        //EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginVertical();
        //GUILayout.Label("Loading Color", EditorStyles.label, GUILayout.Width(80), GUILayout.Height(15));
        //GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Height(2));
        //loadingColor = EditorGUILayout.ColorField("", loadingColor, guiImages);
        //  EditorGUILayout.EndVertical();

        //GUILayout.FlexibleSpace();

        //GUILayout.Label(".\n.\n.\n.\n.\n.\n.\n", EditorStyles.boldLabel, guiMargin);
        //GUILayout.Label("Small", EditorStyles.boldLabel, guiTitleImages);
        //smallIconSprite = (Sprite)EditorGUILayout.ObjectField(smallIconSprite, typeof(Sprite), true, guiImages);
        //GUILayout.FlexibleSpace();
        //GUILayout.Label(".\n.\n.\n.\n.\n.\n.\n", EditorStyles.boldLabel, guiMargin);
        //GUILayout.Label("Loading", EditorStyles.boldLabel, guiTitleImages);
        //higameIconSprite = (Sprite)EditorGUILayout.ObjectField(higameIconSprite, typeof(Sprite), true, guiImages);
        //loadingColor = EditorGUILayout.ColorField("", loadingColor, guiImages);
        //   EditorGUILayout.EndHorizontal();
        GUILayout.Label("", EditorStyles.boldLabel, guiRow);
        //screenOrientation = (ScreenOrientation)EditorGUILayout.EnumPopup("Screen Orientation: ", screenOrientation, GUILayout.Width(300));
        //InstantiateScreenOrientation(screenOrientation);
        useCustomKeystore = EditorGUILayout.Toggle("Use Custom Keystore", useCustomKeystore);
        EditorGUI.BeginDisabledGroup(!useCustomKeystore);

        EditorGUILayout.BeginHorizontal();
        keystorePath = EditorGUILayout.TextField("keystore Path", keystorePath, GUILayout.Width(400));
        if (GUILayout.Button("Browser", guiButton))
        {
            string path = EditorUtility.OpenFilePanel("keystore", "", "keystore,jks,ks");
            if (path.Length != 0)
            {
                keystorePath = path;
                //var fileContent = File.ReadAllBytes(path);
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(keystorePath));
        keystorePass = EditorGUILayout.TextField("Password", keystorePass, guiField);
        keyaliasName = EditorGUILayout.TextField("Alias", keyaliasName, guiField);
        keyaliasPass = EditorGUILayout.TextField("Password", keyaliasPass, guiField);
        EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();
        buildType = (ScriptingImplementation)EditorGUILayout.EnumPopup("Build Type: ", buildType, GUILayout.Width(300));
        InstantiateScriptingImplementation(buildType);
        devolopBuild = EditorGUILayout.Toggle("Devolop Build", devolopBuild);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label("                         ", EditorStyles.boldLabel, guiMargin);

        //if (GUILayout.Button("Apply Config", guiButton))
        //{
        //    //CoppyToResource(iconSprite, "hg_ic_sp");
        //    //CoppyToResource(smallIconSprite, "hg_ic_sm_sp");
        //    //CoppyToResource(higameIconSprite, "hg_ld_ic_sp");

        //    OpenConfigFie();
        //}
        if (GUILayout.Button("Build", guiButton))
        {
            SetConfig(true);
            Build(buildType);
        }
        GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(10));
        EditorGUILayout.EndHorizontal();
        SetConfig(false);
    }

    static string CoppyToResource(Sprite sprite, string outName)
    {
        if (sprite == null) return null;
        var from = AssetDatabase.GetAssetPath(sprite);
        if (string.IsNullOrEmpty(from)) return null;
        var to = "Assets/_SDK/Resources/" + outName + ".png";
        if (from != to)
        {
            AssetDatabase.DeleteAsset(to);
            AssetDatabase.MoveAsset(from, to);
            AssetDatabase.Refresh();

        }
        return to;

    }
    public static void OpenConfigFie()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            Debug.LogError("Current platform is " + EditorUserBuildSettings.activeBuildTarget + " -------->" + " please swich to ANDROID");
            return;
        }
        bool ckeck = false;
        var fullPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
        ckeck = File.Exists(fullPath);
        if (!ckeck)
        {
            Debug.LogError("<b><color=#00FF36>Enable</color></b> Player Settings -> Publishing Settings -> Build -> <b><color=#00FF36>Custom Main Manifest</color></b>");
            return;
        }
        var doc = File.ReadAllText(fullPath);
        if (!doc.Contains("com.google.android.gms.permission.AD_ID"))
        {
            doc = doc.Replace("<application", "<uses-permission android:name=\"com.google.android.gms.permission.AD_ID\"/>\n  <uses-permission android:name=\"com.google.android.gms.permission.INTERNET\"/>\n  <uses-permission android:name=\"com.google.android.gms.permission.ACCESS_NETWORK_STATE\"/>\n  <application android:allowNativeHeapPointerTagging=\"false\" android:name=\"androidx.multidex.MultiDexApplication\" ");
            File.WriteAllText(fullPath, doc);
        }


        fullPath = Path.Combine(Application.dataPath, "Plugins/Android/mainTemplate.gradle");
        ckeck = File.Exists(fullPath);
        if (!ckeck)
        {
            Debug.LogError("<b><color=#00FF36>Enable</color></b> Player Settings -> Publishing Settings -> Build -> <b><color=#00FF36>Custom Main Gradle Template</color></b>");
            return;
        }
        doc = File.ReadAllText(fullPath);
        if (!doc.Contains("multiDexEnabled"))
        {
            doc = doc.Replace("implementation fileTree", "implementation \"androidx.multidex:multidex:2.0.1\"\n    implementation fileTree");
            doc = doc.Replace("minSdkVersion", "multiDexEnabled true\n        minSdkVersion");
            File.WriteAllText(fullPath, doc);
        }
        fullPath = Path.Combine(Application.dataPath, "Plugins/Android/baseProjectTemplate.gradle");
        ckeck = File.Exists(fullPath);
        if (!ckeck)
        {
            Debug.LogError("<b><color=#00FF36>Enable</color></b> Player Settings -> Publishing Settings -> Build -> <b><color=#00FF36>Custom Base Gradle Template</color></b>");
            return;
        }
        doc = File.ReadAllText(fullPath);
        if (doc.Contains("4.0.1"))
        {
            doc = doc.Replace("4.0.1", "4.2.2");
            File.WriteAllText(fullPath, doc);
        }
        var arr = File.ReadAllLines(fullPath);
        File.WriteAllLines(fullPath, arr);
        string proguard_str = "-keep class com.google.android.play.core.** { *; }";
        fullPath = Path.Combine(Application.dataPath, "Plugins/Android/proguard-user.txt");
        ckeck = File.Exists(fullPath);
        if (!ckeck)
        {
            Debug.LogError("<b><color=#00FF36>Enable</color></b> Player Settings -> Publishing Settings -> Build -> <b><color=#00FF36>Custom Proguard File</color></b>");
            return;
        }
        File.WriteAllText(fullPath, proguard_str);
        PlayerSettings.Android.optimizedFramePacing = false;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)33;
        PlayerSettings.Android.minifyWithR8 = true;
        PlayerSettings.Android.minifyRelease = false;
        PlayerSettings.Android.minifyDebug = false;
        var jsonTextFile = Resources.Load<TextAsset>("editor_offline_data");
        var config = JsonUtility.FromJson<HGInfor>(jsonTextFile.text);
        PlayerSettings.Android.useCustomKeystore = config.useCustomKeystore;
        Debug.Log("<b><color=#fc921a>HiGame:</color></b> <b><color=#00FF36>Ready For Build</color></b>");
    }





    static void GetConfig()
    {
        var jsonTextFile = Resources.Load<TextAsset>("offline_data.json");
        var config = JsonUtility.FromJson<HGInfor>(jsonTextFile.text);
        PlayerSettings.Android.useCustomKeystore = useCustomKeystore;
    }


    public static void Build(ScriptingImplementation scriptingImplementation)
    {
#if UNITY_IOS || UNITY_ANDROID
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, productID);
        //PlayerSettings.productName = productName;
        //PlayerSettings.bundleVersion = versionName;
        //PlayerSettings.Android.bundleVersionCode = versionCode;
#endif
#if UNITY_ANDROID
        PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, scriptingImplementation);
        if (useCustomKeystore)
        {
            PlayerSettings.Android.keystoreName = keystorePath;
            PlayerSettings.Android.keyaliasName = keyaliasName;
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
            PlayerSettings.Android.keystorePass = keystorePass;
        }
#endif
        AssetDatabase.Refresh();
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        string[] levels = EditorBuildSettings.scenes.Where(x => x.enabled).Select(scene => scene.path).ToArray();
        EditorUserBuildSettings.buildAppBundle = true;
        BuildOptions buildOptions = BuildOptions.CompressWithLz4;
        if (scriptingImplementation == ScriptingImplementation.IL2CPP)
        {
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;
        }
        else
        {
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Disabled;
        }
        if (devolopBuild)
        {
            EditorUserBuildSettings.development = true;
            buildOptions = BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.CompressWithLz4HC;
        }
        var path = Path.ChangeExtension(GetFilePath("All"), "aab");
        var report = BuildPipeline.BuildPlayer(levels, path, BuildTarget.Android, buildOptions);
        ConvertAabToApks(report.summary.outputPath);
        OpenFileBuild();
    }
    public static void OpenFileBuild()
    {
        string path = Path.Combine(Application.dataPath, string.Format("../Builds"));
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = path;
        proc.Start();
    }
    static void ConvertAabToApks(string path)
    {
        string pathAPK = path;
        path = Path.ChangeExtension(path, "aab");
        if (!File.Exists(path)) return;
        string filename = Path.GetFileNameWithoutExtension(path);
        // Get bundletool.jar from UnityEditor
        string bundletoolPath = Path.Combine(EditorApplication.applicationPath, "../Data/PlaybackEngines/AndroidPlayer/Tools");
        bundletoolPath = Directory.GetFiles(bundletoolPath, "bundletool*.jar", SearchOption.TopDirectoryOnly).FirstOrDefault();
        Debug.Log(bundletoolPath);
        // Running java command to execute bundletool and build apks file
        string buildApksCmd =
            $" java -jar \"{bundletoolPath}\" build-apks " + $"--bundle=\"{filename}.aab\" --output=\"{filename}.apks\" --mode=universal";
        if (useCustomKeystore)
        {
            buildApksCmd = buildApksCmd + $" --ks=\"{keystorePath}\" --ks-pass=pass:{keystorePass} "

           + $"--ks-key-alias=\"{keyaliasName}\" --key-pass=pass:{keyaliasPass} ";
        }

        Debug.Log("buildApksCmd: " + buildApksCmd);
        // Apks to Zip
        string renameToZipCmd = $" rename \"{filename}.apks\"  \"{filename}\".zip";

        // Wait for command to finish execute
        string pauseCmd = $" PAUSE ";

        var process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                WorkingDirectory = Path.GetDirectoryName(path) ?? string.Empty,
                RedirectStandardInput = true,
                UseShellExecute = false
            }
        };
        process.Start();
        using (StreamWriter sw = process.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine(buildApksCmd);
                sw.WriteLine(renameToZipCmd);
                sw.WriteLine($" tar -xf \"{filename}\".zip");
                sw.WriteLine($" rename \"universal.apk\"  \"{filename}\".apk");
                sw.WriteLine($" del toc.pb");
                sw.WriteLine($" del {filename}\".zip");
                sw.WriteLine($" adb install \"{filename}\".apk");
                sw.WriteLine($" adb shell monkey -p {productID} -c android.intent.category.LAUNCHER 1");
                sw.WriteLine($" adb shell pm clear {productID}");
                sw.WriteLine(" pause ");
            }
        }
        Debug.Log(pathAPK);
    }


    static string GetFilePath(string surFix)
    {
        string outputPath = GetArg("-outputPath");
        string gameName = GetValidFileName(productName);
        if (outputPath != null)
        {
            return $"{outputPath}/{gameName}_{surFix}_{DateTime.Now.ToString("dd-MM_h-mm-tt")}.apk";
        }
        else
        {
            return Path.Combine(Application.dataPath,
                $"../Builds/{gameName}_{surFix}_{DateTime.Now.ToString("dd-MM_h-mm-tt")}.apk");
        }
    }

    static string GetValidFileName(string fileName)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '_');
        }

        return fileName;
    }

    static string GetFilePathReplace(string surFix)
    {
        string outputPath = GetArg("-outputPath");
        if (outputPath != null)
        {
            return string.Format("{0}/{1}.apk", outputPath, "Smasher_" + surFix);
        }
        else
        {
            return Path.Combine(Application.dataPath, string.Format("../Builds/{0}.apk", "Smasher_" + surFix));
        }
    }

    static string GetArg(string name)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

    [MenuItem("HIGAME/Clear/Clear Android Data &_c")]
    public static void ClearAndroidData()
    {
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                WorkingDirectory = string.Empty,
                RedirectStandardInput = true,
                UseShellExecute = false
            }
        };
        process.Start();
        using (StreamWriter sw = process.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine($" adb shell pm clear {PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android)}");
            }
        }
    }
    [MenuItem("HIGAME/Clear/Clear All Build &#_c")]
    public static void ClearAllBuild()
    {
        string path = Path.Combine(Application.dataPath, string.Format("../Builds"));
        string[] filePaths = Directory.GetFiles(path);
        foreach (string filePath in filePaths)
            File.Delete(filePath);
    }
}
#endif