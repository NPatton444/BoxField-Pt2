using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys 
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, bDown, nDown, mDown, spaceDown;

        //used to draw boxes on screen
        SolidBrush boxBrush = new SolidBrush(Color.White);

        List<Cube> cubesLeft = new List<Cube>();
        List<Cube> cubesRight = new List<Cube>();

        int leftStartX = 300;
        int gap = 300;
        
        Pen cubePen = new Pen(Color.White, 3);

        SolidBrush whiteBrush = new SolidBrush(Color.Orange);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        SolidBrush[] colors;

        int newCubeCounter = 4;
        int cubeSpeed = 7;
        int cubeSize = 30;
        int patternDirection = 0;  // 0 = left, 1 = right
        int patternLength = 4;
        int xChange = 7;

        Random rand = new Random();

        Character ch = new Character(500, 400, 30, 4);

        public GameScreen()
        {
            InitializeComponent();

            colors = new SolidBrush[] { whiteBrush, redBrush, yellowBrush };
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            this.Focus();

            Cube c = new Cube(leftStartX, 0, cubeSize, cubeSpeed, rand.Next(0, 3));
            cubesLeft.Add(c);

            c = new Cube(leftStartX + gap, 0, cubeSize, cubeSpeed, rand.Next(0, 3));
            cubesRight.Add(c);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.B:
                    bDown = true;
                    break;
                case Keys.N:
                    nDown = true;
                    break;
                case Keys.M:
                    mDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                default:
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.B:
                    bDown = false;
                    break;
                case Keys.N:
                    nDown = false;
                    break;
                case Keys.M:
                    mDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                default:
                    break;
            }
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            #region create new cube if it is time

            if (newCubeCounter == 0)  // if time to create a new cube
            {
                if (patternLength != 0)  // if pattern is not done continue in same direction.
                {
                    if (patternDirection == 1)  // if moving left set start position of new cube to left of top most cube
                    {
                        leftStartX = cubesLeft[cubesLeft.Count() - 1].x - xChange;
                    }
                    else  // else set start position of new cube to right of top most cube
                    {
                        leftStartX = cubesLeft[cubesLeft.Count() - 1].x + xChange;
                    }
                }
                else // change direction and create new pattern length and new left right move amount
                {
                    patternLength = rand.Next(5, 15);
                    xChange = rand.Next(10, 20);

                    if (patternDirection == 0)
                    {
                        patternDirection = 1;
                        leftStartX = cubesLeft[cubesLeft.Count() - 1].x - xChange;
                    }
                    else
                    {
                        patternDirection = 0;
                        leftStartX = cubesLeft[cubesLeft.Count() - 1].x + xChange;
                    }
                }

                // create left and right cubes
                Cube c = new Cube(leftStartX, 0, cubeSize, cubeSpeed, rand.Next(0, 3));
                cubesLeft.Add(c);

                c = new Cube(leftStartX + gap, 0, cubeSize, cubeSpeed, rand.Next(0, 3));
                cubesRight.Add(c);

                newCubeCounter = 4;
                patternLength--;
            }
            else
            {
                newCubeCounter--;
            }
            
            #endregion

            #region update position of each cube

            foreach (Cube c in cubesLeft)
            {
                c.y += c.speed;
            }

            foreach (Cube c in cubesRight)
            {
                c.y += c.speed;
            }
           
            #endregion

            #region Remove cubes from list that have gone off the screen
            
            if (cubesLeft[0].y > this.Height)
            {
                cubesLeft.RemoveAt(0);
                cubesRight.RemoveAt(0);
            }

            #endregion

            #region Character Move

            if(leftArrowDown)
            {
                ch.move(ch, "left");
            }
            else if(rightArrowDown)
            {
                ch.move(ch, "right");
            }

            #endregion

            foreach (Cube c in cubesLeft)
            {
                if (ch.collision(ch, c))
                {
                    gameLoop.Stop();

                    Form f = this.FindForm();
                    f.Controls.Remove(this);
                    GameOver go = new GameOver();
                    f.Controls.Add(go);

                    break;
                }
            }

            foreach (Cube c in cubesRight)
            {
                if (ch.collision(ch, c))
                {
                    gameLoop.Stop();

                    Form f = this.FindForm();
                    f.Controls.Remove(this);
                    GameOver go = new GameOver();
                    f.Controls.Add(go);            
                    
                    break;
                }
            }

            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            foreach (Cube c in cubesLeft)
            {
                e.Graphics.DrawRectangle(cubePen, c.x, c.y, c.size, c.size);
                e.Graphics.FillRectangle(colors[c.colour], c.x, c.y, c.size, c.size);
            }

            foreach (Cube c in cubesRight)
            {
                e.Graphics.DrawRectangle(cubePen, c.x, c.y, c.size, c.size);
                e.Graphics.FillRectangle(colors[c.colour], c.x, c.y, c.size, c.size);
            }

            e.Graphics.DrawEllipse(cubePen, ch.x, ch.y, ch.size, ch.size);
        }
    }
}
