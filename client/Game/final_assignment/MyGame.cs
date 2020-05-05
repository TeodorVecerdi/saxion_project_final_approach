using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GXPEngine;
using physics_programming.final_assignment.Components;

namespace physics_programming.final_assignment {
    public class MyGame : Game {
        public readonly List<Ball> Movers;
        public readonly List<Bullet> Bullets;
        public readonly List<DestructibleBlock> DestructibleBlocks;
        public readonly List<DestructibleChunk> DestructibleChunks;
        public readonly List<DoubleDestructibleLineSegment> DestructibleLines;
        public readonly List<LineSegment> Lines;
        public readonly List<TankAIBase> Enemies;
        public Player Player;
        public Scene currentScene = null;
        public Scene newScene = null;

        private bool paused;

        public MyGame() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;

            Movers = new List<Ball>();
            Lines = new List<LineSegment>();
            Bullets = new List<Bullet>();
            DestructibleLines = new List<DoubleDestructibleLineSegment>();
            DestructibleBlocks = new List<DestructibleBlock>();
            DestructibleChunks = new List<DestructibleChunk>();
            Enemies = new List<TankAIBase>();

            newScene = new Tutorial();
            Restart();
            PrintInfo();
        }

        public void AddLine(Vec2 start, Vec2 end, bool addReverseLine = false, bool addLineEndings = true, uint color = 0xffffffff) {
            var line = new LineSegment(start, end, color, 4);
            AddChild(line);
            Lines.Add(line);
            if (addReverseLine) {
                var reverseLine = new LineSegment(end, start, color, 4);
                AddChild(reverseLine);
                Lines.Add(reverseLine);
            }

            if (addLineEndings) {
                Movers.Add(new Ball(0, start, isKinematic: true));
                Movers.Add(new Ball(0, end, isKinematic: true));
            }
        }
        public void AddDestructibleLine(Vec2 start, Vec2 end, bool addLineEndings = true, uint color = 0xff00ff00) {
            var line = new DoubleDestructibleLineSegment(start, end, color, 2);
            AddChild(line);
            DestructibleLines.Add(line);

            if (addLineEndings) {
                Movers.Add(new Ball(0, start, isKinematic: true));
                Movers.Add(new Ball(0, end, isKinematic: true));
            }
        }

        public void AddDestructibleBlock(Vec2 start, Vec2 end, float blockWidth) {
            var block = new DestructibleBlock(blockWidth, start, end);
            DestructibleBlocks.Add(block);
            AddChild(block);
        }
        public void AddBullet(Bullet bullet) {
            Bullets.Add(bullet);
            AddChild(bullet);
        }
        public void AddBorders() {
            AddLine(new Vec2(0, Globals.HEIGHT), new Vec2(0, 0));
            AddLine(new Vec2(Globals.WIDTH, 0), new Vec2(Globals.WIDTH, Globals.HEIGHT));
            AddLine(new Vec2(0, 0), new Vec2(Globals.WIDTH, 0));
            AddLine(new Vec2(Globals.WIDTH, Globals.HEIGHT), new Vec2(0, Globals.HEIGHT));
        }

        private void PrintInfo() {
            Console.WriteLine("Hold SPACE to slow down the frame rate.");
            Console.WriteLine("Press P to toggle pause.");
            Console.WriteLine("Press R to reset scene");
        }

        private void HandleInput() {
            targetFps = Input.GetKey(Key.SPACE) ? 10 : 60;
            if (Input.GetKeyDown(Key.P))
                paused ^= true;
            if (Input.GetKeyDown(Key.R))
                Restart();
        }

        private void Restart() {
            Player?.Destroy();
            foreach (var enemy in Enemies)
                enemy.Destroy();
            Enemies.Clear();
            foreach (var line in Lines)
                line.Destroy();
            Lines.Clear();
            foreach (var mover in Movers)
                mover.Destroy();
            Movers.Clear();
            foreach (var bullet in Bullets)
                bullet.Destroy();
            Bullets.Clear();
            foreach (var destructibleLine in DestructibleLines)
                destructibleLine.Destroy();
            DestructibleLines.Clear();
            foreach (var destructibleBlock in DestructibleBlocks)
                destructibleBlock.Destroy();
            DestructibleBlocks.Clear();
            foreach (var destructibleChunk in DestructibleChunks)
                destructibleChunk.Destroy();
            DestructibleChunks.Clear();
            
            currentScene?.Finalise(this);
            currentScene = newScene;
            currentScene?.Initialise(this);
            // movers.Add(new Ball(30, new Vec2(200, 300), new Vec2(0, 0)));
            foreach (var b in Movers)
                AddChild(b);
            foreach (var enemy in Enemies)
                AddChild(enemy);
        }

        private void StepThroughMovers() {
            Movers.ForEach(mover => mover.Step());
            UpdateTanks();
            UpdateBullets();
            UpdateDestructibleEnvironment();
            var shouldSwitch = currentScene.SwitchScene(this);
            if (shouldSwitch != null) {
                newScene = shouldSwitch;
                Restart();
            }
        }

        private void UpdateTanks() {
            Enemies.Where(enemy => enemy.Dead).ToList().ForEach(enemy => {
                enemy.Destroy();
                Enemies.Remove(enemy);
            });
            if (Player.Dead) {
                Player.Destroy();
                Player = null;
            }
        }

        private void UpdateBullets() {
            Bullets.ForEach(bullet => bullet.Step());
            Bullets.Where(b => b.Dead).ToList().ForEach(bullet => {
                bullet.Destroy();
                Bullets.Remove(bullet);
            });
        }

        private void UpdateDestructibleEnvironment() {
            var minSizeSqr = Globals.World.DestructibleLineMinLength * 1.5f;
            minSizeSqr *= minSizeSqr;

            //// LINES
            DestructibleLines.Where(l => l.ShouldRemove || (l.SideA.End - l.SideA.Start).sqrMagnitude <= minSizeSqr)
                .ToList().ForEach(l => {
                    l.Destroy();
                    DestructibleLines.Remove(l);
                });

            //// CHUNKS
            DestructibleChunks.Where(chunk => chunk.ShouldRemove)
                .ToList().ForEach(chunk => {
                    chunk.Destroy();
                    DestructibleChunks.Remove(chunk);
                });

            //// BLOCKS
            DestructibleBlocks.Where(block => block.ShouldRemove || (block.Length1.End - block.Length1.Start).sqrMagnitude <= minSizeSqr || (block.Length2.End - block.Length2.Start).sqrMagnitude <= minSizeSqr)
                .ToList().ForEach(block => {
                    block.Destroy();
                    DestructibleBlocks.Remove(block);
                });
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Update() {
            HandleInput();
            if (!paused)
                StepThroughMovers();
        }

        private static void MainFA() {
            new MyGame().Start();
        }
    }
}