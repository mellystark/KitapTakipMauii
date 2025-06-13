using KitapTakipMauii.ViewModels;
using System;

namespace KitapTakipMauii.Pages
{
    public partial class BooksPage : ContentPage
    {
        private readonly BooksViewModel _viewModel;

        public BooksPage(BooksViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;

            // Alternatif olarak burada da event baðlayabilirsin:
            // GenrePicker.SelectedIndexChanged += GenrePicker_SelectedIndexChanged;
            // AuthorPicker.SelectedIndexChanged += AuthorPicker_SelectedIndexChanged;
        }

        private void GenrePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewModel.FilterByGenreCommand?.Execute(null);
        }

        private void AuthorPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewModel.FilterByAuthorCommand?.Execute(null);
        }

        private void SearchTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.FilterByTitleCommand?.Execute(null);
        }
    }
}
