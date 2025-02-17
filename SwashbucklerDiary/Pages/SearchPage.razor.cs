﻿using SwashbucklerDiary.Components;
using SwashbucklerDiary.Extend;
using SwashbucklerDiary.Models;
using System.Linq.Expressions;

namespace SwashbucklerDiary.Pages
{
    public partial class SearchPage : DiaryEntriesPageComponentBase
    {
        private bool ShowFilter;
        private SearchForm SearchForm = new()
        {
            ShowSearch = true
        };

        protected override async Task OnInitializedAsync()
        {
            HandleCurrentCache();
            await base.OnInitializedAsync();
        }

        protected override async Task UpdateDiariesAsync()
        {
            Expression<Func<DiaryEntryModel, bool>> func = Func();
            var diaries = await DiaryService.QueryAsync(func);
            Diaries = diaries.OrderByDescending(it => it.CreateTime).ToList();
        }

        private bool ShowSearch
        {
            get => SearchForm.ShowSearch;
            set => SearchForm.ShowSearch = value;
        }
        private string? Search
        {
            get => SearchForm.Search;
            set => SearchForm.Search = value;
        }
        private DateOnly DateOnlyMin => SearchForm.DateFilter.GetDateMinValue();
        private DateOnly DateOnlyMax => SearchForm.DateFilter.GetDateMaxValue();
        private bool IsSearchFiltered => !string.IsNullOrWhiteSpace(Search);
        private bool IsDateFiltered => DateOnlyMin != DateOnly.MinValue || DateOnlyMax != DateOnly.MaxValue;


        private Expression<Func<DiaryEntryModel, bool>> Func()
        {
            Expression<Func<DiaryEntryModel, bool>>? exp = null;
            Expression<Func<DiaryEntryModel, bool>> expPrivate;
            Expression<Func<DiaryEntryModel, bool>> expSearch;
            Expression<Func<DiaryEntryModel, bool>> expDate;

            expPrivate = it => !it.Private;
            expSearch = it => (it.Title ?? string.Empty).ToLower().Contains((Search ?? string.Empty).ToLower()) ||
                (it.Content ?? string.Empty).ToLower().Contains((Search ?? string.Empty).ToLower());


            DateTime DateTimeMin = DateOnlyMin.ToDateTime(default);
            DateTime DateTimeMax = DateOnlyMax.ToDateTime(TimeOnly.MaxValue);
            expDate = it => it.CreateTime >= DateTimeMin && it.CreateTime <= DateTimeMax;

            if (IsDateFiltered)
            {
                exp = exp.And(expDate);
            }

            if (IsSearchFiltered)
            {
                exp = exp.And(expSearch);
            }

            if (exp == null)
            {
                return it => false;
            }
            else
            {
                return exp.And(expPrivate);
            }
        }

        private void HandleCurrentCache()
        {
            SearchForm = (SearchForm?)NavigateService.GetCurrentCache() ?? new();
            NavigateService.SetCurrentCache(() => SearchForm);
        }
    }
}
