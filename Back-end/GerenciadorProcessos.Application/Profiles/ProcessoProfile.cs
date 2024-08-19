using AutoMapper;
using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Entidades;

namespace GerenciadorProcessos.Application.Profiles
{
    public class ProcessoProfile : Profile
    {
        public ProcessoProfile()
        {
            CreateMap<Processo, ProcessoDTO>();

            CreateMap<AdicionarProcessoCommand, Processo>()
                .ForMember(dest => dest.Subprocessos, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DataUltimaAlteracao, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
