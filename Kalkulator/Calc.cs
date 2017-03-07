using System;

namespace Kalkulator
{
    public class Calc
    {
        public enum TypyLiczenia { Netto, Brutto }
        //public enum TypyLiczeniaMarzy { wStu, odSta }

        public Calc(int precyzja, TypyLiczenia sposob)
        {
            Precyzja = precyzja;
            SposobLiczenia = sposob;
        }

        public int Precyzja { get; private set; }
        public TypyLiczenia SposobLiczenia { get; private set; }
        //public TypyLiczeniaMarzy SposobLiczeniaMarzy { get; private set; }

        private decimal _vatProc;
        public decimal VatProc
        {
            get { return _vatProc; }
            set
            {
                _vatProc = value;
                PrzeliczZakup(TypyLiczenia.Netto);
                PrzeliczSprzedaz(TypyLiczenia.Netto);
            }
        }

        private decimal _ilosc;
        public decimal Ilosc
        {
            get { return _ilosc; }
            set
            {
                _ilosc = value;
                PrzeliczSprzedaz(SposobLiczenia);
            }
        }

        private decimal _cenaSprzedazyNetto;
        public decimal CenaSprzedazyNetto
        {
            get { return _cenaSprzedazyNetto; }
            set
            {
                _cenaSprzedazyNetto = value;
                PrzeliczSprzedaz(TypyLiczenia.Netto);
                _narzut = WyliczNarzut(_cenaZakupuNetto, _cenaSprzedazyNetto);
                _marza = WyliczMarze(_cenaZakupuNetto, _cenaSprzedazyNetto);
            }
        }
        private decimal _cenaSprzedazyBrutto;
        public decimal CenaSprzedazyBrutto
        {
            get { return _cenaSprzedazyBrutto; }
            set
            {
                _cenaSprzedazyBrutto = value;
                PrzeliczSprzedaz(TypyLiczenia.Brutto);
                _narzut = WyliczNarzut(_cenaZakupuBrutto, _cenaSprzedazyBrutto);
                _marza = WyliczMarze(_cenaZakupuBrutto, _cenaSprzedazyBrutto);
            }
        }
        public decimal WartoscSprzedazyNetto { get; private set; }
        public decimal WartoscSprzedazyBrutto { get; private set; }

        private void PrzeliczSprzedaz(TypyLiczenia typ)
        {
            if (typ != SposobLiczenia)
            {
                if (typ == TypyLiczenia.Netto)
                    _cenaSprzedazyBrutto = OdNetto(CenaSprzedazyNetto);
                else
                    _cenaSprzedazyNetto = OdBrutto(CenaSprzedazyBrutto);
            }

            if (SposobLiczenia == TypyLiczenia.Netto)
            {
                _cenaSprzedazyNetto = DostosujZaokraglenie(_cenaSprzedazyNetto);
                _cenaSprzedazyBrutto = OdNetto(CenaSprzedazyNetto);
                WartoscSprzedazyNetto = WartoscNaPodstawieCeny(CenaSprzedazyNetto, _rabat);
                WartoscSprzedazyBrutto = OdNetto(WartoscSprzedazyNetto);
            }
            else
            {
                _cenaSprzedazyBrutto = DostosujZaokraglenie(_cenaSprzedazyBrutto);
                _cenaSprzedazyNetto = OdBrutto(CenaSprzedazyBrutto);
                WartoscSprzedazyBrutto = WartoscNaPodstawieCeny(CenaSprzedazyBrutto, _rabat);
                WartoscSprzedazyNetto = OdBrutto(WartoscSprzedazyBrutto);
            }
        }

        private decimal OdNetto(decimal wartosc)
        {
            return DostosujZaokraglenie(wartosc * (VatProc / 100.0M + 1));
        }

        private decimal OdBrutto(decimal wartosc)
        {
            return DostosujZaokraglenie(wartosc / (VatProc / 100.0M + 1));
        }

        private decimal WartoscNaPodstawieCeny(decimal cena, decimal rabat)
        {
            return DostosujZaokraglenie(DostosujZaokraglenie(cena * (100 - rabat) / 100) * Ilosc);
        }

        private decimal DostosujZaokraglenie(decimal wartosc)
        {
            return Math.Round(wartosc, Precyzja, MidpointRounding.AwayFromZero);
        }

