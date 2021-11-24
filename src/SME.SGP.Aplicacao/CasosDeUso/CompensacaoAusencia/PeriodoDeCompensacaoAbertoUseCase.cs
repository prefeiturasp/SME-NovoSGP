using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PeriodoDeCompensacaoAbertoUseCase : IPeriodoDeCompensacaoAbertoUseCase
    {
        private readonly IMediator mediator;

        public PeriodoDeCompensacaoAbertoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> VerificarPeriodoAberto(string turmaCodigo, int bimestre)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));

            var parametroSistema = await mediator
                                         .Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PermiteCompensacaoForaPeriodo,
                                                                                         turma.AnoLetivo));

            var periodoEstahAberto = parametroSistema != null
                                    ? parametroSistema.Ativo && turma.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year
                                    : await mediator
                                            .Send(new TurmaEmPeriodoAbertoQuery(turma,
                                                                                DateTimeExtension.HorarioBrasilia(),
                                                                                bimestre,
                                                                                true));

            return periodoEstahAberto;
        }
    }
}