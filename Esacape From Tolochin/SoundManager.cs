using System;
using System.IO;
using NAudio.Wave;

namespace SoloLeveling
{
    public static class SoundManager
    {
        private static readonly string ResourcesPath = MainForm.resourcesPath;

        private static readonly Lazy<WaveOutEvent> lazyWaveOutEvent = new Lazy<WaveOutEvent>(() => new WaveOutEvent());
        private static WaveOutEvent WaveOutEvent => lazyWaveOutEvent.Value;

        private static float volume = 1.0f;
        private static AudioFileReader LoadAudioFile(string fileName)
        {
            return new AudioFileReader(Path.Combine(ResourcesPath, fileName));
        }

        private static void InitAndPlay(AudioFileReader reader)
        {
            reader.Volume = volume;
            WaveOutEvent.Init(reader);
            WaveOutEvent.Play();
        }

        public static void SetVolume(float newVolume)
        {
            volume = newVolume < 0.0f ? 0.0f : (newVolume > 1.0f ? 1.0f : newVolume);
        }

        public static void PlaySound(string fileName, float customVolume = 1.0f)
        {
            var reader = LoadAudioFile(fileName);
            InitAndPlay(reader);
        }

        public static void PlayMainMenuMusic() => PlaySound("Sounds\\MainMenuSound2.wav");
        public static void PlayEndingMusic() => PlaySound("Sounds\\Hymn.wav");
        public static void PlayChargedAttackSound() => PlaySound("Sounds\\FateAttack.wav");
        public static void PlayHitSound() => PlaySound("Sounds\\HitDetected.wav");
        public static void PlayEatingSound() => PlaySound("Sounds\\Eating.wav");
        public static void PlayPlayerDeathSound() => PlaySound("Sounds\\dark-souls-you-died-sound.wav");
        public static void PlayLevelUpSound() => PlaySound("Sounds\\LevelUp.wav");
        public static void PlayClickSound() => PlaySound("Sounds\\Minecraft Menu Button Sound.wav");
        public static void PlayDefaultAttackSound() => PlaySound("Sounds\\SwordAttack1.mp3");
    }
}