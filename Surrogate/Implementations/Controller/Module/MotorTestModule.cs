// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Implementations
{
    using System.Windows.Controls;
    using Surrogate.Modules;
    using Surrogate.Roboter;
    using Surrogate.View;
    using Surrogate.Roboter.MMotor;
    using Surrogate.Model.Module;

    public class MotorTestModule : VisualModule<ModuleProperties, ModuleInfo>
    {
        public Motor _motor;

        public MotorTestModule(ModuleProperties modulProperties) : base(modulProperties)
        {
        }

        public override ModuleView GetPage()
        {
            return new MotorTestView(this);
        }

        public override void OnDisselected()
        {
        }

        public override void OnSelected()
        {
        }

        public override void Start(ModuleInfo info)
        {
            MotorTestInfo mInfo = (MotorTestInfo)info;
            
            switch (mInfo.GetDirection)
            {
                case MotorTestInfo.Direction.Backwards:
                    {
                        try
                        {

                            _motor.LeftSpeed=(-100);
                            _motor.RightSpeed=(-100);
                        } catch(Exception pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                case MotorTestInfo.Direction.Forwards:
                    {
                        try
                        {
                            _motor.LeftSpeed = (100);
                            _motor.RightSpeed = (100);
                        } catch (Exception pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                case MotorTestInfo.Direction.Left:
                    {
                        try
                        {
                            _motor.LeftSpeed = (-100);
                            _motor.RightSpeed = (100);
                        } catch (Exception pnve)
                        {
                        log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                case MotorTestInfo.Direction.Right:
                    {
                        try
                        {
                            Motor.Instance.LeftSpeed = (100);
                            Motor.Instance.RightSpeed = (-100);
                            } catch(Exception pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                case MotorTestInfo.Direction.OnlyRight:
                    {
                        try
                        {

                            Motor.Instance.LeftSpeed = (0);
                            Motor.Instance.RightSpeed = (200);
                        }
                        catch (Exception pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                case MotorTestInfo.Direction.OnlyLeft:
                    {
                        try
                        {
                            Motor.Instance.LeftSpeed =(100);
                            Motor.Instance.RightSpeed = (0);
                        }
                        catch (Exception pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                default:
                    {
                        try
                        {
                            Motor.Instance.PullUp();
                        } catch(Exception pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }

            }
        }

        public override void Stop()
        {
            Motor.Kill();
        }

        public class MotorTestInfo : ModuleInfo
        {
            private Direction _direction;

            public Direction GetDirection { get => _direction;}

            public MotorTestInfo(Direction direction)
            {
                this._direction = direction;
            }
            public enum Direction
            {
                Forwards,
                Backwards,
                Left,
                OnlyLeft,
                Right,
                OnlyRight,
                Stop
            }
        }
    }
}
