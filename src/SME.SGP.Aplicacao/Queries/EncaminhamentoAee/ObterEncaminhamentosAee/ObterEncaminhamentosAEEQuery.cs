using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosAEEQuery : IRequest<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>>
    {
        public ObterEncaminhamentosAEEQuery(long dreId, long ueId, long turmaId, string alunoCodigo, SituacaoAEE? situacao, string responsavelRf, int anoLetivo)
        {
            DreId = dreId;
            UeId = ueId;
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            Situacao = situacao;
            ResponsavelRf = responsavelRf;
            AnoLetivo = anoLetivo;
        }

        public long DreId { get; }
        public long UeId { get; }
        public long TurmaId { get; }
        public string AlunoCodigo { get; }
        public SituacaoAEE? Situacao { get; }
        public string ResponsavelRf { get; }
        public int AnoLetivo { get; }
    }

    public class ObterEncaminhamentosAEEQueryValidator : AbstractValidator<ObterEncaminhamentosAEEQuery>
    {
        public ObterEncaminhamentosAEEQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("A DRE deve ser informada para pesquisa de Encaminhamentos AEE");
        }
    }
}
