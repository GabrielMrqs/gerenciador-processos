using AutoMapper;
using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Entidades;

namespace GerenciadorProcessos.Application.Profiles
{
    public class AreaProfile : Profile
    {
        public AreaProfile()
        {
            CreateMap<AdicionarAreaCommand, Area>()
                .ForMember(dest => dest.Processos, opt => opt.Ignore());

            CreateMap<Area, AreaDTO>();
        }
    }
}
