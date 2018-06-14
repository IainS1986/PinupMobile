using System;
using PinupMobile.Core.Remote.DTO;

namespace PinupMobile.Core.Remote.Model
{
    public class Item
    {
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private int _gameId;
        public int GameID
        {
            get { return _gameId; }
            set { _gameId = value; }
        }

        public Item(){}

        public Item(CurrentItemDTO dto)
        {
            DisplayName = dto?.GameDisplay;
            GameID = dto.GameID;
        }
    }
}
