using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Gif.Components;
using System.Drawing.Drawing2D;

namespace GifBuilder
{
    

    public partial class Form1 : Form
    {
        Image from;
        public Form1()
        {
            InitializeComponent();
        }

        public void printImageAndSize(int index)
        {
            if (index >= 0 && listBox1.Items.Count > 0)
            {
                try
                {
                    if (from != null && pictureBox1.Image != null)
                    {
                        from.Dispose();
                        //pictureBox1.Image.Dispose();
                        //MessageBox.Show("Test");
                    } 
                    from = Image.FromFile(listBox1.Items[index].ToString());
                    label3.Text = from.Width.ToString();
                    label5.Text = from.Height.ToString();
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    
                    pictureBox1.Image = from;
                }
                catch
                {
                    MessageBox.Show("File is not a image");
                    listBox1.Items.RemoveAt(index);
                } 
                
            }
        }

        public Image ResizeImage(string adr)
        {
            Image image = Image.FromFile(adr);
            int width = int.Parse(textBox2.Text);
            int height = int.Parse(textBox3.Text);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            image.Dispose();
            return (Image)destImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach(string msr in openFileDialog1.FileNames){
                    
                    listBox1.Items.Add(msr);

                }
            }
            //MessageBox.Show(listBox1.Items[0].ToString());
            printImageAndSize(0);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string destination = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                destination = saveFileDialog1.FileName;
                
                //MessageBox.Show(destination + " !");
                //MessageBox.Show(checkBox1.Checked.ToString());

                AnimatedGifEncoder enc = new AnimatedGifEncoder();
                enc.Start(destination + ".gif");
                enc.SetDelay(int.Parse(textBox1.Text));

                Image from;
                if (checkBox1.Checked) enc.SetRepeat(0);
                else enc.SetRepeat(1);
                foreach (string str in listBox1.Items)
                {
                    if (checkBox3.Checked)
                    {
                        from = ResizeImage(str);
                    }
                    else
                    {
                        from = Image.FromFile(str);
                    }
                    enc.AddFrame(from);
                    from.Dispose();
                    progressBar1.Increment(progressBar1.Maximum / listBox1.Items.Count);
                    //MessageBox.Show(str);
                }
                enc.Finish();
                MessageBox.Show("Finished");
                progressBar1.Value = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            printImageAndSize(listBox1.SelectedIndex);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int a =0;
            int b = 0;
            int cur = 0;
            if (checkBox2.Checked && label3.Text != "0" && textBox3.Text != "0")
            {
                if (int.TryParse(label3.Text, out a) && int.TryParse(label5.Text, out b) && int.TryParse(textBox2.Text, out cur))
                {
                    textBox3.Text = (b / (a / cur)).ToString();
                }
                else
                {
                    MessageBox.Show(label3.Text + " " + label5.Text + " " + textBox2.Text);
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int a = 0;
            int b = 0;
            int cur = 0;

            
            if (checkBox2.Checked && label5.Text != "0" && textBox3.Text != "0")
            {
                if (int.TryParse(label5.Text, out a) && int.TryParse(label3.Text, out b) && int.TryParse(textBox3.Text, out cur))
                {
                    textBox2.Text = (b / (a / cur)).ToString();
                }
                else
                {
                    MessageBox.Show(label3.Text + " " + label5.Text + " " + textBox3.Text);
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')
            {
                e.Handled = !char.IsNumber(e.KeyChar);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = (int.Parse(textBox2.Text) * 2).ToString();
            textBox3.Text = (int.Parse(textBox3.Text) * 2).ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = (int.Parse(textBox2.Text) / 2).ToString();
            textBox3.Text = (int.Parse(textBox3.Text) / 2).ToString();
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
                catch
                {
                    //MessageBox.Show(listBox1.SelectedIndex + " not valid index");
                }
            }

        }


    }
}
