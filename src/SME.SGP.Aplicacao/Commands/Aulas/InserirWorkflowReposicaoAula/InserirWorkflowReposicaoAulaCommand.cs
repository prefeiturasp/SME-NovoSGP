using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class InserirWorkflowReposicaoAulaCommand : IRequest<long>
    {
        public InserirWorkflowReposicaoAulaCommand(int ano, Aula aula, Turma turma, string componenteCurricularNome, Guid? perfilAtual = null)
        {
            Ano = ano;
            AulaId = aula.Id;
            Quantidade = aula.Quantidade;
            DreCodigo = turma.Ue.Dre.CodigoDre;
            DreNome = turma.Ue.Dre.Nome;
            UeCodigo = turma.Ue.CodigoUe;
            UeTipo = turma.Ue.TipoEscola;
            UeNome = turma.Ue.Nome;
            TurmaNome = turma.Nome;
            TurmaCodigo = turma.CodigoTurma;
            TurmaModalidade = turma.ModalidadeCodigo;
            TurmaSemestre = turma.Semestre;
            
            ComponenteCurricularNome = componenteCurricularNome;
            PerfilAtual = perfilAtual;
        }

        public int Ano { get; set; }
        public long AulaId { get; set; }
        public int Quantidade { get; set; }
        public string DreCodigo { get; set; }
        public string DreNome { get; set; }
        public string UeCodigo { get; set; }
        public TipoEscola UeTipo { get; set; }
        public string UeNome { get; set; }
        public string TurmaNome { get; set; }
        public string TurmaCodigo { get; set; }
        public int TurmaSemestre { get; set; }
        public Modalidade TurmaModalidade { get; set; }
        public string ComponenteCurricularNome { get; set; }
        public Guid? PerfilAtual { get; set; }

        public string UeTipoNome => UeTipo.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName;
        public string TurmaModalidadeNome => TurmaModalidade.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName;
        public string TurmaDescricao => $"{TurmaModalidadeNome} - {TurmaNome} - {UeTipoNome} {UeNome}";
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

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("Deve ser informado o código da turma para geração do workFlow de reposição de aula");

            RuleFor(c => c.ComponenteCurricularNome)
            .NotEmpty()
            .WithMessage("Deve ser informado o nome do componente curricular para geração do workFlow de reposição de aula");            
        }
    }
}
