﻿using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Presentation.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class PaymentListPage
    {
        private PaymentListViewModel ViewModel => BindingContext as PaymentListViewModel;

        public PaymentListPage(int accountId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentListVm;

            ViewModel.AccountId = accountId;

            if(Device.RuntimePlatform == Device.iOS)
            {
                var addItem = new ToolbarItem
                              {
                                  Text = Strings.AddTitle,
                                  Priority = 0,
                                  Order = ToolbarItemOrder.Primary
                              };
                addItem.Clicked += AddItem_Clicked;

                ToolbarItems.Add(addItem);
            }

            var filterItem = new ToolbarItem
                             {
                                 Command = new Command(OpenDialog),
                                 Text = Strings.FilterLabel,
                                 Priority = 1,
                                 Order = ToolbarItemOrder.Primary
                             };

            ToolbarItems.Add(filterItem);

            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private void OpenDialog()
        {
            Navigation.PushPopupAsync(new FilterPopup
                       {
                           BindingContext = ViewModelLocator.SelectFilterDialogVm
                       })
                      .FireAndForgetSafeAsync();
        }
        private void AddItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new AddPaymentPopup { BindingContext = ViewModel.ViewActionViewModel }).FireAndForgetSafeAsync();
        }
    }
}
