using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JarmuKolcsonzoABGyak
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LBFrissit();
        }

        void LBFrissit()
        {
            try
            {
                int sIndex = listBox1.SelectedIndex;
                listBox1.DataSource = null;
                listBox1.DataSource = ABKezelo.TeljesFelolvasas();
                if (sIndex < (listBox1.DataSource as List<Kolcsonzo>).Count)
                {
                    listBox1.SelectedIndex = sIndex;
                }
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.DataSource = null;
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.DataSource = (listBox1.SelectedItem as Kolcsonzo).Jarmuvek;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            KolcsonzoFrm frm = new KolcsonzoFrm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LBFrissit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                KolcsonzoFrm frm = new KolcsonzoFrm((Kolcsonzo)listBox1.SelectedItem);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LBFrissit();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex != -1)
                {
                    ABKezelo.KolcsonzoTorles((Kolcsonzo)listBox1.SelectedItem);
                    LBFrissit();
                }
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                JarmuFrm frm = new JarmuFrm((Kolcsonzo)listBox1.SelectedItem);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LBFrissit();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                JarmuFrm frm = new JarmuFrm((Jarmu)listBox2.SelectedItem);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LBFrissit();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedIndex != -1)
                {
                    ABKezelo.JarmuTorles((Jarmu)listBox2.SelectedItem);
                    LBFrissit();
                }
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
