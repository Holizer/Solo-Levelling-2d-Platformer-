using System;
using System.Collections.Generic;
using System.Drawing;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    internal class Particle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float SpeedX { get; set; }
        public float SpeedY { get; set; }
        public float Friction { get; set; }
        public int Size { get; set; }
        public Color ParticleColor { get; set; }
        public Bitmap Texture { get; set; }
    }
    internal class ParticalMovement
    {
        public static List<Particle> particles = new List<Particle>();

        private static Random random = new Random();
        public static void ExplodeEnemy(Enemy enemy)
        {
            int numDeathParticles = 10;
            for (int i = 0; i < numDeathParticles; i++)
            {
                Particle deathParticle = new Particle();
                deathParticle.X = enemy.X + enemy.Width / 2;
                deathParticle.Y = enemy.Y + enemy.Height / 2;
                deathParticle.SpeedX = (float)(random.NextDouble() * 8 - 5);
                deathParticle.SpeedY = (float)(random.NextDouble() * 8 - 5);
                deathParticle.Friction = 0.02f;
                deathParticle.Size = (int)(clientWidth * 0.006);
                deathParticle.ParticleColor = Color.Red;
                particles.Add(deathParticle);
            }
        }
        public static void SpawnExperienceParticles(Enemy enemy)
        {
            int numExperienceParticles = 3;

            for (int i = 0; i < numExperienceParticles; i++)
            {
                Particle experienceParticle = new Particle();

                float randomXOffset = (float)(random.NextDouble() * 20 - 10);
                float randomYOffset = (float)(random.NextDouble() * 20 - 10);

                experienceParticle.X = enemy.X + (int)randomXOffset;
                experienceParticle.Y = enemy.Y + (int)randomYOffset;

                experienceParticle.SpeedX = 2.5f;
                experienceParticle.Size = (int)(clientWidth * 0.008);
                experienceParticle.Texture = ExpParticleTexture;
                particles.Add(experienceParticle);
            }
        }
        private static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }
        public static void MoveExperienceParticleTowardsPlayer(Particle particle)
        {
            float targetX = player.X < particle.X ? player.X - player.Width / 2 : player.X + player.Width / 2;
            float targetY = player.Y + player.Height / 2;

            float directionX = targetX - particle.X;
            float directionY = targetY - particle.Y;
            float distanceToPlayer = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            float lerpFactor = 0.05f;
            particle.X = (int)Lerp(particle.X, targetX, lerpFactor);
            particle.Y = (int)Lerp(particle.Y, targetY, lerpFactor);

            Rectangle particleRectangle = new Rectangle(particle.X, particle.Y, particle.Size, particle.Size);

            if (particleRectangle.IntersectsWith(player.GetRectangle(clientSize)))
            {
                particles.Remove(particle);
                player.GainExperience(20);
            }
        }
        public static void MoveDeathParticle(Particle particle)
        {
            float randomFactorX = (float)(random.NextDouble() * 0.5 - 0.1);
            float randomFactorY = (float)(random.NextDouble() * 1.0 - 0.15);

            particle.SpeedX += randomFactorX;
            particle.SpeedY += randomFactorY;

            particle.X += (int)particle.SpeedX;
            particle.Y += (int)particle.SpeedY;
        }
        public static void UpdateParticles()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                Particle particle = particles[i];

                if (particle.Texture == ExpParticleTexture)
                {
                    MoveExperienceParticleTowardsPlayer(particle);
                }
                else if (particle.ParticleColor == Color.Red)
                {
                    MoveDeathParticle(particle);
                }

                if (particle.X > cameraX + clientWidth || particle.X < 0 || particle.Y > clientHeight || particle.Y < 0)
                {
                    particles.RemoveAt(i);
                }
            }
        }
    }
}