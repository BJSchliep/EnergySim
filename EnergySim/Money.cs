using EnergySim;
using System.Collections.Generic;

public class Money
{
    private int MoneyTotal;

    public LandValue LandValue { get; }

    private readonly Dictionary<LandValue, int> gridMoney = new()
    {
        { LandValue.Empty, 0 },
        { LandValue.Nuclear, 5 },
        { LandValue.Turbine, 3 },
        { LandValue.House, 0 },
        { LandValue.Business, 0 },
        { LandValue.Geothermal, 6 },
        { LandValue.Hydroelectric, 4 },
        { LandValue.Solar, 1 },
        { LandValue.Biomass, 2 },
        { LandValue.Water, 0 }
    };

    public Money(int money)
    {
        MoneyTotal = money;
    }

    public void AddMoney( LandValue landValue)
    {
        MoneyTotal += gridMoney[landValue];
    }

    public void SubtractMoney( LandValue landValue)
    {
        MoneyTotal -= gridMoney[landValue];
    }

    public bool IsNegative()
    {
        return MoneyTotal < 0;
    }
    public int GetTotalMoney()
    {
        return MoneyTotal;
    }
}
