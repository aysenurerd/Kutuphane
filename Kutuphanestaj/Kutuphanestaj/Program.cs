using System;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Collections.Generic;


class Program
{

    public static void Main()
    {
        Kutuphane kutuphane = new Kutuphane();
        Kitap kitap = new Kitap();
        int secim;

        while (true)
        {
            Console.WriteLine("1- Kitap Ekle");
            Console.WriteLine("2- Kitapları Göster");
            Console.WriteLine("3- Kitap Bul");
            Console.WriteLine("4- Ödünç Alma");
            Console.WriteLine("5- Geri Verme");
            Console.WriteLine("6- Ödünç alınanları Göster");
            Console.WriteLine("0- Çıkış");

            try
            {
                secim = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Geçerli bir değer giriniz");
                continue; // Hatalı giriş durumunda döngüyü tekrar başlat
            }


            switch (secim)
        {
                case 1:
                    Kitap eklenecekkitap = new Kitap();
                    while (true)
                    {
                        try
                        {
                            Console.Write("Başlık: ");
                            eklenecekkitap.baslik = Console.ReadLine();

                            Console.Write("Yazar: ");
                            eklenecekkitap.yazar = Console.ReadLine();

                            Console.Write("ISBN: ");
                            eklenecekkitap.isbn = Console.ReadLine();

                            Console.Write("Kopya Sayısı: ");
                            eklenecekkitap.kopyasayisi = Convert.ToInt32(Console.ReadLine());

                            break; // Bilgiler doğru alındığında döngüden çık
                        }
                        catch
                        {
                            Console.WriteLine("Hatalı bilgi girdiniz. Tekrar deneyin.");
                        }
                    }
                    Console.Clear();

                    kutuphane.KitapEkle(eklenecekkitap);
                    break;

                case 2:
                    Console.Clear();

                    kutuphane.KitapGoster();
                break;
                case 3:
                    while (true)
                    {
                        try
                        {
                            Console.Write("Kitap Adı veya Yazar Adı: ");
                            string input = Console.ReadLine();
                            Console.WriteLine(kutuphane.KitapBul(input));
                            break; // Doğru giriş yapıldığında döngüden çık
                        }
                        catch
                        {
                            Console.WriteLine("Geçerli Kitap Adı giriniz");
                        }
                    }
                    Console.Clear();

                    break;
                case 4:
                    while (true)
                    {


                        try
                        {
                            Console.WriteLine("ISBN Girin: ");
                            kutuphane.oduncAlma(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Gecerli Kitap ISBN giriniz");
                        }
                        break;
                    }
                    Console.Clear();


                    break;
            case 5:
                    while (true)
                    {


                        try
                        {
                            kutuphane.geriVerme(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Gecerli Kitap ISBN giriniz");
                        }
                        break;
                    }
                    Console.Clear();


                    break;
            case 6:
                    Console.Clear();
                    kutuphane.gecmislerigoster();
                    
                    break;

            case 0:
                    Console.Clear();
                    Console.WriteLine("Programdan çıkılıyor...");
                Environment.Exit(0);
                break;
                default:
                    Console.WriteLine("Geçersiz bir seçim yaptınız. Lütfen tekrar deneyin.");
                    break;
            }
            
        }
    }
    















    class Kitap
    {
        public string baslik { get; set; }
        public string yazar { get; set; }
        public string isbn { get; set; }
        public int kopyasayisi { get; set; }
        public DateTime odunctarihi { get; set; }
        public DateTime donustarihi { get; set; }



        public override string ToString()
        {
            return $"Baslik: {baslik}\nYazar: {yazar}\nISBN: {isbn}\nKopya Sayisi: {kopyasayisi}";
                
                
        }
    }






    class Kutuphane
    {

        public List<Kitap> anakutuphane= new List<Kitap>();
        public List<Kitap> okutuphane= new List<Kitap>();

        //yeni kitap ekleme
        public void KitapEkle(Kitap kitap)
        {
         anakutuphane.Add(kitap);
            
        }
        //kütüphanedeki tüm kitapları gösterme
        public void KitapGoster()
        {
            foreach(Kitap k in   anakutuphane) 
            { 
            Console.WriteLine(k.ToString()+"\n");
            
            
            }    
        }

        //başlığa veya yazara göre aratma
        public string KitapBul(string input)
        {
            string sonuc="*****\n";
            foreach(Kitap kitap in anakutuphane)
            {
                if(kitap.yazar.ToLower().Contains(input.ToLower()) || kitap.baslik.ToLower().Contains(input.ToLower() ))
                {

                    sonuc += kitap.ToString();
                }
            }


            return sonuc;
        }

        //ödünç alma
        public void oduncAlma(string isbn)
        {
            foreach(Kitap kitap in anakutuphane)
            {
                if (isbn == kitap.isbn)
                {
                    kitap.kopyasayisi--;
                    kitap.odunctarihi=DateTime.Now;
                    kitap.donustarihi=DateTime.Now.AddDays(14);
                    okutuphane.Add(kitap);


                }
            }

        }

        //iade etme
        public void geriVerme(string isbn)
        {
            try { 
            Kitap silinecek=null;
            foreach (Kitap k in okutuphane)
            {
                foreach(Kitap ek in anakutuphane)
                {

                
                    if(k.isbn == isbn && ek.isbn== isbn)
                    {
                        ek.kopyasayisi++;
                        silinecek=k; 
                    }
                }
            }

            okutuphane.Remove(silinecek);
            }
            catch 
            {
                Console.WriteLine("Null Silme hatası");
            }
        }
        //ödünc alinanlari göster
        public void gecmislerigoster()
        {
            foreach (Kitap k in okutuphane)
            {
                if(k.donustarihi<DateTime.Now)
                { 
                Console.WriteLine("Kitabın teslim süresi gecti"+k.ToString());
                }
                else
                {
                    Console.WriteLine(k.ToString() + "\n" + k.donustarihi.ToString());

                }
            }
        }




    }

















}