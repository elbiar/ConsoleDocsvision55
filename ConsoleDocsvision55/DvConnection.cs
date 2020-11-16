using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;

namespace ConsoleDocsvision55
{

    internal static class DvConnection
    {
        public static UserSession UserSession { get; set; }
        public static ObjectContext ObjectContext { get; set; }
    }
}
