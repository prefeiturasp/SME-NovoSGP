using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisDosEncaminhamentosAEEQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterResponsaveisDosEncaminhamentosAEEQuery(long dreId, long ueId, long turmaId, string alunoCodigo, SituacaoAEE? situacao, int anoLetivo)
        {
            DreId = dreId;
            UeId = ueId;
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            Situacao = situacao;
            AnoLetivo = anoLetivo;
        }

        public long DreId { get; }
        public long UeId { get; }
        public long TurmaId { get; }
        public string AlunoCodigo { get; }
        public SituacaoAEE? Situacao { get; }
        public int AnoLetivo { get; }
    }

    public class ObterResponsaveisDosEncaminhamentosAEEQueryValidator : AbstractValidator<ObterResponsaveisDosEncaminhamentosAEEQuery>
    {
        public ObterResponsaveisDosEncaminhamentosAEEQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("A DRE deve ser informada para pesquisa de Encaminhamentos AEE");
        }
    }
}
