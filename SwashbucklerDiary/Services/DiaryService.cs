using SwashbucklerDiary.Extend;
using SwashbucklerDiary.IRepository;
using SwashbucklerDiary.IServices;
using SwashbucklerDiary.Models;
using System.Linq.Expressions;

namespace SwashbucklerDiary.Services
{
    public class DiaryService : BaseService<DiaryEntryModel>, IDiaryService
    {
        private readonly IDiaryRepository _iDiaryRepository;

        public DiaryService(IDiaryRepository iDiaryRepository)
        {
            base._iBaseRepository = iDiaryRepository;
            _iDiaryRepository = iDiaryRepository;
        }

        public override Task<DiaryEntryModel> FindAsync(Guid id)
        {
            return _iDiaryRepository.GetByIdAsync(id);
        }

        public override Task<DiaryEntryModel> FindAsync(Expression<Func<DiaryEntryModel, bool>> func)
        {
            return _iDiaryRepository.GetFirstAsync(func);
        }

        public override Task<List<DiaryEntryModel>> QueryAsync()
        {
            return _iDiaryRepository.GetListAsync();
        }

        public override Task<List<DiaryEntryModel>> QueryAsync(Expression<Func<DiaryEntryModel, bool>> func)
        {
            return _iDiaryRepository.GetListAsync(func);
        }

        public Task<List<TagModel>> GetTagsAsync(Guid id)
        {
            return _iDiaryRepository.GetTagsAsync(id);
        }

        public Task<bool> UpdateIncludesAsync(DiaryEntryModel model)
        {
            return _iDiaryRepository.UpdateIncludesAsync(model);
        }

        public Task<bool> UpdateTagsAsync(DiaryEntryModel model)
        {
            return _iDiaryRepository.UpdateTagsAsync(model);
        }

        public Task<bool> ImportAsync(List<DiaryEntryModel> diaries)
        {
            return _iDiaryRepository.ImportAsync(diaries);
        }

        public async Task<int> GetWordCount(WordCountType type)
        {
            var diaries = await QueryAsync(it=>!it.Private);
            var wordCount = 0;
            if (type == WordCountType.Word)
            {
                foreach (var item in diaries)
                {
                    wordCount += item.Content?.WordCount() ?? 0;
                }
            }
            if(type == WordCountType.Character)
            {
                foreach (var item in diaries)
                {
                    wordCount += item.Content?.CharacterCount() ?? 0;
                }
            }
            return wordCount;
        }

        public Task<List<DateOnly>> GetAllDates()
        {
            return _iDiaryRepository.GetAllDates();
        }

        public Task<List<DateOnly>> GetAllDates(Expression<Func<DiaryEntryModel, bool>> func)
        {
            return _iDiaryRepository.GetAllDates(func);
        }
    }
}
