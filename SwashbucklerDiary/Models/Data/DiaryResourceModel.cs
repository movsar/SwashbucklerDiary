namespace SwashbucklerDiary.Models
{
    public class DiaryResourceModel : BaseModel
    {
        public Guid DiaryEntryId { get; set; }
        public string? ResourceUri { get; set; }
    }
}
