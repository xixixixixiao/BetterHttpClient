using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BetterHttpClient.CheckService
{
    public class ProxyJudgeService : ProxyJudgeServiceAbstract
    {
        public override bool IsProxyAnonymous(Proxy proxy)
        {
            var client = new HttpClient(proxy, Encoding.UTF8)
            {
                NumberOfAttempts = NumberOfAttempts
            };
            string page = null;

            try
            {
                page = client.Get("http://proxyjudge.info/");
            }
            catch (WebException)
            {
                proxy.IsOnline = false;
            }

            if (page == null)
                return false;
            if (page.Contains("<title>Proxyjudge.info</title>") && !page.Contains(MyIp))
                return true;
            return false;
        }

        protected override string GetMyIp()
        {
            var client = new HttpClient(Encoding.UTF8);
            var page = client.Get("http://proxyjudge.info/");

            var match = Regex.Match(page, "REMOTE_ADDR = (.*?)\\n");
            if (!match.Success)
                throw new GetMyIpException();
            else
                return match.Groups[1].Value;
        }
    }
}