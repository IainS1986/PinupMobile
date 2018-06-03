﻿using System;
namespace PinupMobile.Core.Remote.Model
{
    public class PopperResponse<T> where T : class
    {
        public int Code;
        public string Messsage;
        public T Data;
        public bool Success;

        public PopperResponse()
        {
        }
    }
}
