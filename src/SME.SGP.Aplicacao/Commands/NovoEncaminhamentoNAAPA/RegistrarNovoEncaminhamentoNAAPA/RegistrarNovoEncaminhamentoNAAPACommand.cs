using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPA
{
    public class RegistrarNovoEncaminhamentoNAAPACommand : IRequest<ResultadoNovoEncaminhamentoNAAPADto>
    {
        public long? TurmaId { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }

        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public int Tipo { get; set; }

        public RegistrarNovoEncaminhamentoNAAPACommand()
        {
        }

        public RegistrarNovoEncaminhamentoNAAPACommand(long? turmaId, string alunoNome, string alunoCodigo, SituacaoNAAPA situacao, long? dreId, long? ueId, int tipo)
        {
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
            Situacao = situacao;
            DreId = dreId;
            UeId = ueId;
            Tipo = tipo;
        }
    }

    public class RegistrarNovoEncaminhamentoNAAPACommandValidator : AbstractValidator<RegistrarNovoEncaminhamentoNAAPACommand>
    {
        public RegistrarNovoEncaminhamentoNAAPACommandValidator()
        {
            // Tipo é sempre obrigatório
            RuleFor(x => x.Tipo)
                .GreaterThan(0)
                .WithMessage("O tipo de encaminhamento deve ser informado!");

            // Validações para Encaminhamento INDIVIDUAL (Tipo = 11)
            When(x => x.Tipo == (int)TipoQuestionario.EncaminhamentoNAAPAIndividual, () =>
            {
                RuleFor(x => x.TurmaId)
                    .NotNull()
                    .WithMessage("A turma deve ser informada para encaminhamento individual!")
                    .GreaterThan(0)
                    .WithMessage("O Id da turma deve ser válido!");

                RuleFor(x => x.AlunoCodigo)
                    .NotEmpty()
                    .WithMessage("O código do aluno deve ser informado para encaminhamento individual!");

                RuleFor(x => x.AlunoNome)
                    .NotEmpty()
                    .WithMessage("O nome do aluno deve ser informado para encaminhamento individual!");
            });

            // Validações para Encaminhamento INSTITUCIONAL (Tipo = 12)
            When(x => x.Tipo == (int)TipoQuestionario.EncaminhamentoNAAPAInstitucional, () =>
            {
                RuleFor(x => x.DreId)
                    .NotNull()
                    .WithMessage("A DRE deve ser informada para encaminhamento institucional!")
                    .GreaterThan(0)
                    .WithMessage("O Id da DRE deve ser válido!");

                RuleFor(x => x.UeId)
                    .NotNull()
                    .WithMessage("A UE deve ser informada para encaminhamento institucional!")
                    .GreaterThan(0)
                    .WithMessage("O Id da UE deve ser válido!");
            });
        }
    }
}