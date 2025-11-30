using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.Things;
using System.Collections.Generic;

namespace ProjectZ.InGame.Interface
{
    public static class PageTooltip
    {
        // Sprite font texture.
        public static SpriteFont Font = Resources.GameFont;

        // Text padding.
        static float paddingX = 10f * Game1.UiScale;
        static float paddingY = 10f * Game1.UiScale;

        // Textbox scale: relative to menu size.
        static float widthScale  = 0.85f;
        static float heightScale = 0.40f;

        // Textbox background.
        static Color backgroundColor = Color.Black;
        static float backgroundAlpha = 0.95f;

        // Textbox border.
        static Color borderColor = Color.White;
        static float borderAlpha = 0.95f;
        static int borderThickness = 2 * Game1.UiScale;

        public static void Draw(SpriteBatch spriteBatch, string text)
        {
            // Menu size reference.
            float menuWidth = (Values.MinWidth - 32) * Game1.UiScale;
            float menuHeight = (Values.MinHeight - 32) * Game1.UiScale;

            // Menu top-left position.
            float menuX = (Game1.WindowWidth - menuWidth) / 2f;
            float menuY = (Game1.WindowHeight - menuHeight) / 2f;

            // Tooltip size relative to the menu.
            float boxWidth = menuWidth * widthScale;
            float boxHeight = menuHeight * heightScale;

            // Tooltip centered within the menu.
            float boxX = menuX + (menuWidth - boxWidth) / 2f;
            float boxY = menuY + (menuHeight - boxHeight) / 2f;

            // Rectangle for black textbox.
            var boxRect = new Rectangle((int)boxX, (int)boxY, (int)boxWidth, (int)boxHeight);

            // Draw the border around the textbox.
            spriteBatch.Draw(Resources.SprWhite, new Rectangle(boxRect.X - borderThickness, boxRect.Y - borderThickness, boxRect.Width + borderThickness * 2, borderThickness), borderColor * borderAlpha);
            spriteBatch.Draw(Resources.SprWhite, new Rectangle(boxRect.X - borderThickness, boxRect.Bottom, boxRect.Width + borderThickness * 2, borderThickness), borderColor * borderAlpha);
            spriteBatch.Draw(Resources.SprWhite, new Rectangle(boxRect.X - borderThickness, boxRect.Y, borderThickness, boxRect.Height), borderColor * borderAlpha);
            spriteBatch.Draw(Resources.SprWhite, new Rectangle(boxRect.Right, boxRect.Y, borderThickness, boxRect.Height), borderColor * borderAlpha);

            // Draw the black background.
            spriteBatch.Draw(Resources.SprWhite, boxRect, backgroundColor * backgroundAlpha);

            // Word-wrap text and apply padding.
            var wrappedLines = WrapText(Font, text, boxWidth - paddingX * 2);

            // Compute line height.
            float lineHeight = Font.LineSpacing * Game1.UiScale;

            // Compute total text block height for vertical centering.
            float textBlockHeight = wrappedLines.Count * lineHeight;

            // Starting Y position vertically centered with top/bottom padding/
            float startY = boxRect.Y + paddingY + (boxRect.Height - paddingY * 2 - textBlockHeight) / 2f;

            foreach (var line in wrappedLines)
            {
                var lineSize = Font.MeasureString(line) * Game1.UiScale;
                float lineX = boxRect.X + paddingX + (boxRect.Width - paddingX * 2 - lineSize.X) / 2f; // center horizontally with padding
                spriteBatch.DrawString(Font, line, new Vector2(lineX, startY), Color.White, 0f, Vector2.Zero, Game1.UiScale, SpriteEffects.None, 0f);
                startY += lineHeight;
            }
        }

        private static List<string> WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            // Split text into words, store into a temporary line, and test
            // if it fits in textbox. Compiled results are stored in a list.
            var words = text.Split(' ');
            var lines = new List<string>();
            string currentLine = "";

            // Loop through each word and decide if it fits in the current line.
            foreach (var word in words)
            {
                // Test what the line would look like if added and measure it.
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                float lineWidth = font.MeasureString(testLine).X * Game1.UiScale;

                // If the line with the added word is too wide for the textbox
                // add the line to the list and start a new line.
                if (lineWidth > maxLineWidth)
                {

                    if (!string.IsNullOrEmpty(currentLine))
                        lines.Add(currentLine);
                    currentLine = word;
                }
                // If the word fits add it to the current line.
                else
                    currentLine = testLine;
            }
            // Add remaining text to the final line.
            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine);

            return lines;
        }

        public static void UpdateFont()
        {
            // Reload the font if it's been changed.
            Font = Resources.GameFont;
        }
    }
}
