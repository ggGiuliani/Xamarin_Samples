//using IBM.WatsonDeveloperCloud.Conversation.v1;
//using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using IBM.Watson.Assistant.v1;
using IBM.Watson.Assistant.v1.Model;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XFWatsonDemo.Models;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace XFWatsonDemo
{
    public class ChatBotViewModel : BindableObject
    {
        private AssistantService _conversation;
        private string _outGoingText;
        public ObservableCollection<ChatMessage> Messages { get; }
        private dynamic _context;
        bool _isInitial=true;


        public ChatBotViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            OutGoingText = string.Empty;
            ConnectToWatson();
        }

        private void ConnectToWatson()
        {
            IamAuthenticator authenticator = new IamAuthenticator(
            apikey: "{apiKey}");
            _conversation = new AssistantService("{versionDate}", authenticator);
            _conversation.SetServiceUrl("[serviceUrl}");

        }

        public string OutGoingText
        {
            get
            {
                return _outGoingText;
            }
            set
            {
                _outGoingText = value;
                OnPropertyChanged();
            }
        }

        

        public ICommand SendCommand => new Command(SendMessage);
        

        

        private async void SendMessage()
        {
            if (!string.IsNullOrEmpty(OutGoingText))
            {
                Messages.Add(new ChatMessage { Text = OutGoingText, IsIncoming = false, MessageDateTime = DateTime.Now });
                string temp = OutGoingText;
                OutGoingText = string.Empty;

                await Task.Run(() => {
                    var result = _conversation.Message(
                        workspaceId: "{workspaceId}",
                        input: new MessageInput()
                        {
                            Text = temp
                        }
                    );
                    //var res = _conversation.Message("watson workspace id", mr);
                    _context = result.Response;

                    OnWatsonMessagerecieved(JsonConvert.SerializeObject(result.Result.Output, Formatting.Indented));
                });
            }

        }

        private void OnWatsonMessagerecieved(string data)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                //string dataJson = JsonConvert.SerializeObject(data);
                Output message = JsonConvert.DeserializeObject<Output>(data);

                //WatsonMessage message = new WatsonMessage.FromJson(data);
                //Message
                //MessageResponse message = JsonConvert.DeserializeObject<MessageResponse>(data);

                var listItem = new ChatMessage
                {
                    
                    IsIncoming= true,
                    MessageDateTime= DateTime.Now

                };
                
                if(message.Generic!=null)
                {
                    foreach(var item in message.Generic)
                    {
                        if (item.ResponseType.Equals("image"))
                        {
                            listItem.Image = item.Source.ToString();
                        }
                        if (item.ResponseType.Equals("text"))
                        {
                            listItem.Text = item.Text;
                        }
                    }
                    
                }
                Console.WriteLine(data);
                Messages.Add(listItem);
            });




        }

        
    }
}
