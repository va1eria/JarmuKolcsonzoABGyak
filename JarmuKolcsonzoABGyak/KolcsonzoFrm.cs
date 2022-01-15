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
    public partial class KolcsonzoFrm : Form
    {
        Kolcsonzo kolcsonzo;
        internal Kolcsonzo Kolcsonzo { get => kolcsonzo; }
        public KolcsonzoFrm()
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(KozteruletJelleg));
        }

        internal KolcsonzoFrm(Kolcsonzo modosit)
        {
            kolcsonzo = modosit;
            textBox1.Text = kolcsonzo.Megnevezes;
            numericUpDown1.Value = kolcsonzo.Cim.Irsz;
            textBox2.Text = kolcsonzo.Cim.Telepules;
            textBox3.Text = kolcsonzo.Cim.Kozterulet;
            comboBox1.SelectedItem = kolcsonzo.Cim.KozteruletJellege;
            textBox4.Text = kolcsonzo.Cim.Hazszam;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (kolcsonzo == null)
                {
                    kolcsonzo = new Kolcsonzo(
                        textBox1.Text,
                        new Cim(
                            textBox2.Text,
                            textBox3.Text,
                            textBox4.Text,
                            (short)numericUpDown1.Value,
                            (KozteruletJelleg)comboBox1.SelectedItem
                            ));
                    ABKezelo.KolcsonzoFelvitel(kolcsonzo);
                }
                else
                {
                    kolcsonzo.Megnevezes = textBox1.Text;
                    kolcsonzo.Cim = new Cim(
                            textBox2.Text,
                            textBox3.Text,
                            textBox4.Text,
                            (short)numericUpDown1.Value,
                            (KozteruletJelleg)comboBox1.SelectedItem
                            );
                    ABKezelo.KolcsonzoModositas(kolcsonzo);
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
