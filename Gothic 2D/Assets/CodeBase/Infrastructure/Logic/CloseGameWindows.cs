using CodeBase.UI.Services.Window;

namespace CodeBase.Infrastructure.Logic
{
    public class CloseGameWindows
    {
        private readonly IWindowService _windowService;
        private WindowId[] _whiteIds;

        public CloseGameWindows(IWindowService windowService, WindowId[] whiteIds)
        {
            _windowService = windowService;
            _whiteIds = whiteIds;
        }

        public void Close(WindowId immunityId = WindowId.None)
        {
            foreach (WindowId id in _whiteIds)
            {
                if (immunityId != id)
                    _windowService.Close(id);
            }
        }
    }
}