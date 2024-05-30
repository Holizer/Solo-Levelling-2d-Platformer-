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
        public static string resourcesPath = Path.Combine(Application.StartupPath, @"..\..\Resources");
        public static int clientWidth;
        public static int clientHeight;
        public static Size clientSize;
        public static FormWindowState windowState;

        private SoundPlayer ChargedAttackSound = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\FateAttack.wav"));
        private SoundPlayer clickSound = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\Minecraft Menu Button Sound.wav"));
        private SoundPlayer YouDiedSound = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\dark-souls-you-died-sound.wav"));
        private SoundPlayer InMenuMenuSound = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\MainMenuSound2.wav"));
        private SoundPlayer EndingMusic = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\Hymn.wav"));

        private SoundPlayer EatingSound = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\Eating.wav"));
        public static SoundPlayer HitDetectedSound = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\HitDetected.wav"));
        public static SoundPlayer LevelUpSound = new SoundPlayer(Path.Combine(resourcesPath, "Sounds\\LevelUp.wav"));

        private Bitmap buttonBackground = new Bitmap(Path.Combine(resourcesPath, "PauseButton.png"));

        private bool isGameOver = false;
        private bool ButtonsIsCreated = false;
        public static int overlayAlpha = 0;

        private int maxOverlayAlpha = 180;
        private int overlayDuration = 500;

        private Bitmap backgroundBuffer;

        private Button restartButton;
        private Button quitButton;
        private void CreateButtons()
        {
            restartButton = new Button();
            restartButton.Text = "Начать заново";
            restartButton.Size = new Size(300, 45);
            restartButton.Location = new Point((this.ClientSize.Width - restartButton.Width) / 2, 280);
            restartButton.Click += RestartButton_Click;
            restartButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            restartButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            restartButton.ForeColor = Color.White;
            restartButton.FlatStyle = FlatStyle.Popup;
            restartButton.BackgroundImage = buttonBackground;
            restartButton.BackgroundImageLayout = ImageLayout.Stretch;
            restartButton.Cursor = Cursors.Hand;

            quitButton = new Button();
            quitButton.Text = "Выйти в главное меню";
            quitButton.Size = new Size(300, 45);
            quitButton.Location = new Point((ClientSize.Width - quitButton.Width) / 2, 340);
            quitButton.Click += QuitButton_Click;
            quitButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            quitButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            quitButton.ForeColor = Color.White;
            quitButton.FlatStyle = FlatStyle.Popup;
            quitButton.BackgroundImage = buttonBackground;
            quitButton.BackgroundImageLayout = ImageLayout.Stretch;
            restartButton.Cursor = Cursors.Hand;

            ApplyCustomFont(quitButton, "Planes_ValMore", 13);
            Controls.Add(quitButton);

            ApplyCustomFont(restartButton, "Planes_ValMore", 13);
            Controls.Add(restartButton);
            ButtonsIsCreated = true;
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            if (quitButton.Visible)
            {
                clickSound.Play();
                PauseMenu.Visible = false;
                gamePaused = true;
                Main_Menu.Visible = true;
                gameStarted = false;
            }
        }
        private void RestartButton_Click(object sender, EventArgs e)
        {
            clickSound.Play();
            RestartGame();
        }
        private void RestartGame()
        {
            gamePaused = false;
            isGameOver = false;
            ButtonsIsCreated = false;
            PauseMenu.Visible = false;
            Main_Menu.Visible = false;

            restartButton.Visible = false;
            quitButton.Visible = false;

            gameTimer.Dispose();
            gameTimer.Start();

            enemies.Clear();
            pickups.Clear();
            ground.Clear();
            platforms.Clear();
            pickups.Clear();
            TolochinApple.Clear();

            SetupGameField();

            player.MaxHealth = 100;
            player.CurrentHealth = 100;

            DrawAnimation.SetupInGameAnimation();
            playerAnimationTimer.Dispose();
            playerAnimationTimer.Start();

            cameraX = 0;

            enemyAnimationTimer.Dispose();
            enemyAnimationTimer.Start();

            deathOverlayTimer.Dispose();
            deathOverlayTimer.Start();

            YouAreDeadTimer.Reset();
            GameIsEnd = false;
            isBackgroundInvalidated = true;
        }

        public static Timer enemyAnimationTimer;
        public static Timer playerAnimationTimer;

        private Timer gameTimer;
        private Timer loadingTimer;
        private Timer deathOverlayTimer;

        private bool gamePaused = false;
        public static bool gameStarted = false;

        public static PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        public MainForm()
        {
            this.DoubleBuffered = true;
            gameTimer = new Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += UpdateGame;
            gameTimer.Start();
            SetupGameField();

            DrawAnimation.SetupInGameAnimation();

            loadingTimer = new Timer();
            loadingTimer.Interval = 2500;
            loadingTimer.Tick += LoadingTimer_Tick;

            healthBar = new HealthBar(player.MaxHealth, 200, 20, Brushes.Green, Pens.Black);

            playerAnimationTimer = new Timer();
            playerAnimationTimer.Interval = 16;
            playerAnimationTimer.Tick += UpdatePlayerAnimation;
            playerAnimationTimer.Start();

            deathOverlayTimer = new Timer();
            deathOverlayTimer.Interval = gameTimer.Interval;
            deathOverlayTimer.Tick += DeathOverlayTimer_Tick;


            enemyAnimationTimer = new Timer();
            enemyAnimationTimer.Tick += (sender, e) => UpdateEnemyAnimation(sender, e, CreateGraphics());
            enemyAnimationTimer.Interval = 16;
            enemyAnimationTimer.Start();

            this.KeyDown += MainForm_KeyDown;
            this.MouseDown += MainForm_MouseDown;
            InitializeComponent();
        }

        private int frameCount = 0;
        private float fps = 0.0f;
        private DateTime lastTime = DateTime.Now;
        private void UpdateFPS()
        {
            frameCount++;
            fpsLabel.Visible = true;
            TimeSpan elapsed = DateTime.Now - lastTime;
            if (elapsed.TotalSeconds >= 1)
            {
                fps = frameCount / (float)elapsed.TotalSeconds;
                frameCount = 0;
                lastTime = DateTime.Now;
                fpsLabel.Text = $"FPS: {fps:F2}";
            }
        }

        private string[] loadingPhrases = { "Загрузка", "Загрузка.", "Загрузка. .", "Загрузка. . ." };
        private int loadingPhraseIndex = 0;
        private int loadingTick = 0;
        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            loadingTimer.Stop();
            LoadingScreen.Visible = false;
            Main_Menu.Visible = false;
            gameStarted = true;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //InMenuMenuSound.Play(); 
            
            clientSize = this.ClientSize;
            clientWidth = clientSize.Width;
            clientHeight = clientSize.Height;
     
            this.DoubleBuffered = true;
            LoadCustomFont();
            ApplyCustomFont(EndingText, "Planes_ValMore", 18);
            ApplyCustomFont(LeaveGame, "Planes_ValMore", 18);
            ApplyCustomFont(LoadingLabel, "Planes_ValMore", 15);

            this.MinimumSize = new Size(this.Width, this.Height);

            ApplyCustomFont(start_game_btn, "Planes_ValMore", 18);

            ApplyCustomFont(leave_game_btn, "Planes_ValMore", 18);
            start_game_btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            leave_game_btn.FlatAppearance.MouseOverBackColor = Color.Transparent;

            // Устанавливаем прозрачный цвет фона при нажатии
            start_game_btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            leave_game_btn.FlatAppearance.MouseDownBackColor = Color.Transparent;

            Continue_btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LeaveGame_btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LeaveToMainMenu_btn.FlatAppearance.MouseOverBackColor = Color.Transparent;

            // Устанавливаем прозрачный цвет фона при нажатии
            Continue_btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            LeaveToMainMenu_btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            LeaveGame_btn.FlatAppearance.MouseDownBackColor = Color.Transparent;

            LeaveGame.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LeaveGame.FlatAppearance.MouseDownBackColor = Color.Transparent;

            ApplyCustomFont(Continue_btn, "Planes_ValMore", 13);
            ApplyCustomFont(LeaveToMainMenu_btn, "Planes_ValMore", 13);
            ApplyCustomFont(LeaveGame_btn, "Planes_ValMore", 13);

            LoadingLabel.Text = "Загрузка...";

            if (!ButtonsIsCreated)
            {
                CreateButtons();
            }
            restartButton.Visible = false;
            quitButton.Visible = false;

            this.KeyPreview = true;
        }
        private void LoadCustomFont()
        {
            privateFontCollection.AddFontFile(Path.Combine(resourcesPath, "Font\\8_bit_Limit.ttf"));
            privateFontCollection.AddFontFile(Path.Combine(resourcesPath, "Font\\Planes_ValMore.ttf"));
        }
        private void ApplyCustomFont(Control control, string fontFileName, float fontSize)
        {
            if (privateFontCollection.Families.Any(f => f.Name == fontFileName))
            {
                Font customFont = new Font(privateFontCollection.Families.First(f => f.Name == fontFileName), fontSize);
                control.Font = customFont;
            }
        }
        private void UpdatePlayerAnimation(object sender, EventArgs e)
        {
            if (PauseMenu.Visible)
            {
                return;
            }
            else
            {
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
            }
        }
        private void UpdateEnemyAnimation(object sender, EventArgs e, Graphics g)
        {
            if (!PauseMenu.Visible)
            {
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
            }
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
                }
            }
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

        public static int LevelLength;
        public static Player player;
        public static Sword sword;
        public static float Gravity;
        public static int cameraX = 0;
        internal static HealthBar healthBar;
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
                Y = y;
                Width = width;
                Height = height;
                Texture = texture;
            }
            public RectangleF ToRectangle(Size clientSize)
            {
                return new RectangleF(X, Y, Width, Height);
            }
        }

        public static List<Enemy> enemies;
        List<Pickup> pickups;
        public static List<DynamicRectangle> ground;
        public static List<DynamicRectangle> platforms;
        List<FallingPickup> TolochinApple;

        private void AdaptToResolution()
        {
            if ((originalResolution != Size.Empty && windowState != FormWindowState.Minimized) || GameIsRestarted)
            {
                float xRatio = (float)this.ClientSize.Width / originalResolution.Width;
                float yRatio = (float)this.ClientSize.Height / originalResolution.Height;

                LevelLength = (int)(LevelLength * xRatio);
                traderZone = new TraderZone(traderZone.Bounds.X * xRatio, traderZone.Bounds.Y * yRatio, traderZone.Bounds.Width * xRatio, traderZone.Bounds.Height * yRatio);
                Gravity *= yRatio;
                healthBar.barWidth *= xRatio;
                healthBar.barHeight *= xRatio;

                player.X = (int)(player.X * xRatio);
                player.Y = (int)(player.Y * yRatio);

                if (!gameStarted)
                {
                    LoadingLabel.Location = new Point((int)Math.Round(LoadingLabel.Location.X * xRatio),
                                                           (int)Math.Round(LoadingLabel.Location.Y * yRatio));

                    LoadingLabel.Size = new Size((int)Math.Round(LoadingLabel.Width * xRatio),
                                                 (int)Math.Round(LoadingLabel.Height * yRatio));

                    ResizeAdaptatinon.AdaptFontSize(LoadingLabel, xRatio);
                }

                ResizeAdaptatinon.AdaptFallingPickUps(TolochinApple, xRatio, yRatio);
                ResizeAdaptatinon.AdaptPeekUps(pickups, xRatio, yRatio);
                ResizeAdaptatinon.AdaptDynamicRectangles(ground, xRatio, yRatio);
                ResizeAdaptatinon.AdaptDynamicRectangles(platforms, xRatio, yRatio);

                sword.X = (int)Math.Round(sword.X * xRatio);
                sword.Y = (int)Math.Round(sword.Y * yRatio);

                ResizeAdaptatinon.AdaptFontSize(Continue_btn, xRatio);
                ResizeAdaptatinon.AdaptFontSize(LeaveToMainMenu_btn, xRatio);
                ResizeAdaptatinon.AdaptFontSize(LeaveGame_btn, xRatio);
                ResizeAdaptatinon.AdaptFontSize(EndingText, xRatio);

                if (GameIsEnd)
                {
                    LeaveGame.Location = new Point((int)Math.Round(LeaveGame.Location.X * xRatio),
                                                   (int)Math.Round(LeaveGame.Location.Y * yRatio));

                    LeaveGame.Size = new Size((int)Math.Round(LeaveGame.Width * xRatio),
                                               (int)Math.Round(LeaveGame.Height * yRatio));

                    ResizeAdaptatinon.AdaptFontSize(LeaveGame, xRatio);

                    EndingText.Size = new Size((int)Math.Round(EndingText.Width * xRatio),
                                               (int)Math.Round(EndingText.Height * yRatio));

                    IsItSecondLogo.Location = new Point((int)Math.Round(IsItSecondLogo.Location.X * xRatio),
                                                        (int)Math.Round(IsItSecondLogo.Location.Y * yRatio));

                    IsItSecondLogo.Size = new Size((int)Math.Round(IsItSecondLogo.Width * xRatio),
                                                   (int)Math.Round(IsItSecondLogo.Height * yRatio));
                }
                ResizeAdaptatinon.AdaptAnimationFrames(playerAFKAnimation.Frames, xRatio, yRatio);
                ResizeAdaptatinon.AdaptAnimationFrames(playerChargedAttackAnimation.Frames, xRatio, yRatio);
                ResizeAdaptatinon.AdaptAnimationFrames(playerMovingLeftAnimation.Frames, xRatio, yRatio);
                ResizeAdaptatinon.AdaptAnimationFrames(playerMovingRightAnimation.Frames, xRatio, yRatio);
                ResizeAdaptatinon.AdaptAnimationFrames(playerDeathAnimation.Frames, xRatio, yRatio);
                ResizeAdaptatinon.AdaptAnimationFrames(playerAttackAnimation.Frames, xRatio, yRatio);

                ResizeAdaptatinon.AdaptAnimationFrames(enemyMovingAnimation.Frames, xRatio, yRatio);
                foreach (var enemy in enemies.ToList())
                {
                    int newEnemyX = (int)(enemy.X * xRatio);
                    int newEnemyY = (int)(enemy.Y * yRatio);

                    enemy.X = newEnemyX;
                    enemy.Y = newEnemyY;

                    enemy.HorizontalSpeed = (int)Math.Round(enemy.HorizontalSpeed * xRatio);
                    enemy.JumpSpeed = (int)Math.Round(enemy.JumpSpeed * yRatio);

                    enemy.IsOnGround = false;
                }
                int newCameraX = (int)(cameraX * xRatio);
                cameraX = newCameraX;

                foreach (var particle in particles)
                {
                    particle.Size = (int)(xRatio * particle.Size);
                    particle.SpeedX = xRatio * particle.SpeedX;
                    particle.SpeedY = xRatio * particle.SpeedX;
                }
                Continue_btn.Location = new Point((int)Math.Round(Continue_btn.Location.X * xRatio),
                                                    (int)Math.Round(Continue_btn.Location.Y * yRatio));

                Continue_btn.Size = new Size((int)Math.Round(Continue_btn.Width * xRatio),
                                                (int)Math.Round(Continue_btn.Height * yRatio));

                LeaveToMainMenu_btn.Location = new Point((int)Math.Round(LeaveToMainMenu_btn.Location.X * xRatio),
                                                            (int)Math.Round(LeaveToMainMenu_btn.Location.Y * yRatio));

                LeaveToMainMenu_btn.Size = new Size((int)Math.Round(LeaveToMainMenu_btn.Width * xRatio),
                                                    (int)Math.Round(LeaveToMainMenu_btn.Height * yRatio));

                LeaveGame_btn.Location = new Point((int)Math.Round(LeaveGame_btn.Location.X * xRatio),
                                                    (int)Math.Round(LeaveGame_btn.Location.Y * yRatio));

                LeaveGame_btn.Size = new Size((int)Math.Round(LeaveGame_btn.Width * xRatio),
                (int)Math.Round(LeaveGame_btn.Height * yRatio));
                restartButton.Location = new Point((int)Math.Round(restartButton.Location.X * xRatio),
                                                    (int)Math.Round(restartButton.Location.Y * yRatio));
                restartButton.Size = new Size((int)Math.Round(restartButton.Width * xRatio),
                                                (int)Math.Round(restartButton.Height * yRatio));
                ResizeAdaptatinon.AdaptFontSize(restartButton, xRatio);

                quitButton.Location = new Point((int)Math.Round(quitButton.Location.X * xRatio),
                                                (int)Math.Round(quitButton.Location.Y * yRatio));

                quitButton.Size = new Size((int)Math.Round(quitButton.Width * xRatio),
                                            (int)Math.Round(quitButton.Height * yRatio));

                ResizeAdaptatinon.AdaptFontSize(quitButton, xRatio);
                if (!gameStarted)
                {
                    float newLogoX = logo.Location.X * xRatio;
                    float newLogoY = logo.Location.Y * yRatio;

                    float newLogoWidth = logo.Width * xRatio;
                    float newLogoHeight = logo.Height * yRatio;

                    logo.Location = new Point((int)Math.Round(newLogoX), (int)Math.Round(newLogoY));
                    logo.Size = new Size((int)Math.Round(newLogoWidth), (int)Math.Round(newLogoHeight));

                    float newButtonX = start_game_btn.Location.X * xRatio;
                    float newButtonY = start_game_btn.Location.Y * yRatio;

                    start_game_btn.Location = new Point((int)Math.Round(newButtonX), (int)Math.Round(newButtonY));

                    float newButtonWidth = start_game_btn.Width * xRatio;
                    float newButtonHeight = start_game_btn.Height * yRatio;

                    start_game_btn.Size = new Size((int)Math.Round(newButtonWidth), (int)Math.Round(newButtonHeight));

                    int newButtonX2 = (int)Math.Round(leave_game_btn.Location.X * xRatio);
                    int newButtonY2 = (int)Math.Round(leave_game_btn.Location.Y * yRatio);

                    leave_game_btn.Location = new Point(newButtonX2, newButtonY2);

                    float newButtonWidth2 = leave_game_btn.Width * xRatio;
                    float newButtonHeight2 = leave_game_btn.Height * yRatio;

                    leave_game_btn.Size = new Size((int)Math.Round(newButtonWidth2), (int)Math.Round(newButtonHeight2));

                    ResizeAdaptatinon.AdaptFontSize(start_game_btn, xRatio);
                    ResizeAdaptatinon.AdaptFontSize(leave_game_btn, xRatio);
                }
                isBackgroundInvalidated = true;
                backgroundBuffer = new Bitmap(LevelLength, this.ClientSize.Height * 2);
            }
            gamePaused = WindowState == FormWindowState.Minimized ? true : false;

            originalResolution = this.ClientSize;
        }
        public class TraderZone
        {
            public RectangleF Bounds { get; private set; }

            public TraderZone(float x, float y, float width, float height)
            {
                Bounds = new RectangleF(x, y, width, height);
            }
        }

        public static TraderZone traderZone;

        Bitmap sacredApple = new Bitmap(Path.Combine(resourcesPath, "blessing_apple.png"));
        Bitmap pickupTexture = new Bitmap(Path.Combine(resourcesPath, "golden-apple-minecraft.gif"));
        Bitmap playerTexture = new Bitmap(Path.Combine(resourcesPath, "Idle1.png"));
        Bitmap boxTexture = new Bitmap(Path.Combine(resourcesPath, "box.jpg"));
        private void SetupGameField()
        {
            LevelLength = 2500;
            Gravity = 3;
            backgroundBuffer = new Bitmap(LevelLength, this.ClientSize.Height * 2);
            traderZone = new TraderZone((int)(LevelLength * 0.825), 100, 450, 350);

            ground = new List<DynamicRectangle>
            {
                new DynamicRectangle(0, 405, 1100, 55, GroundTexture),
                new DynamicRectangle(1100, 405, 1100, 55, GroundTexture),
                new DynamicRectangle(2100, 405, 1100, 55, GroundTexture),
            };
            platforms = new List<DynamicRectangle>
            {
                new DynamicRectangle(1200, 355, 50, 50, boxTexture),
                new DynamicRectangle(1250, 355, 50, 50, boxTexture),

                new DynamicRectangle(1250, 305, 50, 50, boxTexture),
                new DynamicRectangle(1300, 305, 50, 50, boxTexture),
                new DynamicRectangle(1350, 305, 50, 50, boxTexture),
                new DynamicRectangle(1400, 305, 50, 50, boxTexture),

                new DynamicRectangle(1400, 355, 50, 50, boxTexture),
                new DynamicRectangle(1450, 355, 50, 50, boxTexture),
            };
            enemies = new List<Enemy>
            {
                //int x, int y, int horizontalSpeed, int jumpSpeed, int health, float heightPercent, float widthPercent, float xPrecent, float yPrecent, float jumpForcePercent
                new Enemy(900, 355, 3, 10, 100, 0.09f, 0.04f, 5, 0.1f, 0.15f),
                new Enemy(1200, 355, 3, 10, 100, 0.09f, 0.04f, 5, 0.1f, 0.15f),
                new Enemy(1000, 355, 3, 10, 100, 0.09f, 0.04f, 5, 0.1f, 0.15f),
                new Enemy(1300, 355, 3, 10, 100, 0.09f, 0.04f, 5, 0.1f, 0.15f),

                new Enemy(1800, 355, 3, 10, 100, 0.09f, 0.04f, 5, 0.1f, 0.15f),
                new Enemy(1900, 355, 3, 10, 100, 0.09f, 0.04f, 5, 0.1f, 0.15f),
                new Enemy(2000, 355, 3, 10, 100, 0.09f, 0.04f, 5, 0.1f, 0.15f),

            };
            pickups = new List<Pickup>
            {
                new Pickup(1320, 250, 50, 50, pickupTexture),
            };
            TolochinApple = new List<FallingPickup>
            {
            };
            player = new Player(
                        100, 310, // => x, y
                        0.02f, 100,  // => spead, hp
                        0.18f, 0.07f, // prec heigth and width
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
        private Stopwatch YouAreDeadTimer = new Stopwatch();
        private bool isBackgroundInvalidated = true;
        public static Bitmap ExpParticleTexture = new Bitmap(Path.Combine(resourcesPath, "ExpPariticle.png"));
        public static Size originalResolution;

        private bool GameIsEnd = false;
        public static bool GameIsRestarted = false;
        private void UpdateGame(object sender, EventArgs e)
        {
            if (!gameStarted || gamePaused || PauseMenu.Visible)
            {
                loadingTick++;
                if (loadingTick % 18 == 0)
                {
                    LoadingLabel.Text = loadingPhrases[loadingPhraseIndex];
                    loadingPhraseIndex = (loadingPhraseIndex + 1) % loadingPhrases.Length;
                }
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

            UpdateFPS();
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
                    EndingMusic.Play();
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
                    EatingSound.Play();
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
                YouDiedSound.Play();
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
            frameCount++;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdaptToResolution();
            Invalidate();
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
                e.Graphics.DrawString($"FPS: {fps:0.00}", Font, Brushes.White, new PointF(cameraX, 0));

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

                e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), sword.GetRectangle(this.ClientSize));

                e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), player.GetRectangle(this.ClientSize));

                DrawAnimation.DrawPlayerAnimation(g);
                DrawAnimation.DrawEnemyAnimation(g);

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
                foreach (var enemy in enemies)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Transparent, 1), enemy.GetRectangle(this.ClientSize));
                }

                healthBar.Draw(g, this.ClientSize, cameraX);

                DrawAnimation.DrawLevelUp(g);

                if (PauseMenu.Visible)
                {
                    DrawAnimation.DrawPauseMenu(g);
                }
                if (isGameOver && YouAreDeadTimer.ElapsedMilliseconds > 2500 && player.IsDead() && !GameIsEnd)
                {
                    DrawAnimation.DrawPlayerDeath(g);
                    restartButton.Visible = true;
                    quitButton.Visible = true;
                }
                else if (isGameOver && YouAreDeadTimer.ElapsedMilliseconds > 1500 && player.IsDead() && GameIsEnd)
                {
                    Color overlayColor = Color.FromArgb(overlayAlpha, 0, 0, 0);
                    Brush overlayBrush = new SolidBrush(overlayColor);

                    g.FillRectangle(overlayBrush, cameraX, 0, this.ClientSize.Width, this.ClientSize.Height);
                    if (YouAreDeadTimer.ElapsedMilliseconds > 2500)
                    {
                        EndingPanel.Visible = true;
                    }
                }
            }
        }
        // Нажатие конпокой мыши
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && player.IsOnGround && !player.IsChargingAttack)
            {
                SwordAttack.AttackWithSword();
            }
            if (e.Button == MouseButtons.Right && player.IsOnGround)
            {
                ChargedAttackSound.Play();
                SwordAttack.ChargingEnhancedAttack();
            }
        }
        // Нажатие кнопки на клавиатуре
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                ToggleFullScreen();
            }
            if (e.KeyCode == Keys.Escape && gameStarted && !player.IsDead())
            {
                PauseMenu.Visible = true;
            }
            if (e.KeyCode == Keys.F && player.IsOnGround && !player.IsChargingAttack)
            {
                SwordAttack.AttackWithSword();
            }
        }

        // Кнопка "START"
        private void start_game_btn_Click(object sender, EventArgs e)
        {
            //InMenuMenuSound.Stop();
            LoadingScreen.Visible = true;
            loadingTimer.Start();
            gamePaused = false;
        }
        
        // Кнопка "EXTI"
        private void leave_game_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void LeaveGame_btn_Click(object sender, EventArgs e)
        {
            if (PauseMenu.Visible)
            {
                Application.Exit();
            }
        }
        private void Continue_btn_Click(object sender, EventArgs e)
        {
            clickSound.Play();
            PauseMenu.Visible = false;
            gamePaused = false;
        }
        private void LeaveToMainMenu_btn_Click(object sender, EventArgs e)
        {
            clickSound.Play();
            PauseMenu.Visible = false;
            gamePaused = true;
            Main_Menu.Visible = true;
            //InMenuMenuSound.Play();
            gameStarted = false;
        }
        private void LeaveGame_Click(object sender, EventArgs e)
        {
            if (GameIsEnd)
            {
                Application.Exit();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
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