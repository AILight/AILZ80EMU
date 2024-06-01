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
        public Memory MemoryMain { get; set; }

        public BoardReference()
        {
            Z80 = new CPUZ80(MainBus);
            MemoryMain = new Memory(64 * 1024, MainBus);

            CrystalOscillator4Mhz.OnClockTick += (clockState) =>
            {
                Z80.ExecuteClock(clockState);
                MemoryMain.ExecuteClock(clockState);
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
            //MemoryMain.Drive();
        }


    }
}
