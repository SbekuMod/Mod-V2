
using static SbekuMod.patches.CreditsPatch;

namespace SbekuMod.utils
{
    public class CreditsUtility
    {
        public static void StartCredits(CustomCreditsType type)
        {
            CustomCredits = type;
            LoadManager.LoadScene(OWScene.Credits_Fast, LoadManager.FadeType.ToBlack);
        }
    }
}
