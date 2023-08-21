using SqlSugar;
using SwashbucklerDiary.IRepository;
using SwashbucklerDiary.Models;
using System.Linq.Expressions;

namespace SwashbucklerDiary.Repository
{
    public class DiaryRepository : BaseRepository<DiaryEntryModel>, IDiaryRepository
    {
        public DiaryRepository(ISqlSugarClient context) : base(context)
        {
        }

        public override Task<List<DiaryEntryModel>> GetListTakeAsync(int count)
        {
            return base.Context.Queryable<DiaryEntryModel>()
                .Includes(it => it.Tags)
                .Includes(it => it.Resources)
                .OrderByDescending(it => it.CreateTime)
                .Take(count)
                .ToListAsync();
        }

        public override Task<List<DiaryEntryModel>> GetListTakeAsync(int count, Expression<Func<DiaryEntryModel, bool>> func)
        {
            return base.Context.Queryable<DiaryEntryModel>()
                .Includes(it => it.Tags)
                .Includes(it => it.Resources)
                .Where(func)
                .OrderByDescending(it => it.CreateTime)
                .Take(count)
                .ToListAsync();
        }

        public override Task<bool> InsertAsync(DiaryEntryModel model)
        {
            return base.Context.InsertNav(model)
            .Include(it => it.Tags)
            .Include(it => it.Resources)
            .ExecuteCommandAsync();
        }

        public override Task<bool> DeleteAsync(DiaryEntryModel model)
        {
            return base.Context.DeleteNav(model)
                .Include(it => it.Tags, new DeleteNavOptions()
                {
                    ManyToManyIsDeleteA = true
                })
                .Include(it => it.Resources, new DeleteNavOptions()
                {
                    ManyToManyIsDeleteA = true
                })
                .ExecuteCommandAsync();
        }

        public override Task<DiaryEntryModel> GetByIdAsync(dynamic id)
        {
            return Context.Queryable<DiaryEntryModel>()
                .Includes(it => it.Tags)
                .Includes(it => it.Resources)
                .InSingleAsync(id);
        }

        public override Task<DiaryEntryModel> GetFirstAsync(Expression<Func<DiaryEntryModel, bool>> whereExpression)
        {
            return Context.Queryable<DiaryEntryModel>()
                .Includes(it => it.Tags)
                .Includes(it => it.Resources)
                .FirstAsync(whereExpression);
        }

        public Task<List<TagModel>> GetTagsAsync(Guid id)
        {
            return base.Context.Queryable<DiaryEntryModel>()
                .LeftJoin<DiaryTagModel>((d, dt) => d.Id == dt.DiaryEntryId)
                .LeftJoin<TagModel>((d, dt, t) => dt.TagId == t.Id)
                .Where(d => d.Id == id)
                .Select((d, dt, t) => t)
                .ToListAsync();
        }

        public override Task<List<DiaryEntryModel>> GetListAsync()
        {
            return base.Context.Queryable<DiaryEntryModel>()
                .Includes(it => it.Tags)
                .Includes(it => it.Resources)
                .OrderByDescending(it => it.CreateTime)
                .ToListAsync();
        }

        public override Task<List<DiaryEntryModel>> GetListAsync(Expression<Func<DiaryEntryModel, bool>> func)
        {
            return base.Context.Queryable<DiaryEntryModel>()
                .Includes(it => it.Tags)
                .Includes(it => it.Resources)
                .Where(func)
                .OrderByDescending(it => it.CreateTime)
                .ToListAsync();
        }

        public Task<bool> UpdateIncludesAsync(DiaryEntryModel model)
        {
            return base.Context.UpdateNav(model)
            .Include(it => it.Tags, new UpdateNavOptions
            {
                ManyToManyIsUpdateA = true
            })
            .Include(it => it.Resources, new UpdateNavOptions
            {
                ManyToManyIsUpdateA = true,
                ManyToManyIsUpdateB = true,
            })
            .ExecuteCommandAsync();
        }

        public Task<bool> UpdateTagsAsync(DiaryEntryModel model)
        {
            return base.Context.UpdateNav(model)
            .Include(it => it.Tags)
            .ExecuteCommandAsync();
        }

        public Task<bool> ImportAsync(List<DiaryEntryModel> diaries)
        {
            return base.Context.UpdateNav(diaries, new UpdateNavRootOptions()
            {
                IsInsertRoot = true
            })
            .Include(it => it.Tags, new UpdateNavOptions
            {
                ManyToManyIsUpdateA = true,
                ManyToManyIsUpdateB = true
            })
            .Include(it => it.Resources, new UpdateNavOptions
            {
                ManyToManyIsUpdateA = true,
                ManyToManyIsUpdateB = true
            })
            .ExecuteCommandAsync();
        }

        public Task<List<DateOnly>> GetAllDates()
        {
            return GetAllDates(it=>true);
        }

        public async Task<List<DateOnly>> GetAllDates(Expression<Func<DiaryEntryModel, bool>> func)
        {
            var dates = await base.Context.Queryable<DiaryEntryModel>()
                .Where(func)
                .Select(s => s.CreateTime.Date)
                .Distinct()
                .ToListAsync();

            return dates.Select(DateOnly.FromDateTime).ToList();
        }
    }
}
