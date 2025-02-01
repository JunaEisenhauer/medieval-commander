using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace StrategyGame.Model.Game
{
    public class Health
    {
        public float FortressHealing = 0.01f;

        public float TowerHealing = 0.001f;

        public Health()
        {
            MinBar = 0;
            MaxBar = 1;    
        }

        public float MinBar { get; set; }

        public float MaxBar { get; set; }
    }
}
