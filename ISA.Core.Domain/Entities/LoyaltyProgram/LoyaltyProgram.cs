namespace ISA.Core.Domain.Entities.LoyaltyProgram;

public class LoyaltyProgram : Entity<Guid>
{
    public string Name { get; set; } // ime
    public int NewPoints { get; set; } // novi poeni ako se obavi transakcija
    public int MinCategoryThresholds { get; set; } // granica od koje pocinje kategorija
    public int MaxCategoryThresholds { get; set; } // granica do koje traje kategorija
    public int MaxPenaltyPoints { get; set; } // koliko moze imati maksimalno penala da bi ostao u kategoriji
    public int CategoryDiscounts { get; set; } // popust na narednu kupovinu

    public LoyaltyProgram(string name, int newPoints, int minCategoryThresholds, int maxCategoryThresholds, int maxPenaltyPoints, int categoryDiscounts)
    {
        Name = name;
        NewPoints = newPoints;
        MinCategoryThresholds = minCategoryThresholds;
        MaxCategoryThresholds = maxCategoryThresholds;
        MaxPenaltyPoints = maxPenaltyPoints;
        CategoryDiscounts = categoryDiscounts;
    }
}
