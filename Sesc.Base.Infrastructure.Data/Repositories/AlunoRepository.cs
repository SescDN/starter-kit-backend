using Sesc.Base.Domain.Entities;
using Sesc.Base.Domain.Repositories;
using Stefanini.Repository.EntityFramework;

namespace Sesc.Base.Infrastructure.Data.Repositories
{
    public class AlunoRepository : BaseRepository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(ApplicationContext context)
            : base(context)
        { }
    }
}
