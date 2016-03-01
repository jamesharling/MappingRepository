# MappingRepository

Reduce your code churn when mapping between your entity and domain layers. Simply have your repositories and entities inherit from the provided classes and interfaces, define your AutoMapper config and you're done. Supports IoC injection of database context and AutoMapper configuration.

## Getting started
In your project that will hold domain layer repositories:
`Install-Package MappingRepository`

In your projects that hold DTOs and EF Entities:
`Install-Package MappingRepository.Interfaces`

## Deriving from the provided classes
Derive your own repository from the MappingRepository base class:
```csharp
public class CustomerRepository : MappingRepository<Customer, CustomerDto, Guid>
{
    public CustomerRepository(IMappingRepositoryContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    { }
}
```

## Implementing the interfaces
Have your entities and DTOs implement the supplied interfaces. Your objects on both sides must implement a common key type, e.g. `Guid` or `int`.
```csharp
public class Customer : IMappingRepositoryEntity<Guid>
{
    public Guid Id;
    public string FirstName;
    public string Surname;
    
    [Computed]
    public string Name => this.FirstName + " " + this.Surname;
}

public class CustomerDto : IMappingRepositoryDestination<Guid>
{
    public Guid Id;
    public string Name;
}
```

If you want to use IoC to inject your database context, have it implement IMappingRepositoryContext:
```csharp
public class MyContext : System.Data.Entity.DbContext, IMappingRepositoryContext
```

## Mapping objects
Define your maps with AutoMapper's non-static API:
```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Customer, CustomerDto>();
    
    cfg.CreateMap<CustomerDto, Customer>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Name.Split(' ')[0]))
        .ForMember(d => d.Surname, o => o.MapFrom(s => s.Name.Split(' ')[1]));
});

var mapper = config.CreateMapper();
```

## Put it to use
Grab your data through a number of built-in methods, or build upon the provided queryables to extend the base functionality:
```csharp
var repo = new CustomerRepository(new MyContext(), mapper);

var id = Guid.Parse("205b9dc9-1fd1-4be6-881f-f18c9a32d95a");
var customer1 = repo.GetById(id);

var customer2 = repo.FirstOrDefault(x => x.Name.Equals("Sarah Barnes"));

var customersWithOrders = repo.FindBy(x => x.Orders.Any());

var customerDropDownList = repo.ProjectTo<CustomerDropDown>();
```

## Efficiency
MappingRepository utilises projections, meaning EF will only select the data it needs to populate your DTOs. It also includes support for DelegateDecompiler, allowing the querying of computed properties in your maps.

```csharp
public class Order : IMappingRepositoryEntity<Guid>
{
    public Guid Id;
    public Guid CustomerId;
    
    public ICollection<Line> Lines;
    
    [Computed]
    public string Total => this.Lines.Sum(x => x.Amount);
}
```

```csharp
var highValueCustomers = repo.FindBy(x => x.Orders.Where(y => y.Total > 1000), i => i.Orders.Select(x => x.Lines));
```
