using IBM.Watson.Assistant.v1;
using IBM.Watson.Assistant.v1.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace XFWatsonDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ChatBotViewModel vm;
            InitializeComponent();


            BindingContext = vm = new ChatBotViewModel();


            vm.Messages.CollectionChanged += (sender, e) =>
            {
                var target = vm.Messages[vm.Messages.Count - 1];
                MessagesList.ScrollTo(target, ScrollToPosition.End, true);
            };
        }
       


       
    }
}
