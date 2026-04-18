using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Dungeon_Masters_Friend.Views;

public partial class CreatureView : ReactiveUserControl<CreatureViewModel>
{
    public CreatureView()
    {
        InitializeComponent();

        var adaptiveElements = new Control[]
        {

        };

        //this.WhenActivated(disposables =>
        //{
        //    this.WhenAnyValue(x => x.Bounds.Width)
        //        .Select(width => width >= 800)
        //        .DistinctUntilChanged()
        //        .Subscribe(isWide =>
        //        {
        //            if (isWide)
        //            {
        //                // Update styles for elements that should change
        //                foreach (var item in adaptiveElements)
        //                {
        //                    item.Classes.Add("wide");
        //                }

        //                // Update grid layout
        //                DetailsGrid.RowDefinitions = new RowDefinitions("Auto, Auto, Auto, Auto");
        //                DetailsGrid.ColumnDefinitions = new ColumnDefinitions("*, *, *, *, *, *");
        //                IntelligenceTextBlock.SetValue(Grid.RowProperty, 1);
        //                IntelligenceTextBlock.SetValue(Grid.ColumnProperty, 3);
        //                WisdomTextBlock.SetValue(Grid.RowProperty, 1);
        //                WisdomTextBlock.SetValue(Grid.ColumnProperty, 4);
        //                CharismaTextBlock.SetValue(Grid.RowProperty, 1);
        //                CharismaTextBlock.SetValue(Grid.ColumnProperty, 5);
        //                TraitsList.SetValue(Grid.RowProperty, 2);
        //                TraitsList.SetValue(Grid.ColumnSpanProperty, 6);
        //                ActionsList.SetValue(Grid.RowProperty, 3);
        //                ActionsList.SetValue(Grid.ColumnSpanProperty, 6);
        //            }
        //            else
        //            {
        //                // Update styles for elements that should change
        //                foreach (var item in adaptiveElements)
        //                {
        //                    item.Classes.Remove("wide");
        //                }

        //                // Update grid layout
        //                DetailsGrid.RowDefinitions = new RowDefinitions("Auto, Auto, Auto, Auto, Auto");
        //                DetailsGrid.ColumnDefinitions = new ColumnDefinitions("*, *, *");
        //                IntelligenceTextBlock.SetValue(Grid.RowProperty, 2);
        //                IntelligenceTextBlock.SetValue(Grid.ColumnProperty, 0);
        //                WisdomTextBlock.SetValue(Grid.RowProperty, 2);
        //                WisdomTextBlock.SetValue(Grid.ColumnProperty, 1);
        //                CharismaTextBlock.SetValue(Grid.RowProperty, 2);
        //                CharismaTextBlock.SetValue(Grid.ColumnProperty, 2);
        //                TraitsList.SetValue(Grid.RowProperty, 3);
        //                TraitsList.SetValue(Grid.ColumnSpanProperty, 3);
        //                ActionsList.SetValue(Grid.RowProperty, 4);
        //                ActionsList.SetValue(Grid.ColumnSpanProperty, 3);
        //            }
        //        })
        //        .DisposeWith(disposables);
        //});
    }
}