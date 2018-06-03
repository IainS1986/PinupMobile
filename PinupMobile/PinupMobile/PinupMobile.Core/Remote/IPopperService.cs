﻿using System;
using System.Threading.Tasks;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.Remote
{
    public interface IPopperService
    {
        Task<Item> GetCurrentItem();

        Task<bool> ServerExists();

        Task<bool> SendGameNext();

        Task<bool> SendGamePrev();

        Task<bool> SendPageNext();

        Task<bool> SendPagePrev();
    }
}