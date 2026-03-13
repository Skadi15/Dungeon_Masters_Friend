using Avalonia.Controls;
using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Dungeon_Masters_Friend.Views;

public partial class TreasureGeneratorView : ReactiveUserControl<TreasureGeneratorViewModel>
{
    public TreasureGeneratorView()
    {
        InitializeComponent();

        var adaptiveElements = new Control[]
        {
            OptionsPanel,
            PlayTierTextBlock,
            PlayTierComboBox,
            TroveProfileTextBlock,
            TroveProfileComboBox
        };

        this.WhenActivated(disposables =>
        {
            this.WhenAnyValue(x => x.Bounds.Width)
                .Select(width => width >= 800)
                .DistinctUntilChanged()
                .Subscribe(isWide =>
                {
                    if (isWide)
                    {
                        // Update styles for elements that should change
                        foreach (var item in adaptiveElements)
                        {
                            item.Classes.Add("wide");
                        }

                        // Update grid layout
                        RootGrid.RowDefinitions = new RowDefinitions("Auto, Auto, Auto, *");
                        RootGrid.ColumnDefinitions = new ColumnDefinitions("Auto, *, Auto, *");
                        OptionsPanel.SetValue(Grid.ColumnSpanProperty, 4);
                        GenerateButton.SetValue(Grid.ColumnSpanProperty, 4);
                        TreasureSeparator.SetValue(Grid.ColumnSpanProperty, 4);
                        ItemsTextBlock.SetValue(Grid.RowProperty, 3);
                        ItemsTextBlock.SetValue(Grid.ColumnProperty, 2);
                        ItemsScrollViewer.SetValue(Grid.RowProperty, 3);
                        ItemsScrollViewer.SetValue(Grid.ColumnProperty, 3);
                    }
                    else
                    {
                        // Update styles for elements that should change
                        foreach (var item in adaptiveElements)
                        {
                            item.Classes.Remove("wide");
                        }

                        // Update grid layout
                        RootGrid.RowDefinitions = new RowDefinitions("Auto, Auto, Auto, Auto, *");
                        RootGrid.ColumnDefinitions = new ColumnDefinitions("Auto, *");
                        OptionsPanel.SetValue(Grid.ColumnSpanProperty, 2);
                        GenerateButton.SetValue(Grid.ColumnSpanProperty, 2);
                        TreasureSeparator.SetValue(Grid.ColumnSpanProperty, 2);
                        ItemsTextBlock.SetValue(Grid.RowProperty, 4);
                        ItemsTextBlock.SetValue(Grid.ColumnProperty, 0);
                        ItemsScrollViewer.SetValue(Grid.RowProperty, 4);
                        ItemsScrollViewer.SetValue(Grid.ColumnProperty, 1);
                    }
                })
                .DisposeWith(disposables);
        });
    }
}