using System;

class Hesap
{
    public string HesapNo { get; set; }
    public string HesapSahibi { get; set; }
    public decimal Bakiye { get; set; }

    public virtual void ParaYatir(decimal miktar)
    {
        Bakiye += miktar;
        Console.WriteLine($"{miktar:C} yatırıldı. Güncel bakiye: {Bakiye:C}");
    }

    public virtual void ParaCek(decimal miktar)
    {
        if (Bakiye >= miktar)
        {
            Bakiye -= miktar;
            Console.WriteLine($"{miktar:C} çekildi. Güncel bakiye: {Bakiye:C}");
        }
        else
        {
            Console.WriteLine("Yetersiz bakiye.");
        }
    }

    public virtual void BilgiYazdir()
    {
        Console.WriteLine($"Hesap No: {HesapNo}");
        Console.WriteLine($"Hesap Sahibi: {HesapSahibi}");
        Console.WriteLine($"Bakiye: {Bakiye:C}");
    }
}

class VadesizHesap : Hesap
{
    public decimal EkHesapLimiti { get; set; }

    public override void ParaCek(decimal miktar)
    {
        if (Bakiye + EkHesapLimiti >= miktar)
        {
            decimal fark = miktar - Bakiye;
            if (fark > 0)
            {
                EkHesapLimiti -= fark;
                Bakiye = 0;
            }
            else
            {
                Bakiye -= miktar;
            }
            Console.WriteLine($"{miktar:C} çekildi. Güncel bakiye: {Bakiye:C}, Ek hesap limiti: {EkHesapLimiti:C}");
        }
        else
        {
            Console.WriteLine("Yetersiz bakiye ve ek hesap limiti.");
        }
    }

    public override void BilgiYazdir()
    {
        base.BilgiYazdir();
        Console.WriteLine($"Ek Hesap Limiti: {EkHesapLimiti:C}");
    }
}

class VadeliHesap : Hesap
{
    public int VadeSuresi { get; set; } // Gün cinsinden
    public decimal FaizOrani { get; set; } // Yüzde cinsinden

    public override void ParaCek(decimal miktar)
    {
        if (VadeSuresi > 0)
        {
            Console.WriteLine("Vade dolmadan para çekme işlemi yapılamaz.");
        }
        else
        {
            base.ParaCek(miktar);
        }
    }

    public override void BilgiYazdir()
    {
        base.BilgiYazdir();
        Console.WriteLine($"Vade Süresi: {VadeSuresi} gün");
        Console.WriteLine($"Faiz Oranı: {FaizOrani}%");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Hesap Türünü Seçiniz (1: Vadesiz, 2: Vadeli): ");
        int secim = int.Parse(Console.ReadLine());

        Hesap hesap = null;

        if (secim == 1)
        {
            hesap = new VadesizHesap();
            Console.WriteLine("Hesap Sahibinin Adını Giriniz:");
            hesap.HesapSahibi = Console.ReadLine();
            Console.WriteLine("Hesap Numarasını Giriniz:");
            hesap.HesapNo = Console.ReadLine();
            Console.WriteLine("Başlangıç Bakiyesini Giriniz:");
            hesap.Bakiye = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Ek Hesap Limitini Giriniz:");
            ((VadesizHesap)hesap).EkHesapLimiti = decimal.Parse(Console.ReadLine());
        }
        else if (secim == 2)
        {
            hesap = new VadeliHesap();
            Console.WriteLine("Hesap Sahibinin Adını Giriniz:");
            hesap.HesapSahibi = Console.ReadLine();
            Console.WriteLine("Hesap Numarasını Giriniz:");
            hesap.HesapNo = Console.ReadLine();
            Console.WriteLine("Başlangıç Bakiyesini Giriniz:");
            hesap.Bakiye = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Vade Süresini (gün):");
            ((VadeliHesap)hesap).VadeSuresi = int.Parse(Console.ReadLine());
            Console.WriteLine("Faiz Oranını Giriniz (%):");
            ((VadeliHesap)hesap).FaizOrani = decimal.Parse(Console.ReadLine());
        }
        else
        {
            Console.WriteLine("Geçersiz seçim!");
            return;
        }

        Console.WriteLine("Hangi işlemi yapmak istiyorsunuz? (1: Para Yatır, 2: Para Çek, 3: Bilgi Yazdır): ");
        int islem = int.Parse(Console.ReadLine());

        switch (islem)
        {
            case 1:
                Console.WriteLine("Yatırmak istediğiniz miktarı giriniz:");
                decimal yatirilanMiktar = decimal.Parse(Console.ReadLine());
                hesap.ParaYatir(yatirilanMiktar);
                break;

            case 2:
                Console.WriteLine("Çekmek istediğiniz miktarı giriniz:");
                decimal cekilenMiktar = decimal.Parse(Console.ReadLine());
                hesap.ParaCek(cekilenMiktar);
                break;

            case 3:
                hesap.BilgiYazdir();
                break;

            default:
                Console.WriteLine("Geçersiz işlem!");
                break;
        }
    }
}
