using AutoMapper;
using Sesc.Base.Application.ViewModel;
using Sesc.Base.Domain.Entities;

namespace Sesc.Base.Application
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AlunoViewModel, Aluno>();
        }
    }
}
