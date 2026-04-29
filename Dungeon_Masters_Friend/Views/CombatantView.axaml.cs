using Avalonia.Controls;
using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Dungeon_Masters_Friend.Views;

public partial class CombatantView : ReactiveUserControl<CombatantViewModel>
{
    public CombatantView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.WhenAnyValue(x => x.Bounds.Width)
                .Select(width => width >= 800)
                .DistinctUntilChanged()
                .Subscribe(isWide =>
                {
                    if (isWide)
                    {
                        SummaryGrid.RowDefinitions = new RowDefinitions("Auto");
                        SummaryGrid.ColumnDefinitions = new ColumnDefinitions("*, *, *, Auto");

                        CurrentHpTextBlock.SetValue(Grid.RowProperty, 0);
                        CurrentHpTextBlock.SetValue(Grid.ColumnProperty, 2);
                        HpControlPanel.SetValue(Grid.RowProperty, 0);
                        HpControlPanel.SetValue(Grid.ColumnProperty, 3);
                    }
                    else
                    {
                        SummaryGrid.RowDefinitions = new RowDefinitions("Auto, Auto");
                        SummaryGrid.ColumnDefinitions = new ColumnDefinitions("*, Auto");

                        CurrentHpTextBlock.SetValue(Grid.RowProperty, 1);
                        CurrentHpTextBlock.SetValue(Grid.ColumnProperty, 0);
                        HpControlPanel.SetValue(Grid.RowProperty, 1);
                        HpControlPanel.SetValue(Grid.ColumnProperty, 1);
                    }
                })
                .DisposeWith(disposables);
        });
    }
}