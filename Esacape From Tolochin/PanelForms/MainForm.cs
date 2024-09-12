using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Text;
using Button = System.Windows.Forms.Button;
using static SoloLeveling.AnimationManagaer;
using static SoloLeveling.ParticalMovement;

namespace SoloLeveling
{
    public partial class MainForm : Form
    {

        public static readonly string resourcesPath = Path.Combine(Application.StartupPath, @"..\..\Resources");
        public static Size clientSize;
        public static FormWindowState windowState;

        public static int overlayAlpha = 0;
        public static int maxOverlayAlpha = 180;
        public static int overlayDuration = 500;

        public static Timer enemyAnimationTimer;
        public static Timer playerAnimationTimer;

        private Timer gameTimer;
        private Timer loadingTimer;
        private Timer deathOverlayTimer;

        public static bool gamePaused = false;
        public static bool gameStarted = false;
        public static bool isGameOver = false;

        private Bitmap backgroundBuffer;

        public static PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        public MainForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.KeyPreview = true;

            gameTimer = new Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += UpdateGame;
            gameTimer.Start();
            SetupGameField();

            DrawAnimation.SetupInGameAnimation();

            healthBar = new HealthBar(player.MaxHealth, 200, 20, Brushes.Green, Pens.Black);

            playerAnimationTimer = new Timer();
            playerAnimationTimer.Interval = 16;
            playerAnimationTimer.Tick += UpdatePlayerAnimation;
            playerAnimationTimer.Start();

            enemyAnimationTimer = new Timer();
            enemyAnimationTimer.Tick += (sender, e) => UpdateEnemyAnimation(sender, e, CreateGraphics());
            enemyAnimationTimer.Interval = 16;
            enemyAnimationTimer.Start();

            this.KeyDown += MainForm_KeyDown;
            this.MouseDown += MainForm_MouseDown;
        }

        public static Loading Loading = new Loading();
        public Panel loadingPanel = Loading.GetPanel();

        public static PauseMenu PauseMenu = new PauseMenu();
        public Panel PauseMenuPanel = PauseMenu.GetPanel();
     
        // Кнопка "START"
        private void StartGameBTN_Click(object sender, EventArgs e)
        {
            loadingTimer = new Timer();
            loadingTimer.Interval = 10;
            loadingTimer.Tick += LoadingTimer_Tick;

            this.Controls.Clear();
            this.Controls.Add(loadingPanel);
            //InMenuMenuSound.Stop();
            loadingTimer.Start();
        }

        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            loadingTimer.Stop();
            loadingTimer.Dispose();

            loadingPanel.Dispose();
            Loading.Dispose();

