// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using OpenTok;
using Surrogate.Implementations;
using Surrogate.Model;
using System;
using System.Collections.Generic;

namespace Surrogate.Roboter.MCamera
{

    public interface ICameraConnection<C> : IConnection
    {
        C GetCamera();
    }

    public class Camera0 : AbstractConnection, ICameraConnection<IVideoCapturer>
    {
        public override string Name => FrameworkConstants.InternalCameraName;
        private IVideoCapturer _capturer;

        public override bool Connect()
        {
            IList<VideoCapturer.VideoDevice> devices = VideoCapturer.EnumerateDevices();
            if(devices.Count > 0)
            {
                _capturer = devices[0].CreateVideoCapturer();
                Status = ConnectionStatus.Ready;
                return true;
            }
            Status = ConnectionStatus.Disconnected;
            return false;
        }

        public IVideoCapturer GetCamera()
        {
            return _capturer;
        }

        public override bool Disconnect()
        {
            return false;
        }
    }

    public class Camera1 : AbstractConnection, ICameraConnection<IVideoCapturer>
    {
        public override string Name => FrameworkConstants.Camera1Name;

        private IVideoCapturer _capturer;

        public override bool Connect()
        {
            IList<VideoCapturer.VideoDevice> devices = VideoCapturer.EnumerateDevices();
            if (devices.Count > 1)
            {
                _capturer = devices[1].CreateVideoCapturer();
                Status = ConnectionStatus.Ready;
                return true;
            }
            Status = ConnectionStatus.Disconnected;
            return false;
        }

        public IVideoCapturer GetCamera()
        {
            return _capturer;
        }

        public override bool Disconnect()
        {
            return false;
        }
    }

    public class Camera2 : AbstractConnection, ICameraConnection<IVideoCapturer>
    {
        public override string Name => FrameworkConstants.Camera2Name;

        private IVideoCapturer _capturer;

        public override bool Connect()
        {
            IList<VideoCapturer.VideoDevice> devices = VideoCapturer.EnumerateDevices();
            if (devices.Count > 2)
            {
                _capturer = devices[2].CreateVideoCapturer();
                Status = ConnectionStatus.Ready;
                return true;
            }
            Status = ConnectionStatus.Disconnected;
            return false;
        }

        public IVideoCapturer GetCamera()
        {
            return _capturer;
        }

        public override bool Disconnect()
        {
            return false;
        }
    }
}
