namespace physics_programming.final_assignment {
    public abstract class Scene {
        public abstract void Initialise(MyGame myGame);
        public abstract void Finalise(MyGame myGame);
        public abstract Scene SwitchScene(MyGame myGame);
    }
}