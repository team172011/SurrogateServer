using Surrogate.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Controller
{
    /// <summary>
    /// Interface describing a visual Module. A visual Module is a <see cref="IModule"/> with
    /// an <see cref="UserControl"/> as view component and can be selected or disselected from
    /// the user
    /// </summary>
    public interface IVisualModule : IModule
    {
        void OnSelected();
        void OnDisselected();
        /// <summary>
        /// Possibility to get the view element of the IModule
        /// </summary>
        /// <returns>a ContentControl object that can be added to center of the mainWindow</returns>
        ModuleView GetPage();
    }
}
