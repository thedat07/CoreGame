using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities
{
    public class SaveExtensions
    {
        public const string keyFileMasterData = "FileMasterData";
        public const string keyFileMasterData2 = "FileMasterData2";

        public const string keyFileData = "FileData";
        public const string keyFileSettingData = "FileSettingData";
        public const string keyFileAdsData = "FileAdsData";
        public const string keyFileShopData = "FileShopData";
        public const string keyFileHomeData = "FileHomeData";

        // ========== Generic ==========

        /// <summary>GET: Lấy dữ liệu từ file</summary>
        public static T Get<T>(string key, T defaultValue, string filePath = "FileGame")
        {
            if (!ES3.KeyExists(key, filePath))
                Post(key, defaultValue, filePath); // POST nếu chưa tồn tại

            return ES3.Load(key, filePath, defaultValue);
        }

        /// <summary>POST: Tạo dữ liệu nếu chưa tồn tại</summary>
        public static void Post<T>(string key, T value, string filePath = "FileGame")
        {
            if (!ES3.KeyExists(key, filePath))
                ES3.Save(key, value, filePath);
        }

        /// <summary>PUT: Ghi đè dữ liệu</summary>
        public static void Put<T>(string key, T value, string filePath = "FileGame")
        {
            ES3.Save(key, value, filePath);
        }

        /// <summary>DELETE: Xóa dữ liệu</summary>
        public static void Delete(string key, string filePath = "FileGame")
        {
            if (ES3.KeyExists(key, filePath))
                ES3.DeleteKey(key, filePath);
        }

        // ========== SETTING DATA ==========

        public static T GetSetting<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue, keyFileSettingData);
        }

        public static void PutSetting<T>(string key, T value)
        {
            Put<T>(key, value, keyFileSettingData);
        }

        public static void DeleteSetting(string key)
        {
            Delete(key, keyFileSettingData);
        }

        // ========== ADS DATA ==========

        public static T GetAds<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue, keyFileAdsData);
        }

        public static void PutAds<T>(string key, T value)
        {
            Put<T>(key, value, keyFileAdsData);
        }

        public static void DeleteAds(string key)
        {
            Delete(key, keyFileAdsData);
        }

        // ========== SHOP DATA ==========

        public static T GetShop<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue, keyFileShopData);
        }

        public static void PutShop<T>(string key, T value)
        {
            Put<T>(key, value, keyFileShopData);
        }

        public static void DeleteShop(string key)
        {
            Delete(key, keyFileShopData);
        }

        // ========== HOME DATA ==========

        public static T GetHome<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue, keyFileHomeData);
        }

        public static void PutHome<T>(string key, T value)
        {
            Put<T>(key, value, keyFileHomeData);
        }

        public static void DeleteHome(string key)
        {
            Delete(key, keyFileHomeData);
        }

        public static readonly string[] AllSaveFiles = new string[]
        {
            keyFileMasterData,
            keyFileMasterData2,
            keyFileData,
            keyFileSettingData,
            keyFileAdsData,
            keyFileShopData,
            keyFileHomeData
        };
        
        public static void DeleteAllLocalData()
        {
            foreach (var fileKey in AllSaveFiles)
            {
                if (ES3.FileExists(fileKey))
                {
                    ES3.DeleteFile(fileKey);
                    Console.Log($"[EZSave] Deleted file: {fileKey}");
                }
            }

            // Xóa tất cả file .es3 còn sót lại (phòng trường hợp có file chưa liệt kê)
            foreach (var file in System.IO.Directory.GetFiles(Application.persistentDataPath))
            {
                if (file.EndsWith(".es3"))
                {
                    System.IO.File.Delete(file);
                    Console.Log($"[EZSave] Deleted leftover: {System.IO.Path.GetFileName(file)}");
                }
            }

            Console.Log("[EZSave] ✅ All local save files deleted successfully.");
        }
    }
}