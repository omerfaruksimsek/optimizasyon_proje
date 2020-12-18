using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;   
using System.IO;
using System.Linq;
public class Control : MonoBehaviour
{
    public InputField satirIF;
    public InputField sutunIF;
    public GameObject MMHBtn;
    public GameObject hesaplaBtn;
    public GameObject sifirlaBtn;
    public GameObject cikisBtn;
    public GameObject matrisKutucugu;
    public GameObject canvas;
    public GameObject sonuc;
    public GameObject cozum;
    public int satir;
    public int sutun;
    public string gecici;
    public List<GameObject> maliyetIFListesi = new List<GameObject>();
    public List<GameObject> GecicimaliyetIFListesi = new List<GameObject>();
    public bool hesaplaIzni = true;

    bool esitlikVarmi = false;
    int kaynakTalepFark = 0;

    public static List<int[]> cost_inputs = new List<int[]>();
    public static int[] benimIndexes = new int[2];

    // Update is called once per frame
    void Update()
    {
        if(satirIF.text != "" && int.Parse(satirIF.text) > 12)
        {
            satir = 0;
            satirIF.text = "";
        }
        if (sutunIF.text != "" && int.Parse(sutunIF.text) > 12)
        {
            sutun = 0;
            sutunIF.text = "";
        }
    }
    public void Cikis()
    {
        Application.Quit();
    }
    public void Sifirla()
    {
        maliyetIFListesi.Clear();
        cost_inputs.Clear();
        GecicimaliyetIFListesi.Clear();
        SceneManager.LoadScene(0);
    }
    public void Hesapla()
    {
        if(MMHBtn.GetComponent<Button>().interactable == false)
        {
            hesaplaIzni = true;
            foreach (var i in maliyetIFListesi)
            {
                if (i.GetComponent<InputField>().text == "")
                {
                    hesaplaIzni = false;
                }
            }
            if (hesaplaIzni)
            {
                hesaplaBtn.GetComponent<Button>().interactable = false;
                ///////////////////////////////////////////////////////////////////////////

                ///
                //Satır Sütun Eşitsizlik işlemleri
                int kaynakToplam = 0;
                int talepToplam = 0;
                int kacinciSatir = 1;
                int kacinciSutun = 1;
                foreach (var i in maliyetIFListesi)
                {
                    if (kacinciSutun == sutun)
                    {
                        kaynakToplam += int.Parse(i.GetComponent<InputField>().text);
                        kacinciSatir++;
                        kacinciSutun = 1;
                    }
                    else
                    {
                        if (kacinciSatir == satir)
                        {
                            talepToplam += int.Parse(i.GetComponent<InputField>().text);
                        }
                        kacinciSutun++;
                    }
                }
                /*foreach (var i in kaynakDizisi)
                {
                    kaynakToplam += i;
                }
                foreach (var i in talepDizisi)
                {
                    talepToplam += i;
                }*/
                kaynakTalepFark = kaynakToplam - talepToplam;
                if (kaynakToplam < talepToplam)
                {
                    esitlikVarmi = false;
                }
                else if (kaynakToplam > talepToplam)
                {
                    esitlikVarmi = false;
                }
                else if (kaynakToplam == talepToplam)
                {
                    esitlikVarmi = true;
                    kaynakTalepFark = 0;
                }


                if (esitlikVarmi == false)
                {
                    //Debug.Log("kaynakTalepFark" + kaynakTalepFark);
                    //Debug.Log("talepToplam" + talepToplam);
                    //Debug.Log("kaynakToplam" + kaynakToplam);

                    if(kaynakTalepFark > 0)
                    {
                        kacinciSatir = 1;
                        kacinciSutun = 1;
                        int matrisKont = 0;
                        for (int i = 0; i < satir * sutun - 1; i++)
                        {
                            if (kacinciSutun == sutun)
                            {
                                //Debug.Log("maliyetIFListesi[i]:" + maliyetIFListesi[matrisKont].GetComponent<InputField>().text);
                                GameObject gecici = GameObject.Instantiate(matrisKutucugu);
                                gecici.transform.SetParent(canvas.transform, false);
                                gecici.GetComponent<RectTransform>().anchoredPosition = new Vector2((kacinciSutun) * 50, maliyetIFListesi[matrisKont].GetComponent<RectTransform>().anchoredPosition.y);
                                gecici.SetActive(true);
                                gecici.GetComponent<InputField>().text = "0";
                                //gecici.transform.GetChild(2).gameObject.GetComponent<Text>().text = "0";

                                //gecici.GetComponent<InputField>().readOnly = true;
                                maliyetIFListesi[matrisKont].GetComponent<RectTransform>().anchoredPosition = new Vector2((kacinciSutun + 1) * 50, maliyetIFListesi[matrisKont].GetComponent<RectTransform>().anchoredPosition.y);
                                maliyetIFListesi.Insert(matrisKont, gecici);
                                matrisKont++;
                                kacinciSatir++;
                                kacinciSutun = 1;
                            }
                            else
                            {
                                kacinciSutun++;
                            }
                            matrisKont++;
                        }
                        GameObject geciciSonEleman = GameObject.Instantiate(matrisKutucugu);
                        geciciSonEleman.transform.SetParent(canvas.transform, false);
                        geciciSonEleman.GetComponent<RectTransform>().anchoredPosition = new Vector2((sutun) * 50, maliyetIFListesi[maliyetIFListesi.Count-1].GetComponent<RectTransform>().anchoredPosition.y);
                        geciciSonEleman.SetActive(true);
                        geciciSonEleman.GetComponent<InputField>().text = kaynakTalepFark.ToString();
                        maliyetIFListesi.Insert(maliyetIFListesi.Count, geciciSonEleman);
                        sutun++;
                        Debug.Log("sutun" + sutun);
                        Debug.Log("maliyetIFListesi.Count"+maliyetIFListesi.Count);

                    }
                    else if (kaynakTalepFark < 0)
                    {
                        kacinciSatir = 1;
                        kacinciSutun = 1;
                        int matrisKont = 0;
                        for (int i = 0; i < satir * sutun - 1; i++)
                        {
                            if (kacinciSutun == sutun)
                            {
                                kacinciSatir++;
                                kacinciSutun = 1;
                            }
                            else
                            {
                                if (kacinciSatir == satir)
                                {
                                    Debug.Log("maliyetIFListesi[i]:" + maliyetIFListesi[matrisKont].GetComponent<InputField>().text);
                                    Debug.Log("matrisKont" + matrisKont);
                                    GameObject gecici = GameObject.Instantiate(matrisKutucugu);
                                    gecici.transform.SetParent(canvas.transform, false);
                                    gecici.GetComponent<RectTransform>().anchoredPosition = new Vector2(kacinciSutun * 50, -(kacinciSatir) * 50 - 75);
                                    gecici.SetActive(true);
                                    gecici.GetComponent<InputField>().text = "0";
                                    //maliyetIFListesi[matrisKont].GetComponent<RectTransform>().anchoredPosition = new Vector2(maliyetIFListesi[matrisKont].GetComponent<RectTransform>().anchoredPosition.x, maliyetIFListesi[matrisKont].GetComponent<RectTransform>().anchoredPosition.y -50);
                                    maliyetIFListesi[matrisKont].GetComponent<RectTransform>().anchoredPosition = new Vector2(kacinciSutun * 50, -(kacinciSatir+1) * 50 -75);
                                    maliyetIFListesi.Insert(i, gecici);
                                    matrisKont++;
                                }

                                kacinciSutun++;
                            }
                            matrisKont++;
                        }
                        GameObject geciciSonEleman = GameObject.Instantiate(matrisKutucugu);
                        geciciSonEleman.transform.SetParent(canvas.transform, false);
                        geciciSonEleman.GetComponent<RectTransform>().anchoredPosition = new Vector2((sutun) * 50, -(satir) * 50 - 75);
                        geciciSonEleman.SetActive(true);
                        geciciSonEleman.GetComponent<InputField>().text = (-kaynakTalepFark).ToString();
                        //geciciSonEleman.GetComponent<InputField>().readOnly = true;
                        maliyetIFListesi.Insert((maliyetIFListesi.Count-sutun+1), geciciSonEleman);
                        satir++;
                        Debug.Log("satir" + satir);

                    }

                    kacinciSatir = 1;
                    kacinciSutun = 1;
                    int geciciSayac = 0;
                    for (int i = 0; i < satir * sutun - 1; i++)
                    {

                        //Debug.Log(kacinciSatir + "," + kacinciSutun);
                        if (kacinciSutun == sutun)
                        {
                            kacinciSatir++;
                            kacinciSutun = 1;
                        }
                        else
                        {
                            kacinciSutun++;
                        }
                    }


                }
                //else
                //{

                
                //benimIndexes hesabı
                benimIndexes[0] = satir - 1;
                    benimIndexes[1] = sutun - 1;
                //////////////////////////////////
                //Kaynak ve talep dizilerini oluşturma.//////////////////////////////////////////////
                int[] kaynakDizisi = new int[satir - 1];
                int[] talepDizisi = new int[sutun - 1];


                 kacinciSatir = 1;
                 kacinciSutun = 1;
                foreach (var i in maliyetIFListesi)
                {
                    if (kacinciSutun == sutun)
                    {
                        kaynakDizisi[kacinciSatir - 1] = int.Parse(i.GetComponent<InputField>().text);
                        kacinciSatir++;
                        kacinciSutun = 1;
                    }
                    else
                    {
                        if (kacinciSatir == satir)
                        {
                            talepDizisi[kacinciSutun - 1] = int.Parse(i.GetComponent<InputField>().text);
                        }
                        kacinciSutun++;
                    }
                    i.GetComponent<InputField>().readOnly = true;
                }

                ////////////////////////////////////////////////////////////////////////////////////////////
                //Satır satır maliyet bilgisinin dizilere ve listeye alındığı yer.
                int geciciT = 0;
                    int satirTakip = 1;
                    for (int t = 0; t < satir - 1; t++)
                    {
                        int[] satirMaliyetBilgisi = new int[sutun - 1];
                        kacinciSatir = 1;
                        kacinciSutun = 1;
                        foreach (var i in maliyetIFListesi)
                        {
                            if (t == geciciT && kacinciSutun < sutun && satirTakip == kacinciSatir)
                            {
                                satirMaliyetBilgisi[kacinciSutun - 1] = int.Parse(i.GetComponent<InputField>().text);
                                //Debug.Log("atirMaliyetBilgisi[kacinciSutun - 1]:" + satirMaliyetBilgisi[kacinciSutun - 1]);
                            }

                            //Debug.Log(kacinciSatir + "," + kacinciSutun);
                            if (kacinciSutun == sutun)
                            {
                                kacinciSatir++;
                                kacinciSutun = 1;
                            }
                            else
                            {
                                //satirMaliyetBilgisi[kacinciSutun - 1] = int.Parse(i.GetComponent<InputField>().text);
                                kacinciSutun++;
                            }
                        }
                        geciciT++;
                        satirTakip++;
                        cost_inputs.Add(satirMaliyetBilgisi);
                    }

                    Init(benimIndexes, kaynakDizisi, talepDizisi);
                    NorthWestCornerRule();
                    PrintResult(satir, sutun);
                //}
            }
        }

    }
    public void MaliyetMatrisiniOlustur()
    {
        if (satirIF.text != "" && sutunIF.text != "" && int.Parse(satirIF.text) > 1 && int.Parse(sutunIF.text) > 1)
        {
            satir = int.Parse(satirIF.text);
            satir++;
            sutun = int.Parse(sutunIF.text);
            sutun++;
            MMHBtn.GetComponent<Button>().interactable = false;
            satirIF.GetComponent<InputField>().readOnly = true;
            sutunIF.GetComponent<InputField>().readOnly = true;
        }
        else
        {
            satir = 0;
            sutun = 0;
        }

        if ((satir != 0) && (sutun != 0))
        {
            
            int kacinciSatir = 1;
            int kacinciSutun = 1;
            for (int i= 0; i < satir*sutun -1; i++)
            {

                GameObject gecici = GameObject.Instantiate(matrisKutucugu);
                gecici.transform.SetParent(canvas.transform, false);

                gecici.GetComponent<RectTransform>().anchoredPosition = new Vector2(kacinciSutun * 50, -kacinciSatir * 50);
                gecici.SetActive(true);
                maliyetIFListesi.Add(gecici);
                
               // Debug.Log(kacinciSatir + "," + kacinciSutun);
                if (kacinciSutun == sutun)
                {
                    kacinciSatir++;
                    kacinciSutun = 1;
                }
                else
                {
                    kacinciSutun++;
                }


            }
            float konumDuzeltX = maliyetIFListesi[satir * sutun - 2].GetComponent<RectTransform>().anchoredPosition.x - maliyetIFListesi[0].GetComponent<RectTransform>().anchoredPosition.x;
            float konumDuzeltY = maliyetIFListesi[0].GetComponent<RectTransform>().anchoredPosition.y - maliyetIFListesi[satir * sutun - 2].GetComponent<RectTransform>().anchoredPosition.y;

            foreach (var i in maliyetIFListesi)
            {
                //i.GetComponent<RectTransform>().anchoredPosition = new Vector2(i.GetComponent<RectTransform>().anchoredPosition.x + ksonumDuzeltX, i.GetComponent<RectTransform>().anchoredPosition.y - konumDuzeltY);
                i.GetComponent<RectTransform>().anchoredPosition = new Vector2(i.GetComponent<RectTransform>().anchoredPosition.x, i.GetComponent<RectTransform>().anchoredPosition.y - 75);
            }
        }

    }

    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////
    //////////////////////////////
    /////////////
    /////////
    /////
    ///


