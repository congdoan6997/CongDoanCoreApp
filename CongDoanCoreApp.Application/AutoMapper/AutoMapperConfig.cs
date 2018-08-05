using AutoMapper;

namespace CongDoanCoreApp.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMapping()
        {
            return new MapperConfiguration(cfg =>
           {
               cfg.AddProfile(new DomainToViewModelMappingProfile());
               cfg.AddProfile(new ViewModelToDomainMappingProfile());
           });
        }
    }
}