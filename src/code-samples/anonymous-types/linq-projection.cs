var result = new[] 
{ 
    new { Name = "Alice", Age = 30 },
    new { Name = "Bob", Age = 25 }
};

var linqProjection = result.Select(p => new { p.Name, UpperName = p.Name.ToUpper() });
