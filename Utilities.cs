using BBI.Unity.Game;

namespace TemplatePatch
{
    public static class Utilities
    {
        public static string Localize(string unlocalized) => Main.Instance.LocalizationService.Localize(unlocalized, null);
    }
}
