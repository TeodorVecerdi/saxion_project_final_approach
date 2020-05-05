using GXPEngine;

namespace physics_programming.final_assignment {
    public class Level2 : Scene {
        private EasyDraw text;
        public override void Initialise(MyGame myGame) {
            myGame.AddChild(myGame.Player = new Player(75, 100, 300));
            myGame.Player.Tank.rotation += 90;
            myGame.Enemies.Add(new SmartEnemy(1000,100, timeToShoot: 4f));
            myGame.Enemies.Add(new SmartEnemy(1000,700, timeToShoot: 3f));

            myGame.AddBorders();

            myGame.AddDestructibleBlock(new Vec2(150, 0), new Vec2(150, 400), 30);
            myGame.AddDestructibleBlock(new Vec2(180, 0), new Vec2(180, 400), 30);
            myGame.AddDestructibleBlock(new Vec2(900, -50), new Vec2(850, 60), 30);
            myGame.AddDestructibleBlock(new Vec2(850, 60), new Vec2(850, 90), 30);
            myGame.AddDestructibleBlock(new Vec2(850, 110), new Vec2(850, 190), 30);
            myGame.AddDestructibleLine(new Vec2(865, 190), new Vec2(1200, 300));
            text = new EasyDraw(Globals.WIDTH, Globals.HEIGHT, false);
            text.Text("I was too lazy to create another level so I just replaced\nthe 'dumb' tanks with tanks that have the\nsmart aiming where they lead their shots.", 100, 500);
            myGame.AddChild(text);
        }

        public override void Finalise(MyGame myGame) {
            text.Destroy();
        }    

        public override Scene SwitchScene(MyGame myGame) {
            if (myGame.Player.Dead) return new Level2();
            if (myGame.Enemies.TrueForAll(enemy => enemy.Dead)) return new Tutorial();
            return null;
        }
    }
}