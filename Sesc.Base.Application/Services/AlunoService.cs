using AutoMapper;
using Stefanini.Application;
using Sesc.Base.Application.Interfaces;
using Sesc.Base.Application.ViewModel;
using Sesc.Base.Domain.Business.Interfaces;
using Sesc.Base.Domain.Entities;
using Stefanini.Repository;
using System.Collections.Generic;

namespace Sesc.Base.Application.Services
{
    public class AlunoService : BaseService, IAlunoService
    {
        private readonly IAlunoBusiness _alunoBusiness;        

        public AlunoService(IAlunoBusiness alunoBusiness, IMapper mapper)
            : base(mapper)
        {
            this._alunoBusiness = alunoBusiness;           
        }

        public void Delete(int id)
        {
            this._alunoBusiness.Delete(id);
        }

        public IEnumerable<AlunoViewModel> GetAll()
        {
            IEnumerable<Aluno> listaAluno = this._alunoBusiness.GetAll();
            return this.Map<IEnumerable<AlunoViewModel>>(listaAluno);
        }

        public AlunoViewModel GetById(int id)
        {
            Aluno aluno = this._alunoBusiness.GetById(id);
            return this.Map<AlunoViewModel>(aluno);
        }

        [Transactional]
        public void Insert(AlunoViewModel entity)
        {
            Aluno aluno = this.Map<Aluno>(entity);
            this._alunoBusiness.Insert(aluno);
        }

        public void Update(AlunoViewModel entity)
        {
            Aluno aluno = this.Map<Aluno>(entity);
            this._alunoBusiness.Update(aluno);
        }
    }
}
