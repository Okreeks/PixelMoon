using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace PixelMoon
{
    public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;

        private Color[] colors = new Color[6];
        Random r = new Random();

        public Int32 throttle = 0;

        public ParticleEngine(Game1 game)
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(ContentLoader.Textures[ContentLoader.TextureNames.diamondParticle]);

            colors[0] = new Color(0, 0, 0);
            colors[1] = new Color(255, 62, 34);
            colors[2] = new Color(192, 192, 192);
            colors[3] = new Color(255, 128, 0);
            colors[4] = new Color(255, 128, 64);
            colors[5] = new Color(128, 128, 128);

            EmitterLocation = new Vector2(400, 240);
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1),
                    1f * (float)(random.NextDouble() + 3)); // The direction of downward travel.
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = colors[r.Next(0, 6)];
            float size = (float)random.NextDouble();
            int ttl =  (throttle /2) +random.Next(20);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        public void Update(bool addParticle)
        {
            int total = 4;

            if (addParticle)
            {
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }

            //spriteBatch.End();
        }
    }
}
