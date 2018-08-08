// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Controller;

namespace Surrogate.View
{
    /// <summary>
    /// WPF/XAML Wrapper. Normally the XYView.cs should extend ModuleView<XYModule> with the generic class parameter XYModule
    /// that describes the Controller class (maybe XYModule<XYModulInfo, XYModulProperties>
    /// But partial classes do not support generic class arguments and we have to specifie a general controller class in this base class
    /// </summary>
    public class ModuleViewBase : ModuleView<AbstractController>
    {
        public ModuleViewBase()
        {

        }

        public ModuleViewBase(AbstractController controller = null) : base(controller)
        {

        }
    }
}
