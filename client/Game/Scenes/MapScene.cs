using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;
using Image = game.ui.Image;

namespace game {
    public class MapScene : Scene {

        private Image coffeeTooltip;
        private Image felliniTooltip;
        private Image rocksTooltip;
        
        public MapScene() {
            SceneName = "Map";
        }
        
        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new game.Sprite("data/sprites/map/map.jpg")));
            Root.AddChild(new Button(887, 0, 200, 320, "Fellini", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.JoinLocation("Fellini");
            }, () => {
                felliniTooltip.x = 612f;
            }, () => {
                felliniTooltip.x = -100000f;
            }));
            Root.AddChild(new Button(1075, 572, 200, 300, "Rock's", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.JoinLocation("Rock's");
            }, () => {
                rocksTooltip.x = 800f;
            }, () => {
                rocksTooltip.x = -100000f;
            }));
            Root.AddChild(new Button(423, 88, 200, 320, "Coffee Fellows", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.JoinLocation("Coffee Fellows");
            }, () => {
                coffeeTooltip.x = 145f;
            }, () => {
                coffeeTooltip.x = -100000f;
            }));
            Root.AddChild(coffeeTooltip = new Image(145, 445, 750, 150, new game.Sprite("data/sprites/map/coffee_fellows_slogan.jpg")) {x = -100000f});
            Root.AddChild(felliniTooltip = new Image(612, 375, 750, 150, new game.Sprite("data/sprites/map/fellini_slogan.jpg")) {x = -100000f});
            Root.AddChild(rocksTooltip = new Image(800, 400, 750, 150, new game.Sprite("data/sprites/map/rocks_slogan.jpg")) {x = -100000f});
            IsLoaded = true;
        }
    }
}