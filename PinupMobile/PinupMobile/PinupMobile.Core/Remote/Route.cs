using System;
namespace PinupMobile.Core.Remote
{
    [AttributeUsage(AttributeTargets.Class)]  
    public class Route : Attribute
    {
        private string _url;

        public string Url
        {
            get { return _url; }
        }

        public Route(string url)
        {
            _url = url;
        }
    }
}
