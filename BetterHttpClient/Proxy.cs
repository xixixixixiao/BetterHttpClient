using BetterHttpClient.CheckService;
using System;
using System.Net;

namespace BetterHttpClient
{
    public class Proxy : ICloneable
    {
        private volatile bool _isBusy;
        private          bool _isAnonymous;
        private          bool _isChecked;

        /// <summary>
        /// Check if proxy is busy.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            internal set => _isBusy = value;
        }


        public bool IsOnline { get; set; } = true;
        public ProxyTypeEnum ProxyType { get; internal set; }
        internal WebProxy ProxyItem { get; set; }

        public Proxy()
        {
            ProxyItem = new WebProxy();
            ProxyType = ProxyTypeEnum.None;
        }

        public Proxy(string address)
        {
            ProxyItem = new WebProxy(address);
        }

        public Proxy(string ip, int port)
        {
            ProxyItem = new WebProxy(ip, port);
        }

        public Proxy(string ip, int port, ProxyTypeEnum proxyType, NetworkCredential credentials)
        {
            ProxyItem = new WebProxy(ip, port)
            {
                Credentials = credentials
            };
            ProxyType = proxyType;
        }

        /// <summary>
        /// Returns true if proxy can hide your ip address
        /// </summary>
        public bool IsAnonymous(ProxyJudgeService service)
        {
            if (_isChecked)
                return _isAnonymous;

            _isAnonymous = service.IsProxyAnonymous(this);
            _isChecked = true;
            IsOnline = _isAnonymous;
            return _isAnonymous;
        }

        public object Clone()
        {
            var proxy = new Proxy
            {
                _isAnonymous = _isAnonymous,
                _isBusy = _isBusy,
                _isChecked = _isChecked,
                IsOnline = IsOnline,
                ProxyItem = ProxyItem,
                ProxyType = ProxyType
            };
            return proxy;
        }
    }

    public enum ProxyTypeEnum
    {
        Unknown,
        None,
        Http,
        Socks
    }
}