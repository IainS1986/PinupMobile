using System;
using PinupMobile.Core.Remote.DTO;

namespace PinupMobile.Core.Remote.Model
{
    public class CurrentItem
    {
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public CurrentItem(){}

        public CurrentItem(CurrentItemDTO dto)
        {
            DisplayName = dto?.GameDisplay;
        }
    }
}
