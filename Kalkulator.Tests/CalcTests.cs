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
            Calc calc = new Calc(2, Calc.TypyLiczenia.Brutto);

            Assert.AreEqual(calc.Precyzja, 2);
            Assert.AreEqual(Calc.TypyLiczenia.Brutto, calc.SposobLiczenia);
        }

        [Test]
        public void sprzedaz_od_netto_przez_ustawienie_ceny_netto()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

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
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

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
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

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
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

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
            Calc calc = new Calc(2, Calc.TypyLiczenia.Brutto);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 4;
            calc.CenaSprzedazyBrutto = 150;

            //Assert
            Assert.AreEqual(121.95M, calc.CenaSprzedazyNetto);
            Assert.AreEqual(487.80M, calc.WartoscSprzedazyNetto);
            Assert.AreEqual(600M, calc.WartoscSprzedazyBrutto);
        }

        [TestCase(Calc.TypyLiczenia.Brutto, 23, 35, 56.91, 70)]
        [TestCase(Calc.TypyLiczenia.Netto, 23, 35, 56.92, 70.01)]
        public void zmiana_ilosc_powoduje_zmiene_wartosci_sprzedazy(Calc.TypyLiczenia typ, int vat, decimal cenaBrutto, decimal wartoscNetto, decimal wartoscBrutto)
        {
            //Arrange
            Calc calc = new Calc(2, typ);

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
            Calc calc = new Calc(precyzja, Calc.TypyLiczenia.Netto);

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
            Calc calc = new Calc(precyzja, Calc.TypyLiczenia.Brutto);

            //Act
            calc.CenaSprzedazyBrutto = cena;

            //Assert
            Assert.AreEqual(cenaZaokraglona, calc.CenaSprzedazyBrutto);
        }

        [Test]
        public void zakup_od_netto_przez_ustawienie_ceny_netto()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 1;
            calc.CenaZakupuNetto = 28.46M;

            //Assert
            Assert.AreEqual(28.46M, calc.CenaZakupuNetto);
            Assert.AreEqual(35.01M, calc.CenaZakupuBrutto);
        }

        [Test]
        public void zakup_od_netto_przez_ustawienie_ceny_brutto()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

            //Act
            calc.VatProc = 23;
            calc.Ilosc = 1;
            calc.CenaZakupuBrutto = 35.00M;

            //Assert
            Assert.AreEqual(28.46M, calc.CenaZakupuNetto);
            Assert.AreEqual(35.01M, calc.CenaZakupuBrutto);
        }

        [TestCase(90, 18.18, 110, 22.22)]
        public void wyliczanie_sprzedazy_na_podstawie_marzy(decimal cenaZakupu, decimal marza, decimal oczekiwanaCenaSprzedazy, decimal oczekiwanyNarzut)
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

            //Act
            calc.CenaZakupuNetto = cenaZakupu;
            calc.Marza = marza;

            //Assert
            Assert.AreEqual(cenaZakupu, calc.CenaZakupuNetto);
            Assert.AreEqual(marza, calc.Marza);
            Assert.AreEqual(oczekiwanaCenaSprzedazy, calc.CenaSprzedazyNetto);
            Assert.AreEqual(oczekiwanyNarzut, calc.Narzut);
        }

        [TestCase(90, 22.22, 110, 18.18)]
        [TestCase(35, 30, 45.5, 23.08)]
        public void wyliczanie_sprzedazy_na_podstawie_narzutu(decimal cenaZakupu, decimal narzut, decimal oczekiwanaCenaSprzedazy, decimal oczekiwanaMarza)
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

            //Act
            calc.CenaZakupuNetto = cenaZakupu;
            calc.Narzut = narzut;

            //Assert
            Assert.AreEqual(oczekiwanaCenaSprzedazy, calc.CenaSprzedazyNetto);
            Assert.AreEqual(oczekiwanaMarza, calc.Marza);
        }

        [TestCase(90, 110, 18.18, 22.22)]
        [TestCase(0, 123, 100, null)]
        [TestCase(0, 0, null, null)]
        [TestCase(1, 1, 0, 0)]
        public void wyliczanie_marzy_na_podstawie_zakup_sprzedaz(decimal cenaZakupu, decimal cenaSprzedazy, decimal? oczekiwanaMarza, decimal? oczekiwanyNarzut)
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Netto);

            //Act
            calc.CenaZakupuNetto = cenaZakupu;
            calc.CenaSprzedazyNetto = cenaSprzedazy;

            //Assert
            Assert.AreEqual(oczekiwanaMarza, calc.Marza);
            Assert.AreEqual(oczekiwanyNarzut, calc.Narzut);
        }

        [Test]
        public void zmiana_vat_powoduje_zmiane_brutto()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Brutto);

            //Act
            calc.CenaZakupuNetto = 10M;
            calc.CenaSprzedazyNetto = 10M;
            calc.VatProc = 23;

            //Assert
            Assert.AreEqual(10M, calc.CenaZakupuNetto);
            Assert.AreEqual(10M, calc.CenaSprzedazyNetto);
            Assert.AreEqual(12.3M, calc.CenaZakupuBrutto);
            Assert.AreEqual(12.3M, calc.CenaSprzedazyBrutto);

            calc.VatProc = 8;

            //Assert
            Assert.AreEqual(10M, calc.CenaZakupuNetto);
            Assert.AreEqual(10M, calc.CenaSprzedazyNetto);
            Assert.AreEqual(10.8M, calc.CenaZakupuBrutto);
            Assert.AreEqual(10.8M, calc.CenaSprzedazyBrutto);
        }

        [Test]
        public void rabat_zbyt_wysoki_excetpion()
        {
            //Arrange
            Calc calc = new Calc(2, Calc.TypyLiczenia.Brutto);

            //Act
            calc.Ilosc = 1;
            calc.CenaSprzedazyBrutto = 10;

            //Assert
            Assert.Catch<CalcRabatException>(() => calc.Rabat = 100);
            //Assert.Throws(Is.TypeOf<CalcRabatException>().And.Message)
        }

        [TestCase(Calc.TypyLiczenia.Netto, 150, 4, 6, 458.52, 563.98)]
        [TestCase(Calc.TypyLiczenia.Brutto, 150, 4, 6, 458.54, 564)]
        public void rabat_poprawnosc_wyliczania(Calc.TypyLiczenia typ, decimal cenaBrutto, decimal ilosc, decimal rabat, decimal oczekiwanaWN, decimal oczekiwanaWB)
        {
            //Arrange
            Calc calc = new Calc(2, typ);
            
            //Act
            calc.VatProc = 23;
            calc.Ilosc = ilosc;
            calc.CenaSprzedazyBrutto = cenaBrutto;
            calc.Rabat = rabat;

            //Assert
            Assert.AreEqual(oczekiwanaWN, calc.WartoscSprzedazyNetto);
            Assert.AreEqual(oczekiwanaWB, calc.WartoscSprzedazyBrutto);
        }

    }
}
