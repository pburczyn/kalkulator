using System;
using NUnit.Framework;
using Kalkulator;

namespace Kalkulator.Tests
{
    [TestFixture]
    public class CalcTests
    {
        [Test]
        public void konstruktor_inizcjalizacja()
        {
            Calc calc = new Calc(2, Calc.LiczenieTyp.Brutto, Calc.LiczenieMarzaTyp.odSta);

            Assert.AreEqual(calc.Precyzja, 2);
            Assert.AreEqual(Calc.LiczenieTyp.Brutto, calc.SposobLiczenia);
            Assert.AreEqual(Calc.LiczenieMarzaTyp.odSta, calc.SposobLiczeniaMarzy);
        }

        [Test]
        public void sprzedaz_od_netto_przez_ustawienie_ceny_netto()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.LiczenieTyp.Netto, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 1;
            calc.CenaSprzedazyNetto = 28.46M;

            //Assert
            Assert.AreEqual(28.46M, calc.CenaSprzedazyNetto);
            Assert.AreEqual(35.01M, calc.CenaSprzedazyBrutto);
        }

        [Test]
        public void sprzedaz_od_netto_przez_ustawienie_ceny_brutto()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.LiczenieTyp.Netto, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 1;
            calc.CenaSprzedazyBrutto = 35.00M;

            //Assert
            Assert.AreEqual(28.46M, calc.CenaSprzedazyNetto);
            Assert.AreEqual(35.01M, calc.CenaSprzedazyBrutto);
        }

        [Test]
        public void sprzedaz_od_netto_przez_ustawienie_ceny_brutto_wylicz_wartosci()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.LiczenieTyp.Netto, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 2;
            calc.CenaSprzedazyBrutto = 35.00M;

            //Assert
            Assert.AreEqual(56.92M, calc.WartoscSprzedazyNetto);
            Assert.AreEqual(70.01M, calc.WartoscSprzedazyBrutto);
        }

        [Test]
        public void sprzedaz_od_netto_przez_ustawienie_ceny_brutto_wylicz_wartosci_inny_przypadek()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.LiczenieTyp.Netto, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 4;
            calc.CenaSprzedazyBrutto = 150;

            //Assert
            Assert.AreEqual(121.95M, calc.CenaSprzedazyNetto);
            Assert.AreEqual(487.80M, calc.WartoscSprzedazyNetto);
            Assert.AreEqual(599.99M, calc.WartoscSprzedazyBrutto);
        }

        [Test]
        public void sprzedaz_od_brutto_przez_ustawienie_ceny_brutto_wylicz_wartosci_inny_przypadek()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.LiczenieTyp.Brutto, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 4;
            calc.CenaSprzedazyBrutto = 150;

            //Assert
            Assert.AreEqual(121.95M, calc.CenaSprzedazyNetto);
            Assert.AreEqual(487.80M, calc.WartoscSprzedazyNetto);
            Assert.AreEqual(600M, calc.WartoscSprzedazyBrutto);
        }

        [TestCase(Calc.LiczenieTyp.Brutto, 23, 35, 56.91, 70)]
        [TestCase(Calc.LiczenieTyp.Netto, 23, 35, 56.92, 70.01)]
        public void zmiana_ilosc_powoduje_zmiene_wartosci_sprzedazy(Calc.LiczenieTyp typ, int vat, decimal cenaBrutto, decimal wartoscNetto, decimal wartoscBrutto)
        {
            //Arrange
            Calc calc = new Calc(2, typ, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.VatProc = vat;
            calc.Ilosc = 1;
            calc.CenaSprzedazyBrutto = cenaBrutto;
            calc.Ilosc = 2;

            //Assert
            Assert.AreEqual(wartoscNetto, calc.WartoscSprzedazyNetto);
            Assert.AreEqual(wartoscBrutto, calc.WartoscSprzedazyBrutto);
        }

        [TestCase(2, 1.224, 1.22)]
        [TestCase(2, 1.225, 1.23)]
        [TestCase(2, 1.234, 1.23)]
        [TestCase(2, 1.235, 1.24)]
        [TestCase(3, 1.2224, 1.222)]
        [TestCase(3, 1.2225, 1.223)]
        [TestCase(3, 1.2334, 1.233)]
        [TestCase(3, 1.2335, 1.234)]
        public void zmiana_zaokraglenia_cena_sprzedazy_netto(int precyzja, decimal cena, decimal cenaZaokraglona)
        {
            //Arrange
            Calc calc = new Calc(precyzja, Calc.LiczenieTyp.Netto, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.CenaSprzedazyNetto = cena;

            //Assert
            Assert.AreEqual(cenaZaokraglona, calc.CenaSprzedazyNetto);
        }

        [TestCase(2, 1.224, 1.22)]
        [TestCase(2, 1.225, 1.23)]
        [TestCase(2, 1.234, 1.23)]
        [TestCase(2, 1.235, 1.24)]
        [TestCase(3, 1.2224, 1.222)]
        [TestCase(3, 1.2225, 1.223)]
        [TestCase(3, 1.2334, 1.233)]
        [TestCase(3, 1.2335, 1.234)]
        public void zmiana_zaokraglenia_cena_sprzedazy_brutto(int precyzja, decimal cena, decimal cenaZaokraglona)
        {
            //Arrange
            Calc calc = new Calc(precyzja, Calc.LiczenieTyp.Brutto, Calc.LiczenieMarzaTyp.odSta);

            //Act
            calc.CenaSprzedazyBrutto = cena;

            //Assert
            Assert.AreEqual(cenaZaokraglona, calc.CenaSprzedazyBrutto);
        }
    }
}
