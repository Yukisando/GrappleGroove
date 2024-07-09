using UnityEngine;

namespace DeTools.BasicMeshRenderer.Data
{
    /// <summary>
    /// Keeps track of the user his/her preferences.
    /// </summary>
    public class BasicMeshCombinerPreferences
    {
        /// <summary>
        /// private variable that keeps track of adding existing components.
        /// </summary>
        private static bool addExistingComponents = false;

        /// <summary>
        /// private variable that keeps track of adding a meshCollider.
        /// </summary>
        private static bool addMeshCollider = false;

        /// <summary>
        /// private variable that keeps track of adding boxColliders around the submeshes.
        /// </summary>
        private static bool addBoxColliders = false;

        /// <summary>
        /// private variable that keeps track of making an object static.
        /// </summary>
        private static bool makeObjectStatic = false;


        /// <summary>
        ///  public variable that keeps track of adding existing components.
        /// </summary>
        public static bool AddExistingComponents => addExistingComponents;

        /// <summary>
        /// public variable that keeps track of adding a meshCollider.
        /// </summary>
        public static bool AddMeshCollider => addMeshCollider;

        /// <summary>
        ///public variable that keeps track of adding boxColliders around the submeshes.
        /// </summary>
        public static bool AddBoxColliders => addBoxColliders;

        /// <summary>
        /// public variable that keeps track of making an object static.
        /// </summary>
        public static bool MakeObjectStatic => makeObjectStatic;

        /// <summary>
        /// Void that gets the saves and sets the tracking variables.
        /// </summary>
        public static void SetSaves()
        {
            PlayerPrefs.DeleteAll();
            if(CheckSave("addExistingComponents"))
                addExistingComponents = JsonUtility.FromJson<BasicBoolData>(PlayerPrefs.GetString("addExistingComponents")).Value;

            if (CheckSave("addMeshCollider"))
                addMeshCollider = JsonUtility.FromJson<BasicBoolData>(PlayerPrefs.GetString("addMeshCollider")).Value;

            if (CheckSave("addBoxColliders"))
                addBoxColliders = JsonUtility.FromJson<BasicBoolData>(PlayerPrefs.GetString("addBoxColliders")).Value;

            if (CheckSave("makeObjectStatic"))
                makeObjectStatic = JsonUtility.FromJson<BasicBoolData>(PlayerPrefs.GetString("makeObjectStatic")).Value;
        }

        /// <summary>
        /// Sets the variable of AddExistingComponents bool.
        /// </summary>
        /// <param name="_newValue">The new variable</param>
        public static void SetAddExistingComponents(bool _newValue)
        {
            addExistingComponents = _newValue;
            BasicBoolData data = new BasicBoolData(_newValue);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("addExistingComponents", json);
        }

        /// <summary>
        /// Sets the variable of AddMeshCollider bool.
        /// </summary>
        /// <param name="_newValue">The new variable</param>
        public static void SetAddMeshCollider(bool _newValue)
        {
            addMeshCollider = _newValue;
            BasicBoolData data = new BasicBoolData(_newValue);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("addMeshCollider", json);
        }

        /// <summary>
        /// Sets the variable of AddBoxColliders bool.
        /// </summary>
        /// <param name="_newValue">The new variable</param>
        public static void SetAddBoxColliders(bool _newValue)
        {
            addBoxColliders = _newValue;
            BasicBoolData data = new BasicBoolData(_newValue);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("addBoxColliders", json);
        }

        /// <summary>
        /// Sets the variable of MakeObjectStatic bool.
        /// </summary>
        /// <param name="_newValue">The new variable</param>
        public static void SetMakeObjectStatic(bool _newValue)
        {
            makeObjectStatic = _newValue;
            BasicBoolData data = new BasicBoolData(_newValue);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("makeObjectStatic", json);
        }

        private static bool CheckSave(string key)
        {
           string temperayString =  PlayerPrefs.GetString(key);

            if (temperayString == string.Empty)
                return false;

            return true;
        }
    }
}
