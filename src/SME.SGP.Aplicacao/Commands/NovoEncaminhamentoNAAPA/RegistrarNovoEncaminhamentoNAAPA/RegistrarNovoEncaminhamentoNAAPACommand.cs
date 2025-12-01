using FluentValidation;
using MediatR;
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
        public long TurmaId { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }

        public RegistrarNovoEncaminhamentoNAAPACommand()
        {
        }

        public RegistrarNovoEncaminhamentoNAAPACommand(long turmaId, string alunoNome, string alunoCodigo, SituacaoNAAPA situacao)
        {
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
            Situacao = situacao;
        }
    }

    public class RegistrarNovoEncaminhamentoNAAPACommandValidator : AbstractValidator<RegistrarNovoEncaminhamentoNAAPACommand>
    {
        public RegistrarNovoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada para o registro do encaminhamento NAAPA!");
        }
    }
}