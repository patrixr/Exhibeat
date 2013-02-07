using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exhibeat.Settings
{
    public class ExhibeatSettings
    {
        public static bool ResolutionIndependent = true;
        public static bool Fullscreen = false;
        public static int WindowHeight = 720;
        public static int WindowWidth = 1280;

        public static Vector2[] TilePositions = {
            new Vector2(0.5f, 0.23f),
            new Vector2(1.5f, 0.23f),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(2, 1),
            new Vector2(0.5f, 1.77f),
            new Vector2(1.5f, 1.77f)
        };

        public static bool TileGrowth = true;
        public static float TileGrowthStartScale = 0;

#if ANIMATED_TILE
        public static String GrowthAnimationName = "tile_animation_test";
#endif
    }
}