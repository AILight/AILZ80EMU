using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80IOP
{
    public class CrystalOscillator : Device
    {
        public event Action<bool>? OnClockTick; // クロック信号を通知するイベント
        public bool ClockState { get; private set; }

        private int Hertz { get; set; }

        private Stopwatch Stopwatch = new Stopwatch();
        private long IntervalTicks;
        private long Interval4Ticks;
        private long NextIntervalTicks;
        private long NextInterval4Ticks;
        private int IntervalCounter = 0;
        private long ElapsedTicks;
        private long Counter = 0;

        public CrystalOscillator(int hertz)
        {
            this.Hertz = hertz;
            this.IntervalTicks = (Stopwatch.Frequency) / this.Hertz;
            this.Interval4Ticks = (Stopwatch.Frequency * 4) / this.Hertz;
        }

        public override void PowerOn()
        {
            base.PowerOn();

            // 初回起動処理
            this.ClockState = false;
            this.Stopwatch.Start();
            this.ElapsedTicks = this.Stopwatch.ElapsedTicks;
            this.NextIntervalTicks = this.ElapsedTicks + this.IntervalTicks;
            this.NextInterval4Ticks = this.ElapsedTicks + this.Interval4Ticks;
            this.IntervalCounter = 0;
        }

        public override void PowerOff()
        {
            base.PowerOff();

            this.Stopwatch.Stop();
        }

        public override void Drive()
        {
            if (!this.IsPowerOn)
            {
                return;
            }
            this.ElapsedTicks = this.Stopwatch.ElapsedTicks;

            if (this.ElapsedTicks > this.NextInterval4Ticks)
            {
                for (var index = this.IntervalCounter; index < 4; index++)
                {
                    OnClockTick_Invoce();
                }
                this.NextIntervalTicks = this.ElapsedTicks + this.IntervalTicks;
                this.NextInterval4Ticks = this.ElapsedTicks + this.Interval4Ticks;
                this.IntervalCounter = 0;
                this.Counter = 0;

            }
            else if (this.ElapsedTicks > this.NextIntervalTicks && this.IntervalCounter < 3)
            {
                this.IntervalCounter++;
                OnClockTick_Invoce();
                this.NextIntervalTicks = this.ElapsedTicks + this.IntervalTicks;
            }
            else
            {
                Counter++;
            }
        }

        private void OnClockTick_Invoce()
        {
            this.ClockState = !this.ClockState;
            this.OnClockTick?.Invoke(this.ClockState);
        }
    }
}
