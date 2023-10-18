using AudioMixingApp.Models;
using AudioMixingApp.Models.Effects;
using AudioMixingApp.Viewmodels;

namespace AudioMixingApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void navigateMixingPage_Clicked(object sender, EventArgs e)
    {
        //var employeeDetailViewModel = new EmployeeDetailViewModel
        //{
        //    EmployeeId = "3",
        //    EmployeeName = "Hank",
        //    Email = "Hank@gmail.com",
        //    IsPartTimer = true,
        //};
        Song song = new Song
        {
            Effects = new List<Models.Effect> {
                new Equalizer(1, 2, 3),
                new Equalizer(2, 3, 1),
                new Equalizer(3, 1, 2),
            },
            Title = "Stricken",
        };
        var mixingPage = new MixingPage();
        mixingPage.BindingContext = song;
        Navigation.PushAsync(mixingPage);
    }
}