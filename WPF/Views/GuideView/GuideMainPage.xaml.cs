﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.Domain.Models;
using BookingApp.View;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.GuidesViewModel;

namespace BookingApp.View.GuideView
{
    /// <summary>
    /// Interaction logic for GuideMainPage1.xaml
    /// </summary>
    public partial class GuideMainPage : Page
    {
        public GuideMainPage(User user)
        {
            InitializeComponent();
            DataContext = new GuideMainPageViewModel(user);
        }

       
       

      

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}

