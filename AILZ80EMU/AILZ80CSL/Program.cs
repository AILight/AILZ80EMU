using AILZ80BRD;

namespace AILZ80CSL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var board = new BoardReference();
            board.PowerOn();

            while (true)
            {
                var state = board.CrystalOscillator4Mhz.ClockState;
                board.Drive();
                /*
                if (state != board.CrystalOscillator4Mhz.State)
                {
                    if (counter > 10000)
                    {
                        counter = 0;
                        Console.Write(board.CrystalOscillator4Mhz.State ? "-" : "_");
                    }
                    counter++;
                }
                */
            }

        }
    }
}
