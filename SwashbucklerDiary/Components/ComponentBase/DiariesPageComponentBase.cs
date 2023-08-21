using Microsoft.AspNetCore.Components;
using SwashbucklerDiary.IServices;
using SwashbucklerDiary.Models;

namespace SwashbucklerDiary.Components
{
    public class DiaryEntriesPageComponentBase
        : PageComponentBase
    {
        [Inject]
        protected IDiaryService DiaryService { get; set; } = default!;
        [Inject]
        protected ITagService TagService { get; set; } = default!;

        protected virtual List<DiaryEntryModel> Diaries { get; set; } = new();
        protected virtual List<TagModel> Tags { get; set; } = new();
        protected static bool IsEnabled = false;
        public void OnPasswordEntered(ChangeEventArgs changeEventArgs)
        {
            if (changeEventArgs?.Value?.ToString() == "abc")
            {
                IsEnabled = true;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitializeDiariesAsync();
            await UpdateTagsAsync();
        }

        protected virtual Task InitializeDiariesAsync()
        {
            return UpdateDiariesAsync();
        }

        protected virtual async Task UpdateDiariesAsync()
        {
            Diaries = await DiaryService.QueryAsync(it => !it.Private);
        }

        protected virtual async Task UpdateTagsAsync()
        {
            Tags = await TagService.QueryAsync();
        }


    }
}
