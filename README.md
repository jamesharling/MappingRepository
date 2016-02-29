# MappingRepository

Reduce your code churn when mapping between your entity and domain layers. Simply have your repositories and entities inherit from the provided classes and interfaces, define your AutoMapper config and you're done. Supports IoC injection of database context and AutoMapper configuration.

## Getting started

`Install-Package AutoMapper`

## Deriving from the provided classes
Derive your own repository from the MappingRepository base class:
```csharp
public class CustomerRepository : MappingRepository<Customer, CustomerDto, Guid>
{
    public CustomerRepository(IMappingRepositoryContext dbContext, IMapper mapper) : base(dbContext, mapper)
    { }
}
```

## Mapping objects
Define your maps with AutoMapper's non-static API:
```csharp
private IMapper mapper
{
    get
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Customer, CustomerDto>();
            
            cfg.CreateMap<CustomerDto, Customer>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Name.Split(' ')[0]))
                .ForMember(d => d.Surname, o => o.MapFrom(s => s.Name.Split(' ')[1]));
        });

        return config.CreateMapper();
    }
}
```

## Put it to use
Grab your data through a number of built-in methods, or build upon the provided queryables to extend the base functionality:
```csharp
var repo = new CustomerRepository(new DbContext(), mapper);
var customer = repo.FirstOrDefault(x => x.Name.Equals("Sarah Barnes"));
```
