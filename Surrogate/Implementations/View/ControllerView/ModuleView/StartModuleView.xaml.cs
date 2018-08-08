// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Surrogate.View
{
    /// <summary>
    /// Interaktionslogik für StartModuleView.xaml
    /// </summary>
    public partial class StartModuleView : ModuleViewBase
    {
        public StartModuleView():base(null)
        {
            InitializeComponent();
            if(SurrogateFramework.MainController.ConnectionHandler.GetConnection(FrameworkConstants.ControllerName).Status != Model.ConnectionStatus.Ready)
            {
                MessageBox.Show("Achtung! Es ist kein Controller angeschlossen. Für die manuelle Steuerung wird das Anschließen eines Controllers empfohlen.", "Kein Controller angeschlossen");
            }
        }
    }
}
