using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommandHandler : AbstractUseCase, IRequestHandler<ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommand, bool>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;

        public ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                     IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
                                                     IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
                                                     IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem, IMediator mediator) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }
        public async Task<bool> Handle(ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommand comando, CancellationToken cancellationToken)
        {
            var planejamentoAnual = await repositorioPlanejamentoAnual.ObterIdPorTurmaEComponenteCurricular(comando.TurmaId, comando.ComponenteCurricularId);
            if (planejamentoAnual == 0)
                return true;

            var planejamentoAnualPeriodosEscolares = await repositorioPlanejamentoAnualPeriodoEscolar.ObterPorPlanejamentoAnualId(planejamentoAnual);

            if (comando.IdsPlanejamentoAnualPeriodoEscolar != null && comando.IdsPlanejamentoAnualPeriodoEscolar.Any())
            {
                var periodosEscolaresConsiderados = (from id in comando.IdsPlanejamentoAnualPeriodoEscolar
                                                     select repositorioPlanejamentoAnualPeriodoEscolar.ObterPorId(id).PeriodoEscolarId);

                var bimestresConsiderados = (from id in periodosEscolaresConsiderados
                                             select mediator.Send(new ObterPeriodoEscolarePorIdQuery(id)).Result.Bimestre);

                planejamentoAnualPeriodosEscolares = planejamentoAnualPeriodosEscolares.Where(pape => bimestresConsiderados.Contains(pape.Bimestre));
            }

            foreach (var pape in planejamentoAnualPeriodosEscolares)
            {
                var planejamentoAnualComponente = await repositorioPlanejamentoAnualComponente
                    .ObterListaPorPlanejamentoAnualPeriodoEscolarId(comando.TurmaId, comando.ComponenteCurricularId, pape.Bimestre);

                var planejamentoAnualObjetivosAprendizagem = await repositorioPlanejamentoAnualObjetivosAprendizagem
                    .ObterPorPlanejamentoAnualComponenteId(planejamentoAnualComponente.Select(pc => pc.Id).ToArray());

                foreach (var paoa in planejamentoAnualObjetivosAprendizagem)
                    await repositorioPlanejamentoAnualObjetivosAprendizagem.RemoverLogico(paoa.Id);

                await repositorioPlanejamentoAnualComponente.RemoverLogicamenteAsync(planejamentoAnualComponente.Select(pc => pc.Id).ToArray());

                await repositorioPlanejamentoAnualPeriodoEscolar.RemoverLogicamentePorTurmaBimestreAsync(comando.TurmaId, pape.Bimestre);
            }

            await repositorioPlanejamentoAnual.RemoverLogicamenteAsync(planejamentoAnual);

            return true;
        }
    }
}
