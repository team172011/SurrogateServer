﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Parameters
{
    public static class Dirs
    {

        public static readonly string userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static readonly string programDir = String.Format(userDir + "{0}", "/Surrogate");

        public static readonly string propertiesDir = String.Format(programDir + "{0}", "/Properties");
        public static readonly string loggingFile = String.Format(programDir + "{0}", "/log.txt");
    }

    
}
