using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace jr01
{
    public partial class jr01 : Form
    {
        private int[] slidePositions;
        private Boolean[,] slideHoles;
        private Boolean lampV;
        private Boolean lampX;
        private Boolean lampY;
        private Boolean lampZ;
        private Boolean inputPressed;
        private int patchCord;
        private int[] patchStart;
        private int[] patchEnd;
        private int numPatches;

        public jr01()
        {
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            InitializeComponent();
            slidePositions = new int[3];
            slideHoles = new Boolean[3,14];
            patchStart = new int[16];
            patchEnd = new int[16];
        }

        private void newProgram()
        {
            int x, y;
            for (y = 0; y < 3; y++)
                for (x = 0; x < 14; x++) slideHoles[y, x] = false;
            slidePositions[0] = 0;
            slidePositions[1] = 0;
            slidePositions[2] = 0;
            lampV = false;
            lampX = false;
            lampY = false;
            lampZ = false;
            inputPressed = false;
            patchCord = -1;
            numPatches = 0;
            drawPanel();
            drawLights();
            drawSlide(0);
            drawSlide(1);
            drawSlide(2);
            drawPatchCords();
        }

        private void jr01_Load(object sender, EventArgs e)
        {
            mainPanel.Image = new Bitmap(645, 422);
            newProgram();
        }

        private void drawPanel()
        {
            Graphics gc;
            Pen pn;
            Brush br;
            int i;
            Font fnt;
            gc = Graphics.FromImage(mainPanel.Image);
            br = new SolidBrush(Color.LightSeaGreen);
            gc.FillRectangle(br, 0, 0, mainPanel.Width, mainPanel.Height);
            pn = new Pen(Color.LightCyan);
            br = new SolidBrush(Color.LightCyan);
            fnt = new Font("Courier New", 12);
            for (i = 1; i < 8; i++)
            {
                gc.DrawLine(pn, 75 * i, 40, 75 * i, 230);
                gc.DrawString(i.ToString(), fnt, br, 75*i-8, 24);
                gc.DrawString(i.ToString(), fnt, br, 75 * i - 8, 230);
            }
            gc.DrawLine(pn, 93, 360, 93, 367);
            gc.DrawLine(pn, 150, 360, 150, 367);
            gc.DrawLine(pn, 300, 360, 300, 367);
            gc.DrawLine(pn, 450, 360, 450, 367);
            gc.DrawLine(pn, 93, 338, 93, 346);
            gc.DrawLine(pn, 150, 338, 150, 346);
            gc.DrawLine(pn, 300, 338, 300, 346);
            gc.DrawLine(pn, 450, 338, 450, 346);
            gc.DrawString("A", fnt, br, 7, 55);
            gc.DrawString("B", fnt, br, 7, 125);
            gc.DrawString("C", fnt, br, 7, 195);
            gc.DrawString("V", fnt, br, 85, 343);
            gc.DrawString("X", fnt, br, 142, 343);
            gc.DrawString("Y", fnt, br, 292, 343);
            gc.DrawString("Z", fnt, br, 442, 343);
            fnt.Dispose();
            fnt = new Font("Courier New", 10);
            gc.DrawString("0 1  (4)", fnt, br, 560, 35);
            gc.DrawString("0 1  (2)", fnt, br, 560, 105);
            gc.DrawString("0 1  (1)", fnt, br, 560, 175);
            gc.DrawString("(8)", fnt, br, 78, 395);
            gc.DrawString("(4)", fnt, br, 135, 395);
            gc.DrawString("(2)", fnt, br, 285, 395);
            gc.DrawString("(1)", fnt, br, 435, 395);
            br.Dispose();
            br = new SolidBrush(Color.Gray);
            for (i = 0; i < 7; i++)
            {
                gc.FillEllipse(br, 68+(i*75), 255, 14, 14);
                gc.DrawLine(pn, 75 + (i * 75), 246, 75 + (i * 75), 253);
            }
            gc.FillEllipse(br, 86, 321, 14, 14);
            gc.FillEllipse(br, 143, 321, 14, 14);
            gc.FillEllipse(br, 293, 321, 14, 14);
            gc.FillEllipse(br, 443, 321, 14, 14);
            br.Dispose();
            br = new SolidBrush(Color.Black);
            for (i = 0; i < 7; i++)
            {
                gc.FillEllipse(br, 70 + (i * 75), 257, 10, 10);
            }
            gc.FillEllipse(br, 88, 323, 10, 10);
            gc.FillEllipse(br, 145, 323, 10, 10);
            gc.FillEllipse(br, 295, 323, 10, 10);
            gc.FillEllipse(br, 445, 323, 10, 10);
            br.Dispose();
            br = new SolidBrush(Color.White);
            gc.FillEllipse(br, 25, 340, 25, 25);
            br.Dispose();
            br = new SolidBrush(Color.LightGray);
            gc.FillEllipse(br, 30, 345, 15, 15);
            br.Dispose();
            fnt.Dispose();
            pn.Dispose();
            gc.Dispose();
        }

        private void drawLights()
        {
            Graphics gc;
            Brush br;
            gc = Graphics.FromImage(mainPanel.Image);
            br = new SolidBrush(Color.DarkRed);
            gc.FillEllipse(br, 80, 370, 25, 25);
            br.Dispose();
            br = new SolidBrush(Color.DarkGreen);
            gc.FillEllipse(br, 137, 370, 25, 25);
            gc.FillEllipse(br, 287, 370, 25, 25);
            gc.FillEllipse(br, 437, 370, 25, 25);
            br.Dispose();
            br = new SolidBrush(Color.Red);
            if (lampV) gc.FillEllipse(br, 83, 373, 19, 19);
            br.Dispose();
            br = new SolidBrush(Color.LightGreen);
            if (lampX) gc.FillEllipse(br, 140, 373, 19, 19);
            if (lampY) gc.FillEllipse(br, 290, 373, 19, 19);
            if (lampZ) gc.FillEllipse(br, 440, 373, 19, 19);
            br.Dispose();
            gc.Dispose();
        }

        private void drawSlide(int num)
        {
            Graphics gc;
            Brush br1;
            Brush br2;
            Pen pn;
            int baseX;
            int baseY;
            int i;
            baseX = 30 + (20 * slidePositions[num]);
            baseY = 50 + 70 * num;
            gc = Graphics.FromImage(mainPanel.Image);
            br1 = new SolidBrush(Color.LightSeaGreen);
            gc.FillRectangle(br1, 30, baseY, 580, 30);
            br1.Dispose();
            br1 = new SolidBrush(Color.FromArgb(0, 0, 120));
            gc.FillRectangle(br1, baseX, baseY, 550, 30);
            br1.Dispose();
            br1 = new SolidBrush(Color.Black);
            br2 = new SolidBrush(Color.Gray);
            pn = new Pen(Color.FromArgb(0,0,200));
            pn.Width = 2;
            for (i = 0; i < 7; i++)
            {
                if (slideHoles[num,i*2])
                    gc.FillEllipse(br2, baseX + 20 + (75 * i), baseY + 9, 12, 12);
                else
                    gc.FillEllipse(br1, baseX + 20 + (75 * i), baseY + 10, 10, 10);
                if (slideHoles[num,i*2+1])
                    gc.FillEllipse(br2, baseX + 40 + (75 * i), baseY + 9, 12, 12);
                else
                    gc.FillEllipse(br1, baseX + 40 + (75 * i), baseY + 10, 10, 10);
                gc.DrawLine(pn, baseX + 35 + (75 * i), baseY + 5, baseX + 35 + (75 * i), baseY + 25);
                gc.DrawLine(pn, baseX + 42 + (75 * i), baseY + 7, baseX + 50 + (75 * i), baseY + 7);
            }
            for (i = 0; i < 10; i++)
                gc.DrawLine(pn, baseX + 520 + (i * 3), baseY + 15, baseX + 520 + (i * 3), baseY + 29);
            gc.DrawLine(pn, baseX + 520 + 15, baseY + 2, baseX + 520 + 15, baseY + 29);
            br1.Dispose();
            pn.Dispose();
            gc.Dispose();
        }

        private void mainPanel_Click(object sender, EventArgs e)
        {
            int i;
            Point pos;
            pos = PointToClient(Control.MousePosition);
            if (pos.Y >= 62 && pos.Y < 92)
            {
                if ((pos.X >= 560 + slidePositions[0] * 20) &&
                    (pos.X <= 592 + slidePositions[0] * 20))
                  slidePositions[0] = (slidePositions[0] == 0) ? 1 : 0;
                for (i = 0; i < 7; i++)
                {
                    if ((pos.X >= 62 + slidePositions[0] * 20 + i * 75) &&
                        (pos.X < 77 + slidePositions[0] * 20 + i * 75))
                        slideHoles[0, i * 2] = (slideHoles[0, i * 2]) ? false : true;
                    if ((pos.X > 77 + slidePositions[0] * 20 + i * 75) &&
                        (pos.X <= 92 + slidePositions[0] * 20 + i * 75))
                        slideHoles[0, i * 2 + 1] = (slideHoles[0, i * 2 + 1]) ? false : true;
                }
                  
                drawSlide(0);
                mainPanel.Invalidate();
            }
            if (pos.Y >= 132 && pos.Y < 162)
            {
                if ((pos.X >= 560 + slidePositions[1] * 20) &&
                    (pos.X <= 592 + slidePositions[1] * 20))
                    slidePositions[1] = (slidePositions[1] == 0) ? 1 : 0;
                for (i = 0; i < 7; i++)
                {
                    if ((pos.X >= 62 + slidePositions[1] * 20 + i * 75) &&
                        (pos.X < 77 + slidePositions[1] * 20 + i * 75))
                        slideHoles[1, i * 2] = (slideHoles[1, i * 2]) ? false : true;
                    if ((pos.X > 77 + slidePositions[1] * 20 + i * 75) &&
                        (pos.X <= 92 + slidePositions[1] * 20 + i * 75))
                        slideHoles[1, i * 2 + 1] = (slideHoles[1, i * 2 + 1]) ? false : true;
                }
                drawSlide(1);
                mainPanel.Invalidate();
            }
            if (pos.Y >= 202 && pos.Y < 232)
            {
                if ((pos.X >= 560 + slidePositions[2] * 20) &&
                    (pos.X <= 592 + slidePositions[2] * 20))
                    slidePositions[2] = (slidePositions[2] == 0) ? 1 : 0;
                for (i = 0; i < 7; i++)
                {
                    if ((pos.X >= 62 + slidePositions[2] * 20 + i * 75) &&
                        (pos.X < 77 + slidePositions[2] * 20 + i * 75))
                        slideHoles[2, i * 2] = (slideHoles[2, i * 2]) ? false : true;
                    if ((pos.X > 77 + slidePositions[2] * 20 + i * 75) &&
                        (pos.X <= 92 + slidePositions[2] * 20 + i * 75))
                        slideHoles[2, i * 2 + 1] = (slideHoles[2, i * 2 + 1]) ? false : true;
                }
                drawSlide(2);
                mainPanel.Invalidate();
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            newProgram();
            Text = "JR01";
            mainPanel.Invalidate();
        }

        private void performMatrix()
        {
            Boolean[] registers = new Boolean[7];
            int i,j;
            lampV = false;
            lampX = false;
            lampY = false;
            lampZ = false;
            for (i = 0; i < 7; i++)
            {
                registers[i] = false;
                if (((slidePositions[0] == 0 && slideHoles[0, i * 2 + 1]) ||
                     (slidePositions[0] == 1 && slideHoles[0, i * 2])) &&
                    ((slidePositions[1] == 0 && slideHoles[1, i * 2 + 1]) ||
                     (slidePositions[1] == 1 && slideHoles[1, i * 2])) &&
                    ((slidePositions[2] == 0 && slideHoles[2, i * 2 + 1]) ||
                     (slidePositions[2] == 1 && slideHoles[2, i * 2]))) registers[i] = true;
            }
            for (i=0; i<7; i++) 
                for (j=0; j< numPatches; j++)
                    if (registers[i] && patchStart[j] == i)
                        switch (patchEnd[j])
                        {
                            case 0: lampV = true; break;
                            case 1: lampX = true; break;
                            case 2: lampY = true; break;
                            case 3: lampZ = true; break;
                        }
        }

        private void mainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            int i;
            Point pos;
            pos = PointToClient(Control.MousePosition);
            if (pos.X >= 37 && pos.X <= 62 &&
                 pos.Y >= 353 && pos.Y <= 377)
            {
                performMatrix();
                drawLights();
                inputPressed = true;
                mainPanel.Invalidate();
            }
            patchCord = -1;
            if (pos.Y >= 268 && pos.Y <= 281)
            {
                for (i = 0; i < 7; i++)
                    if ((pos.X >= 81 + i * 75) &&
                        (pos.X <= 94 + i * 75)) patchCord = i;
            }
        }

        private void addPatchCord(int start,int end) {
            int i;
            int j;
            j = -1;
            for (i = 0; i < numPatches; i++)
                if (patchStart[i] == start &&
                    patchEnd[i] == end) j = i;
            if (j >= 0)
            {
                for (i = j; i < numPatches-1; i++)
                {
                    patchStart[i] = patchStart[i + 1];
                    patchEnd[i] = patchEnd[i + 1];
                }
                numPatches--;
                drawPanel();
                drawLights();
                drawSlide(0);
                drawSlide(1);
                drawSlide(2);
                drawPatchCords();
                return;
            }
            if (numPatches > 15) return;
            patchStart[numPatches] = start;
            patchEnd[numPatches++] = end;
        }

        private void drawPatchCords() {
            int i;
            Graphics gr;
            Pen pn;
            int x1, x2;
            gr = Graphics.FromImage(mainPanel.Image);
            pn = new Pen(Color.Black);
            pn.Width = 4;
            for (i = 0; i < numPatches; i++)
            {
                x1 = patchStart[i] * 75 + 75;
                x2 = 105;
                switch (patchEnd[i])
                {
                    case 0: x2 = 93; break;
                    case 1: x2 = 151; break;
                    case 2: x2 = 301; break;
                    case 3: x2 = 451; break;
                }
                gr.DrawLine(pn, x1, 262, x2, 328);
            }
            pn.Dispose();
            gr.Dispose();
        }

        private void mainPanel_MouseUp(object sender, MouseEventArgs e)
        {
            Point pos;
            pos = PointToClient(Control.MousePosition);
            int patchLamp;
            if (inputPressed)
            {
                inputPressed = false;
                if (autoSet.Checked)
                {
                    slidePositions[0] = (lampX) ? 1 : 0;
                    slidePositions[1] = (lampY) ? 1 : 0;
                    slidePositions[2] = (lampZ) ? 1 : 0;
                    drawSlide(0);
                    drawSlide(1);
                    drawSlide(2);
                    mainPanel.Invalidate();
                }
                lampV = false;
                lampX = false;
                lampY = false;
                lampZ = false;
                drawLights();
                mainPanel.Invalidate();
            }
            if (patchCord >= 0)
            {
                patchLamp = -1;
                if (pos.Y >= 334 && pos.Y <= 347) {
                    if (pos.X >= 99 && pos.X <= 112) patchLamp = 0;
                    if (pos.X >= 156 && pos.X <= 169) patchLamp = 1;
                    if (pos.X >= 306 && pos.X <= 319) patchLamp = 2;
                    if (pos.X >= 456 && pos.X <= 469) patchLamp = 3;
                    if (patchLamp >= 0)
                    {
                        addPatchCord(patchCord, patchLamp);
                        drawPatchCords();
                        mainPanel.Invalidate();
                    }
                }
                patchCord = -1;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int x, y;
            BinaryWriter file;
            FileStream fs;
            saveFileDialog1.Filter = "JR01 files|*.jr1";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Text = "JR01 - " + openFileDialog1.FileName;
                fs = File.Create(saveFileDialog1.FileName, 1024, FileOptions.None);
                file = new BinaryWriter(fs);
                for (y = 0; y < 3; y++)
                    for (x = 0; x < 14; x++)
                        file.Write(slideHoles[y, x]);
                file.Write(numPatches);
                for (x = 0; x < numPatches; x++)
                {
                    file.Write(patchStart[x]);
                    file.Write(patchEnd[x]);
                }
                file.Close();
                fs.Close();
            }

        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            int x, y;
            BinaryReader file;
            FileStream fs;
            openFileDialog1.Filter = "JR01 files|*.jr1";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Text = "JR01 - " + openFileDialog1.FileName;
                fs = File.Open(openFileDialog1.FileName, FileMode.Open);
                file = new BinaryReader(fs);
                for (y = 0; y < 3; y++)
                    for (x = 0; x < 14; x++)
                        slideHoles[y, x] = file.ReadBoolean();
                numPatches = file.ReadInt32();
                for (x = 0; x < numPatches; x++)
                {
                    patchStart[x] = file.ReadInt32();
                    patchEnd[x] = file.ReadInt32();
                }
                file.Close();
                fs.Close();
                drawPanel();
                drawLights();
                drawSlide(0);
                drawSlide(1);
                drawSlide(2);
                drawPatchCords();
                mainPanel.Invalidate();
            }
        }
    }
}
