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

    public class MotorTestModule : Module<ModulProperties, ModuleInfo>
    {
        public MotorTestModule(ModulProperties modulProperties) : base(modulProperties)
        {
        }

        public override ContentControl GetPage()
        {
            return new MotorTestView(this);
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
                        Motor motor = Motor.GetInstance();
                        motor.SetLeftSpeed(-100);
                        motor.SetRightSpeed(-100);
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
                            Motor motor = Motor.GetInstance();
                            motor.SetLeftSpeed(100);
                            motor.SetRightSpeed(100);
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
                            Motor motor = Motor.GetInstance();
                            motor.SetLeftSpeed(-100);
                            motor.SetRightSpeed(100);
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
                            Motor motor = Motor.GetInstance();
                            motor.SetLeftSpeed(100);
                            motor.SetRightSpeed(-100);
                            } catch(Exception pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                default:
                    {
                        try
                        {
                            Motor motor = Motor.GetInstance();
                            motor.PullUp();
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
                Right,
                Stop
            }
        }
    }
}