        private decimal _cenaZakupuNetto;
        public decimal CenaZakupuNetto
        {
            get { return _cenaZakupuNetto; }
            set
            {
                _cenaZakupuNetto = value;
                PrzeliczZakup(TypyLiczenia.Netto);
            }
        }

        private decimal _cenaZakupuBrutto;
        public decimal CenaZakupuBrutto
        {
            get { return _cenaZakupuBrutto; }
            set
            {
                _cenaZakupuBrutto = value;
                PrzeliczZakup(TypyLiczenia.Brutto);
            }
        }

        public decimal WartoscZakupuNetto { get; private set; }
        public decimal WartoscZakupuBrutto { get; private set; }

        private void PrzeliczZakup(TypyLiczenia typ)
        {
            if (typ != SposobLiczenia)
            {
                if (typ == TypyLiczenia.Netto)
                    _cenaZakupuBrutto = OdNetto(CenaZakupuNetto);
                else
                    _cenaZakupuNetto = OdBrutto(CenaZakupuBrutto);
            }

            if (SposobLiczenia == TypyLiczenia.Netto)
            {
                _cenaZakupuNetto = DostosujZaokraglenie(_cenaZakupuNetto);
                _cenaZakupuBrutto = OdNetto(CenaZakupuNetto);
                WartoscZakupuNetto = WartoscNaPodstawieCeny(CenaZakupuNetto, 0);
                WartoscZakupuBrutto = OdNetto(WartoscZakupuNetto);
            }
            else
            {
                _cenaZakupuBrutto = DostosujZaokraglenie(_cenaZakupuBrutto);
                _cenaZakupuNetto = OdBrutto(CenaZakupuBrutto);
                WartoscZakupuBrutto = WartoscNaPodstawieCeny(CenaZakupuBrutto, 0);
                WartoscZakupuNetto = OdBrutto(WartoscZakupuBrutto);
            }
        }

        private decimal? _marza = null;
        public decimal? Marza
        {
            get { return _marza; }
            set
            {
                _marza = value;
                if (SposobLiczenia == TypyLiczenia.Netto)
                    CenaSprzedazyNetto = WyliczCenePoMarzy(_cenaZakupuNetto, _marza);
                else
                    CenaSprzedazyBrutto = WyliczCenePoMarzy(_cenaZakupuBrutto, _marza);
            }
        }

        private decimal? _narzut = null;
        public decimal? Narzut
        {
            get { return _narzut; }
            set 
            { 
                _narzut = value;
                if (SposobLiczenia == TypyLiczenia.Netto)
                    CenaSprzedazyNetto = WyliczCenePoNarzucie(_cenaZakupuNetto, _narzut);
                else
                    CenaSprzedazyBrutto = WyliczCenePoNarzucie(_cenaZakupuBrutto, _narzut);
            }
        }


        private decimal? WyliczMarze(decimal cenaZakup, decimal cenaSprzedazy)
        {
            if (cenaSprzedazy == 0)
                return null;

            return DostosujZaokraglenie((cenaSprzedazy - cenaZakup) / cenaSprzedazy * 100);
        }

        private decimal? WyliczNarzut(decimal cenaZakup, decimal cenaSprzedazy)
        {
            if (cenaZakup == 0)
                return null;

            return DostosujZaokraglenie((cenaSprzedazy - cenaZakup) / cenaZakup * 100);
        }

        private decimal WyliczCenePoMarzy(decimal cenaZakupu, decimal? marza)
        {
            if (marza == null)
                return 0;

            _narzut = DostosujZaokraglenie((decimal)(100 / (100 - marza) * 100 - 100));

            return DostosujZaokraglenie((decimal)(cenaZakupu * (_narzut / 100 + 1)));
        }

        private decimal WyliczCenePoNarzucie(decimal cenaZakupu, decimal? narzut)
        {
            if (narzut == null)
                return 0;

            _marza = DostosujZaokraglenie((decimal)(100 - 100 / (narzut + 100) * 100));

            return DostosujZaokraglenie((decimal)(cenaZakupu * (_narzut / 100 + 1)));
        }

        private decimal _rabat;
        public decimal Rabat
        {
            get { return _rabat; }
            set 
            {
                if (value >= 100)
                    throw new CalcRabatException("Zbyt wysoki rabat.");

                _rabat = value;
                PrzeliczSprzedaz(SposobLiczenia);
            }
        }

        private decimal WyliczIloscNaPodstawieWartosci(decimal cena, decimal wartosc)
        {
            return wartosc / cena;
        }
    }
}
