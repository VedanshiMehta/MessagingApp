using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public class PrefixEntry : Entry
    {
        private bool isPrefixVisible;

        public static readonly BindableProperty PrefixProperty =
            BindableProperty.Create(nameof(Prefix), typeof(string), typeof(PrefixEntry), "");

        public string Prefix
        {
            get => (string)GetValue(PrefixProperty);
            set => SetValue(PrefixProperty, value);
        }
        public PrefixEntry()
        {
          
            TextChanged += PrefixEntry_TextChanged;
        }

        [Obsolete]
        private void PrefixEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isPrefixVisible && !string.IsNullOrEmpty(e.NewTextValue))
            {
                isPrefixVisible = true;

                
                Text = Prefix + e.NewTextValue;

                // Set cursor position after modifying the text
                Device.BeginInvokeOnMainThread(() =>
                {
                    CursorPosition = Text.Length;
                });
            }
            else if (isPrefixVisible && string.IsNullOrEmpty(e.NewTextValue))
            {
                isPrefixVisible = false;
                Text = string.Empty;
            }
        }

       

    }
}
