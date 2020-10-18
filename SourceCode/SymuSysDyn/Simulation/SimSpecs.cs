#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using Symu.Common.Classes;
using static Symu.Common.Constants;

#endregion


namespace Symu.SysDyn.Simulation
{
    /// <summary>
    ///     SimSpecs is the structure to define and store information about the simulation
    ///     It is based on sim_specs element of the xmile schema (schema.xsd)
    /// </summary>
    public class SimSpecs
    {
        public SimSpecs()
        {
        }

        public SimSpecs(ushort start, ushort stop)
        {
            Stop = stop;
            Start = start;
        }

        public SimSpecs(ushort start, ushort stop, float deltaTime) : this(start, stop)
        {
            DeltaTime = deltaTime;
        }

        public SimSpecs(string start, string stop, string deltaTime)
        {
            Start = ushort.Parse(start ?? "0", CultureInfo.InvariantCulture);
            Stop = ushort.Parse(stop ?? "0", CultureInfo.InvariantCulture);
            DeltaTime = float.Parse(deltaTime ?? "1", CultureInfo.InvariantCulture);
        }

        public SimSpecs(string start, string stop, string deltaTime, string pause, string timeUnits) : this(start, stop,
            deltaTime)
        {
            if (!string.IsNullOrEmpty(pause))
            {
                PauseInterval = ushort.Parse(pause, CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(timeUnits))
            {
                TimeUnits = TimeStepTypeService.GetValue(timeUnits);
            }
        }

        public bool OnPause { get; private set; }
        public ushort Step { get; set; }
        public ushort Time { get; set; }

        /// <summary>
        ///     Pause, default false, PauseInterval can be ignored
        /// </summary>
        public bool Pause { get; set; }

        public void Clear()
        {
            Step = 0;
        }

        public bool Run()
        {
            TimeManagement();
            // with pause
            if (Pause && Time > 0)
            {
                if (OnPause)
                {
                    Step++;
                    OnPause = false;
                }

                //TODO use Symu.Common.Constants
                bool run;
                if (DeltaTime < 1)
                {
                    run = Time % PauseInterval != 0 || Math.Abs(Step * DeltaTime % 1) > 0.0001;
                }
                else
                {
                    run = Step % PauseInterval != 0;
                }

                if (run)
                {
                    Step++;
                }
                else
                {
                    OnPause = true;
                }

                return run;
            }

            // without pause
            if (Time >= Stop)
            {
                return false;
            }

            Step++;
            return true;
        }

        public void TimeManagement()
        {
            Time = (ushort) Math.Floor(Step * DeltaTime);
            if (!OnPause && Step * DeltaTime % 1 < Tolerance)
            {
                OnTimer?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler OnTimer;

        #region XML attributes

        public ushort Stop { get; set; }
        public ushort Start { get; set; }

        private float _deltaTime = 1;

        /// <summary>
        ///     Step size
        /// </summary>
        public float DeltaTime
        {
            get => _deltaTime;
            set
            {
                if (value > 1 || value <= 0)
                {
                    throw new ArgumentOutOfRangeException("DeltaTime should be <= 1 and >= 0");
                }

                _deltaTime = value;
            }
        }

        public TimeStepType TimeUnits { get; set; }
        private ushort _pauseInterval;

        /// <summary>
        ///     Pause interval if Pause is true
        /// </summary>
        public ushort PauseInterval
        {
            get => _pauseInterval;
            set
            {
                Pause = true;
                _pauseInterval = value;
            }
        }

        #endregion
    }
}