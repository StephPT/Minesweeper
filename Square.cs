using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Miner_Game
{
    class Square : Label
    {
        //These are allowing the Square class to hold these values inside itself and be interacted with outside of the class. 
        public bool bomb;
        public bool pointBlock;
        public bool diamondBlock;
        public int bombRemaining;
        
        public Square(int x, int y, Color colour, bool Bomb, bool pointBlock, bool diamondBlock, int tokens)
        {

            //Controls for creating the label on the display.
            //All of these are properties of a label. They're being passed to the constructor ready for form1 to build squres.
            this.AutoSize = false;
            this.Location = new System.Drawing.Point(x, y);
            this.Size = new System.Drawing.Size(25, 25);
            this.TabIndex = 0;
            this.BackColor = colour;
            this.Image = Properties.Resources.stone;
            //These are the boolean values to allow the square to know if it's more then a normal square.
            this.bomb = Bomb;
            this.pointBlock = pointBlock;
            this.diamondBlock = diamondBlock;
            //Click to check if there is a bomb
            this.bombRemaining = tokens;
        }
    }
}
