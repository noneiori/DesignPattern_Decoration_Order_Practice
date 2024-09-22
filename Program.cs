Order _order = new Order();
_order.Products.Add(new Product() { Name = "NoteBook", Price = 30000 });
_order.Products.Add(new Product() { Name = "Macbook", Price = 70000 });

var seasonDiscount = new SeasonDiscount(_order);
var seasonMemberDiscount = new MemberDiscount(seasonDiscount);

Console.WriteLine("Order:");
Console.WriteLine(_order);
Console.WriteLine("---");
Console.WriteLine("seasonDiscount:");
Console.WriteLine(seasonDiscount);
Console.WriteLine("---");
Console.WriteLine("seasonMemberDiscount:");
Console.WriteLine(seasonMemberDiscount);
Console.Read();


class Order
{
    public Order()
    {
        this.Products = new List<Product>();
        this.DiscountLabel = new List<string>();
    }

    public List<Product> Products { get; set; }
    public string? DiscountMsg { get; set; }    
    public List<string> DiscountLabel { get; set; }

    public virtual decimal DiscountedPrice()
    {
        return this.TotalPrice();
    }    

    public decimal TotalPrice()
    {
        return this.Products.Sum(x => x.Price);
    }

    public override string ToString()
    {

        return $"訂單總金額: ${this.TotalPrice()}";
    }
}

internal class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

abstract class Discount :Order
{
    protected Order _order;
    
    public Discount(Order order)
    {        
        this._order = order;    
        this.Products = order.Products;
        this.DiscountLabel.AddRange(order.DiscountLabel);
    }

    public override decimal DiscountedPrice()
    {
        return _order.DiscountedPrice();
    }

    public override string ToString()
    {
        return $"訂單總金額: ${this.TotalPrice()}\n應用折扣: {string.Join(" + ", this.DiscountLabel)}\n最終金額: ${DiscountedPrice()}";
    }
}

class SeasonDiscount : Discount
{
    public SeasonDiscount(Order order) : base(order)
    {
        this.DiscountLabel.Add("季節性折扣 10%");         
    }

    public override decimal DiscountedPrice()
    {
        return _order.DiscountedPrice() * 0.9m;
    }    
}

class NewUserDiscount : Discount
{
    public NewUserDiscount(Order order) : base(order)
    {
        this.DiscountLabel.Add("新用戶折扣 100元");
    }

    public override decimal DiscountedPrice()
    {
        return _order.DiscountedPrice() - 100;
    }
}

class MemberDiscount : Discount
{
    public MemberDiscount(Order order) : base(order)
    {
        this.DiscountLabel.Add("會員折扣 50元");
    }

    public override decimal DiscountedPrice()
    {
        return _order.DiscountedPrice() - 50;
    }
}