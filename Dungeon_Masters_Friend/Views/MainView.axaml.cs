using Dungeon_Masters_Friend.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Dungeon_Masters_Friend;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.WhenAnyValue(x => x.Bounds.Width)
                .Select(width => width >= 800)
                .DistinctUntilChanged()
                .Subscribe(isWide => {
                    ViewModel.IsWide = isWide;
                    if (isWide)
                    {
                        RootSplitView.LeftDrawerOpened = true;
                    }
                    else
                    {
                        RootSplitView.LeftDrawerOpened = false;
                    }
                })
                .DisposeWith(disposables);
        });
    }
}