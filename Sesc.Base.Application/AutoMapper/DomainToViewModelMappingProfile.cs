using AutoMapper;
using Sesc.Base.Application.ViewModel;
using Sesc.Base.Domain.Entities;

namespace Sesc.Base.Application
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Aluno, AlunoViewModel>();
        }
    }
}
