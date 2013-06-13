using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enigma_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        Data enigma = new Data();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {   
            string rotated = string.Join(" ", enigma._rotors[0].Keys);//string.Join(" ", enigma._key_rotated.ToCharArray()); 
            Font mono_font = new System.Drawing.Font("Consolas", 15, FontStyle.Bold);
            var PB = sender as PictureBox;
            
            Size textsize = TextRenderer.MeasureText("A", new Font("Consolas", 15, FontStyle.Bold));
            Size textsize2 = TextRenderer.MeasureText(string.Join(" ", enigma._alphabet), new Font("Consolas", 15, FontStyle.Bold));
            
            draw_rotor(0,e.Graphics, 110, 220);
            draw_rotor(1, e.Graphics, 280, 390);
            draw_rotor(2, e.Graphics, 450, 560);
            draw_reflector(e.Graphics, 630);


            e.Graphics.DrawString(string.Join(" ", enigma._alphabet.ToCharArray()), mono_font, Brushes.Black, PB.Location); // input
            e.Graphics.DrawString(string.Join(" ", enigma._rotors[0].Keys), mono_font, Brushes.Black, PB.Location.X, PB.Location.Y + 60); // first rotor up
            e.Graphics.DrawString(string.Join(" ", enigma._rotors[0].Keys), mono_font, Brushes.Black, PB.Location.X, PB.Location.Y + 200); // first rotor down
            e.Graphics.DrawString(string.Join(" ", enigma._rotors[1].Keys), mono_font, Brushes.Black, PB.Location.X, PB.Location.Y + 230); // second rotor up
            e.Graphics.DrawString(string.Join(" ", enigma._rotors[1].Keys), mono_font, Brushes.Black, PB.Location.X, PB.Location.Y + 370); // second rotor down
            e.Graphics.DrawString(string.Join(" ", enigma._rotors[2].Keys), mono_font, Brushes.Black, PB.Location.X, PB.Location.Y + 400); // third rotor up
            e.Graphics.DrawString(string.Join(" ", enigma._rotors[2].Keys), mono_font, Brushes.Black, PB.Location.X, PB.Location.Y + 540); // third rotor down                       
            e.Graphics.DrawString(string.Join(" ", enigma._alphabet.ToCharArray()), mono_font, Brushes.Black, PB.Location.X, PB.Location.Y + 580); //reflector

        }
        private void draw_reflector(Graphics gr, float Y1)
        {
            Size textsize = TextRenderer.MeasureText("A", new Font("Consolas", 15, FontStyle.Bold));
            float X1, X2, Y2=Y1;
            List<char> used_letters = new List<char>();

            foreach (var el in enigma._reflector_A.Select((value, index) => new { value, index }))
            {
                if (used_letters.IndexOf(el.value) != -1) { continue; }

                X1 = (el.index + 0.5f) * textsize.Width + el.index * 1.2f + 1; // ибо TextRenderer.MeasureText(text,font)/text.lengh != const О_о ???
                X2 = ((int)el.value - 65 + 0.5f) * textsize.Width + ((int)el.value - 65) * 1.2f - 1;
                Y2 += 4;
                gr.DrawLine(Pens.Black, X1, Y1, X1, Y2);
                gr.DrawLine(Pens.Black, X1, Y2, X2, Y2);
                gr.DrawLine(Pens.Black, X2, Y2, X2, Y1);
                used_letters.Add((char)(el.index + 65));
            }
        }
        private void draw_rotor(int rotor_index,Graphics gr, float Y1, float Y3)
        {
            int indent = 0, index_of_key, index_of_rot;
            float X1, X2, Y2;
            Size textsize = TextRenderer.MeasureText("A", new Font("Consolas", 15, FontStyle.Bold));

            foreach (var el in enigma._rotors[rotor_index].Select((value, index) => new { value, index }))
            {

                index_of_key = (string.Join(string.Empty, enigma._rotors[rotor_index].Keys)).IndexOf(el.value.Key); // Х_х
                index_of_rot = (string.Join(string.Empty, enigma._rotors[rotor_index].Keys)).IndexOf(el.value.Value); // Х_х

                X1 = (index_of_key + 0.5f) * textsize.Width + index_of_key * 1.2f - 1; // ибо TextRenderer.MeasureText(text,font)/text.lengh != const О_о ???                              
                X2 = (index_of_rot + 0.5f) * textsize.Width + index_of_rot * 1.2f + 1; // ибо TextRenderer.MeasureText(text,font)/text.lengh != const О_о ???                              
                Y2 = Y1 + 5 + indent;
                gr.DrawLine(Pens.Black, X1, Y1, X1, Y2);
                gr.DrawLine(Pens.Black, X1, Y2, X2, Y2);
                gr.DrawLine(Pens.Black, X2, Y2, X2, Y3);

                indent += 4;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (enigma.interactivity)
            {
                //textBox1.Text = e.X.ToString() + "    " + e.Y.ToString();
                
                if (e.X >= 5 && e.X <= 725 && e.Y >= 85 && e.Y <= 100)
                {
                    if (enigma.last_index_1 != e.X / 28)
                    {
                        draw_commutation(0, 110, 220, e.X / 28);//1st rotor
                        enigma.last_index_1 = e.X / 28;
                    }                   
                }
                else if (e.X >= 5 && e.X <= 725 && e.Y >= 255 && e.Y <= 270)
                {
                    if (enigma.last_index_2 != e.X / 28)
                    {
                        draw_commutation(1, 280, 390, e.X / 28);//2nd rotor
                        enigma.last_index_2 = e.X / 28;
                    } 
                }
                else if (e.X >= 5 && e.X <= 725 && e.Y >= 425 && e.Y <= 440)
                {
                    if (enigma.last_index_3 != e.X / 28)
                    {
                        draw_commutation(2, 450, 560, e.X / 28);//3rd rotor
                        enigma.last_index_3 = e.X / 28;
                    }
                }
                else if (e.X >= 5 && e.X <= 725 && e.Y >= 605 && e.Y <= 620)
                {
                    draw_commutation(3, 630, 0, e.X / 28); //reflector
                }

                else { pictureBox1.Refresh(); enigma.last_index_1 = -1; enigma.last_index_2 = -1; enigma.last_index_3 = -1; }
            }
        }

        private void draw_commutation(int rotor_index, int Y1, int Y3 ,int current_pos)
        {
            pictureBox1.Refresh();
            Size textsize = TextRenderer.MeasureText("A", new Font("Consolas", 15, FontStyle.Bold));
             

            Pen Pen_Red = new Pen(Color.Blue);
            Pen_Red.Width = 3;
            //Pen_Red.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            Graphics gr = pictureBox1.CreateGraphics();
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            
            string keys = (string.Join(string.Empty, enigma._rotors[rotor_index].Keys)); // key rotated
            string values = (string.Join(string.Empty, enigma._rotors[rotor_index].Values));
            var key = keys[current_pos];

            float index_of_rot = keys.IndexOf(enigma._rotors[rotor_index][key]);
            float index_of_key = current_pos;

            var X1 = (current_pos + 0.5f) * textsize.Width + current_pos * 1.2f + 1; // ибо TextRenderer.MeasureText(text,font)/text.lengh != const О_о ???
            var X2 = (index_of_rot + 0.5f) * textsize.Width + index_of_rot * 1.2f + 1; // ибо TextRenderer.MeasureText(text,font)/text.lengh != const О_о ???                              
            var Y2 = Y1+5 + current_pos * 4;

            if (rotor_index==3) Y3 = Y1;

            gr.DrawLine(Pen_Red, X1, Y1, X1, Y2);
            gr.DrawLine(Pen_Red, X1, Y2, X2, Y2);
            gr.DrawLine(Pen_Red, X2, Y2, X2, Y3);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { trackBar_rotorI.Value -= 1; }
            catch { }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = ((char)(26 - trackBar_rotorI.Value + 65)).ToString();

            enigma.rotor_rebuild(trackBar_rotorI.Value,0); // 0 - first rotor
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try { trackBar_rotorI.Value += 1; }
            catch { }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text = string.Empty;
            string input_key = textBox2.Text;
            char symbol;
            int position;
            Graphics gr = pictureBox1.CreateGraphics();
            Pen Pen_1 = new Pen(Color.Blue);
            Pen_1.Width = 2;
            Pen Pen_2 = new Pen(Color.Red);
            Pen_2.Width = 3;
            Size textsize = TextRenderer.MeasureText("A", new Font("Consolas", 15, FontStyle.Bold));

            foreach (var el in textBox_input.Text)
            {
                try { trackBar_rotorI.Value -= 1; }
                catch
                {
                    try { trackBar_rotorII.Value -= 1; }
                    catch
                    {
                        try { trackBar_rotorIII.Value -= 1; }
                        catch
                        {
                            trackBar_rotorIII.Value = 26;
                        }
                        trackBar_rotorII.Value = 26;
                    }
                    trackBar_rotorI.Value = 26;
                }

                position = enigma._alphabet.IndexOf(el);
 
                #region RotorI
                float X1 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                float Y1 = 115 + position * 4;

                symbol = string.Join(string.Empty, enigma._rotors[0].Keys)[position]; // rotorI up
                symbol = enigma._rotors[0][symbol]; // rotorI down
                position = string.Join(string.Empty, enigma._rotors[0].Keys).IndexOf(symbol);

                float X2 = (position + 0.5f) * textsize.Width + position * 1.2f + 1;
                float Y2 = 220;

                gr.DrawLine(Pen_1, X1, 110, X1, Y1);
                gr.DrawLine(Pen_1, X1, Y1, X2, Y1);
                gr.DrawLine(Pen_1, X2, Y1, X2, Y2);
                #endregion
                #region RotorII
                float X3 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                float Y3 = 285 + position * 4;

                symbol = string.Join(string.Empty, enigma._rotors[1].Keys)[position]; // rotorII up
                symbol = enigma._rotors[1][symbol]; // rotorII down
                position = string.Join(string.Empty, enigma._rotors[1].Keys).IndexOf(symbol);

                float X4 = (position + 0.5f) * textsize.Width + position * 1.2f + 1;
                float Y4 = 390;

                gr.DrawLine(Pen_1, X3, 280, X3, Y3);
                gr.DrawLine(Pen_1, X3, Y3, X4, Y3);
                gr.DrawLine(Pen_1, X4, Y3, X4, Y4);
                #endregion
                #region RotorIII
                float X5 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                float Y5 = 455 + position * 4;

                symbol = string.Join(string.Empty, enigma._rotors[2].Keys)[position]; // rotorII up
                symbol = enigma._rotors[2][symbol]; // rotorII down
                position = string.Join(string.Empty, enigma._rotors[2].Keys).IndexOf(symbol);

                float X6 = (position + 0.5f) * textsize.Width + position * 1.2f + 1;
                float Y6 = 560;

                gr.DrawLine(Pen_1, X5, 450, X5, Y5);
                gr.DrawLine(Pen_1, X5, Y5, X6, Y5);
                gr.DrawLine(Pen_1, X6, Y5, X6, Y6);
                #endregion
                #region Reflector
                float X7 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                float Y7 = 635 + position * 4;

                symbol = string.Join(string.Empty, enigma._rotors[3].Keys)[position]; // rotorII up
                symbol = enigma._rotors[3][symbol]; // rotorII down
                position = string.Join(string.Empty, enigma._rotors[3].Keys).IndexOf(symbol);

                float X8 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                float Y8 = 630;

                gr.DrawLine(Pen_1, X7, Y8, X7, Y7);
                gr.DrawLine(Pen_1, X7, Y7, X8, Y7);
                gr.DrawLine(Pen_1, X8, Y7, X8, Y8);

                gr.DrawRectangle(Pen_1, X5 - 10, 390, 25, 60);
                gr.DrawRectangle(Pen_1, X1 - 10, 20, 25, 90);
                gr.DrawRectangle(Pen_1, X3 - 10, 220, 25, 60);
                gr.DrawRectangle(Pen_1, X7 - 10, 560, 25, 70);
                gr.DrawRectangle(Pen_2, X8 - 10, 560, 25, 70);
                gr.DrawString("Input", new Font("Consolas", 9, FontStyle.Italic), Brushes.Blue, X1 - 20, 0);
                #endregion
                #region Back_rotorIII
                X6 = (position + 0.5f) * textsize.Width + position * 1.2f + 1;
                Y6 = 560;

                symbol = string.Join(string.Empty, enigma._rotors[2].Keys)[position]; // rotorIII down
                symbol = enigma._rotors[2].First(x => x.Value == symbol).Key; //rotorIII up
                position = string.Join(string.Empty, enigma._rotors[2].Keys).IndexOf(symbol); //position of m

                X5 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                Y5 = 455 + position * 4;

                gr.DrawLine(Pen_2, X5, 450, X5, Y5);
                gr.DrawLine(Pen_2, X5, Y5, X6, Y5);
                gr.DrawLine(Pen_2, X6, Y5, X6, Y6);
                #endregion
                #region Back_rotorII
                X4 = (position + 0.5f) * textsize.Width + position * 1.2f + 1;
                Y4 = 390;

                symbol = string.Join(string.Empty, enigma._rotors[1].Keys)[position]; // rotorII down
                symbol = enigma._rotors[1].First(x => x.Value == symbol).Key; //rotorII  up
                position = string.Join(string.Empty, enigma._rotors[1].Keys).IndexOf(symbol);

                X3 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                Y3 = 285 + position * 4;

                gr.DrawLine(Pen_2, X3, 280, X3, Y3);
                gr.DrawLine(Pen_2, X3, Y3, X4, Y3);
                gr.DrawLine(Pen_2, X4, Y3, X4, Y4);
                #endregion
                #region Back_RotorI
                X2 = (position + 0.5f) * textsize.Width + position * 1.2f + 1;
                Y2 = 220;

                symbol = string.Join(string.Empty, enigma._rotors[0].Keys)[position]; // rotorI down
                symbol = enigma._rotors[0].First(x => x.Value == symbol).Key; //rotorI  up
                position = string.Join(string.Empty, enigma._rotors[0].Keys).IndexOf(symbol);

                X1 = (position + 0.5f) * textsize.Width + position * 1.2f - 1;
                Y1 = 115 + position * 4;

                gr.DrawLine(Pen_2, X1, 110, X1, Y1);
                gr.DrawLine(Pen_2, X1, Y1, X2, Y1);
                gr.DrawLine(Pen_2, X2, Y1, X2, Y2);
                
                gr.DrawRectangle(Pen_2, X5 - 10, 390, 25, 60);
                gr.DrawRectangle(Pen_2, X1 - 10, 20, 25, 90);
                gr.DrawRectangle(Pen_2, X3 - 10, 220, 25, 60);
                #endregion

                symbol = enigma._alphabet[position];
                textBox4.Text += symbol;

                gr.DrawString("Output", new Font("Consolas", 9, FontStyle.Italic), Brushes.Red, X1 - 20, -4);

            }

            listView1.Items.Add(new ListViewItem(new string[] { textBox_input.Text, input_key, textBox4.Text, textBox2.Text }));

            if (listView1.Items.Count % 2 == 0)
                listView1.Items[listView1.Items.Count - 1].BackColor = Color.LightGray;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
                enigma.interactivity = radioButton2.Checked ? false : true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simple Enigma emulator \nChastov Anton \n31.05.2013", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
      
        }

        private void button6_Click(object sender, EventArgs e)
        {
            trackBar_rotorI.Value = 26;
        }

        private void trackBar_rotorII_ValueChanged(object sender, EventArgs e)
        {
            textBox_rotorII_key.Text= ((char)(26 - trackBar_rotorII.Value + 65)).ToString();            
            enigma.rotor_rebuild(trackBar_rotorII.Value, 1); // 1 - second rotor
            pictureBox1.Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try { trackBar_rotorII.Value += 1; }
            catch { }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try { trackBar_rotorII.Value -= 1; }
            catch { }
        }

        private void trackBar_rotorIII_ValueChanged(object sender, EventArgs e)
        {
            textBox_rotorIII_key.Text = ((char)(26 - trackBar_rotorIII.Value + 65)).ToString();
            enigma.rotor_rebuild(trackBar_rotorIII.Value, 2); // 2 - third rotor
            pictureBox1.Refresh();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try { trackBar_rotorIII.Value += 1; }
            catch { }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try { trackBar_rotorIII.Value -= 1; }
            catch { }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            trackBar_rotorI.Value = 26;
            trackBar_rotorII.Value = 26;
            trackBar_rotorIII.Value = 26;
        }

    }
}
