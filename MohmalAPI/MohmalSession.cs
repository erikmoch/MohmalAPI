using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MohmalAPI
{
    public class MohmalSession
    {
        private HttpClient _httpClient;

        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36";
        private const string EMAIL_PATTERN = "data-email=\"(.+?)\"";
        private const string MSGCOUNT_PATTERN = "badge\">(.+?)<";
        private const string MSGID_PATTERN = "msg-id=\"(.+?)\"";
        private const string SUBJ_PATTERN = "icon\"></i>(.+?)<";
        private const string SENDER_PATTERN = "sender\"><a href=\"#\">(.+?)<";

        private const string IP_API = "https://api.ipify.org";
        private const string MOHMAL_INBOX = "https://www.mohmal.com/en/inbox";
        private const string MOHMAL_CHANGE = "https://www.mohmal.com/pt/change";
        private const string MOHMAL_MESSAGE = "https://www.mohmal.com/pt/message/";

        public string Create(WebProxy webProxy = null)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            if (webProxy != null)
            {
                httpClientHandler.UseProxy = true;
                httpClientHandler.Proxy = webProxy;
            }
            _httpClient = new HttpClient(httpClientHandler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);

            return GetEmail();
        }

        public List<MohmalMessage> GetMessages()
        {
            List<MohmalMessage> messages = new List<MohmalMessage>();

            string stringResponse = Get(MOHMAL_INBOX);
            int messagesCount = int.Parse(Regex.Match(stringResponse, MSGCOUNT_PATTERN).Groups[1].Value);

            if (messagesCount > 0)
            {
                var ids = Regex.Matches(stringResponse, MSGID_PATTERN);
                var subjects = Regex.Matches(stringResponse, SUBJ_PATTERN);
                var senders = Regex.Matches(stringResponse, SENDER_PATTERN);

                for (int i = 0; i < messagesCount; i++)
                {
                    string id = ids[i].Groups[1].Value;
                    string subject = subjects[i].Groups[1].Value;
                    string sender = senders[i].Groups[1].Value;
                    string message = Get(MOHMAL_MESSAGE + id);

                    messages.Add(new MohmalMessage(id, subject, sender, message));
                }
            }
            return messages;
        }

        public string GetEmail()
        {
            string stringResponse = Get(MOHMAL_INBOX);
            string email = Regex.Match(stringResponse, EMAIL_PATTERN).Groups[1].Value;
            return email;
        }

        public string ChangeEmail()
        {
            Get(MOHMAL_CHANGE);
            return GetEmail();
        }

        public string GetSessionIp() => 
            Get(IP_API);

        private string Get(string url)
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                string stringResponse = response.Content.ReadAsStringAsync().Result;

                return stringResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}