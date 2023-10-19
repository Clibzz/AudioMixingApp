using AudioMixingApp.Models;
using AudioMixingApp.Models.Effects;

namespace AudioMixingApp.Viewmodels;

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
            Effects = new List<IEffect> {
                new Equalizer(0, 5, 2),
                new Equalizer(0, 10, 5),
                new Equalizer(0, 10, 7),
            },
        };
        var mixingPage = new MixingPage();
        mixingPage.BindingContext = song;
        Navigation.PushAsync(mixingPage);
    }
}