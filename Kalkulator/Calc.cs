using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalkulator
{
    public class Calc
    {
        public enum LiczenieTyp { Netto, Brutto }
        public enum LiczenieMarzaTyp { wStu, odSta }

        public Calc(int precyzja, LiczenieTyp sposob, LiczenieMarzaTyp marzaSposob)
        {
            Precyzja = precyzja;
            SposobLiczenia = sposob;
            SposobLiczeniaMarzy = marzaSposob;
        }

        public int Precyzja { get; private set; }
        public LiczenieTyp SposobLiczenia { get; private set; }
        public LiczenieMarzaTyp SposobLiczeniaMarzy { get; private set; }

        private decimal _vatProc;
        public decimal VatProc
        {
            get { return _vatProc; }
            set 
            { 
                _vatProc = value;
                PrzeliczSprzedaz(SposobLiczenia);
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
                PrzeliczSprzedaz(LiczenieTyp.Netto);
            }
        }
        private decimal _cenaSprzedazyBrutto;
        public decimal CenaSprzedazyBrutto
        {
            get { return _cenaSprzedazyBrutto; }
            set
            {
                _cenaSprzedazyBrutto = value;
                PrzeliczSprzedaz(LiczenieTyp.Brutto);
            }
        }
        public decimal WartoscSprzedazyNetto { get; private set; }
        public decimal WartoscSprzedazyBrutto { get; private set; }

        private void PrzeliczSprzedaz(LiczenieTyp typ)
        {
            if (typ != SposobLiczenia)
            {
                if (typ == LiczenieTyp.Netto)
                    _cenaSprzedazyBrutto = OdNetto(CenaSprzedazyNetto);
                else
                    _cenaSprzedazyNetto = OdBrutto(CenaSprzedazyBrutto);
            }

            if (SposobLiczenia == LiczenieTyp.Netto)
            {
                _cenaSprzedazyNetto = DostosujZaokraglenie(_cenaSprzedazyNetto);
                _cenaSprzedazyBrutto = OdNetto(CenaSprzedazyNetto);
                WartoscSprzedazyNetto = WartoscNaPodstawieCeny(CenaSprzedazyNetto);
                WartoscSprzedazyBrutto = OdNetto(WartoscSprzedazyNetto);
            }
            else
            {
                _cenaSprzedazyBrutto = DostosujZaokraglenie(_cenaSprzedazyBrutto);
                _cenaSprzedazyNetto = OdBrutto(CenaSprzedazyBrutto);
                WartoscSprzedazyBrutto = WartoscNaPodstawieCeny(CenaSprzedazyBrutto);
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

        private decimal WartoscNaPodstawieCeny(decimal cena)
        {
            return DostosujZaokraglenie(cena * Ilosc);
        }

        private decimal DostosujZaokraglenie(decimal wartosc)
        {
            return Math.Round(wartosc, Precyzja, MidpointRounding.AwayFromZero);
        }
    }
}
