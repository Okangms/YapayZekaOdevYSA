using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace YapayZekaOdevYSA
{
    internal class YSA1
    {
        public double[] weightInputHidden;
        public double[] weightHiddenOutput;
        public double[] biasHidden;
        public double[] biasOutput;

        int hiddenSize = 1;
        int outputSize = 5;

        Random rnd = new Random();

        public int[,] istenenCikti = new int[5, 5]
        {
            {0,0,0,0,1},
            {0,0,0,1,0},
            {0,0,1,0,0},
            {0,1,0,0,0},
            {1,0,0,0,0}
        };
        public int[,,] egitimVerisi = new int[5, 7, 5]
        {
            {
                  /* A */
                {0,0,1,0,0},
                {0,1,0,1,0},
                {1,0,0,0,1},
                {1,0,0,0,1},
                {1,1,1,1,1},
                {1,0,0,0,1},
                {1,0,0,0,1}
            },
            {
                  /* B */
                {1,1,1,1,0},
                {1,0,0,0,1},
                {1,0,0,0,1},
                {1,1,1,1,0},
                {1,0,0,0,1},
                {1,0,0,0,1},
                {1,1,1,1,0}
            },
            {
                  /* C */
                {0,0,1,1,1},
                {0,1,0,0,0},
                {1,0,0,0,0},
                {1,0,0,0,0},
                {1,0,0,0,0},
                {0,1,0,0,0},
                {0,0,1,1,1}
            },
            {
                /* D */
                {1,1,1,0,0},
                {1,0,0,1,0},
                {1,0,0,0,1},
                {1,0,0,0,1},
                {1,0,0,0,1},
                {1,0,0,1,0},
                {1,1,1,0,0}
            },
            {
                  /* E */
                {1,1,1,1,1},
                {1,0,0,0,0},
                {1,0,0,0,0},
                {1,1,1,1,1},
                {1,0,0,0,0},
                {1,0,0,0,0},
                {1,1,1,1,1}
            }
        };

        public YSA1()
        {
            weightInputHidden = new double[1];
            weightHiddenOutput = new double[5];

            biasHidden = new double[hiddenSize];
            biasOutput = new double[outputSize];

            initializeWeightAndBias();
        }

        private void initializeWeightAndBias()
        {
            //giriş- gizli katman ağırlıkları
            weightInputHidden[0] = (rnd.NextDouble() * 2) - 1;

            // gizli-çıkış katmanı ağırlıkları
            for (int i = 0; i < 5; i++)
            {
                weightHiddenOutput[i] = (rnd.NextDouble() * 2) - 1;
            }
            //gizli katman bias
            biasHidden[0] = (rnd.NextDouble() * 2) - 1;

            //çıkış katmanı bias
            for (int i = 0; i < 5; i++)
            {
                biasOutput[i] = (rnd.NextDouble() * 2) - 1;
            }
        }

        public void Egitim(int[,,] egitimVerisiX, int[,] beklenenSonuc, int epochs, double momentum, double ogrenmeKatsayisi)
        {
            int veriSayisi = egitimVerisiX.GetLength(0);
            int satirSayisi = egitimVerisiX.GetLength(1);
            int sutunSayisi = egitimVerisiX.GetLength(2);

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                for (int veriIndex = 0; veriIndex < veriSayisi; veriIndex++)
                {
                    // 3 boyutlu veriyi 2 boyutlu ele alma
                    double[,] egitimmatris = new double[satirSayisi, sutunSayisi];
                    for (int k = 0; k < satirSayisi; k++)
                    {
                        for (int l = 0; l < sutunSayisi; l++)
                        {
                            egitimmatris[k, l] = egitimVerisiX[veriIndex, k, l];
                        }
                    }
                    double gizliKatman = HiddenLayerOutputCalc(egitimmatris);
                    double[] CikisKatman = IleriYayilim(gizliKatman);
                    GeriYayilim(CikisKatman, beklenenSonuc, gizliKatman, ogrenmeKatsayisi, momentum);
                }
            }
        }
        public double[] IleriYayilim(double hiddenOutput)
        {
            double[] outputLayer = new double[5];
            for (int i = 0; i < 5; i++)
            {
                outputLayer[i] = (hiddenOutput * weightHiddenOutput[i]) + biasOutput[i];
            }

            for (int i = 0; i < 5; i++)
            {
                outputLayer[i] = Sigmoid(outputLayer[i]);
            }
            return outputLayer;
        }

        public double HiddenLayerOutputCalc(double[,] girdiMatris)
        {
            double net = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    net += girdiMatris[i, j] * weightInputHidden[0];
                }
            }
            net += biasHidden[0];
            double Sonuc = Sigmoid(net);

            return Sonuc;
        }


        public void GeriYayilim(double[] CikisKatman, int[,] istenenCikti, double gizliKatmanSonuc, double ogrenmeKatsayisi, double momentum)
        {
            //hata hesabı
            double toplamhata = 0;
            for (int i = 0; i < 5; i++)
            {
                double hata = CikisKatman[i] - istenenCikti[i, i];
                toplamhata += hata * hata;
            }
            double ortalamaHata = toplamhata / 5;

            //gizli-çıkış katmanı ağırlık güncelleme


            for (int i = 0; i < 5; i++)
            {
                double deltax = SigmoidTurev(CikisKatman[i]) * (1 - CikisKatman[i]);
                weightHiddenOutput[i] = (ogrenmeKatsayisi * deltax * gizliKatmanSonuc) + (momentum * weightHiddenOutput[i]);
                biasOutput[i] = ((ogrenmeKatsayisi * deltax) + (momentum * biasOutput[i]));
            }

            //giriş-gizli katman ağırlık güncelleme


            double toplamHesap = 0;
            for (int k = 0; k < 5; k++)
            {
                double deltacikis = SigmoidTurev(CikisKatman[k]) * (1 - CikisKatman[k]);
                toplamHesap += deltacikis * CikisKatman[k];
            }
            double delta = SigmoidTurev(gizliKatmanSonuc) * (toplamHesap);

            weightInputHidden[0] = (ogrenmeKatsayisi * delta * gizliKatmanSonuc) + (momentum * weightInputHidden[0]);

            //gizlikatman bias güncelleme          
            biasHidden[0] = (ogrenmeKatsayisi * delta) + (momentum * biasHidden[0]);
        }

        public double[] TanimlaA(double[,] input)
        {
            double hiddenLayerOutput = HiddenLayerOutputCalc(input);
            double[] sonucMatrisi = IleriYayilim(hiddenLayerOutput);
            
            return sonucMatrisi;
        }
        //public double TanimlaB(double[,] input)
        //{
        //    double hiddenLayerOutput = HiddenLayerOutputCalc(input);
        //    double[] sonucMatrisi = IleriYayilim(hiddenLayerOutput);
        //    return sonucMatrisi[1];
        //}
        //public double TanimlaC(double[,] input)
        //{
        //    double hiddenLayerOutput = HiddenLayerOutputCalc(input);
        //    double[] sonucMatrisi = IleriYayilim(hiddenLayerOutput);
        //    return sonucMatrisi[2];
        //}
        //public double TanimlaD(double[,] input)
        //{
        //    double hiddenLayerOutput = HiddenLayerOutputCalc(input);
        //    double[] sonucMatrisi = IleriYayilim(hiddenLayerOutput);
        //    return sonucMatrisi[3];
        //}
        //public double TanimlaE(double[,] input)
        //{
        //    double hiddenLayerOutput = HiddenLayerOutputCalc(input);
        //    double[] sonucMatrisi = IleriYayilim(hiddenLayerOutput);
        //    return sonucMatrisi[4];
        //}


        private double SigmoidTurev(double x)
        {
            double sonuc = Sigmoid(x) * (1 - Sigmoid(x));
            return sonuc;

        }
        private double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
        public double[,] DiziAktar(Button[,] btn)
        {
            double[,] girdiDizi = new double[7, 5];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (btn[i, j].BackColor == Color.Black)
                    {
                        girdiDizi[i, j] = 1;
                    }
                    else
                    {
                        girdiDizi[i, j] = 0;
                    }
                }
            }
            return girdiDizi;
        }


    }
}
