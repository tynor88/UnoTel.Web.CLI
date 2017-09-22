﻿using System;
using System.Net;

namespace UnoTel.Web.Cli
{
    public class CookieProvider
    {
        private static readonly CookieContainer _cookieContainer = new CookieContainer();

        public CookieContainer CookieContainer => _cookieContainer;

        public Uri UnoTelBaseAddress => new Uri("https://www.unotel.dk");
    }
}
