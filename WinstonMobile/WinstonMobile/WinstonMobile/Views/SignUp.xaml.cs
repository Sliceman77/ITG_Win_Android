using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace WinstonMobile
{ 
    public partial class SignUp : ContentPage
    {
        private TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

        public SignUp()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            tapGestureRecognizer.Tapped += OnLoginStackTapped;
            loginStack.GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            tapGestureRecognizer.Tapped -= OnLoginStackTapped;
            loginStack.GestureRecognizers.Remove(tapGestureRecognizer);
        }

        public async void OnLoginStackTapped(object sender, EventArgs e)
        {
            if (Login.IsPageInNavigationStack<Login>(Navigation))
            {
                await Navigation.PopAsync();
                return;
            }

            var loginPage = new Login();
            await Navigation.PushAsync(loginPage);
        }

        async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}
