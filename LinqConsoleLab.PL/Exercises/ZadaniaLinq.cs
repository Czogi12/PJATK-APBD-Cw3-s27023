using System.Runtime.InteropServices.JavaScript;
using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci.Where(student => student.Miasto == "Warsaw")
            .Select(student => $"{student.Imie}, {student.Nazwisko}, {student.Miasto}");
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci.Select(s => s.Email);
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci.OrderBy((s) => s.Nazwisko)
            .ThenBy(s => s.Imie).Select(s => $"{s.NumerIndeksu} {s.Nazwisko} {s.Imie}");
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var ret = DaneUczelni.Przedmioty.FirstOrDefault(p => p.Kategoria == "Analytics");

        if (ret is null) return ["Brak przedmiotu"];
        return [$"{ret.Nazwa} {ret.DataStartu}"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        return DaneUczelni.Zapisy.FirstOrDefault(z => !z.CzyAktywny) is null ? ["Nie"] : ["Tak"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        return DaneUczelni.Prowadzacy.All(p => p.Katedra.Length != 0) ? ["Tak"] : ["Nie"];
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        return [DaneUczelni.Zapisy.Count(z => z.CzyAktywny).ToString()];
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci.Select(s => s.Miasto).Distinct().OrderBy(s => s);
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy.OrderByDescending(z => z.DataZapisu).Take(3)
            .Select(z => $"{z.DataZapisu} {z.StudentId} {z.PrzedmiotId}");
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty.OrderBy(p => p.Nazwa).Skip(2).Take(2).Select(p => $"{p.Nazwa} {p.Kategoria}");
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci.Join(
            DaneUczelni.Zapisy,
            s => s.Id,
            z => z.StudentId,
            (s, z) => $"{s.Imie} {s.Nazwisko} {z.DataZapisu}"
        );
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Zapisy.Join(
            DaneUczelni.Studenci,
            z => z.StudentId,
            s => s.Id,
            (zapis, student) => new { zapis, student }
        ).Join(
            DaneUczelni.Przedmioty,
            zs => zs.zapis.PrzedmiotId,
            p => p.Id,
            ((zs, przedmiot) => $"{zs.student.Imie} {zs.student.Nazwisko} {przedmiot.Nazwa}")
        );
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Przedmioty.GroupJoin(
            DaneUczelni.Zapisy,
            (przedmiot => przedmiot.Id),
            zapis => zapis.PrzedmiotId,
            (przedmiot, enumerable) => $"{przedmiot.Nazwa} {enumerable.Count()}"
        );
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Przedmioty.GroupJoin(
            DaneUczelni.Zapisy,
            przedmiot => przedmiot.Id,
            zapis => zapis.PrzedmiotId,
            (przedmiot, enumerable) =>
                $"{przedmiot.Nazwa} {enumerable.Where(z => z.OcenaKoncowa is not null).Average(z => z.OcenaKoncowa)}"
        );
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy.GroupJoin(
            DaneUczelni.Przedmioty,
            prowadzacy => prowadzacy.Id,
            przedmioty => przedmioty.ProwadzacyId,
            (prowadzacy, przedmiots) => $"${prowadzacy.Imie} ${prowadzacy.Nazwisko} ${przedmiots.Count()}"
        );
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Studenci.GroupJoin(
            DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa is not null),
            s => s.Id,
            z => z.StudentId,
            (student, enumerable) => $"{student.Imie} {student.Nazwisko} {enumerable.Max(z => z.OcenaKoncowa)}"
        );
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Studenci.GroupJoin(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, enumerable) => new { student, enumerable }
            )
            .Where(a => a.enumerable.Any())
            .Select(a => $"{a.student.Imie} {a.student.Nazwisko} {a.enumerable.Count()}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty.GroupJoin(
                DaneUczelni.Zapisy.Where(z => z.DataZapisu.Month == 4 && z.DataZapisu.Year == 2026),
                przedmioty => przedmioty.Id,
                zapis => zapis.PrzedmiotId,
                (przedmiot, zapisy) => new { przedmiot, zapisy }
            )
            .Where(a => a.zapisy.Any() && a.zapisy.All(zapis => zapis.OcenaKoncowa is null))
            .Select(a => a.przedmiot.Nazwa);
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        return DaneUczelni.Prowadzacy.GroupJoin(
            DaneUczelni.Przedmioty.GroupJoin(
                DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa is not null),
                przedmiot => przedmiot.Id,
                zapis => zapis.PrzedmiotId,
                (przedmiot, zapisy) => new { przedmiot, Srednia = zapisy.Average(z => z.OcenaKoncowa) }
            ),
            prowadzacy => prowadzacy.Id,
            p => p.przedmiot.ProwadzacyId,
            (prowadzacy, enumerable) => $"{prowadzacy.Imie} {prowadzacy.Nazwisko} {enumerable.Average(p => p.Srednia)}"
        );
        // 
        //     DaneUczelni.Przedmioty,
        //     prowadzacy => prowadzacy.Id,
        //     przedmiot => przedmiot.ProwadzacyId,
        //     (prowadzacy, przedmioty) => new  {prowadzacy, przedmioty} 
        //     ).GroupJoin(
        //     DaneUczelni.Zapisy,
        //     pp => pp.prowadzacy.Id,
        //     zapis => zapis.PrzedmiotId 
        //     )
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Studenci.GroupJoin(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapisy) => new { student, RecordCount = zapisy.Count() }
            ).GroupBy(a => a.student.Miasto)
            .Select(grouping => new {Miasto = grouping.Key, Ilosc = grouping.Sum(a => a.RecordCount)})
            .OrderByDescending(a => a.Ilosc)
            .Select(a => $"{a.Miasto} {a.Ilosc}");
            ;
        // return DaneUczelni.Studenci.GroupBy(student => student.Miasto).GroupJoin()
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}