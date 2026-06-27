Single computed property:
```csharp
public decimal DiscountedPrice => Price * (1 - Discount);
```

Simple validation method:
```csharp
public bool IsOverdue => DateTime.Now > DueDate;
```

Chained property access:
```csharp
public string FullName => $"{FirstName} {LastName}";
```

Operator overload:
```csharp
public static Vector operator +(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);
```

Getter and setter with computed backing:
```csharp
public int Age => DateTime.Now.Year - BirthYear;
```
