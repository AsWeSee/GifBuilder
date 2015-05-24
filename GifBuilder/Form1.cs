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

namespace GifBuilder
{
    

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void printImageSize(int index)
        {
            label3.Text = Image.FromFile(listBox1.Items[index].ToString()).Width.ToString();
            label5.Text = Image.FromFile(listBox1.Items[index].ToString()).Height.ToString();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = Image.FromFile(listBox1.Items[index].ToString());

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
            try
            {
                printImageSize(0);
            }
            catch
            {
                MessageBox.Show("File is not a image");
                listBox1.Items.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string destination = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                destination = saveFileDialog1.FileName;
            }
            MessageBox.Show(destination + " !");
            AnimatedGifEncoder enc = new AnimatedGifEncoder();
            enc.Start(destination + ".gif");
            enc.SetDelay(int.Parse(textBox1.Text));
            MessageBox.Show(checkBox1.Checked.ToString());
            if (checkBox1.Checked) enc.SetRepeat(0);
            else enc.SetRepeat(1);
            foreach(string str in listBox1.Items){
                enc.AddFrame(Image.FromFile(str));
                //MessageBox.Show(str);
            }
            enc.Finish();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            printImageSize(listBox1.SelectedIndex);
        }

    }
}
