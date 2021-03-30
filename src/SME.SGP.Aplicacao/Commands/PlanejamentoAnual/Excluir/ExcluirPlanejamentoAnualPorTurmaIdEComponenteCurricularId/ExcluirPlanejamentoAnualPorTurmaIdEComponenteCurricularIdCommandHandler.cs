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
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                     IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
                                                     IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
                                                     IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem, 
                                                     IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                     IMediator mediator) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<bool> Handle(ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommand comando, CancellationToken cancellationToken)
        {
            var planejamentoAnualIdDestino = await repositorioPlanejamentoAnual.ObterIdPorTurmaEComponenteCurricular(comando.TurmaId, comando.ComponenteCurricularId);
            if (planejamentoAnualIdDestino == 0)
                return true;

            IList<PlanejamentoAnualPeriodoEscolar> listaPlanejamentoAnualPeriodoEscolarOrigem = new List<PlanejamentoAnualPeriodoEscolar>();
            comando.IdsPlanejamentoAnualPeriodoEscolarSelecionados.ToList().ForEach(id => 
            {
                listaPlanejamentoAnualPeriodoEscolarOrigem.Add(repositorioPlanejamentoAnualPeriodoEscolar.ObterPorId(id));
            });

            var bimestresConsiderados = listaPlanejamentoAnualPeriodoEscolarOrigem
                .Select(pe => repositorioPeriodoEscolar.ObterPorId(pe.PeriodoEscolarId).Bimestre);

            var planejamentoAnualPeriodosEscolares = await repositorioPlanejamentoAnualPeriodoEscolar
                .ObterPorPlanejamentoAnualId(planejamentoAnualIdDestino, bimestresConsiderados.ToArray());

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

            await repositorioPlanejamentoAnual.RemoverLogicamenteAsync(planejamentoAnualIdDestino);

            return true;
        }
    }
}
