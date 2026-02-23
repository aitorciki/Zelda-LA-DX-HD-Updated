using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.Things;

namespace ProjectZ.Base.UI
{
    public class UiRectangle : UiElement
    {
        public Color BlurColor;
        public float Radius = 0;

        public UiRectangle(Rectangle rectangle, string elementId, string screen, Color color, Color blurColor, UiFunction update)
            : base(elementId, screen)
        {
            Rectangle = rectangle;
            BackgroundColor = color;
            BlurColor = blurColor;
            UpdateFunction = update;
        }

        public override void DrawBlur(SpriteBatch spriteBatch)
        {
            SetParameter("scale", Game1.UiScale);
            SetParameter("blurColor", BlurColor.ToVector4());
            SetParameter("radius", Radius);
            SetParameter("width", Rectangle.Width / Game1.UiScale);
            SetParameter("height", Rectangle.Height / Game1.UiScale);
            SetParameter("screenWidth", (float)Game1.WindowWidth);
            SetParameter("screenHeight", (float)Game1.WindowHeight);

            // draw the blur texture
            spriteBatch.Draw(Resources.SprWhite, Rectangle, BackgroundColor);
        }

        private void SetParameter(string name, float value)
        {
            if (Resources.RoundedCornerBlurEffect.Parameters[name] != null)
                Resources.RoundedCornerBlurEffect.Parameters[name].SetValue(value);
        }

        private void SetParameter(string name, Vector4 value)
        {
            if (Resources.RoundedCornerBlurEffect.Parameters[name] != null)
                Resources.RoundedCornerBlurEffect.Parameters[name].SetValue(value);
        }

        private void SetParameter(string name, int value)
        {
            if (Resources.RoundedCornerBlurEffect.Parameters[name] != null)
                Resources.RoundedCornerBlurEffect.Parameters[name].SetValue(value);
        }
    }
}