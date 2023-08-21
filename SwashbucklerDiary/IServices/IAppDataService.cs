﻿using SwashbucklerDiary.Models;

namespace SwashbucklerDiary.IServices
{
    public interface IAppDataService
    {
        string GetBackupFileName();
        /// <summary>
        /// 清除缓存
        /// </summary>
        void ClearCache();
        /// <summary>
        /// 获取缓存大小
        /// </summary>
        /// <returns></returns>
        string GetCacheSize();
        Task<Stream> BackupDatabase(List<DiaryEntryModel> diaries, bool copyResources);
        Task<string?> BackupDatabase(string path,List<DiaryEntryModel> diaries, bool copyResources);
        /// <summary>
        /// 恢复数据库
        /// </summary>
        /// <param name="filePath">数据库文件路径</param>
        Task<bool> RestoreDatabase(string filePath);
        Task<bool> RestoreDatabase(Stream stream);
        Task<string> ExportTxtZipFileAsync(List<DiaryEntryModel> diaries);
        Task<string> ExportJsonZipFileAsync(List<DiaryEntryModel> diaries);
        Task<string> ExportMdZipFileAsync(List<DiaryEntryModel> diaries);
        Task<string> ExportDBZipFileAsync(List<DiaryEntryModel> diaries, bool copyResources);
        Task<bool> ExportTxtZipFileAndSaveAsync(List<DiaryEntryModel> diaries);
        Task<bool> ExportJsonZipFileAndSaveAsync(List<DiaryEntryModel> diaries);
        Task<bool> ExportMdZipFileAndSaveAsync(List<DiaryEntryModel> diaries);
        Task<string> CreateCacheFileAsync(string filePath, string contents);
        Task<string> CreateCacheFileAsync(string filePath, byte[] contents);
        Task<string> CreateAppDataFileAsync(string fn, string filePath);
        Task<string> CreateAppDataImageFileAsync(string filePath);
        Task<string> CreateAppDataAudioFileAsync(string filePath);
        Task<string> CreateAppDataVideoFileAsync(string filePath);
        Task<bool> DeleteAppDataFileByFilePathAsync(string filePath);
        Task<bool> DeleteAppDataFileByCustomSchemeAsync(string uri);
        Task<List<DiaryEntryModel>> ImportJsonFileAsync(string filePath);
    }
}
