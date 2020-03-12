using System;
using System.Drawing;
using System.Windows.Forms;

namespace MazeGenerator_GUI {
    public class MazeGUI : Form {

        MazeGenerator mg = null;

        Graphics gp = null;
        Pen pen = null;

        TextBox tbxRow = null;
        TextBox tbxColumn = null;

        int numRow = 0, numCol = 0;

        public MazeGUI() {
            this.GUISetting();

            pen = new Pen(Color.Blue, 2);

            numRow = 10;
            numCol = 10;
            mg = new MazeGenerator(numRow, numCol);
        }

        public void GUISetting() {
            this.Width = 1000;
            this.Height = 600;

            GroupBox gbx = new GroupBox();
            gbx.Width = this.Width * 95 / 100;
            gbx.Height = this.Height / 10;
            gbx.Left = (this.ClientSize.Width - gbx.Width) / 2;
            gbx.Top = 10;
            gbx.Text = "Maze Generator";

            Label labelRow = new Label();
            tbxRow = new TextBox();

            labelRow.Text = "Row";
            labelRow.BorderStyle = BorderStyle.FixedSingle;
            labelRow.Width = 50;
            labelRow.Left = 10;
            labelRow.Top = (gbx.Height - labelRow.Height) / 2;
            labelRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            tbxRow.Text = "10";
            tbxRow.Width = 50;
            tbxRow.Left = 10+ labelRow.Left + labelRow.Width;
            tbxRow.Top = (gbx.Height - tbxRow.Height) / 2;

            Label labelColumn = new Label();
            tbxColumn = new TextBox();

            labelColumn.Text = "Column";
            labelColumn.BorderStyle = BorderStyle.FixedSingle;
            labelColumn.Width = 60;
            labelColumn.Left = 20 + tbxRow.Left + tbxRow.Width;
            labelColumn.Top = (gbx.Height - labelColumn.Height) / 2;
            labelColumn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            tbxColumn.Text = "10";
            tbxColumn.Width = 50;
            tbxColumn.Left = 10 + labelColumn.Left + labelColumn.Width;
            tbxColumn.Top = (gbx.Height - tbxColumn.Height) / 2;

            Button btnChange = new Button();
            btnChange.Text = "Change";
            btnChange.Left = 15 + tbxColumn.Left + tbxColumn.Width;
            btnChange.Top = (gbx.Height - btnChange.Height) / 2;

            btnChange.Click += ChangeSetting;

            Button btnGen = new Button();

            btnGen.Text = "Generate Maze";
            btnGen.Width = 100;
            btnGen.Left = 40 + btnChange.Left + btnChange.Width;
            btnGen.Top = (gbx.Height - btnGen.Height) / 2;

            btnGen.Click += GenerateMaze;

            Button btnPrint = new Button();

            btnPrint.Text = "Print Maze";
            btnPrint.Width = 80;
            btnPrint.Left = 10 + btnGen.Left + btnGen.Width;
            btnPrint.Top = (gbx.Height - btnPrint.Height) / 2;

            btnPrint.Click += PrintMaze;

            TextBox tbxMaze = new TextBox();

            tbxMaze.Multiline = true;
            tbxMaze.Width = this.Width * 95 / 100;
            tbxMaze.Height = this.Height * 78 / 100;
            tbxMaze.Left = gbx.Left;
            tbxMaze.Top = 10 + gbx.Top + gbx.Height;

            gp = tbxMaze.CreateGraphics();

            gbx.Controls.Add(labelRow);
            gbx.Controls.Add(tbxRow);

            gbx.Controls.Add(labelColumn);
            gbx.Controls.Add(tbxColumn);

            gbx.Controls.Add(btnChange);

            gbx.Controls.Add(btnGen);
            gbx.Controls.Add(btnPrint);

            this.Controls.Add(gbx);

            this.Controls.Add(tbxMaze);
        }

        public void GenerateMaze(object sender, EventArgs e) {
            mg.GenerateMaze();
        }

        public void PrintMaze(object sender, EventArgs e) {
            mg.PrintMaze(gp, pen);
        }

        public void ChangeSetting(object sender, EventArgs e) {
            numRow = Convert.ToInt32(tbxRow.Text);
            numCol = Convert.ToInt32(tbxColumn.Text);

            mg.Init(numRow, numCol);
        }
    
    }
}
