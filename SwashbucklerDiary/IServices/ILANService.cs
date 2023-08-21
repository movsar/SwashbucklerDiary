﻿using SwashbucklerDiary.Models;
using System.Net;

namespace SwashbucklerDiary.IServices
{
    public interface ILANService
    {
        bool IsConnection();
        string GetLocalIPv4();
        string GetIPPrefix(string ipAddress);
        bool Ping(IPAddress address);
        LANDeviceInfo GetLocalLANDeviceInfo();
        string GetLocalDeviceName();
        DevicePlatformType GetLocalDevicePlatformType();
        string GetDevicePlatformTypeIcon(DevicePlatformType platformType);
        Task LANSendAsync(List<DiaryEntryModel> diaries, Stream stream, Func<long, long, Task> action);
        Task<List<DiaryEntryModel>> LANReceiverAsync(Stream stream, long size, Func<long, long, Task> action);
    }
}
