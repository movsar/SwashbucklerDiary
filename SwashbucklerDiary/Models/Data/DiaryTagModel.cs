namespace SwashbucklerDiary.Models
{
    public class DiaryTagModel : BaseModel
    {
        public Guid DiaryEntryId { get; set; }
        public Guid TagId { get; set; }
    }
}
