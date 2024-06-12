using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao.CasosDeUso.Turma
{
    public class ObterTurmasPapPorAnoLetivoUseCase : ConsultasBase, IObterTurmasPapPorAnoLetivoUseCase
    {
        private readonly IMediator mediator;

        public ObterTurmasPapPorAnoLetivoUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(
            contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TurmasPapDto>> Executar(long anoLetivo, string codigoUe)
        {
            return await mediator.Send(new ObterTurmasPapPorAnoLetivoQuery(anoLetivo,codigoUe));
        }
    }
}