﻿using BlazorComponent;
using BlazorComponent.I18n;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NoDecentDiary.IServices;
using NoDecentDiary.Services;
using NoDecentDiary.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoDecentDiary.Pages
{
    public partial class PageMine : IAsyncDisposable
    {
        [Inject]
        public INavigateService? NavigateService { get; set; }
        [Inject]
        private IDiaryService? DiaryService { get; set; }
        [Inject]
        private IPopupService? PopupService { get; set; }
        [Inject]
        private I18n? I18n { get; set; }
        [Inject]
        private ISettingsService? SettingsService { get; set; }
        [Inject]
        public IJSRuntime? JS { get; set; }
        [Inject]
        private ISystemService? SystemService { get; set; }

        [CascadingParameter]
        public Error? Error { get; set; }
        private IJSObjectReference? module;
        private const string DefaultAvatar = "./logo/logo.svg";
        private int DiaryCount { get; set; }
        private long WordCount { get; set; }
        private int ActiveDayCount { get; set; }
        private string? Language { get; set; }
        private string? UserName { get; set; }
        private string? Sign { get; set; }
        private string? Avatar { get; set; }
        private bool _showLanguage;
        private bool ShowLanguage
        {
            get => _showLanguage;
            set => SetShowLanguage(value);
        }
        private bool _showFeedback;
        private bool ShowFeedback
        {
            get => _showFeedback;
            set => SetShowFeedback(value);
        }
        private readonly static Dictionary<string, string> Languages = new()
        {
            {"中文","zh-CN" },
            {"English","en-US" }
        };

        protected override async Task OnInitializedAsync()
        {
            await SetCount();
            await LoadSettings();
        }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS!.InvokeAsync<IJSObjectReference>("import", "./js/getNativeImage.js");
                bool flag = await SettingsService!.ContainsKey(nameof(Avatar));
                if (!flag)
                {
                    Avatar = DefaultAvatar;
                }
                else
                {
                    var avatar = await SettingsService!.Get(nameof(Avatar), DefaultAvatar);
                    await SetAvatar(avatar);
                }
                StateHasChanged();
            }
        }

        private async Task SetCount()
        {
            DiaryCount = await DiaryService!.CountAsync();
            var diaries = await DiaryService!.QueryAsync();
            var wordCount = 0;
            if (I18n!.T("Write.Word") == "1")
            {
                foreach (var item in diaries)
                {
                    wordCount += item.Content?.Split(' ').Length ?? 0;
                }
            }

            if (I18n!.T("Write.Character") == "1")
            {
                foreach (var item in diaries)
                {
                    wordCount += item.Content?.Length ?? 0;
                }
            }
            WordCount = wordCount;
            ActiveDayCount = diaries.Select(it => DateOnly.FromDateTime(it.CreateTime)).Distinct().Count();
        }
        private async Task LoadSettings()
        {
            Language = await SettingsService!.Get(nameof(Language), Languages.First().Value);
            UserName = await SettingsService!.Get<string?>(nameof(UserName), null);
            Sign = await SettingsService!.Get<string?>(nameof(Sign), null);
        }
        private Task ToDo()
        {
            return PopupService!.ToastAsync(it =>
            {
                it.Type = AlertTypes.Info;
                it.Title = I18n!.T("ToDo.Title");
                it.Content = I18n!.T("ToDo.Content");
            });
        }
        private void NavigateToSearch()
        {
            NavigateService!.NavigateTo("/Search");
        }
        private void NavigateToUser()
        {
            NavigateService!.NavigateTo("/User");
        }
        private void NavigateToAbout()
        {
            NavigateService!.NavigateTo("/About");
        }
        private async Task OnChangeLanguage(string value)
        {
            ShowLanguage = false;
            Language = value;
            I18n!.SetCulture(new CultureInfo(value));
            await SettingsService!.Save(nameof(Language), Language);
            await SetCount();
        }
        private async Task SetAvatar(string path)
        {
            using var imageStream = File.OpenRead(path);
            var dotnetImageStream = new DotNetStreamReference(imageStream);
            Avatar = await module!.InvokeAsync<string>("streamToUrl", new object[1] { dotnetImageStream });
        }
        private async Task SendMail()
        {
            ShowFeedback = false;
            //There are some problems in Windows. https://github.com/microsoft/microsoft-ui-xaml/issues/7300
            var mail = "yu-core@qq.com";
            try
            {
                if (SystemService!.IsMailSupported())
                {
                    List<string> recipients = new() { mail };

                    await SystemService!.SendEmail(recipients);
                }
                else
                {
                    await SystemService!.SetClipboard(mail);
                    await PopupService!.ToastSuccessAsync(I18n!.T("Mine.MailCopy"));
                }
            }
            catch (Exception ex)
            {
                Error!.ProcessError(ex);
            }
            
        }
        private async Task ToGithub()
        {
            ShowFeedback = false;
            try
            {
                Uri uri = new Uri("https://github.com/Yu-Core/NoDecentDiary");
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.External);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
                Error!.ProcessError(ex);
            }
        }
        private void SetShowLanguage(bool value)
        {
            if (_showLanguage != value)
            {
                _showLanguage = value;
                if (value)
                {
                    NavigateService!.Action += CloseLanguage;
                }
                else
                {
                    NavigateService!.Action -= CloseLanguage;
                }
            }
        }
        private void CloseLanguage()
        {
            ShowLanguage = false;
            StateHasChanged();
        }
        private void SetShowFeedback(bool value)
        {
            if (_showFeedback != value)
            {
                _showFeedback = value;
                if (value)
                {
                    NavigateService!.Action += CloseFeedback;
                }
                else
                {
                    NavigateService!.Action -= CloseFeedback;
                }
            }
        }
        private void CloseFeedback()
        {
            ShowFeedback = false;
            StateHasChanged();
        }
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (!string.IsNullOrEmpty(Avatar) && Avatar != DefaultAvatar)
            {
                await module!.InvokeVoidAsync("revokeUrl", new object[1] { Avatar });
            }

            if (module is not null)
            {
                await module.DisposeAsync();
            }
            if (ShowLanguage)
            {
                NavigateService!.Action -= CloseLanguage;
            }
            GC.SuppressFinalize(this);
        }
    }
}
