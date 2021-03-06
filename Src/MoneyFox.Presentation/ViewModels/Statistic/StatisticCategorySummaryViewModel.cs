﻿using MediatR;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics.Queries.GetCategorySummary;
using MoneyFox.Presentation.Services;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using static MoneyFox.Presentation.Views.PaymentForCategoryListPage;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel"/>
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;

        private ObservableCollection<CategoryOverviewViewModel> categorySummary;

        public StatisticCategorySummaryViewModel(IMediator mediator,
                                                 ISettingsFacade settingsFacade,
                                                 IDialogService dialogService,
                                                 INavigationService navigationService) : base(mediator, settingsFacade)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>();
            IncomeExpenseBalance = new IncomeExpenseBalanceViewModel();
        }

        private IncomeExpenseBalanceViewModel incomeExpenseBalance;

        public IncomeExpenseBalanceViewModel IncomeExpenseBalance
        {
            get => incomeExpenseBalance;
            set
            {
                if(incomeExpenseBalance == value)
                    return;
                incomeExpenseBalance = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CategoryOverviewViewModel> CategorySummary
        {
            get => categorySummary;
            private set
            {
                categorySummary = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasData));
            }
        }

        /// <inheritdoc/>
        public bool HasData => CategorySummary.Any();

        public Command<CategoryOverviewViewModel> ShowCategoryPaymentsCommand => new Command<CategoryOverviewViewModel>(ShowCategoryPayments);

        /// <summary>
        /// Overrides the load method to load the category summary data.
        /// </summary>
        protected override async Task Load()
        {
            try
            {
                CategorySummaryModel categorySummaryModel =
                await Mediator.Send(new GetCategorySummaryQuery { EndDate = EndDate, StartDate = StartDate });

                CategorySummary = new ObservableCollection<CategoryOverviewViewModel>(categorySummaryModel
                                                                                         .CategoryOverviewItems
                                                                                         .Select(x => new CategoryOverviewViewModel
                                                                                         {
                                                                                             CategoryId = x.CategoryId,
                                                                                             Value = x.Value,
                                                                                             Average = x.Average,
                                                                                             Label = x.Label,
                                                                                             Percentage = x.Percentage
                                                                                         }));

                IncomeExpenseBalance = new IncomeExpenseBalanceViewModel
                {
                    TotalEarned = categorySummaryModel.TotalEarned,
                    TotalSpent = categorySummaryModel.TotalSpent
                };
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Error during loading. {1}", ex);
                await dialogService.ShowMessageAsync("Error", ex.ToString());
            }
        }

        private void ShowCategoryPayments(CategoryOverviewViewModel categoryOverviewModel)
        {
            navigationService.NavigateToModal(ViewModelLocator.PaymentForCategoryList, new PaymentForCategoryParameter
            {
                CategoryId = categoryOverviewModel.CategoryId,
                TimeRangeFrom = StartDate,
                TimeRangeTo = EndDate
            });
        }
    }
}
