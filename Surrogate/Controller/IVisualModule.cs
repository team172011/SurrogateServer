using Surrogate.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Surrogate.Controller
{
    /// <summary>
    /// Interface describing a visual Module. A visual Module is a <see cref="IModule"/> with
    /// an <see cref="UserControl"/> as view component and can be selected or disselected by
    /// user
    /// </summary>
    public interface IVisualModule
    {
        /// <summary>
        /// This function will be called if the visual module has been selected by user
        /// </summary>
        void OnSelected();

        /// <summary>
        /// This function will be called if the visual module has been disselected by user. E.g 
        /// abother viusal module has been selected, or the application has been closed
        /// </summary>
        void OnDisselected();

        /// <summary>
        /// Possibility to get the view element of the IModule
        /// </summary>
        /// <returns>a ContentControl object that can be added to center of the mainWindow</returns>
        UserControl GetPage();
    }
}
