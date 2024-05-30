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
using static SoloLeveling.Player;
using static SoloLeveling.Enemy;
using static SoloLeveling.ParticalLogic;

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
        private int overlayAlpha = 0;

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

            InGameAnimation();
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

        private Timer playerAnimationTimer;
        private Timer enemyAnimationTimer;

        private Timer gameTimer;
        private Timer loadingTimer;
        private Timer deathOverlayTimer;

        private bool gamePaused = false;
        public static bool gameStarted = false;
        private void InGameAnimation()
        {
            playerAFKAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle1.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle2.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle3.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle4.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle5.png")), new RectangleF(8, 2, 60, 80)),
            }, 200);
            playerMovingRightAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame1.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame2.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame3.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame4.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame5.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame6.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame7.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame8.png")), new RectangleF(0, 2, 65, 80)),
            }, 90);
            playerMovingLeftAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame1.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame2.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame3.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame4.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame5.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame6.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame7.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame8.png")), new Rectangle(0, 2, 65, 80)),
            }, 90);
            playerDeathAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame1.png")), new RectangleF(0, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame2.png")), new RectangleF(0, 2, 60, 85)),
                //new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame3.png")), new Rectangle(0, -4, 70, 85)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame4.png")), new RectangleF(0, 20, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame5.png")), new RectangleF(0, 20, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame6.png")), new RectangleF(0, 21, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame7.png")), new RectangleF(0, 22, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame8.png")), new RectangleF(0, 22, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame9.png")), new RectangleF(0, 23, 70, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame10.png")), new RectangleF(0, 24, 75, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame11.png")), new RectangleF(0, 24, 75, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame12.png")), new RectangleF(0, 24, 75, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame13.png")), new RectangleF(0, 24, 90, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame14.png")), new RectangleF(0, 24, 90, 60)),
            }, 275);
            playerChargedLeftAttackAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame1.png")), new RectangleF(-40, 2, 100, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame2.png")), new RectangleF(0, -8, 80, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame3.png")), new RectangleF(-20, -11, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame4.png")), new RectangleF(-20, -13, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame5.png")), new RectangleF(-15, -48, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame6.png")), new RectangleF(-15, -53, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame7.png")), new RectangleF(-15, -60, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame8.png")), new RectangleF(-15, -64, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame9.png")), new RectangleF(-15, -99, 110, 183)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame10.png")), new RectangleF(-35, -78, 95, 160)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame11.png")), new RectangleF(-10, -60, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame12.png")), new RectangleF(-10, -52, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame13.png")), new RectangleF(-10, 3, 200, 87)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame14.png")), new RectangleF(-215, -7, 450, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame15.png")), new RectangleF(-160, -128, 300, 210)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame16.png")), new RectangleF(-160, -138, 330, 220)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame17.png")), new RectangleF(-175, -118, 370, 200)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame18.png")), new RectangleF(-120, -98, 310, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame19.png")), new RectangleF(-2, -98, 210, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame20.png")), new RectangleF(0, -53, 170, 135)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame21.png")), new RectangleF(0, -43, 145, 125)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame22.png")), new RectangleF(0, -3, 100, 85)),
            }, 120);
            playerChargedAttackAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame1.png")), new RectangleF(0, 2, 100, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame2.png")), new RectangleF(0, -8, 80, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame3.png")), new RectangleF(0, -11, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame4.png")), new RectangleF(-5, -13, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame5.png")), new RectangleF(-5, -48, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame6.png")), new RectangleF(-5, -53, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame7.png")), new RectangleF(-5, -60, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame8.png")), new RectangleF(-5, -64, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame9.png")), new RectangleF(-5, -99, 110, 183)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame10.png")), new RectangleF(-35, -78, 95, 160)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame11.png")), new RectangleF(-135, -60, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame12.png")), new RectangleF(-135, -52, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame13.png")), new RectangleF(-135, 3, 200, 87)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame14.png")), new RectangleF(-155, -7, 450, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame15.png")), new RectangleF(-70, -128, 300, 210)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame16.png")), new RectangleF(-100, -138, 330, 220)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame17.png")), new RectangleF(-115, -118, 370, 200)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame18.png")), new RectangleF(-110, -98, 310, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame19.png")), new RectangleF(-135, -98, 210, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame20.png")), new RectangleF(-98, -53, 170, 135)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame21.png")), new RectangleF(-75, -33, 145, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame22.png")), new RectangleF(-30, -3, 100, 85)),
            }, 120);
            playerAttackAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "AttackAnimLeft\\frame1.png")), new RectangleF(0, 2, 95, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "AttackAnimLeft\\frame3.png")), new RectangleF(-30, 2, 95, 80))
            }, 100);
            enemyMovingAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame1.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame2.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame3.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame4.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame5.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame6.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame7.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame8.png")), new RectangleF(0, 2, 35, 40)),
            }, 275);
        }

        private PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        public MainForm()
        {
            this.DoubleBuffered = true;
            gameTimer = new Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += UpdateGame;
            gameTimer.Start();
            SetupGameField();

            InGameAnimation();

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
        private void DrawPlayerAnimation(Graphics g)
        {
            Animation currentAnimation;

            if (player.IsDead())
            {
                currentAnimation = playerDeathAnimation;
                player.CurrentAnimation = playerDeathAnimation;
                playerAnimationTimer.Interval = playerDeathAnimation.Interval;
            }
            else if (player.IsChargingAttack && !player.IsDead())
            {
                int moveDirection = player.CurrentDirection == Direction.Right ? 1 : -1;
                currentAnimation = player.CurrentDirection == Direction.Right ? playerChargedAttackAnimation : playerChargedLeftAttackAnimation;
                TimeSpan elapsed = DateTime.Now - SwordAttack.chargedAttackStartTime;

                if (elapsed <= playerChargedAttackAnimation.TotalDuration)
                {
                    player.JumpForcePercent = 0;
                    player.SpeedPrecent = 0;

                    int framesCount = currentAnimation.Frames.Count;
                    player.currentSpriteIndex = (int)(framesCount * elapsed.TotalMilliseconds / playerChargedAttackAnimation.TotalDuration.TotalMilliseconds) % framesCount;

                    if (player.currentSpriteIndex == 2)
                    {
                        TryMovePlayer((int)(this.ClientSize.Width * 0.0025) * moveDirection);
                    }
                    if (player.currentSpriteIndex == 12)
                    {
                        SwordAttack.EnhancedAttack();
                    }
                    if (player.currentSpriteIndex == 9)
                    {
                        TryMovePlayer((int)(this.ClientSize.Width * 0.017) * moveDirection);
                    }
                }
                else
                {
                    player.IsChargingAttack = false;

                    player.JumpForcePercent = prevJumpForcePercent;
                    player.SpeedPrecent = prevSpeedPrecent;
                    player.currentSpriteIndex = 0;
                }

                player.CurrentAnimation = playerChargedAttackAnimation;
                playerAnimationTimer.Interval = playerChargedAttackAnimation.Interval;
            }
            else if (!player.IsChargingAttack && player.IsAttack && !player.IsDead())
            {
                currentAnimation = player.CurrentDirection == Player.Direction.Right ? AnimationManagaer.CreateInvertedAnimation(playerAttackAnimation) : playerAttackAnimation;
                TimeSpan elapsed = DateTime.Now - SwordAttack.AttackStartTime;
                int directionX = player.CurrentDirection == Player.Direction.Right ? 1 : -1;

                if (elapsed <= currentAnimation.TotalDuration)
                {
                    player.JumpForcePercent = 0;
                    player.SpeedPrecent = 0;

                    int framesCount = currentAnimation.Frames.Count;
                    player.currentSpriteIndex = (int)(framesCount * elapsed.TotalMilliseconds / currentAnimation.TotalDuration.TotalMilliseconds) % framesCount;
                    if (player.currentSpriteIndex == 1)
                    {
                        TryMovePlayer((int)(this.ClientSize.Width * 0.005) * directionX);
                        sword.X = directionX < 0 ? player.X - player.Width : player.X + player.Width / 4;
                    }
                }
                else
                {
                    SwordAttack.Attack();
                    player.IsAttack = false;
                    player.JumpForcePercent = prevJumpForcePercent;
                    player.SpeedPrecent = prevSpeedPrecent;
                    player.currentSpriteIndex = 0;
                }

                player.CurrentAnimation = currentAnimation;
                playerAnimationTimer.Interval = currentAnimation.Interval;
            }

            else if (player.IsAFK())
            {
                currentAnimation = player.CurrentDirection == Player.Direction.Left ? AnimationManagaer.CreateInvertedAnimation(playerAFKAnimation) : playerAFKAnimation;
                player.CurrentAnimation = playerAFKAnimation;
                playerAnimationTimer.Interval = playerAFKAnimation.Interval;
            }
            else
            {
                if (player.CurrentDirection == Player.Direction.Left)
                {
                    currentAnimation = playerMovingLeftAnimation;
                    player.CurrentAnimation = playerMovingLeftAnimation;
                    playerAnimationTimer.Interval = playerMovingLeftAnimation.Interval;
                }
                else
                {
                    currentAnimation = playerMovingRightAnimation;
                    player.CurrentAnimation = playerMovingRightAnimation;
                    playerAnimationTimer.Interval = playerMovingRightAnimation.Interval;
                }
            }

            if (player.currentSpriteIndex < 0 || player.currentSpriteIndex >= currentAnimation.Frames.Count)
            {
                player.currentSpriteIndex = 0;
            }

            g.DrawImage(
                currentAnimation.Frames[player.currentSpriteIndex].Frame,
                player.X + currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.X,
                player.Y + currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.Y,
                currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.Width,
                currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.Height
            );
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
        private void DrawEnemyAnimation(Graphics g)
        {
            Animation currentAnimation = enemyMovingAnimation;

            foreach (var enemy in enemies)
            {
                if (enemy.CurrentDirection == Enemy.EnemyDirection.Right)
                {
                    currentAnimation = enemyMovingAnimation;
                    enemy.CurrentAnimation = currentAnimation;
                    enemyAnimationTimer.Interval = enemyMovingAnimation.Interval;
                }
                else if (enemy.CurrentDirection == Enemy.EnemyDirection.Left)
                {
                    currentAnimation = AnimationManagaer.CreateInvertedAnimation(enemyMovingAnimation);
                    enemy.CurrentAnimation = currentAnimation;
                    enemyAnimationTimer.Interval = enemyMovingAnimation.Interval;
                }
                g.DrawImage(
                    currentAnimation.Frames[enemy.currentSpriteIndex].Frame,
                    enemy.X + currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.X,
                    enemy.Y + currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.Y,
                    currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.Width,
                    currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.Height
                );
            }
        }
        private void TryMovePlayer(int deltaX)
        {
            int proposedX = player.X + deltaX;
            bool willCollide = WillCollideWithObstacles(proposedX, player.Y, player.Width, player.Height);

            if (!willCollide)
            {
                player.X = proposedX;
            }
        }
        private bool WillCollideWithObstacles(int x, int y, int width, int height)
        {
            if (x < 0 || x + width > LevelLength)
            {
                return true;
            }

            // Пример проверки столкновения с платформами
            foreach (var platformRect in platforms)
            {
                var rect = platformRect.ToRectangle(this.ClientSize);
                if (CheckCollision(x, y, width, height, rect.X, rect.Y, rect.Width, rect.Height))
                {
                    return true;
                }
            }

            // Пример проверки столкновения с землей
            foreach (var groundRect in ground)
            {
                var rect = groundRect.ToRectangle(this.ClientSize);
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
        private int cameraX = 0;
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
        List<DynamicRectangle> ground;
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
        void HandlePlayerDead()
        {
            player.SpeedPrecent = 0;

            if (!player.IsOnGround)
            {
                player.VerticalSpeed = Gravity;
            }
            else
            {
                player.JumpForcePercent = 0;
                player.CurrentAnimation = playerDeathAnimation;
                playerAnimationTimer.Interval = playerDeathAnimation.Interval;

                if (player.currentSpriteIndex == player.CurrentAnimation.Frames.Count - 1)
                {
                    playerAnimationTimer.Stop();
                }
            }
        }
        void HandlePlayerAFK()
        {
            player.CurrentAnimation = playerAFKAnimation;
            playerAnimationTimer.Interval = playerAFKAnimation.Interval;
        }
        void HandlePlayerAlive()
        {
            if (player.CurrentDirection == Player.Direction.Left)
            {
                player.CurrentAnimation = playerMovingLeftAnimation;
                playerAnimationTimer.Interval = playerMovingLeftAnimation.Interval;
            }
            else
            {
                player.CurrentAnimation = playerMovingRightAnimation;
                playerAnimationTimer.Interval = playerMovingRightAnimation.Interval;
            }
        }
        void UpdatePlayer()
        {
            if (player.IsDead())
            {
                HandlePlayerDead();
            }
            else if (player.IsAFK())
            {
                HandlePlayerAFK();
            }
            else
            {
                HandlePlayerAlive();
            }

            sword.Y = player.Y;

            if (Keyboard.IsKeyDown(Keys.A) || Keyboard.IsKeyDown(Keys.D))
            {
                int direction = Keyboard.IsKeyDown(Keys.A) ? -1 : 1;

                player.CurrentDirection = direction == -1 ? Direction.Left : Direction.Right;

                sword.X = player.CurrentDirection == Direction.Left ? player.X - player.Width - player.Width / 4 : player.X + player.Width / 4;

                for (int speed = (int)player.Speed(this.ClientSize); speed > 0; speed--)
                {
                    int proposedLocation = player.X + speed * direction;
                    if (proposedLocation >= 0 && proposedLocation <= LevelLength - player.Width)
                    {
                        bool playerWillCollide = ground.Any(rect => CheckCollision(proposedLocation, player.Y, player.Width, player.Height, rect.X, rect.Y, rect.Width, rect.Height))
                                              || platforms.Any(rect => CheckCollision(proposedLocation, player.Y, player.Width, player.Height, rect.X, rect.Y, rect.Width, rect.Height));

                        if (!playerWillCollide)
                        {
                            player.X = proposedLocation;
                            break;
                        }
                    }
                }
            }

            player.IsOnGround = false;

            player.IsOnGround = ground.Any(rect => CheckCollision(player.X, player.Y + 1, player.Width, player.Height, rect.X, rect.Y, rect.Width, rect.Height))
                             || platforms.Any(rect => CheckCollision(player.X, player.Y + 1, player.Width, player.Height, rect.X, rect.Y, rect.Width, rect.Height));

            foreach (var platformRect in platforms)
            {
                var rect = platformRect.ToRectangle(this.ClientSize);
                if (CheckCollision(player.X, player.Y + 1, player.Width, player.Height, rect.X, rect.Y, rect.Width, rect.Height))
                {
                    player.IsOnGround = true;
                    break;
                }
            }

            if (Keyboard.IsKeyDown(Keys.Space) && player.IsOnGround)
            {
                player.VerticalSpeed = -player.JumpForce(this.ClientSize) / 4;
                player.IsOnGround = false;
            }

            int oldY = player.Y;
            float newY = oldY + player.VerticalSpeed;

            bool collisionDetected = false;
            RectangleF collidedObstacle = new RectangleF();

            foreach (var groundRect in ground)
            {
                var rect = groundRect.ToRectangle(this.ClientSize);
                if (CheckCollision(player.X, (int)newY, player.Width, player.Height, rect.X, rect.Y, rect.Width, rect.Height))
                {
                    collisionDetected = true;
                    collidedObstacle = rect;
                    break;
                }
            }

            foreach (var platformRect in platforms)
            {
                var rect = platformRect.ToRectangle(this.ClientSize);
                if (CheckCollision(player.X, (int)newY, player.Width, player.Height, rect.X, rect.Y, rect.Width, rect.Height))
                {
                    collisionDetected = true;
                    collidedObstacle = rect;
                    break;
                }
            }

            player.VerticalSpeed += Gravity;

            if (!collisionDetected)
            {
                player.Y = (int)newY;
            }
            else
            {
                if (player.VerticalSpeed > 0)
                {
                    player.Y = (int)(collidedObstacle.Y - player.Height);
                    player.VerticalSpeed = 0;
                }
                else if (player.VerticalSpeed < 0)
                {
                    player.Y = (int)(collidedObstacle.Y + collidedObstacle.Height);
                    player.VerticalSpeed = 0;
                }
            }
        }
        void UpdateEnemies()
        {
            foreach (var enemy in enemies)
            {
                // Расчет расстояния между игроком и врагом
                float distanceX = player.X - enemy.X;
                float distanceY = player.Y - enemy.Y;
                double distanceToPlayer = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

                // Логика для прыжка врага, когда игрок близко
                if (distanceToPlayer < this.ClientSize.Width * 0.3 && enemy.IsOnGround && !player.IsDead())
                {
                    enemy.JumpDirectionX = player.X < enemy.X ? -1 : 1;
                    enemy.CurrentDirection = enemy.JumpDirectionX < 0 ? EnemyDirection.Left : EnemyDirection.Right;

                    if (!enemy.JumpTimer.IsRunning)
                    {
                        enemy.JumpTimer.Restart();
                    }

                    if (enemy.JumpTimer.ElapsedMilliseconds >= 500 && !enemy.IsJumping && enemy.IsOnGround)
                    {
                        enemy.IsJumping = true;
                        enemy.VerticalSpeed = -enemy.JumpForce(this.ClientSize) / 4;
                        enemy.JumpTimer.Reset();
                    }
                }
                else
                {
                    // Логика для движения врага по платформам
                    enemy.JumpTimer.Stop();
                    enemy.JumpTimer.Reset();

                    if (!enemy.IsJumping && enemy.IsOnGround)
                    {
                        Rectangle enemyRect = new Rectangle(enemy.X, enemy.Y, enemy.Width, enemy.Height);

                        bool PlatformCollision = false;

                        if (enemy.X <= 0 || enemy.X + enemy.Width >= LevelLength)
                        {
                            enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                        }

                        foreach (var platformRect in platforms)
                        {
                            var rect = platformRect.ToRectangle(this.ClientSize);

                            if (CheckCollision(enemy.X, enemy.Y, enemy.Width, enemy.Height, rect.X, rect.Y, rect.Width, rect.Height))
                            {
                                PlatformCollision = true;

                                if (enemyRect.Left < rect.Left)
                                {
                                    enemy.X = (int)rect.Left - enemy.Width;
                                    enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                    enemy.CurrentDirection = EnemyDirection.Left;
                                }
                                else
                                {
                                    enemy.X = (int)rect.Right;
                                    enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                    enemy.CurrentDirection = EnemyDirection.Right;
                                }
                            }
                        }

                        // Логика для определения направления движения врага
                        if (enemy.HorizontalSpeed < 0)
                        {
                            enemy.CurrentDirection = EnemyDirection.Left;
                        }
                        else if (enemy.HorizontalSpeed > 0)
                        {
                            enemy.CurrentDirection = EnemyDirection.Right;
                        }

                        enemy.X += enemy.HorizontalSpeed;

                        if (!PlatformCollision)
                        {
                            enemy.IsOnGround = false;
                            enemy.IsJumping = true;
                        }
                    }
                }

                if (enemy.IsJumping || !enemy.IsOnGround)
                {

                    if (enemy.X <= 0 || enemy.X - enemy.Width >= LevelLength)
                    {
                        enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                    }

                    int proposedX = enemy.X;
                    bool collisionWithPlatform = false;
                    int proposedY = enemy.Y + (int)enemy.VerticalSpeed;

                    enemy.VerticalSpeed += (int)Gravity;
                    enemy.IsOnGround = enemy.IsJumping ? false : true;

                    if (distanceToPlayer < this.ClientSize.Width * 0.3 && player.IsOnGround && !player.IsDead())
                    {
                        proposedX = enemy.X + enemy.JumpSpeed * enemy.JumpDirectionX;
                        enemy.X = proposedX;
                    }

                    RectangleF enemyRect = new RectangleF(proposedX, proposedY, enemy.Width, enemy.Height);

                    foreach (var groundRect in ground)
                    {
                        var rect = groundRect.ToRectangle(this.ClientSize);
                        if (enemyRect.IntersectsWith(rect))
                        {
                            enemy.IsOnGround = true;
                            enemy.IsJumping = false;
                            enemy.VerticalSpeed = 0;
                            enemy.Y = (int)rect.Y - enemy.Height;
                            collisionWithPlatform = true;
                            break;
                        }
                    }

                    foreach (var platformRect in platforms)
                    {
                        var rect = platformRect.ToRectangle(this.ClientSize);
                        if (enemyRect.IntersectsWith(rect))
                        {
                            bool fromTop = enemyRect.Bottom >= rect.Top && enemyRect.Bottom <= rect.Top + (int)(enemy.VerticalSpeed + Gravity);

                            if (enemyRect.Left < rect.Left && enemyRect.Bottom <= rect.Bottom && !fromTop)
                            {
                                enemy.X = (int)rect.Left - enemy.Width;
                                enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                enemy.CurrentDirection = EnemyDirection.Left;
                            }
                            else if (enemyRect.Right > rect.Right && !fromTop)
                            {
                                enemy.X = (int)rect.Right;
                                enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                enemy.CurrentDirection = EnemyDirection.Right;
                            }
                            else
                            {
                                if (enemy.VerticalSpeed < 0 && enemyRect.Bottom >= rect.Bottom)
                                {
                                    enemy.Y = (int)rect.Bottom;
                                    collisionWithPlatform = true;
                                    enemy.IsJumping = true;
                                    enemy.IsOnGround = false;
                                    enemy.VerticalSpeed = 0;
                                }
                                else if (fromTop)
                                {
                                    collisionWithPlatform = true;
                                    enemy.IsJumping = false;
                                    enemy.IsOnGround = true;
                                }
                                if (enemyRect.Bottom > rect.Top && enemyRect.Bottom < rect.Bottom)
                                {
                                    collisionWithPlatform = true;
                                    enemy.Y = (int)rect.Top - enemy.Height;
                                    enemy.VerticalSpeed = 0;
                                    enemy.IsJumping = false;
                                    enemy.IsOnGround = true;
                                }
                            }
                        }
                    }
                    if (!collisionWithPlatform)
                    {
                        enemy.Y = proposedY;
                    }
                }
            }
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

        Font levelFont;
        Font DeathFont;

        private bool f11KeyPressed = false;
        private Stopwatch YouAreDeadTimer = new Stopwatch();
        private bool isBackgroundInvalidated = true;
        public static Bitmap ExpParticleTexture = new Bitmap(Path.Combine(resourcesPath, "ExpPariticle.png"));
        public static Size originalResolution;
        void MoveExperienceParticleTowardsPlayer(Particle particle)
        {
            float targetX = player.X < particle.X ? player.X - player.Width / 2 : player.X + player.Width / 2;
            float targetY = player.Y + player.Height / 2;

            float directionX = targetX - particle.X;
            float directionY = targetY - particle.Y;
            float distanceToPlayer = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            float lerpFactor = 0.05f;
            particle.X = (int)MathHelper.Lerp(particle.X, targetX, lerpFactor);
            particle.Y = (int)MathHelper.Lerp(particle.Y, targetY, lerpFactor);

            Rectangle particleRectangle = new Rectangle(particle.X, particle.Y, particle.Size, particle.Size);

            if (particleRectangle.IntersectsWith(player.GetRectangle(this.ClientSize)))
            {
                particles.Remove(particle);
                player.GainExperience(20);
            }
        }
        void UpdateParticles()
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
                    ParticalLogic.MoveDeathParticle(particle);
                }

                if (particle.X > cameraX + this.ClientSize.Width || particle.X < 0 || particle.Y > this.ClientSize.Height || particle.Y < 0)
                {
                    particles.RemoveAt(i);
                }
            }
        }

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
               () => UpdatePlayer(),
               () => UpdateEnemies(),
               () => UpdateParticles()
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
                ParticalLogic.ExplodeEnemy(enemy);
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

                DrawPlayerAnimation(g);
                DrawEnemyAnimation(g);

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

                DrawLevelUp(g);

                if (PauseMenu.Visible)
                {
                    DrawPauseMenu(g);
                }
                if (isGameOver && YouAreDeadTimer.ElapsedMilliseconds > 2500 && player.IsDead() && !GameIsEnd)
                {
                    DrawPlayerDeath(g);
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
        private void DrawLevelUp(Graphics g)
        {
            string levelText = $"Level: {player.Level}";
            string experienceText = $"Experience: {player.Experience}/{player.ExperienceToNextLevel}";

            levelFont = new Font(privateFontCollection.Families.First(f => f.Name == "Planes_ValMore"), (int)(this.ClientSize.Width * 0.018), FontStyle.Bold);

            SizeF levelTextSize = g.MeasureString(levelText, levelFont);
            SizeF experienceTextSize = g.MeasureString(experienceText, levelFont);

            float levelTextX = cameraX + this.ClientSize.Width / 2 - levelTextSize.Width / 2;
            float levelTextY = this.ClientSize.Width * 0.012f;
            PointF levelTextLocation = new PointF(levelTextX, levelTextY);

            float experienceTextX = cameraX + this.ClientSize.Width / 2 - experienceTextSize.Width / 2;
            float experienceTextY = levelTextY + levelTextSize.Height;
            PointF experienceTextLocation = new PointF(experienceTextX, experienceTextY);

            g.DrawString(levelText, levelFont, Brushes.White, levelTextLocation);
            g.DrawString(experienceText, levelFont, Brushes.White, experienceTextLocation);
        }
        private void DrawPlayerDeath(Graphics g)
        {
            string deathText = "ВЫ ПОГИБЛИ";
            DeathFont = new Font(privateFontCollection.Families.First(f => f.Name == "Planes_ValMore"), (int)(this.ClientSize.Width * 0.05), FontStyle.Bold);

            SizeF TextSize = g.MeasureString(deathText, DeathFont);

            float TextX = cameraX + this.ClientSize.Width / 2 - TextSize.Width / 2;
            float TextY = this.ClientSize.Height * 0.35f;

            PointF DeathTextLocation = new PointF(TextX, TextY);

            Color overlayColor = Color.FromArgb(overlayAlpha, 0, 0, 0);
            Brush overlayBrush = new SolidBrush(overlayColor);

            g.FillRectangle(overlayBrush, cameraX, 0, this.ClientSize.Width, this.ClientSize.Height);

            g.DrawString(deathText, DeathFont, Brushes.Red, DeathTextLocation);
        }
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
        private void DrawPauseMenu(Graphics g)
        {
            Color overlayColor = Color.FromArgb(160, 0, 0, 0);
            Brush overlayBrush = new SolidBrush(overlayColor);
            g.FillRectangle(overlayBrush, cameraX, 0, this.ClientSize.Width, this.ClientSize.Height);
        }
        private void start_game_btn_Click(object sender, EventArgs e)
        {
            //InMenuMenuSound.Stop();
            LoadingScreen.Visible = true;
            loadingTimer.Start();
            gamePaused = false;
        }
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