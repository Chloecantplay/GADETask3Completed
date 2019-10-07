using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GADETask1
{
    [Serializable]

    public partial class Form1 : Form
    {
        GameEngine engine;

        public Form1()
        {
            InitializeComponent();
        }

        private void RoundLabel_Click(object sender, EventArgs e)
        {

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RoundLabel.Text = "Round: " + engine.Round.ToString();
            engine.Update();
            Display();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            Timer.Enabled = true;
            engine = new GameEngine(20, MapBox, Convert.ToInt32(WidthInput.Text), Convert.ToInt32(HeightInput.Text));// this allows for the size of the map to be passed to the game engine
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            Timer.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            engine.Save();
        }

        private void Readbutton_Click(object sender, EventArgs e)
        {
            engine.Load();
            engine.Update();
            Display();
        }
        public void Display()// the display method was moved here so that the save function could work
        {
            Form1 form = new Form1();
            MapBox.Controls.Clear();
            foreach (Unit u in engine.map.Units)
            {
                Button b = new Button();
                if (u is MeleeUnit)
                {
                    MeleeUnit mu = (MeleeUnit)u;
                    b.Size = new Size(20, 20);
                    b.Location = new Point(mu.xPos * 20, mu.yPos * 20);
                    b.Text = mu.symbol;
                    if (mu.team == 0)
                    {
                        b.ForeColor = Color.Red;
                    }
                    else
                    {
                        b.ForeColor = Color.Green;
                    }
                }
                else if(u is RangedUnit)
                {
                    RangedUnit ru = (RangedUnit)u;
                    b.Size = new Size(20, 20);
                    b.Location = new Point(ru.xPos * 20, ru.yPos * 20);
                    b.Text = ru.symbol;
                    if (ru.team == 0)
                    {
                        b.ForeColor = Color.Red;
                    }
                    else
                    {
                        b.ForeColor = Color.Green;
                    }
                }
                else if (u is WizzardUnit)// wizzards were added to the list
                {
                    WizzardUnit wu = (WizzardUnit)u;
                    b.Size = new Size(20, 20);
                    b.Location = new Point(wu.xPos * 20, wu.yPos * 20);
                    b.Text = wu.symbol;
                    if (wu.team == 0)
                    {
                        b.ForeColor = Color.Red;
                    }
                    else if (wu.team==2)
                    {
                        b.ForeColor = Color.Yellow;
                    }
                    else
                    {
                        b.ForeColor = Color.Green;
                    }
                }
                b.Click += Unit_Click;
                MapBox.Controls.Add(b);
            }
            foreach (Building b in engine.map.Buildings)
            {
                Button bn = new Button();
                if (b is ResourceBuilding)
                {
                    ResourceBuilding r = (ResourceBuilding)b;

                    bn.Size = new Size(20, 20);
                    bn.Location = new Point(r.xPos * 20, r.yPos * 20);
                    bn.Text = r.symbol;

                    if (r.team == 0)
                    {
                        bn.ForeColor = Color.Red;
                    }
                    else
                    {
                        bn.ForeColor = Color.Blue;
                    }
                }
                else
                {
                    FactoryBuilding f = (FactoryBuilding)b;

                    bn.Size = new Size(20, 20);
                    bn.Location = new Point(f.xPos * 20, f.yPos * 20);
                    bn.Text = f.symbol;

                    if (f.team == 0)
                    {
                        bn.ForeColor = Color.Red;
                    }
                    else
                    {
                        bn.ForeColor = Color.Blue;
                    }
                }
                bn.Click += Building_Click;
                MapBox.Controls.Add(bn);
            }
        }

        public void Unit_Click(object sender, EventArgs e)// all units can be selected and their stats will show
        {
            int x, y;
            Button b = (Button)sender;
            x = b.Location.X / 20;
            y = b.Location.Y / 20;
            foreach (Unit u in engine.map.Units)
            {
                if (u is RangedUnit)
                {
                    RangedUnit ru = (RangedUnit)u;
                    if (ru.xPos == x && ru.yPos == y)
                    {
                        UnitInfoDisplay.Text = "";
                        UnitInfoDisplay.Text = ru.Info();
                    }
                }
                else if (u is MeleeUnit)
                {
                    MeleeUnit mu = (MeleeUnit)u;
                    if (mu.xPos == x && mu.yPos == y)
                    {
                        UnitInfoDisplay.Text = "";
                        UnitInfoDisplay.Text = mu.Info();
                    }
                }
                else if (u is WizzardUnit)
                {
                    WizzardUnit wu = (WizzardUnit)u;
                    if (wu.xPos == x && wu.yPos == y)
                    {
                        UnitInfoDisplay.Text = "";
                        UnitInfoDisplay.Text = wu.Info();
                    }
                }
            }


        }
        public void Building_Click(object sender, EventArgs e)// both buildings can be selected
        {
            int x, y;
            Button b = (Button)sender;
            x = b.Location.X / 20;
            y = b.Location.Y / 20;
            foreach (Building bl in engine.map.Buildings)
            {
                if (bl is ResourceBuilding)// the resource building takes very long to show its stats but eventually it does
                {
                    ResourceBuilding r = (ResourceBuilding)bl;
                    if (r.xPos == x && r.xPos == y)
                    {
                        UnitInfoDisplay.Text = "";
                        UnitInfoDisplay.Text = r.Info();
                    }
                }
                else if (bl is FactoryBuilding)
                {
                    FactoryBuilding f = (FactoryBuilding)bl;
                    if (f.xPos == x && f.yPos == y)
                    {
                        UnitInfoDisplay.Text = "";
                        UnitInfoDisplay.Text = f.Info();
                    }
                }
            }
        }

        private void MapBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
