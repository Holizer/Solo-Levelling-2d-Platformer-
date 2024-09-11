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
using System.Media;
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

        public static bool isGameOver = false;

        private Bitmap backgroundBuffer;

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
        Bitmap boxTexture = new Bitmap(Path.Combine(resourcesPath, "box.jpg"));
        private void SetupGameField()
        {        
            LevelLength = 3000;
            Gravity = 3;
            backgroundBuffer = new Bitmap(LevelLength, 720);
            traderZone = new TraderZone((int)(LevelLength * 0.2), 100, 450, 350);
            ground = new List<DynamicRectangle>
            {
                new DynamicRectangle(0, this.ClientSize.Height - 80, LevelLength/2, 80, GroundTexture),
                new DynamicRectangle(LevelLength/2, this.ClientSize.Height - 80, LevelLength/2, 80, GroundTexture)
            };
            platforms = new List<DynamicRectangle>
            {

            };
            enemies = new List<Enemy>
            {
            };
            pickups = new List<Pickup>
            {
            };
            TolochinApple = new List<FallingPickup>
            {
                new FallingPickup(300, 100, 150, 150, 10, sacredApple)
            };
            player = new Player(
                        100, 310, // => x, y
                        0.02f, 100,  // => spead, hp
                        0.17f, 0.06f, // prec heigth and width
                        0.1f, 0.1f, // prec x and y
                        0.2f, playerTexture); // JumpForce prec and start texture
            //public Sword(int x, int y, float width, float height, int damage)
            sword = new Sword(player.X, player.Y, 0.15f, 0.18f, 35);
        }
        public static bool CheckCollision(int x1, int y1, int w1, int h1, float x2, float y2, float w2, float h2)
        {
            return x1 < x2 + w2
                && x1 + w1 > x2
                && y1 < y2 + h2
                && y1 + h1 > y2;
        }

        public static float prevJumpForcePercent;
        public static float prevSpeedPrecent;
       
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

        private bool f11KeyPressed = false;
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
            if (player.GetRectangle(ClientSize).IntersectsWith(TradeZoneRect))
            {
                if (TolochinApple.Count == 0 && !GameIsEnd)
                {
                    FallingPickup newPickup = new FallingPickup((int)(LevelLength * 0.9), 300, 40, 40, 10, sacredApple);
                    TolochinApple.Add(newPickup);
                }
            }

            healthBar.UpdateHealth(player.CurrentHealth);

            if (f11KeyPressed)
            {
                ToggleFullScreen();
                f11KeyPressed = false;
            }

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
                if (AppleRect.IntersectsWith(player.GetRectangle(this.ClientSize)))
                {
                    player.TakeDamage(1000);
                    TolochinApple.Remove(Apple);
                    GameIsEnd = true;
                    //EndingMusic.Play();
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

                if (player.GetRectangle(ClientSize).IntersectsWith(pickupRect))
                {
                    SoundManager.PlayEndingMusic();
                    player.RestoreHealth(20);
                    pickups.Remove(pickup);
                    break;
                }
            }

            foreach (var enemy in enemies.Where(enemy => player.GetRectangle(this.ClientSize).IntersectsWith(enemy.GetRectangle(this.ClientSize))))
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
            if (YouAreDeadTimer.ElapsedMilliseconds > 2500 && !GameIsEnd)
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
            AdaptToResolution();
            Invalidate();
        }
        private void AdaptToResolution()
        {
            if ((originalResolution != Size.Empty && windowState != FormWindowState.Minimized))
            {
                float widthRatio = (float)this.ClientSize.Width / originalResolution.Width;
                float heightRatio = (float)this.ClientSize.Height / originalResolution.Height;

                LevelLength = (int)(LevelLength * widthRatio);
                traderZone = new TraderZone(traderZone.Bounds.X * widthRatio, traderZone.Bounds.Y * heightRatio, traderZone.Bounds.Width * widthRatio, traderZone.Bounds.Height * heightRatio);
                Gravity *= heightRatio;
                healthBar.barWidth *= widthRatio;
                healthBar.barHeight *= widthRatio;

                player.X = (int)(player.X * widthRatio);
                player.Y = (int)(player.Y * heightRatio);

                ResizeAdaptatinon.AdaptFallingPickUps(TolochinApple, widthRatio, heightRatio);
                ResizeAdaptatinon.AdaptPeekUps(pickups, widthRatio, heightRatio);
                ResizeAdaptatinon.AdaptDynamicRectangles(ground, widthRatio, heightRatio);
                ResizeAdaptatinon.AdaptDynamicRectangles(platforms, widthRatio, heightRatio);

                sword.X = (int)Math.Round(sword.X * widthRatio);
                sword.Y = (int)Math.Round(sword.Y * widthRatio);

                //ResizeAdaptatinon.AdaptAnimationFrames(playerAFKAnimation.Frames, widthRatio, heightRatio);
                //ResizeAdaptatinon.AdaptAnimationFrames(playerChargedAttackAnimation.Frames, widthRatio, heightRatio);
                //ResizeAdaptatinon.AdaptAnimationFrames(playerMovingLeftAnimation.Frames, widthRatio, heightRatio);
                //ResizeAdaptatinon.AdaptAnimationFrames(playerMovingRightAnimation.Frames, widthRatio, heightRatio);
                //ResizeAdaptatinon.AdaptAnimationFrames(playerDeathAnimation.Frames, widthRatio, heightRatio);
                //ResizeAdaptatinon.AdaptAnimationFrames(playerAttackAnimation.Frames, widthRatio, heightRatio);

                //ResizeAdaptatinon.AdaptAnimationFrames(enemyMovingAnimation.Frames, widthRatio, heightRatio);
                foreach (var enemy in enemies.ToList())
                {
                    int newEnemyX = (int)(enemy.X * widthRatio);
                    int newEnemyY = (int)(enemy.Y * heightRatio);

                    enemy.X = newEnemyX;
                    enemy.Y = newEnemyY;

                    enemy.HorizontalSpeed = (int)Math.Round(enemy.HorizontalSpeed * widthRatio);
                    enemy.JumpSpeed = (int)Math.Round(enemy.JumpSpeed * heightRatio);

                    enemy.IsOnGround = false;
                }
                int newCameraX = (int)(cameraX * widthRatio);
                cameraX = newCameraX;

                foreach (var particle in particles)
                {
                    particle.Size = (int)(widthRatio * particle.Size);
                    particle.SpeedX = widthRatio * particle.SpeedX;
                    particle.SpeedY = widthRatio * particle.SpeedX;
                }

                if (!gameStarted)
                {
                    float newLogoX = LogoPicture.Location.X * widthRatio;
                    float newLogoY = LogoPicture.Location.Y * heightRatio;

                    float newLogoWidth = LogoPicture.Width * widthRatio;
                    float newLogoHeight = LogoPicture.Height * heightRatio;

                    LogoPicture.Location = new Point((int)Math.Round(newLogoX), (int)Math.Round(newLogoY));
                    LogoPicture.Size = new Size((int)Math.Round(newLogoWidth), (int)Math.Round(newLogoHeight));

                    float newButtonX = StartGameBTN.Location.X * widthRatio;
                    float newButtonY = StartGameBTN.Location.Y * heightRatio;

                    StartGameBTN.Location = new Point((int)Math.Round(newButtonX), (int)Math.Round(newButtonY));

                    float newButtonWidth = StartGameBTN.Width * widthRatio;
                    float newButtonHeight = StartGameBTN.Height * heightRatio;

                    StartGameBTN.Size = new Size((int)Math.Round(newButtonWidth), (int)Math.Round(newButtonHeight));

                    int newButtonX2 = (int)Math.Round(LeaveGameBTN.Location.X * widthRatio);
                    int newButtonY2 = (int)Math.Round(LeaveGameBTN.Location.Y * heightRatio);

                    LeaveGameBTN.Location = new Point(newButtonX2, newButtonY2);

                    float newButtonWidth2 = LeaveGameBTN.Width * widthRatio;
                    float newButtonHeight2 = LeaveGameBTN.Height * heightRatio;

                    LeaveGameBTN.Size = new Size((int)Math.Round(newButtonWidth2), (int)Math.Round(newButtonHeight2));

                    ResizeAdaptatinon.AdaptFontSize(StartGameBTN, widthRatio);
                    ResizeAdaptatinon.AdaptFontSize(LeaveGameBTN, widthRatio);
                }
                isBackgroundInvalidated = true;
                backgroundBuffer = new Bitmap(LevelLength, this.ClientSize.Height * 2);
            }
            gamePaused = WindowState == FormWindowState.Minimized ? true : false;

            originalResolution = this.ClientSize;
        }
        private void ToggleFullScreen()
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
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

                using (Pen pen = new Pen(Color.Red, 2))
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

                e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), sword.GetRectangle(this.ClientSize));

                e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), player.GetRectangle(this.ClientSize));

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
                    e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), enemy.GetRectangle(this.ClientSize));
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
                //SoundManager.PlayDefaultAttackSound();
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
            if (e.KeyCode == Keys.F11)
            {
                ToggleFullScreen();
            }
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