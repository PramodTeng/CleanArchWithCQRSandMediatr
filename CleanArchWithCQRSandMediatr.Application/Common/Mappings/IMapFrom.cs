using AutoMapper;

namespace CleanArchWithCQRSandMediatr.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), destinationType: GetType());
}
