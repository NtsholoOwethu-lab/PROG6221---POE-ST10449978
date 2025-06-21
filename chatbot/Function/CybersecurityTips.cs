using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatbot.Function
{
   public class CybersecurityTips
    {
        private readonly Dictionary<string, List<string>> TipResponses = new(){ // tips stored in a list
            { "phishing", new List<string> {
                " Always check the sender's email address before clicking any links.",
                " Never download attachments from unknown or suspicious emails.",
                " If something feels off, it probably is. Trust your instincts."
            }},
            { "password", new List<string> {
                " Use a passphrase instead of a single word — it's harder to crack.",
                " Enable two-factor authentication wherever possible.",
                " Avoid using personal information in your passwords."
            }},
            { "privacy", new List<string> {
                " Review your privacy settings on social media regularly.",
                " Use a VPN when connecting to public Wi-Fi.",
                " Be mindful of the data apps are collecting from you."
            }},
            { "safe browsing", new List<string> {
                " Keep your browser and extensions up to date to patch security vulnerabilities.",
                " Only visit websites with HTTPS for a secure connection.",
                " Avoid clicking on suspicious ads or pop-ups — they may lead to malware."
            }},
            { "Authentication", new List<string> {
               " Always enable 2FA for critical accounts like email and banking.",
               " Use an authenticator app rather than SMS when possible — it's more secure.",
               " Don’t share your 2FA codes with anyone, even if they claim to be support staff."
            }},


            { " Wifi safety", new List<string> {
               " Avoid accessing sensitive accounts (like banking) over public Wi-Fi.",
               " Use a trusted VPN to encrypt your traffic when on public networks.",
               " Turn off auto-connect to public Wi-Fi networks to avoid rogue hotspots."
            }}
        };

        private readonly Random rng = new();

        public string GetTip(string topic)
        {
            if (TipResponses.ContainsKey(topic))
            {
                var tips = TipResponses[topic];
                return tips[rng.Next(tips.Count)];
            }

            return null;
        }

        public IEnumerable<string> GetTopics()
        {
            return TipResponses.Keys;
        }
    }
}
