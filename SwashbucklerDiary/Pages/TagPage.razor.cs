using Microsoft.AspNetCore.Components;
using SwashbucklerDiary.Components;
using SwashbucklerDiary.Models;

namespace SwashbucklerDiary.Pages
{
    public partial class TagPage : DiaryEntriesPageComponentBase
    {
        private TagModel Tag = new();

        [Parameter]
        public Guid Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var tagModel = await TagService.FindIncludesAsync(Id);
            if (tagModel == null)
            {
                NavigateToBack();
                return;
            }
            Tag = tagModel;
        }

        protected override List<DiaryEntryModel> Diaries => Tag.Diaries ?? new();

        private void NavigateToWrite()
        {
            NavigateService.NavigateTo($"/write?tagId={Id}");
        }
    }
}
