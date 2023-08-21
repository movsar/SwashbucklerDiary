using SwashbucklerDiary.Models;
using System.Linq.Expressions;

namespace SwashbucklerDiary.IServices
{
    public interface IDiaryService : IBaseService<DiaryEntryModel>
    {
        Task<bool> UpdateTagsAsync(DiaryEntryModel model);
        Task<List<TagModel>> GetTagsAsync(Guid id);
        Task<bool> UpdateIncludesAsync(DiaryEntryModel model);
        Task<int> GetWordCount(WordCountType type);
        Task<bool> ImportAsync(List<DiaryEntryModel> diaries);
        Task<List<DateOnly>> GetAllDates();
        Task<List<DateOnly>> GetAllDates(Expression<Func<DiaryEntryModel, bool>> func);
    }
}
