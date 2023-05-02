#region

using CodeBase.Services;
using CodeBase.UI.Windows;
using System;
using System.Threading.Tasks;

#endregion

namespace CodeBase.UI.Services.Window
{
    public interface IWindowService : IService
    {
        Task Open(WindowId id);

        void Close(WindowId id);

        bool TryGetWindow<TWindow>(WindowId id, out TWindow result) where TWindow : BaseWindow;

        Action<WindowId> OnWindowOpen { get; set; }

        Task<TWindow> GetOrOpenWindow<TWindow>(WindowId id) where TWindow : BaseWindow;

        bool IsWindowOpen(WindowId id);
    }
}