﻿using Windows.UI.Notifications;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;
using NotificationsExtensions.Tiles;

namespace MoneyFox.Windows
{
    public class UserNotification : IUserNotification
    {
        /// <summary>
        ///     Sets the MainTile with new Information
        /// </summary>
        /// <param name="income">Income of these month</param>
        /// <param name="spending">Expense of these month</param>
        /// <param name="earnings">Earnings of these month </param>
        public void UpdateMainTile(string income, string spending, string earnings)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            if (Mvx.Resolve<SettingsShortcutsViewModel>().ShowInfoOnMainTile)
            {
                var content = new TileContent
                {
                    Visual = new TileVisual
                    {
                        TileMedium = GetBindingMediumContent(income, spending, earnings),
                        TileWide = GetBindingWideContent(income, spending, earnings),
                        TileLarge = GetBindingLargeContent(income, spending, earnings)
                    }
                };
                // Update Tile
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(content.GetXml()));
            }
        }

        private static TileBinding GetBindingMediumContent(string income, string spending, string earnings)
        {
            var content = new TileBindingContentAdaptive
            {
                PeekImage = new TilePeekImage
                {
                    Source = new TileImageSource("Assets/Square150x150Logo.scale-400.png")
                },
                Children =
                {
                    new TileText
                    {
                        Text = income,
                        Style = TileTextStyle.CaptionSubtle,
                        Wrap = true
                    },
                    new TileText
                    {
                        Text = spending,
                        Style = TileTextStyle.CaptionSubtle,
                        Wrap = true
                    },
                    new TileText
                    {
                        Text = earnings,
                        Wrap = true,
                        Style = TileTextStyle.Caption
                    }
                }
            };

            return new TileBinding
            {
                Branding = TileBranding.NameAndLogo,
                DisplayName = Strings.ApplicationTitle,
                Content = content
            };
        }

        private static TileBinding GetBindingWideContent(string income, string spending, string earnings)
        {
            var content = new TileBindingContentAdaptive
            {
                PeekImage = new TilePeekImage
                {
                    Source = new TileImageSource("Assets/Wide310x150Logo.scale-400.png")
                },
                Children =
                {
                    new TileText
                    {
                        Text = income,
                        Style = TileTextStyle.CaptionSubtle
                    },
                    new TileText
                    {
                        Text = spending,
                        Style = TileTextStyle.CaptionSubtle
                    },
                    new TileText
                    {
                        Text = earnings,
                        Wrap = true,
                        Style = TileTextStyle.Body
                    }
                }
            };

            return new TileBinding
            {
                Branding = TileBranding.NameAndLogo,
                DisplayName = Strings.ApplicationTitle,
                Content = content
            };
        }

        private static TileBinding GetBindingLargeContent(string income, string spending, string earnings)
        {
            var content = new TileBindingContentAdaptive
            {
                PeekImage = new TilePeekImage
                {
                    Source = new TileImageSource("Assets/Square310x310Logo.scale-400.png")
                },
                Children =
                {
                    new TileText
                    {
                        Text = income,
                        Style = TileTextStyle.CaptionSubtle
                    },
                    new TileText
                    {
                        Text = spending,
                        Style = TileTextStyle.CaptionSubtle
                    },
                    new TileText
                    {
                        Text = earnings,
                        Wrap = true,
                        Style = TileTextStyle.Body
                    }
                }
            };

            return new TileBinding
            {
                Branding = TileBranding.NameAndLogo,
                DisplayName = Strings.ApplicationTitle,
                Content = content
            };
        }
    }
}