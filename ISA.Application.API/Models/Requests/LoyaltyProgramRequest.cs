namespace ISA.Application.API.Models.Requests;

public class LoyaltyProgramRequest
{
    public string Name { get; set; } // ime
    public int NewPoints { get; set; } // novi poeni ako se obavi transakcija
    public int MinCategoryThresholds { get; set; } // granica od koje pocinje kategorija
    public int MaxCategoryThresholds { get; set; } // granica do koje traje kategorija
    public int MaxPenaltyPoints { get; set; } // koliko moze imati maksimalno penala da bi ostao u kategoriji
    public int CategoryDiscounts { get; set; } // popust na narednu kupovinu
}