using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLR_Simulator
{
    public class SimulationResult
    {
        public DateTime Timestamp { get; set; }
        public int Compressions { get; set; }
        public int Ventilations { get; set; }
    }
}
