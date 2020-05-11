namespace game {
    public static class Globals {
        public const bool FULLSCREEN = false;
        public const bool PIXEL_ART = false;
        public const bool USE_ASPECT_RATIO = false;
        public const bool VSYNC = true;
        
        public const int WIDTH = 1920;
        private const int H_MAIN = 1000;
        private const int H_ASPECT = (int) (WIDTH / ASPECT_RATIO);
        public static int HEIGHT => USE_ASPECT_RATIO ? H_ASPECT : H_MAIN;
        
        public const float ASPECT_RATIO = 1.7777777f;
        public const float TILE_SIZE = 92f;
        public const string WINDOW_TITLE = "Project Final Approach";
        
        public static float[] QUAD_VERTS = {0, 0, TILE_SIZE, 0, TILE_SIZE, TILE_SIZE, 0, TILE_SIZE};
        public static float[] QUAD_UV = {0, 0, 1, 0, 1, 1, 0, 1};

    }
}