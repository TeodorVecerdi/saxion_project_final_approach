using GXPEngine;

namespace physics_programming.final_assignment {
    public class Tutorial : Scene {
        private EasyDraw tutorialText;
        private TankAIBase enemy;
        
        public override void Initialise(MyGame myGame) {
            myGame.AddChild(myGame.Player = new Player(350, 800, 300));
            myGame.Enemies.Add(enemy = new TutorialEnemy(800,100));

            myGame.AddBorders();

            myGame.AddDestructibleBlock(new Vec2(0, 700+10), new Vec2(50, 700+10), 30);
            myGame.AddDestructibleBlock(new Vec2(50, 700+10), new Vec2(200, 600+10), 30);
            myGame.AddDestructibleBlock(new Vec2(185, 600+15), new Vec2(500, 600+15), 30);
            myGame.AddDestructibleBlock(new Vec2(475, 600+5), new Vec2(550, 700), 30);
            myGame.AddDestructibleBlock(new Vec2(545, 680), new Vec2(545, 740), 30);
            myGame.AddDestructibleLine(new Vec2(560, 740), new Vec2(561, 900));
            
            tutorialText = new EasyDraw(Globals.WIDTH, Globals.HEIGHT, false);
            tutorialText.Text("The tank rotates the barrel to face your mouse.\n" +
                              "You can fire by clicking the mouse.\n\n" +
                              "The environment is destructible. By shooting glass you break through it\n" +
                              "and by shooting walls you break them down\n" +
                              "into smaller chunks which you can destroy\n\n" +
                              "Break out of the house and kill the enemy to proceed to the next level", 50, 450);
            myGame.AddChild(tutorialText);
        }

        public override void Finalise(MyGame myGame) {
            tutorialText.Destroy();
        }

        public override Scene SwitchScene(MyGame myGame) {
            if (myGame.Player.Dead) return new Tutorial();
            if (enemy.Dead) return new Level1();
            return null;
        }
    }
}