using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace Snake
{
    public partial class SnakeForm : Form,IMessageFilter
    {
        SnakePlayer Player1;
        FoodManager FoodMngr;
        Random r = new Random();
        private int score = 0;
        public SnakeForm()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            this.FormClosed += (s, e) => Application.RemoveMessageFilter(this);
            Player1 = new SnakePlayer(this);
            FoodMngr = new FoodManager(GameCanvas.Width, GameCanvas.Height);
            FoodMngr.AddRandomFood(10);
            ScoreTxtBox.Text = score.ToString();
            
        }
        

        public void ToggleTimer()
        {
            GameTimer.Enabled = !GameTimer.Enabled;
        }

        public void ResetGame()
        {
            Player1 = new SnakePlayer(this);
            FoodMngr = new FoodManager(GameCanvas.Width, GameCanvas.Height);
            FoodMngr.AddRandomFood(10);
            score = 0;
        }

        public bool PreFilterMessage(ref Message msg)
        {
            if(msg.Msg == 0x0101) 
                Input.SetKey((Keys)msg.WParam, false);
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(msg.Msg == 0x100) 
                Input.SetKey((Keys)msg.WParam, true);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Player1.Draw(canvas);
            FoodMngr.Draw(canvas);
        }

        private void CheckForCollisions()
        {
            if (Player1.IsIntersectingRect(new Rectangle(-100, 0, 100, GameCanvas.Height)))
                
            Player1.OnHitWall(Direction.left);

            if (Player1.IsIntersectingRect(new Rectangle(0, -100, GameCanvas.Width, 100)))
                
            Player1.OnHitWall(Direction.up);

            if (Player1.IsIntersectingRect(new Rectangle(GameCanvas.Width, 0, 100, GameCanvas.Height)))
                
            Player1.OnHitWall(Direction.right);

            if (Player1.IsIntersectingRect(new Rectangle(0, GameCanvas.Height, GameCanvas.Width, 100)))
                
            Player1.OnHitWall(Direction.down);

            
            List<Rectangle> SnakeRects = Player1.GetRects();
            foreach(Rectangle rect in SnakeRects)
            {
                if(FoodMngr.IsIntersectingRect(rect,true))
                {
                    SystemSounds.Asterisk.Play();
                    FoodMngr.AddRandomFood();
                    Player1.AddBodySegments(1);
                    score++;
                    ScoreTxtBox.Text = score.ToString();
                }
            }
        }

        private void SetPlayerMovement()
        {
            if (Input.IsKeyDown(Keys.Left))
            {
                Player1.SetDirection(Direction.left);
                SystemSounds.Hand.Play();

            }
            else if (Input.IsKeyDown(Keys.Right))
            {
                Player1.SetDirection(Direction.right);
                SystemSounds.Hand.Play();
            }
            else if (Input.IsKeyDown(Keys.Up))
            {
                Player1.SetDirection(Direction.up);
                SystemSounds.Hand.Play();
            }
            else if (Input.IsKeyDown(Keys.Down))
            {
                Player1.SetDirection(Direction.down);
                SystemSounds.Hand.Play();
            }
            Player1.MovePlayer();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            SetPlayerMovement();
            CheckForCollisions();
            GameCanvas.Invalidate();
        }

        private void Start_Btn_Click(object sender, EventArgs e)
        {
            ToggleTimer();
        }

        private void DareBtn_Click(object sender, EventArgs e)
        {
            int index = r.Next(0);
            switch(index)
            {
              case 0:
                    MessageBox.Show("More food added");
                    FoodMngr.AddRandomFood(20);
                    GameCanvas.Invalidate();
                    break;
                default:
                    break;
            }
        }

        private void SnakeForm_Load(object sender, EventArgs e)
        {

        }
    }
}
