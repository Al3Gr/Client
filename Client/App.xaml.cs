﻿using Client.Services;
using Client.Views;

namespace Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

    }

   
}