    /** 
     * Gönderiler için oluşturulan 'Shipment' sınıfımız bizim
     * gönderi nesnemiz olacaktır.
     */
    class Shipment
    {
        public Shipment(double q, double cpu, int r, int c)
        {
            Quantity = q; // Miktar
            CostPerUnit = cpu; // Birim Maliyet
            R = r; // Request - Talep
            C = c; // Cost - Maliyet
        }

        public double CostPerUnit { get; }

        public double Quantity { get; set; }

        public int R { get; }

        public int C { get; }
    }


        private static int[] demand; // talep
        private static int[] supply; // arz
        private static double[,] costs; // maliyet
        private static Shipment[,] matrix; // Gönderilerin oluşturduğu matris

        static void Init(int[] indexes, int[] sources, int[] destinations)
        {
            //int[] indexes = { 2, 3 };
            int soruceNumber = indexes[0];
            int destinationNumber = indexes[1];

            List<int> src = new List<int>();
            List<int> dst = new List<int>();

            //int[] sources = { 25, 35 };
            for (int i = 0; i < soruceNumber; i++)
            {
                src.Add(sources[i]);
            }

            //int[] destinations = { 20, 30, 10 };
            for (int i = 0; i < destinationNumber; i++)
            {
                dst.Add(destinations[i]);
            }

            int totalSrc = src.Sum(); // Toplam Kaynak
            int totalDst = dst.Sum(); // Toplam Hedef

            /**
                * Eğer toplam kaynak, toplam hedeften büyük ise 
                * aradaki farkı hedef listesine ekliyoruz
                * 
                * Eğer toplam hedef toplam kaynaktan büyük ise 
                * aralarındaki farkı kaynak listesine ekliyoruz
                */
            if (totalSrc > totalDst)
            {
                dst.Add(totalSrc - totalDst);
            }
            else if (totalDst > totalSrc)
            {
                src.Add(totalDst - totalSrc);
            }


            /** 
                * Sınıf global değişkeninlerine değer ataması yapılıyor.
                * Buradaki kaynak ve hedef değerleri tüm uygulamada geçerli
                * olması bekleniyor.
                */
            supply = src.ToArray();
            demand = dst.ToArray();

            costs = new double[supply.Length, demand.Length];
            matrix = new Shipment[supply.Length, demand.Length];


            //List<int[]> cost_inputs = new List<int[]>();
            //int[] cost_1 = { 3, 5, 7 };
            //int[] cost_2 = { 3, 2, 5 };
            //cost_inputs.Add(cost_1);
            //cost_inputs.Add(cost_2);

            for (int i = 0; i < soruceNumber; i++)
            {
                indexes = cost_inputs[i];
                for (int j = 0; j < destinationNumber; j++)
                {
                    costs[i, j] = indexes[j];
                }
            }
        }

