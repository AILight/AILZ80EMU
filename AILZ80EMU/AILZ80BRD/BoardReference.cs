using AILZ80CPU;
using AILZ80IOP;
using AILZ80LIB;

namespace AILZ80BRD
{
    public class BoardReference : Device
    {
        private Bus MainBus { get; set; } = new Bus();

        public CrystalOscillator CrystalOscillator4Mhz { get; set; } = new CrystalOscillator(3993600);
        public CPUZ80 Z80 { get; set; }


        public BoardReference()
        {
            Z80 = new CPUZ80(MainBus);
            CrystalOscillator4Mhz.OnClockTick += () =>
            {
                Z80.ExecuteClock();
            };
        }

        public override void PowerOn()
        {
            base.PowerOn();

            CrystalOscillator4Mhz.PowerOn();
            Z80.PowerOn();
        }


        public override void Drive()
        {
            CrystalOscillator4Mhz.Drive();
            //CPU.Drive();
        }


    }
}
