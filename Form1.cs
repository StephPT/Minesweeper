using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;


namespace Miner_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //These are all of the options that are in charge of controlling how the game functions/loads.
        const int GSize = 25;
        int x;
        int y;
        int m = GSize - 1;
        int m2 = GSize / 2;
        bool Bomb = true;
        bool pointBlock = true;
        bool diamondBlock = true;
        int diff = 0;
        bool dead = true;
        int pointValue = 0;
        int bal;
        int winValue = 15;
        int curLevel = 1;
        int token = 0;
        int lives = 0;


        Square[,] square = new Square[GSize, GSize]; //creates a new array that using the object Square.

        Random Ran = new Random(); //sets up a random generator.



        private void Form1_Load(object sender, EventArgs e)
        {

            //sets the values of the textboxes and trackbar in the form.
            this.Focus();
            trackBar1.Value = diff / 2;
            textBox4.Text = curLevel.ToString();
            textBox3.Text = pointValue.ToString();

            //This double for loop is in charge of generating the X and Y values of all of the lables.

            for (int o = 0; o < GSize; o++)
            {
                x = 0;
                for (int i = 0; i < GSize; i++)
                {
                    //These if statements are generating the random position of the bombs, coal and diamonds in the level.
                    //This is also where it ensures that a squares can be equal to more than one block type.
                    if (Ran.Next(0, 100) > diff)
                    {
                        Bomb = false;
                    }
                    else
                    {
                        Bomb = true;
                        pointBlock = false;
                        diamondBlock = false;
                    }

                    if (Ran.Next(0, 100) > 4)
                    {
                        pointBlock = false;
                    }
                    else
                    {
                        pointBlock = true;
                        diamondBlock = false;
                        Bomb = false;
                    }

                    if (Ran.Next(0, 250) > 0)
                    {
                        diamondBlock = false;
                    }
                    else
                    {
                        diamondBlock = true;
                        pointBlock = false;
                        Bomb = false;
                    }
                    //Here is where it is generating the Squares and putting them in the 2D array.
                    square[o, i] = new Square(x, y, Color.Blue, Bomb, pointBlock, diamondBlock, token);
                    //This is setting the image of the square depending on properties that are assigned to it.
                    if (square[o, i].pointBlock == true)
                    {
                        square[o, i].Image = Properties.Resources.coal;
                    }


                    if (square[o, i].diamondBlock == true)
                    {
                        square[o, i].Image = Properties.Resources.diamond;
                    }

                    //this finally adds each of the squares in to the panel - therefore displaying them on the screen.
                    this.panel1.Controls.Add(square[o, i]);
                    square[o, i].Click += new System.EventHandler(this.Square_Click);
                    //Adding click function to each square
                    x = x + 25;
                }
                y = y + 25;

            }

            if (square[m, m2].bomb == true) square[m, m2].bomb = false;
            if (square[m, m2].pointBlock == true) square[m, m2].pointBlock = false;
            if (square[m, m2].diamondBlock == true) square[m, m2].diamondBlock = false;
            textBox2.Text = trackBar1.Value.ToString();
            if (dead == true)
            {
                btnBackwards.Enabled = false;
                btnLeft.Enabled = false;
                btnRight.Enabled = false;
                btnForward.Enabled = false;
            }
            ModifyObject(square[m, m2]);
            this.Focus();
            BombCheck();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            /*all btns with directions in the name are used to control the character on the screen.
            They're all doing the same checking the same thing (if they're on a bomb, coal or diamond) with the only
            difference being that it's moving it in a different direction.*/
            if (m2 > 0)
            {
                m2--;
                ModifyObject(square[m, m2]);
                PrevObject(square[m, m2 + 1]);
            }
            CoalCheck();
            BombCheck();
            if (square[m, m2].bomb == true)
            {
                BombHit();
            }
        }

        public void CoalCheck()
        {
            //This is checking if the position of the player is on top of a block that can give them a point.
            if (square[m, m2].pointBlock == true)
            {
                pointValue = pointValue + 1;
                bal = bal + 1;
                square[m, m2].pointBlock = false;

            }
            else if (square[m, m2].diamondBlock == true)
            {
                pointValue = pointValue + 5;
                bal = bal + 5;
                square[m, m2].diamondBlock = false;

            }

            textBox3.Text = pointValue.ToString();
            textBox5.Text = bal.ToString();
            /*This is checking then if the score of the player is equal to the winning value.
              If it is, it'll update the level settings.*/
            if (pointValue >= winValue)
            {
                MessageBox.Show($"Level {curLevel.ToString()} Completed");
                diff = diff + 5;
                curLevel = curLevel + 1;
                dead = true;
                textBox4.Text = curLevel.ToString();
                btnStart.Enabled = true;
                rest();
            }

        }

        public void BombCheck()
        {
            int bombCount = 0;
            //this is the bomb checking statement to see if there is a bomb in a block around the player. If so, 1+ the counter.

            if (m == 24) { } else if (square[m + 1, m2].bomb == true) { bombCount = bombCount + 1; textBox1.Text = bombCount.ToString(); }
            if (m == 0) { } else if (square[m - 1, m2].bomb == true) { bombCount = bombCount + 1; textBox1.Text = bombCount.ToString(); }
            if (m2 == (GSize - 1)) { } else if (square[m, m2 + 1].bomb == true) { bombCount = bombCount + 1; textBox1.Text = bombCount.ToString(); }
            if (m2 == 0) { } else if (square[m, m2 - 1].bomb == true) { bombCount = bombCount + 1; textBox1.Text = bombCount.ToString(); }
            else textBox1.Text = (bombCount).ToString();


        }

        public void BombDisplay()
        {
            /*This gets activated when the player steps on a bomb.
              It loops through all of the blocks and sets the image on them to tnt if they're bool value is true. */
            for (int i = 0; i < GSize; i++)
            {
                for (int o = 0; o < GSize; o++)
                {
                    if (square[i, o].bomb == true)
                    {
                        square[i, o].Image = Properties.Resources.tnt;
                    }
                }
            }

        }

        static void ModifyObject(Square obj)
        {
            //setting the current position of the player to a player image and the background to black.
            obj.BackColor = Color.Black;
            obj.Image = Properties.Resources.rsz_player;
        }

        static void PrevObject(Square obj)
        {
            //this changes the image to make it a cobble texture. This removes the colour and player image behind the player.
            obj.Image = Properties.Resources.cobble;
        }

        private void btnBackwards_Click(object sender, EventArgs e)
        {

            if (m < (GSize - 1))
            {
                m++;
                ModifyObject(square[m, m2]);
                PrevObject(square[m - 1, m2]);
            }
            CoalCheck();
            BombCheck();
            if (square[m, m2].bomb == true)
            {
                BombHit();
            }

        }

        private void btnForward_Click(object sender, EventArgs e)
        {

            if (m > 0)
            {
                m--;
                ModifyObject(square[m, m2]);
                PrevObject(square[m + 1, m2]);
            }
            CoalCheck();
            BombCheck();
            if (square[m, m2].bomb == true)
            {
                BombHit();
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {

            if (m2 < (GSize - 1))
            {
                m2++;
                ModifyObject(square[m, m2]);
                PrevObject(square[m, m2 - 1]);
            }

            CoalCheck();
            BombCheck();
            if (square[m, m2].bomb == true)
            {
                BombHit();
            }

        }

        public void BombHit()
        {
            /*this is called when a bomb is hit. 
              This disables controls and sets the difficult back to default. It also plays an explosion sound*/
            if (lives != 0)
            {
                lives--;
                SoundPlayer oof = new System.Media.SoundPlayer(Properties.Resources.oof);
                oof.Play();
                textBox7.Text = lives.ToString();
                square[m, m2].bomb = false;
            }
            else if (lives == 0)
            {
                textBox1.Text = "Dead";
                dead = true;
                diff = 0;
                btnBackwards.Enabled = false;
                btnLeft.Enabled = false;
                btnRight.Enabled = false;
                btnForward.Enabled = false;
                BombDisplay();
                bal = 0;
                token = 0;
                lives = 0;
                this.BackColor = Color.Red;
                SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.minecraft_tnt_explosion_sound_effect);
                player.Play();
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //This is checking what keys are being pressed. If it is one of the follow it'll execute the button related to it.
            if (dead == true)
            {

                if (e.KeyCode == Keys.R)
                {
                    BtnReset_Click(null, null);
                }
            }
            else if (dead == false)
            {
                switch (e.KeyCode)
                {
                    case (Keys.W):
                        btnForward_Click(null, null);
                        break;
                    case (Keys.S):
                        btnBackwards_Click(null, null);
                        break;
                    case (Keys.A):
                        btnLeft_Click(null, null);
                        break;
                    case (Keys.D):
                        btnRight_Click(null, null);
                        break;
                    case (Keys.R):
                        BtnReset_Click(null, null);
                        break;
                }
            }

        }

        private void rest()
        {
            this.BackColor = Color.FromName("Control");
            for (int o = 0; o < GSize; o++)
            {
                x = 0;
                for (int i = 0; i < GSize; i++)
                {
                    y = 0;
                    this.panel1.Controls.Remove(square[o, i]);
                }
                
            }
            gameReset();

        }

        private void gameReset()
        {
            //this is setting all of the values back to default. It is called when the player dies.
            m = GSize - 1;
            m2 = GSize / 2;
            pointValue = 0;
            if (diff > 20) //Prevents the game crashing when the difficulty gets above 10.
            {
                MessageBox.Show("Congrats, you've completed the game!");
                diff = 0;
                trackBar1.Value = diff / 2;
                BtnReset_Click(null, null);
            }
            else
            {
                trackBar1.Value = diff / 2;
            }
            btnForward.Enabled = false;
            btnBackwards.Enabled = false;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
            textBox1.Text = "";
            btnBomb.Enabled = true;
            dead = true;
            panel2.BackColor = Color.Gray;
            Form1_Load(null, null);
            textBox2.Text = trackBar1.Value.ToString();
            textBox5.Text = bal.ToString();
            textBox6.Text = token.ToString();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            //when the button on the form called "reset" is pressed. It'll set the level, points and winVal to default.
            //it also executes rest.
            curLevel = 1;
            pointValue = 0;
            winValue = 15;
            bal = 0;
            token = 0;
            lives = 0;
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            btnStart.Enabled = true;
            rest();
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            //updates the difficulty and changes the text in a box.
            textBox2.Text = trackBar1.Value.ToString();
            diff = trackBar1.Value * 2;
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            //Creates and displays the form called "help" to display helpful information to the player.
            help help = new help();
            help.ShowDialog();

        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            dead = false;
            btnStart.Enabled = false;
            btnBackwards.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
            btnForward.Enabled = true;
            panel2.BackColor = Color.Green;
        }

        private void BtnPointBoost_Click(object sender, EventArgs e)
        {
            /*This is the same for all shop items. It checks to see how much is in the balance. If it's enough
              it'll allow the player to purchase the item removing the respective amount from the balance*/
            if (bal >= 5)
            {
                token++;
                textBox6.Text = token.ToString();
                bal = bal - 5;
                textBox5.Text = bal.ToString();
            }
            else if (bal < 5)
            {
                MessageBox.Show("You don't have enough to purchase this");
            }

        }



        public void Square_Click(object sender, EventArgs e)
        {
            Square s = (Square)sender;

            if (token == 0) //This is checking to see what the balance is. If it's 0 it'll give them an error
            {
                MessageBox.Show($"Tokens Avaliable {token.ToString()}");
            }
            else if (token != 0) //If they have tokens for bombs it'll let the player know there's a bomb
            {
                MessageBox.Show($"Bomb {s.bomb.ToString()}");
                token--;
                textBox6.Text = token.ToString();
            }

        }

        private void BtnLife_Click(object sender, EventArgs e)
        {
            if (bal >= 10)
            {
                if (lives == 3)
                {
                    MessageBox.Show("You have the maximum amount of lives");
                }
                else if (lives != 3)
                {
                    lives++;
                    bal = bal - 10;
                    textBox7.Text = lives.ToString();
                    textBox5.Text = bal.ToString();
                }
            }
            else if (bal < 10)
            {
                MessageBox.Show("You don't have enough to purchase this");
            }
        }

        private void BtnBomb_Click(object sender, EventArgs e)
        {
            if (bal >= 20)
            {
                BombDisplay();
                bal = bal - 20;
                btnBomb.Enabled = false;
            }
            else if (bal < 20)
            {
                MessageBox.Show("You don't have enough to purchase this");
            }
        }
    }
}