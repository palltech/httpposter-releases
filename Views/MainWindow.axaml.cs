using Avalonia.Controls;
using Avalonia.Interactivity;
using HttpPostGet.Models;
using HttpPostGet.ViewModels;

namespace HttpPostGet.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnRenameFavoriteClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: SavedRequest req } &&
            DataContext is MainWindowViewModel vm)
            vm.SidePanel.StartRenameFavoriteCommand.Execute(req);
    }

    private void OnDeleteFavoriteClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: SavedRequest req } &&
            DataContext is MainWindowViewModel vm)
            vm.SidePanel.DeleteFavoriteCommand.Execute(req);
    }

    private void OnSaveHistoryAsFavoriteClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: HistoryEntry entry } &&
            DataContext is MainWindowViewModel vm)
            vm.SidePanel.StartSaveHistoryAsFavoriteCommand.Execute(entry);
    }

    private void OnDeleteHistoryEntryClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: HistoryEntry entry } &&
            DataContext is MainWindowViewModel vm)
            vm.SidePanel.DeleteHistoryEntryCommand.Execute(entry);
    }

    private void OnClearHistoryClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
            vm.SidePanel.ClearHistoryCommand.Execute(null);
    }
}