            this.Focus();
            this.BringToFront();
            this.Select();
            gameStarted = true;
        }

        // Загрука формы
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Громкость игры
            SoundManager.SetVolume(.3f);

            this.DoubleBuffered = true;
            // Переключаем фокус на форму и активируем регистрирацию нажатия клавмиш
            this.Focus();
            this.BringToFront();
            //InMenuMenuSound.Play(); 

            // Получение размера окна для остальных модулей 
            clientSize = this.ClientSize;
     
            // Загрузка шрифта
            LoadCustomFont();

            // Применяем кастомизацию стиля для кнопок
            CastomizeButton(StartGameBTN);
            CastomizeButton(SettingsBTN);
            CastomizeButton(AboutGameBTN);
            CastomizeButton(LeaveGameBTN);
        }
        private static void CastomizeButton(Button button, int FontSize = 18)
        {
            ApplyCustomFont(button, "Planes_ValMore", FontSize);
            
            // Устанавливаем прозрачный цвет фона при наведении
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            
            // Устанавливаем прозрачный цвет фона при нажатии
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }
        public static void LoadCustomFont()
        {
            privateFontCollection.AddFontFile(Path.Combine(resourcesPath, "Font\\8_bit_Limit.ttf"));
            privateFontCollection.AddFontFile(Path.Combine(resourcesPath, "Font\\Planes_ValMore.ttf"));
        }
        public static void ApplyCustomFont(Control control, string fontFileName, float fontSize)
        {
            if (privateFontCollection.Families.Any(f => f.Name == fontFileName))
            {
                Font customFont = new Font(privateFontCollection.Families.First(f => f.Name == fontFileName), fontSize);
                control.Font = customFont;
            }
        }
        
        private void UpdatePlayerAnimation(object sender, EventArgs e)
        {
            /*
            if (PauseMenu.Visible)
            {
                return;
            }
            else
            {
            */
                Animation currentAnimation = player.CurrentAnimation;

                if (currentAnimation.Frames != null && currentAnimation.Frames.Count > 0)
                {
                    int framesCount = currentAnimation.Frames.Count;
                    player.currentSpriteIndex = (player.currentSpriteIndex + 1) % framesCount;
                    if (player.currentSpriteIndex == 0)
                    {
                        playerAnimationTimer.Interval = currentAnimation.Interval;
                    }
                }
                Invalidate();
            //}
        }
        private void UpdateEnemyAnimation(object sender, EventArgs e, Graphics g)
        {
            /*
            if (!PauseMenu.Visible)
            {
            */
                foreach (var enemy in enemies)
                {
                    Animation currentAnimation = enemy.CurrentAnimation;
                    if (currentAnimation.Frames != null && currentAnimation.Frames.Count > 0)
                    {
                        int framesCount = currentAnimation.Frames.Count;
                        enemy.currentSpriteIndex = (enemy.currentSpriteIndex + 1) % framesCount;

                        if (enemy.currentSpriteIndex == 0)
                        {
                            enemyAnimationTimer.Interval = currentAnimation.Interval;
                        }
                    }
                    Invalidate();
                }
            //}
        }
       
        public static bool WillCollideWithObstacles(int x, int y, int width, int height)
        {
            if (x < 0 || x + width > LevelLength)
            {
                return true;
            }

            // Пример проверки столкновения с платформами
            foreach (var platformRect in platforms)
            {
                var rect = platformRect.ToRectangle(clientSize);
                if (CheckCollision(x, y, width, height, rect.X, rect.Y, rect.Width, rect.Height))
                {
                    return true;
                }
            }

            // Пример проверки столкновения с землей
            foreach (var groundRect in ground)
            {
                var rect = groundRect.ToRectangle(clientSize);
                if (CheckCollision(x, y, width, height, rect.X, rect.Y, rect.Width, rect.Height))
                {
                    return true;
                }
            }

            return false;
        }
        public class DynamicRectangle
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public Bitmap Texture { get; set; }
            public DynamicRectangle(float x, float y, float width, float height, Bitmap texture)
            {
                X = x;
                Y = y; // Проверяем, находится ли объект ниже земли
                Width = width;
                Height = height;
                Texture = texture;
            }
            public RectangleF ToRectangle(Size clientSize)
            {
                return new RectangleF(X, Y, Width, Height);
            }
        }

        public static int LevelLength;
        public static Player player;
        public static Sword sword;
        public static float Gravity;
        public static int cameraX = 0;
        internal static HealthBar healthBar;

        public static List<Enemy> enemies;
        List<Pickup> pickups;
        public static List<DynamicRectangle> ground;
        public static List<DynamicRectangle> platforms;
        List<FallingPickup> TolochinApple;
        public class TraderZone
        {
            public RectangleF Bounds { get; private set; }

            public TraderZone(float x, float y, float width, float height)
            {
                Bounds = new RectangleF(x, y, width, height);
            }
        }

        public TraderZone traderZone;

        Bitmap sacredApple = new Bitmap(Path.Combine(resourcesPath, "blessing_apple.png"));
        Bitmap pickupTexture = new Bitmap(Path.Combine(resourcesPath, "golden-apple-minecraft.gif"));
        Bitmap playerTexture = new Bitmap(Path.Combine(resourcesPath, "Idle1.png"));
        Bitmap boxTexture = new Bitmap(Path.Combine(resourcesPath, "box.png"));
        private void SetupGameField()
        {        
            LevelLength = 3000;
            Gravity = 3;
            backgroundBuffer = new Bitmap(LevelLength, 720);
            traderZone = new TraderZone((int)(LevelLength * 0.8), 200, 450, 400);
            ground = new List<DynamicRectangle>
            {
                new DynamicRectangle(0, this.ClientSize.Height - 80, LevelLength / 2, 80, GroundTexture),
                new DynamicRectangle(LevelLength/2, this.ClientSize.Height - 80, LevelLength / 2, 80, GroundTexture)
            };
            platforms = new List<DynamicRectangle>
            {
                new DynamicRectangle(1300, 533, 60, 60, boxTexture),
                new DynamicRectangle(1360, 533, 60, 60, boxTexture),
                new DynamicRectangle(1420, 533, 60, 60, boxTexture),
                new DynamicRectangle(1480, 533, 60, 60, boxTexture),
                new DynamicRectangle(1800, 533, 60, 60, boxTexture)
            };
            enemies = new List<Enemy>
            {
                new Enemy(500, 400, 50, 50, 50, 5, 25f),
                new Enemy(700, 400, 50, 50, 50, 5, 25f),
                new Enemy(600, 400, 50, 50, 50, 5, 25f),
            };
            pickups = new List<Pickup>
            {
                new Pickup(1400, 480, 50, 50, pickupTexture),
            };
            TolochinApple = new List<FallingPickup> {};
            player = new Player(
                        100, 310,
                        90, 110,  
                        8f, 100,
                        23f, playerTexture);
            sword = new Sword(player.X, player.Y, 150, 100, 25);
        }
        public static bool CheckCollision(int x1, int y1, int w1, int h1, float x2, float y2, float w2, float h2)
        {
            return x1 < x2 + w2
                && x1 + w1 > x2
                && y1 < y2 + h2
                && y1 + h1 > y2;
        }
       
        public static bool CheckCollisionWithPlatforms(int proposedX, int proposedY, Enemy enemy)
        {
            foreach (var platformRect in platforms)
            {
                var rect = platformRect.ToRectangle(clientSize);
                if (CheckCollision(proposedX, proposedY, enemy.Width, enemy.Height, rect.X, rect.Y, rect.Width, rect.Height))
                {
                    return true;
                }
            }
            return false;
        }   
        public static float FindAdaptiveFontSize(Graphics g, string text, float maxWidth, float maxHeight, Size clientSize, Font prototypeFont)
        {
            float fontSize = prototypeFont.Size;

            SizeF stringSize = g.MeasureString(text, prototypeFont);

            float widthScale = maxWidth / stringSize.Width;
            float heightScale = maxHeight / stringSize.Height;

            double scale = Math.Min(widthScale, heightScale) / 1.3;

            fontSize *= (float)scale;

            return fontSize;
        }

        Bitmap GroundTexture = new Bitmap(Path.Combine(resourcesPath, "ground2.png"));
        Bitmap backgroundImage = new Bitmap(Path.Combine(resourcesPath, "bg3.png"));

        public static List<Enemy> toBeExploded = new List<Enemy>();

        public static Font levelFont;
        public static Font DeathFont;

        public static Stopwatch YouAreDeadTimer = new Stopwatch();
        private bool isBackgroundInvalidated = true;
        public static Bitmap ExpParticleTexture = new Bitmap(Path.Combine(resourcesPath, "ExpPariticle.png"));
        public static Size originalResolution;

        public static bool GameIsEnd = false;
        public static bool GameIsRestarted = false;
        private void UpdateGame(object sender, EventArgs e)
        {
            if (!gameStarted || gamePaused)
            {
                return;
            }

            Rectangle cameraViewRectangle = new Rectangle(cameraX, 0, this.ClientSize.Width, this.ClientSize.Height);
            Parallel.Invoke(
               () => PlayerMovement.UpdatePlayer(),
               () => EnemyMovement.UpdateEnemies(),
               () => ParticalMovement.UpdateParticles()
            );
            
            Rectangle TradeZoneRect = new Rectangle((int)traderZone.Bounds.X, (int)traderZone.Bounds.Y, (int)traderZone.Bounds.Width, (int)traderZone.Bounds.Height);
            if (player.GetRectangle().IntersectsWith(TradeZoneRect))
            {
                if (TolochinApple.Count == 0 && !GameIsEnd)
                {
                    FallingPickup newPickup = new FallingPickup((int)(LevelLength * 0.9), 150, 40, 40, 10, sacredApple);
                    TolochinApple.Add(newPickup);
                }
            }

            healthBar.UpdateHealth(player.CurrentHealth);

            foreach (var Apple in TolochinApple)
            {
                RectangleF AppleRect = new RectangleF(Apple.X, Apple.Y + 2 * Apple.Speed, Apple.Width, Apple.Height);

                Apple.UpdatePosition();

                foreach (var groundRect in ground)
                {
                    var rect = groundRect.ToRectangle(this.ClientSize);
                    if (AppleRect.IntersectsWith(rect))
                    {
                        Apple.IsFalling = false;
                        break;
                    }
                }
                if (AppleRect.IntersectsWith(player.GetRectangle()))
                {
                    player.TakeDamage(1000);
                    TolochinApple.Remove(Apple);
                    GameIsEnd = true;
                    SoundManager.PlayEndingMusic();
                    deathOverlayTimer = new Timer();
                    deathOverlayTimer.Interval = gameTimer.Interval;
                    deathOverlayTimer.Tick += DeathOverlayTimer_Tick;
                    deathOverlayTimer.Start();
                    break;
                }
            }

            foreach (var pickup in pickups.ToList())
            {
                RectangleF pickupRectF = pickup.GetRectangle();
                Rectangle pickupRect = new Rectangle((int)pickupRectF.X, (int)pickupRectF.Y, (int)pickupRectF.Width, (int)pickupRectF.Height);

                if (player.GetRectangle().IntersectsWith(pickupRect))
                {
                    SoundManager.PlayEatingSound();
                    player.RestoreHealth(20);
                    pickups.Remove(pickup);
                    break;
                }
            }

            foreach (var enemy in enemies.Where(enemy => player.GetRectangle().IntersectsWith(enemy.GetRectangle())))
            {
                if (enemy.AttackCooldown.Elapsed.TotalSeconds >= 1)
                {
                    player.TakeDamage(10);
                    enemy.AttackCooldown.Restart();
                }
            }

            foreach (var enemy in toBeExploded.ToList())
            {
                ExplodeEnemy(enemy);
                SpawnExperienceParticles(enemy);
                toBeExploded.Remove(enemy);
            }
            enemies.RemoveAll(enemy => enemy.IsDead());

            if (player.IsDead() && !YouAreDeadTimer.IsRunning && !isGameOver && !GameIsEnd)
            {
                YouAreDeadTimer.Start();
                deathOverlayTimer.Start();
                //YouDiedSound.Play();
                isGameOver = true;
            }
            else if (player.IsDead() && !YouAreDeadTimer.IsRunning && !isGameOver && GameIsEnd)
            {
                YouAreDeadTimer.Start();
                isGameOver = true;
            }
            float expectedCameraX = Math.Max(0, Math.Min(LevelLength - this.ClientSize.Width, player.X - this.ClientSize.Width / 2));

            if (cameraX < expectedCameraX)
            {
                cameraX += (int)((expectedCameraX - cameraX) * 0.08);
            }
            else if (cameraX > expectedCameraX)
            {
                cameraX -= (int)((cameraX - expectedCameraX) * 0.08);
            }

            player.CurrentHealth = Math.Max(0, player.CurrentHealth);
            this.Invalidate();
        }
        private void DeathOverlayTimer_Tick(object sender, EventArgs e)
        {
            if (YouAreDeadTimer.ElapsedMilliseconds > 3500 && !GameIsEnd)
            {
                int increment = maxOverlayAlpha / (overlayDuration / deathOverlayTimer.Interval);

                if (overlayAlpha < maxOverlayAlpha)
                {
                    overlayAlpha = Math.Min(overlayAlpha + increment, maxOverlayAlpha);
                    Invalidate();
                }
                else
                {
                    deathOverlayTimer.Stop();
                }
            }
            else if (YouAreDeadTimer.ElapsedMilliseconds > 1500 && GameIsEnd)
            {
                int increment = maxOverlayAlpha / (overlayDuration / deathOverlayTimer.Interval);

                if (overlayAlpha < maxOverlayAlpha)
                {
                    overlayAlpha = Math.Min(overlayAlpha + increment, maxOverlayAlpha);
                    Invalidate();
                }
                else
                {
                    deathOverlayTimer.Stop();
                    this.Controls.Clear();

                    Ending Ending = new Ending();
                    Panel endingPanel = Ending.GetPanel();

                    this.Controls.Clear();
                    this.Controls.Add(endingPanel);
                }
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (gameStarted)
            {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                //ПЕРЕДВИЖЕНИЕ КАМЕРЫ
                Rectangle cameraViewRectangle = new Rectangle(cameraX, 0, (int)LevelLength, this.ClientSize.Height);
                g.TranslateTransform(-cameraX, 0);

                //Перерисовываем фон только при изменении размеров окна
                if (isBackgroundInvalidated)
                {
                    using (Graphics backgroundGraphics = Graphics.FromImage(backgroundBuffer))
                    {
                        backgroundGraphics.Clear(Color.Transparent);
                        backgroundGraphics.DrawImage(backgroundImage, new RectangleF(0, 0, LevelLength, this.ClientSize.Height));

                        foreach (var rectangle in ground)
                        {
                            backgroundGraphics.DrawImage(rectangle.Texture, rectangle.ToRectangle(this.ClientSize));
                        }
                        foreach (var platform in platforms)
                        {
                            backgroundGraphics.DrawImage(platform.Texture, platform.ToRectangle(this.ClientSize));
                        }
                    }
                    isBackgroundInvalidated = false;
                }
                //Отрисовка анимации, меча и прочих элементов поверх фона
                g.DrawImageUnscaled(backgroundBuffer, 0, 0, LevelLength, ClientSize.Height);

                using (Pen pen = new Pen(Color.Transparent, 2))
                {
                    g.DrawRectangle(pen, traderZone.Bounds.X, traderZone.Bounds.Y, traderZone.Bounds.Width, traderZone.Bounds.Height);
                }

                foreach (var pickup in pickups)
                {
                    g.DrawImage(pickup.Texture, pickup.X, pickup.Y, pickup.Width, pickup.Height);
                }

                foreach (var Apple in TolochinApple)
                {
                    g.DrawImage(Apple.Texture, Apple.X, Apple.Y, Apple.Width, Apple.Height);
                }

                e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), sword.GetRectangle());

                e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), player.GetRectangle());

                DrawAnimation.DrawPlayerAnimation(g);
                DrawAnimation.DrawEnemyAnimation(g);
                DrawAnimation.DrawLevelUp(g);
                
                if (PauseMenu.Active)
                {
                    this.Controls.Add(PauseMenuPanel);
                    DrawAnimation.DrawPauseMenu_BG(g);
                } 
                else
                {
                    this.Controls.Remove(PauseMenuPanel);
                }

                // Отрисовка партиклов
                foreach (var particle in particles)
                {
                    if (particle.Texture != null)
                    {
                        e.Graphics.DrawImage(particle.Texture, particle.X, particle.Y, particle.Size, particle.Size);
                    }
                    else
                    {
                        using (SolidBrush brush = new SolidBrush(particle.ParticleColor))
                        {
                            e.Graphics.FillRectangle(brush, particle.X, particle.Y, particle.Size, particle.Size);
                        }
                    }
                }
                
                // Отрисовка врагов
                foreach (var enemy in enemies)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), enemy.GetRectangle());
                }

                // Отрисовка HealthBar-а
                healthBar.Draw(g, this.ClientSize, cameraX);

                DrawAnimation.DrawDeath(g);
            }
        }
        
        // Нажатие конпок мыши
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && player.IsOnGround && !player.IsChargingAttack)
            {
                SwordAttack.AttackWithSword();
            }
            if (e.Button == MouseButtons.Right && player.IsOnGround)
            {
                SoundManager.PlayChargedAttackSound();
                SwordAttack.ChargingEnhancedAttack();
            }
        }
        
        // Нажатие кнопк на клавиатуре
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && gameStarted && !player.IsDead())
            {
                if(PauseMenu.Active == false)
                {
                    PauseMenu.Active = true;
                }
                else
                {
                    PauseMenu.Active = false;
                }
            }
            if (e.KeyCode == Keys.F && player.IsOnGround && !player.IsChargingAttack)
            {
                SwordAttack.AttackWithSword();
            }
        }

        // Кнопка "Выйти"
        private void LeaveGameBTN_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Кнопка "Настройки"
        private void SettingsBTN_Click(object sender, EventArgs e)
        {

        }

        // Кнопка "Об игре"
        private void AboutGameBTN_Click(object sender, EventArgs e)
        {
            
        }
    }
    public static class Keyboard
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        public static bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }
    }
}