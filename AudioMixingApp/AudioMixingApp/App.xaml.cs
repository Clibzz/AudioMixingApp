﻿using AudioMixingApp.Viewmodels;

namespace AudioMixingApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new PlaylistOverview());
	}

}