        static void NorthWestCornerRule()
        {
            for (int r = 0, nw = 0; r < supply.Length; r++)
            {
                for (int c = nw; c < demand.Length; c++)
                {
                    int quantity = Math.Min(supply[r], demand[c]);
                    if (quantity > 0)
                    {
                        matrix[r, c] = new Shipment(quantity, costs[r, c], r, c);

                        supply[r] -= quantity;
                        demand[c] -= quantity;

                        if (supply[r] == 0)
                        {
                            nw = c;
                            break;
                        }
                    }
                }
            }
        }
    public void PrintResult(int satir, int sutun)
    {
        //Mateis için düzenleme
        List<GameObject> geciciList = new List<GameObject>();
        int kacinciSatir = 1;
        int kacinciSutun = 1;
        foreach (var i in maliyetIFListesi)
        {
            if (kacinciSutun == sutun)
            {
                kacinciSatir++;
                kacinciSutun = 1;
            }
            else
            {
                if (kacinciSatir != satir)
                {
                    //Debug.Log("satir" + satir);
                    Debug.Log("kacinciSatir" + kacinciSatir);
                    //Debug.Log("i.transform.GetChild(2).gameObject.name" + i.transform.GetChild(2).gameObject.name);
                    geciciList.Add(i);
                    //Debug.Log("i.GetComponent<InputField>().text:" + i.GetComponent<InputField>().text);
                    if(i.transform.childCount == 4)
                    {
                        i.transform.GetChild(3).gameObject.GetComponent<Text>().text = i.GetComponent<InputField>().text;
                    }else if (i.transform.childCount == 3)
                    {
                        i.transform.GetChild(2).gameObject.GetComponent<Text>().text = i.GetComponent<InputField>().text;
                    }
                    
                }
                kacinciSutun++;
            }
        }
        Debug.Log("Optimal solution:");
        double totalCosts = 0;
        int geciciSayac = 0;
        cozum.GetComponent<Text>().text += "Çözüm: ";
        for (int r = 0; r < supply.Length; r++)
        {
            for (int c = 0; c < demand.Length; c++)
            {
                Shipment s = matrix[r, c];
                if (s != null && s.R == r && s.C == c)
                {
                    //Debug.Log(s.Quantity);
                    geciciList[geciciSayac].GetComponent<InputField>().text = s.Quantity.ToString();
                    geciciSayac++;
                    totalCosts += (s.Quantity * s.CostPerUnit);
                    cozum.GetComponent<Text>().text += s.Quantity + "." + s.CostPerUnit;
                    if(geciciSayac < supply.Length * demand.Length)
                        cozum.GetComponent<Text>().text += "+";
                }
                else
                {
                    //Debug.Log("  -  ");
                    geciciList[geciciSayac].GetComponent<InputField>().text = "-";
                    //cozum.GetComponent<Text>().text += "0";
                    geciciSayac++;
                }
                
            }
            //Console.WriteLine();
        }
        //Debug.Log("Total costs:" + totalCosts);
        sonuc.GetComponent<Text>().text ="Sonuç: " + totalCosts;
        cozum.GetComponent<Text>().text += " = " + totalCosts;
    }

        /*static void Main()
        {
            Init();
            NorthWestCornerRule();
            SteppingStone();
            PrintResult();
        }*/
    
}




