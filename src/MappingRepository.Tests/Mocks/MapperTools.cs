using AutoMapper;

namespace MappingRepository.Tests.Mocks
{
    internal static class MapperTools
    {
        public static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entities.Customer, DomainObjects.Customer>()
                    .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.FirstName} {s.Surname}"));

                cfg.CreateMap<Entities.Order, DomainObjects.Order>();
            });

            return config;
        }

        public static IMapper GetMapper() => GetConfig().CreateMapper();
    }
}
