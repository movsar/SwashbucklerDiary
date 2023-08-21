using SwashbucklerDiary.Models;
using System.Linq.Expressions;

namespace SwashbucklerDiary.IRepository
{
    public interface IDiaryRepository : IBaseRepository<DiaryEntryModel>
    {
        Task<bool> UpdateTagsAsync(DiaryEntryModel model);
        Task<List<TagModel>> GetTagsAsync(Guid id);
        Task<bool> UpdateIncludesAsync(DiaryEntryModel model);
        Task<bool> ImportAsync(List<DiaryEntryModel> diaries);
        Task<List<DateOnly>> GetAllDates();
        Task<List<DateOnly>> GetAllDates(Expression<Func<DiaryEntryModel, bool>> func);
    }
}
