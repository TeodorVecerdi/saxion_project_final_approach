using System.Drawing;
using GXPEngine;

namespace physics_programming.final_assignment {
    public class AttackIndicator : EasyDraw {
        private readonly Color indicatorColor;
        private readonly float initialArcAngle;
        private readonly float initialTimeLeft;
        private readonly float radius;

        private float arcAngle;
        private float startAngle;
        private float timeLeft;

        public AttackIndicator(float initialTimeLeft, Color? color = null, float initialArcAngle = 90f, float radius = 2f) : base((int) (2 * radius), (int) (2 * radius)) {
            this.initialTimeLeft = initialTimeLeft;
            indicatorColor = color ?? Color.White;
            this.initialArcAngle = initialArcAngle;
            this.radius = radius;
            timeLeft = initialTimeLeft;
            Draw();
        }

        public void Draw() {
            Clear(Color.Transparent);
            Fill(indicatorColor, indicatorColor.A);
            NoStroke();
            Arc(radius, radius, 2 * radius, 2 * radius, startAngle, arcAngle);
        }

        private void RecalculateAngles() {
            arcAngle = timeLeft / initialTimeLeft * initialArcAngle;
            startAngle = -arcAngle / 2f;

            // Debug.LogWarning($"arcAngle: {arcAngle} - startAngle {startAngle}");
        }

        public void UpdateIndicator(float timeLeft) {
            this.timeLeft = timeLeft;
            RecalculateAngles();
            Draw();
        }
    }
}