using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirWorkflowReposicaoAulaCommand: IRequest<long>
    {
        public InserirWorkflowReposicaoAulaCommand(int ano, long aulaId, int quantidade, string dreCodigo, string dreNome, string ueCodigo, string ueNome, string turmaNome, string componenteCurricularNome)
        {
            Ano = ano;
            AulaId = aulaId;
            Quantidade = quantidade;
            DreCodigo = dreCodigo;
            DreNome = dreNome;
            UeCodigo = ueCodigo;
            UeNome = ueNome;
            TurmaNome = turmaNome;
            ComponenteCurricularNome = componenteCurricularNome;
        }

        public int Ano { get; set; }
        public long AulaId { get; set; }
        public int Quantidade { get; set; }
        public string DreCodigo { get; set; }
        public string DreNome { get; set; }
        public string UeCodigo { get; set; }
        public string UeNome { get; set; }
        public string TurmaNome { get; set; }
        public string ComponenteCurricularNome { get; set; }
    }

    public class InserirWorkflowReposicaoAulaCommandValidator : AbstractValidator<InserirWorkflowReposicaoAulaCommand>
    {
        public InserirWorkflowReposicaoAulaCommandValidator()
        {
           RuleFor(c => c.Ano)
           .NotEmpty()
           .WithMessage("Deve ser informado o ano para geração do workFlow de reposição de aula");

            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("Deve ser informado o Id da Aula para geração do workFlow de reposição de aula");

            RuleFor(c => c.Quantidade)
            .NotEmpty()
            .WithMessage("Deve ser informado a quantidade de aulas para geração do workFlow de reposição de aula");

            RuleFor(c => c.DreCodigo)
            .NotEmpty()
            .WithMessage("Deve ser informado o codigo da DRE para geração do workFlow de reposição de aula");

            RuleFor(c => c.DreNome)
            .NotEmpty()
            .WithMessage("Deve ser informado o nome da DRE para geração do workFlow de reposição de aula");

            RuleFor(c => c.UeCodigo)
            .NotEmpty()
            .WithMessage("Deve ser informado o codigo da UE para geração do workFlow de reposição de aula");

            RuleFor(c => c.UeNome)
            .NotEmpty()
            .WithMessage("Deve ser informado o nome da UE para geração do workFlow de reposição de aula");

            RuleFor(c => c.TurmaNome)
            .NotEmpty()
            .WithMessage("Deve ser informado o nome da turma para geração do workFlow de reposição de aula");

            RuleFor(c => c.ComponenteCurricularNome)
            .NotEmpty()
            .WithMessage("Deve ser informado o nome do componente curricular para geração do workFlow de reposição de aula");
        }
    }
}
