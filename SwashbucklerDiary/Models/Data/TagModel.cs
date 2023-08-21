using SqlSugar;

namespace SwashbucklerDiary.Models
{
    public class TagModel : BaseModel
    {
        public string? Name { get; set; }

        [Navigate(typeof(DiaryTagModel), nameof(DiaryTagModel.TagId), nameof(DiaryTagModel.DiaryEntryId))]
        public List<DiaryEntryModel>? Diaries { get; set; }
    }
}
