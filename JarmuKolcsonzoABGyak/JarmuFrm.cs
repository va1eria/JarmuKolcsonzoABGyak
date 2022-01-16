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
    public partial class JarmuFrm : Form
    {
        Kolcsonzo kolcsonzo;
        Jarmu jarmu;
        internal Jarmu Jarmu { get => jarmu; }

        internal JarmuFrm(Kolcsonzo kolcsonzo)
        {
            this.kolcsonzo = kolcsonzo;
            InitializeComponent();
            numericUpDown1.Maximum = int.MaxValue;
            comboBox1.DataSource = Enum.GetValues(typeof(AutoTipus));
        }

        internal JarmuFrm(Jarmu modosit) : this((Kolcsonzo)null) //itt azert null, mert a modositashoz nem kell a kolcsonzo csak a felvitelhez. A null nincs tipushoz kotve, azert kell kikasztolni, mert nem lehet tudni hogy melyik parameteru konstruktort kell meghivni (jarmu vagy kolcsonzo)
        {
            jarmu = modosit;
            textBox1.Text = jarmu.Rendszam;
            textBox1.ReadOnly = true;
            textBox2.Text = jarmu.Marka;
            textBox2.ReadOnly = true;
            textBox3.Text = jarmu.Tipus;
            textBox3.ReadOnly = true;
            numericUpDown1.Value = jarmu.FutottKM;
            checkBox1.Checked = jarmu.Kolcsonozve;
            if (jarmu is Auto auto)
            {
                tabControl1.SelectedIndex = 0;
                comboBox1.SelectedItem = auto.AutoTipusa;
                numericUpDown2.Value = auto.SzallithatoSzemSzam;
            }
            else if (jarmu is Motor motor)
            {
                tabControl1.SelectedIndex = 1;
                numericUpDown3.Value = (decimal)motor.Hengerurtartalom;
            }
            tabControl1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (jarmu == null)
                {
                    switch (tabControl1.SelectedIndex)
                    {
                        case 0:
                            jarmu = new Auto(
                                textBox1.Text,
                                textBox2.Text,
                                textBox3.Text,
                                (int)numericUpDown1.Value,
                                checkBox1.Checked,
                                (AutoTipus)comboBox1.SelectedItem,
                                (byte)numericUpDown2.Value);
                            break;
                        case 1:
                            jarmu = new Motor(
                                textBox1.Text,
                                textBox2.Text,
                                textBox3.Text,
                                (int)numericUpDown1.Value,
                                checkBox1.Checked,
                                (double)numericUpDown3.Value);
                            break;
                    }
                    ABKezelo.JarmuFelvitel(kolcsonzo, jarmu);
                }
                else
                {
                    jarmu.FutottKM = (int)numericUpDown1.Value;
                    jarmu.Kolcsonozve = checkBox1.Checked;
                    ABKezelo.JarmuModositas(jarmu);
                }
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
