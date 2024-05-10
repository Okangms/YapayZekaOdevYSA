using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace YapayZekaOdevYSA
{
    public partial class Form1 : Form
    {

        private YSA1 ysa;



        public Form1()
        {
            InitializeComponent();
            ysa = new YSA1();


        }
        private void Form1_Load(object sender, EventArgs e)
        {



        }
        private void temizlik_Click(object sender, EventArgs e)
        {
            Button[] buttons = {
                button1,  button2,  button3,  button4,  button5,
                button6,  button7,  button8,  button9,  button10,
                button11, button12, button13, button14, button15,
                button16, button17, button18, button19, button20,
                button21, button22, button23, button24, button25,
                button26, button27, button28, button29, button30,
                button31, button32, button33, button34, button35
            };
            foreach (Button button in buttons)
            {
                if (button.BackColor == Color.Black)
                {
                    button.BackColor = Color.White;
                }
            }
        }

        private void ekranTemizlik_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";

        }

        public void boyama(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BackColor == Color.White)
            {
                button.BackColor = Color.Black;
            }
            else if (button.BackColor == Color.Black)
            {
                button.BackColor = Color.White;
            }
        }

        private void buttonEgitim_Click(object sender, EventArgs e)
        {
            if (ysa == null)
            {
                ysa = new YSA1(); // Nesne baþlatýlmamýþsa burada baþlatýyoruz
            }
            double ogrenmeKatsayisi = Convert.ToDouble(numericUpDown2.Value);
            double momentum = Convert.ToDouble(numericUpDown1.Value);

            ysa.Egitim(ysa.egitimVerisi,ysa.istenenCikti,1000,ogrenmeKatsayisi, momentum);


        }




        private void buttonTanimla_Click(object sender, EventArgs e)
        {
            if (ysa == null)
            {
                ysa = new YSA1(); // Nesne baþlatýlmamýþsa burada baþlatýyoruz
            }
            Button[,] buttons = {
                { button1,  button2,  button3,  button4,  button5 },
                { button6,  button7,  button8,  button9,  button10 },
                { button11, button12, button13, button14, button15},
                {button16, button17, button18, button19, button20},
                {button21, button22, button23, button24, button25},
                {button26, button27, button28, button29, button30},
                {button31, button32, button33, button34, button35}
            };
            double[,] benimDizim=ysa.DiziAktar(buttons);

            double[] sonucMatris = ysa.TanimlaA(benimDizim);

            label6.Text = sonucMatris[0].ToString();
            label7.Text = sonucMatris[1].ToString();
            label8.Text = sonucMatris[2].ToString();
            label9.Text = sonucMatris[3].ToString();
            label10.Text = sonucMatris[4].ToString();




            //label6.Text = ysa.TanimlaA(benimDizim).ToString();
            //label7.Text = ysa.TanimlaB(benimDizim).ToString();
            //label8.Text = ysa.TanimlaC(benimDizim).ToString();
            //label9.Text = ysa.TanimlaD(benimDizim).ToString();
            //label10.Text = ysa.TanimlaE(benimDizim).ToString();




        }
    }
}
