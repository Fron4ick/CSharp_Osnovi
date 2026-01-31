using System;

namespace HotelAccounting;

public class AccountingModel : ModelBase
{
    private double _price;
    private int _nightsCount;
    private double _discount;
    private double _total;

    public double Price
    {
        get => _price;
        set
        {
            if (value < 0) throw new ArgumentException(nameof(Price));
            _price = value;
            Notify(nameof(Price));
            UpdateTotal();
        }
    }

    public int NightsCount
    {
        get => _nightsCount;
        set
        {
            if (value <= 0) throw new ArgumentException(nameof(NightsCount));
            _nightsCount = value;
            Notify(nameof(NightsCount));
            UpdateTotal();
        }
    }

    public double Discount
    {
        get => _discount;
        set
        {
            _discount = value;
            Notify(nameof(Discount));
            UpdateTotal();
        }
    }

    public double Total
    {
        get => _total;
        set
        {
            if (value < 0) throw new ArgumentException(nameof(Total));
            _total = value;
            Notify(nameof(Total));
            
            _discount = (1 - _total / (_price * _nightsCount)) * 100;
            Notify(nameof(Discount));
        }
    }

    private void UpdateTotal()
    {
        double newTotal = _price * _nightsCount * (1 - _discount / 100);
        if (newTotal < 0) throw new ArgumentException(nameof(Total));
        
        _total = newTotal;
        Notify(nameof(Total));
    }
}