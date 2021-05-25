using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stefanini.Common;
using Sesc.Base.Application.Interfaces;
using Sesc.Base.Application.ViewModel;

namespace Sesc.Base.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoService _service;
        private readonly INotification _notification;

        public AlunoController(IAlunoService service,
                                   INotification notification)
        {
            this._service = service;
            this._notification = notification;
        }

        // GET: api/Aluno
        [HttpGet]
        public IEnumerable<AlunoViewModel> Get()
        {
            return this._service.GetAll();
        }

        // GET: api/Aluno/5
        [HttpGet("{id}", Name = "GetAluno")]
        public AlunoViewModel Get(int id)
        {
            return this._service.GetById(id);
        }

        // POST: api/Aluno
        [HttpPost]
        [Authorize(Policy = "salvar")]
        public void Post([FromBody] AlunoViewModel aluno)
        {
            this._service.Insert(aluno);
        }

        // PUT: api/Aluno/5
        [HttpPut("{id}")]
        [Authorize(Policy = "editar")]
        public void Put(int id, [FromBody] AlunoViewModel aluno)
        {
            this._service.Update(aluno);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this._service.Delete(id);
        }

    }
}
