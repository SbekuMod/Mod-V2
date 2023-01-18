
using SbekuMod.utils;
using System.Linq;
using System.Collections.Generic;
using System.IO;


namespace SbekuMod
{
    internal class Popups
    {
        private static readonly string BASE_PATH = "assets/popups";
        public static void ShowWelcomePopup()
        {
            var modInstance = SbekuMod.Instance;
            modInstance.ModHelper.Console.WriteLine(modInstance.EventStorage.Get().HasSeenWelcomeScreen.ToString());

            if (modInstance.EventStorage.Get().HasSeenWelcomeScreen) return;

            string filePath = Path.Combine(UtilityHelper.GetProjectBasePath(), BASE_PATH, "welcome.txt");

            if (!File.Exists(filePath)) return;

            var textPopup = File.ReadAllText(filePath);

            var popup = modInstance.ModHelper.Menus.PopupManager.CreateMessagePopup(textPopup);
            popup.OnConfirm += () =>
            {
                modInstance.ModHelper.Console.WriteLine("CONFIRMED");

                modInstance.EventStorage.Get().HasSeenWelcomeScreen = true;
                modInstance.EventStorage.Save();
            };

        }

        public static bool CanShowCredits()
        {
            string filePath = Path.Combine(UtilityHelper.GetProjectBasePath(), BASE_PATH, "credits.txt");

            return File.Exists(filePath);
        }

        public static void ShowCreditsPopup()
        {
            SbekuMod.Instance.EventStorage.Get().HasSeenEnding = true;
            SbekuMod.Instance.EventStorage.Save();

            string filePath = Path.Combine(UtilityHelper.GetProjectBasePath(), BASE_PATH, "credits.txt");

            if (!File.Exists(filePath)) return;

            var textLines = File.ReadAllLines(filePath);

            if (textLines.Length == 0) return;

            var PER_PAGE = 7;

            var pages = new List<string>();
            var lastPage = "";

            var i = 0;
            foreach (var line in textLines)
            {
                lastPage = lastPage + "\n" + line;

                i++;

                if(i >= PER_PAGE)
                {
                    pages.Add(lastPage);
                    lastPage = "";
                    i = 0;
                }
            }

            if(!string.IsNullOrEmpty(lastPage))
                pages.Add(lastPage);

            ShowPagesPopup(pages);

        }


        private static void ShowPagesPopup(List<string> pages, int index = 0)
        {
            var popup = SbekuMod.Instance.ModHelper.Menus.PopupManager.CreateMessagePopup(pages.ElementAt(index), okMessage: index < pages.Count - 1 ?  "Prossima Pagina" : "Chiudi");
            popup.OnConfirm += () =>
            {
                if(index < pages.Count - 1) ShowPagesPopup(pages, index + 1);
            };
        }

    }
}
