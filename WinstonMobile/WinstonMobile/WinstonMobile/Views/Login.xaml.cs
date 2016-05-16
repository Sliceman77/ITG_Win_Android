using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace WinstonMobile
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }
        public async void OnSignupStackTapped(object sender, EventArgs e)
        {
            if (Login.IsPageInNavigationStack<SignUp>(Navigation))
            {
                await Navigation.PopAsync();
                return;
            }

            var signUpPage = new SignUp();
            await Navigation.PushAsync(signUpPage);
        }

        async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }

        public static bool IsPageInNavigationStack<TPage>(INavigation navigation) where TPage : Page
        {
            if (navigation.NavigationStack.Count > 1)
            {
                var last = navigation.NavigationStack[navigation.NavigationStack.Count - 2];

                if (last is TPage)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
