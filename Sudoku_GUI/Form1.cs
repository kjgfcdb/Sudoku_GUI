using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sudoku_GUI
{
    public partial class Form1 : Form
    {
        private int choosen = 0;
        private Color activatedBtn = Color.LightBlue;
        private Color inativatedBtn = Color.FromArgb(250, 162, 173);
        private int[,] map, visited;
        private bool[,] checkrow, checkcol, checksqr;
        private int fill;
        private Sudoku.Sudoku creater = new Sudoku.Sudoku();
        public void InitMap()
        {
            visited = new int[9, 9];
            fill = 0;
            checkrow = new bool[9, 10];
            checkcol = new bool[9, 10];
            checksqr = new bool[9, 10];
            int temp;
            for (int i=0;i<9;i++)
            {
                for (int j=0;j<9;j++)
                {
                    buttons[i * 9 + j].ForeColor = Color.Black;
                    if (map[i, j] != 0)
                    {
                        fill++;
                        temp = map[i, j];
                        visited[i, j] = 1;
                        buttons[i * 9 + j].ForeColor = Color.DeepPink;
                        checkrow[i, temp] = checkcol[j, temp] = checksqr[i - i % 3 + j / 3, temp] = true;
                    }
                }
            }
        }
        public Form1()
        {
            map = creater.GeneratePuzzle();
            SuspendLayout();
            buttons = new Button[81];
            numButtons = new Button[10];
            for (int i = 0; i < 81; i++)
            {
                int dx = i / 9;
                int dy = i % 9;
                buttons[i] = new Button
                {
                    Font = new Font("Monaco", 11F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    Margin = new Padding(0),
                    Name =  i.ToString(),
                    Size = new Size(50, 50),
                    TabIndex = i,
                    UseVisualStyleBackColor = true,
                    Location = new Point(150 + dy * 50, 110 + dx * 50),
                    BackColor = Color.FromArgb(246, 246, 246),
                };
                if ((dx >= 3 && dx <= 5) || (dy >= 3 && dy <= 5))
                    if (!(dx >= 3 && dx <= 5 && dy >= 3 && dy <= 5))
                        buttons[i].BackColor = Color.FromArgb(40,176,181);
                buttons[i].Click += new EventHandler(Change);
                Controls.Add(buttons[i]);
                if (map[dx, dy] != 0)
                    buttons[i].Text = map[dx, dy].ToString();
            }
            for (int i=1;i<=9;i++)
            {
                numButtons[i] = new Button
                {
                    Location = new Point(80 * i - 50, 635),
                    Margin = new Padding(0),
                    Name = "Num" + i.ToString(),
                    Size = new Size(50, 50),
                    Text = i.ToString(),
                    UseVisualStyleBackColor = true,
                    Font = new Font("Monaco", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                };
                numButtons[i].Click += new System.EventHandler(this.NumBtnClick);
                Controls.Add(this.numButtons[i]);
                numButtons[i].BackColor = inativatedBtn;

            }
            InitializeComponent();
            InitMap();
            ResumeLayout(false);
        }
        private void Change(object sender,EventArgs e)
        {
            Button btn = (Button)sender;
            int index = int.Parse(btn.Name);
            int dx = index / 9;
            int dy = index % 9;
            if (visited[dx, dy] == 1) return;
            if (fill==81)
            {
                MessageBox.Show("Sudoku Already Solved.");
                return;
            }
            if (choosen!=0)
            {
                if (!checkrow[dx, choosen] && !checkcol[dy, choosen] && !checksqr[dx - dx % 3 + dy / 3, choosen])
                {
                    btn.Text = choosen.ToString();
                    map[dx, dy] = choosen;
                    checkrow[dx, choosen] = checkcol[dy, choosen] = checksqr[dx - dx % 3 + dy / 3, choosen] = true;
                    fill++;
                    if (fill == 81) MessageBox.Show("Sudoku Solved!");
                } else if (choosen==map[dx,dy]) {
                }
                else
                {
                    string warning = "";
                    if (checkrow[dx, choosen])
                        warning += "Number conflicts at row " + (dx + 1) + " \n";
                    if (checkcol[dy, choosen])
                        warning += "Number conflicts at column " + (dy + 1) + " \n";
                    if (checksqr[dx - dx % 3 + dy / 3, choosen])
                        warning += "Number conflicts at square " + (dx - dx % 3 + dy / 3 + 1) + " \n";
                    if (warning.Length != 0)
                        MessageBox.Show(warning);
                }
            } else
            {
                int foo = map[dx, dy];
                checkrow[dx, foo] = checkcol[dy, foo] = checksqr[dx - dx % 3 + dy / 3, foo] = false;
                map[dx, dy] = 0;
                btn.Text = "";
                fill--;
            }
        }
        private void NumBtnClick(object sender,EventArgs e)
        {
            Button btn = (Button)sender;
            for (int i = 1; i <= 9; i++)
                if (numButtons[i].BackColor != inativatedBtn)
                    numButtons[i].BackColor = inativatedBtn;
            choosen = int.Parse(btn.Text);
            numButtons[choosen].BackColor = activatedBtn;
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            choosen = 0;
            for (int i = 1; i <= 9; i++)
                if (numButtons[i].BackColor != inativatedBtn)
                    numButtons[i].BackColor = inativatedBtn;
        }
        private void Form1_DoubleClick(object sender,EventArgs e)
        {
            map = creater.GeneratePuzzle();
            for (int i = 0; i < 81; i++)
            {
                int dx = i / 9;
                int dy = i % 9; ;
                if (map[dx, dy] != 0)
                    buttons[i].Text = map[dx, dy].ToString();
                else
                    buttons[i].Text = "";
            }
            InitMap();
        }
    }
}
