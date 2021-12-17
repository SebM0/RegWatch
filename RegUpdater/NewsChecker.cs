using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RegUpdater
{
    class NewsChecker : RecurrentTool
    {
        public event Notify ChangeDetected;
        public string visited = "https://cosmo-games.com/stock-ps5/#h-infos-sur-les-prochains-stocks-de-ps5-et-disponibilite";
        private string lastText = "";
        protected override TimeSpan timeout => TimeSpan.FromMinutes(1);
        public NewsChecker()
        {
        }

        protected override void Run()
        {
            // Start listening for events.
            var doc = new HtmlWeb().Load(visited);
            var notices = doc.DocumentNode.SelectNodes("//div[contains(@class, 'gb-notice-text')]");
            if (notices.Count == 0)
            {
                Console.WriteLine("gb-notice-text not found");
                return;
            }
            string currentText = "";
            foreach (var paragraph in notices.First().Elements("p"))
            {
                string text = paragraph.InnerText;
                if (text.StartsWith("Rappel"))
                    break;
                if (currentText.Count() > 0)
                    currentText += "\r\n";
                currentText += text;
                Console.WriteLine(text);
            }
            if (!currentText.Equals(lastText))
            {
                lastText = currentText;
                ChangeDetected();
            }
        }
    }
}
