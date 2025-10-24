using Microsoft.Xna.Framework;

namespace ProjectZ.InGame.Overlay.Sequences
{
    class PhotoSequence : GameSequence
    {
        float _counter;

        public PhotoSequence()
        {
            _sequenceWidth = 160;
            _sequenceHeight = 144;
        }

        public override void OnStart()
        {
            Sprites.Clear();
            SpriteDict.Clear();

            var photo = Game1.GameManager.SaveManager.GetString("photoSequencePhoto");

            // background
            if (!string.IsNullOrEmpty(photo))
                Sprites.Add(new SeqSprite(photo, new Vector2(0, 0), 0));

            base.OnStart();
        }

        public override void Update()
        {
            base.Update();

            _counter += Game1.DeltaTime;
            if (_counter > 2500)
                Game1.GameManager.InGameOverlay.CloseOverlay();
        }
    }
}
