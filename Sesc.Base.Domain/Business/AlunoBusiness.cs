using FluentValidation;
using Sesc.Base.Domain.Entities;
using Sesc.Base.Domain.Repositories;
using Stefanini.Common;
using Sesc.Base.Domain.Business.Interfaces;
using Stefanini.Business;

namespace Sesc.Base.Domain.Business
{
    public class AlunoBusiness : CrudBusiness<Aluno>, IAlunoBusiness
    {
        public AlunoBusiness(IAlunoRepository repository,
            IValidator<Aluno> validator,
            INotification notification)
            : base(repository, validator, notification)
        { }
    }
}
