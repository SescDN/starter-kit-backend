using FluentValidation;
using FluentValidation.Results;
using Sesc.Base.Domain.Entities;
using Sesc.Base.Domain.Validations;
using System;
using Xunit;

namespace Sesc.Tests.Unity.Domain.Validations
{
    public class AlunoValidationTest
    {
        [Theory]
        [InlineData("Fulano", "")]
        [InlineData("", "09446199085")]
        public void InsercaoAlunoInvalida(string nome, string documento)
        {
            Aluno aluno = new Aluno() { Nome = nome, Documento = documento };
            IValidator<Aluno> validation = new AlunoValidation();
            ValidationResult result = validation.Validate(aluno, ruleSet: "Insert");

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("Fulano", "09446199085")]
        public void InsercaoAlunoValida(string nome, string documento)
        {
            Aluno aluno = new Aluno() { Nome = nome, Documento = documento };
            IValidator<Aluno> validation = new AlunoValidation();
            ValidationResult result = validation.Validate(aluno, ruleSet: "Insert");

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(1, "Fulano", "")]
        [InlineData(0, "Fulano", "09446199085")]
        [InlineData(1, "", "09446199085")]
        public void AtualizacaoAlunoInvalida(int id, string nome, string documento)
        {
            Aluno aluno = new Aluno() { Id = id, Nome = nome, Documento = documento };
            IValidator<Aluno> validation = new AlunoValidation();
            ValidationResult result = validation.Validate(aluno, ruleSet: "Update");

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(1, "Fulano", "09446199085")]
        public void AtualizacaoAlunoValida(int id, string nome, string documento)
        {
            Aluno aluno = new Aluno() { Id = id, Nome = nome, Documento = documento };
            IValidator<Aluno> validation = new AlunoValidation();
            ValidationResult result = validation.Validate(aluno, ruleSet: "Update");

            Assert.True(result.IsValid);
        }
    }
}
