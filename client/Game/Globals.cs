namespace physics_programming {
    public static class Globals {
        public const float ASPECT_RATIO = 1.7777777f;
        public const bool USE_ASPECT_RATIO = false;
        public const int WIDTH = 1200;
        private const int H_MAIN = 900;
        private const int H_ASPECT = (int) (WIDTH / ASPECT_RATIO);
        public const bool FULLSCREEN = false;
        public const bool VSYNC = false;
        public const bool PIXEL_ART = false;
        public const string WINDOW_TITLE = "Physics Programming";

        public const float TILE_SIZE = 92f;
        public static float[] QUAD_VERTS = {0, 0, TILE_SIZE, 0, TILE_SIZE, TILE_SIZE, 0, TILE_SIZE};
        public static float[] QUAD_UV = {0, 0, 1, 0, 1, 1, 0, 1};
        public static int HEIGHT => USE_ASPECT_RATIO ? H_ASPECT : H_MAIN;

        public static class World {
            public const float BulletDLSDamage = 10;
            public const float DestructibleLineMinLength = 10;
            public const int BlockDestructionPoints = 6;
        }
    }
}