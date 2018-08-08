// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using Surrogate.Modules;
using Surrogate.View;
using Surrogate.Roboter.MMotor;
using Surrogate.Model.Module;
using Surrogate.Model;
using System.Windows.Controls;
using System.Windows;

namespace Surrogate.Implementations
{


    public class MotorTestModule : VisualModule<ModulePropertiesBase, ModuleInfo>
    {
        public Motor _motor = Motor.Instance;
        public override IModuleProperties Properties => GetProperties();

        public MotorTestModule() : base(new ModulePropertiesBase("Motor testen", "Modul zum Testen verschiedener Motorparameter", motor:true))
        {
        }

        public override UserControl GetPage()
        {
            return new MotorTestView(this);
        }

        public override bool IsRunning()
        {
            return true;
        }

        public override void OnDisselected()
        {
        }

        public override void OnSelected()
        {
        }

        public override void Start(ModuleInfo info)
        {
            if(_motor == null || !_motor.IsConnected())
            {
                MessageBox.Show("Es besteht keine Verbindung mit dem Motor", "Motorverbindung fehlt");
                return;
            }
            MotorTestInfo mInfo = (MotorTestInfo)info;
            
            switch (mInfo.SpeedDirection)
            {
                case MotorTestInfo.Direction.Backwards:
                    {
                        try
                        {
                            _motor.Start();
                            _motor.LeftSpeedValue = (-100);
                            _motor.RightSpeedValue = (-100);
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
                            _motor.Start();
                            _motor.LeftSpeedValue = (100);
                            _motor.RightSpeedValue = (100);
                        } catch (NullReferenceException pnve)
                        {
                            log.Error("Could not connect to motor: " + pnve.Message + "\n " + pnve.StackTrace);
                        }
                        break;
                    }
                case MotorTestInfo.Direction.Left:
                    {
                        try
                        {
                            _motor.Start();
                            _motor.LeftSpeedValue = (-100);
                            _motor.RightSpeedValue = (100);
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
                            _motor.Start();
                            Motor.Instance.LeftSpeedValue = (100);
                            Motor.Instance.RightSpeedValue = (-100);
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

                            _motor.Start();
                            Motor.Instance.LeftSpeedValue = (0);
                            Motor.Instance.RightSpeedValue = (200);
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
                            _motor.Start();
                            Motor.Instance.LeftSpeedValue = (100);
                            Motor.Instance.RightSpeedValue = (0);
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
                            _motor.Start();
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
            Motor.Instance.Stop();
        }

        public class MotorTestInfo : ModuleInfo
        {

            public Direction SpeedDirection { get;}

            public MotorTestInfo(Direction direction)
            {
                SpeedDirection = direction;
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
