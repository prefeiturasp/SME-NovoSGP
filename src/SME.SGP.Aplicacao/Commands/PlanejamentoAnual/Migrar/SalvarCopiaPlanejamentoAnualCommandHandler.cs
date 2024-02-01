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
    public class SalvarCopiaPlanejamentoAnualCommandHandler : AbstractUseCase, IRequestHandler<SalvarCopiaPlanejamentoAnualCommand, bool>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;

        public SalvarCopiaPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                          IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
                                                          IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
                                                          IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem,
                                                          IMediator mediator) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }

        public async Task<bool> Handle(SalvarCopiaPlanejamentoAnualCommand request, CancellationToken cancellationToken)
        {
            var planejamentoAnualId = await ObterIdPlanejamentoAnual(request.PlanejamentoAnual);

            foreach (var periodo in request.PlanejamentoAnual.PeriodosEscolares)
            {
                var planejamentoAnualPeriodoEscolar = await ObterPlanejamentoAnualPeriodoEscolar(planejamentoAnualId, periodo.PeriodoEscolarId);

                var planejamentoAnualPeriodoEscolarId = await repositorioPlanejamentoAnualPeriodoEscolar
                    .SalvarAsync(planejamentoAnualPeriodoEscolar);

                foreach (var componente in periodo.ComponentesCurriculares)
                {
                    var planejamentoAnualComponente = await ObterPlanejamentoAnualComponenteCurricular(componente.ComponenteCurricularId,
                                                                                                       planejamentoAnualPeriodoEscolarId,
                                                                                                       componente.Descricao);
                    var planejamentoAnualComponenteId = await repositorioPlanejamentoAnualComponente.SalvarAsync(planejamentoAnualComponente);

                    var listaPlanejamentoAnualObjetivoAprendizagemExistente = await repositorioPlanejamentoAnualObjetivosAprendizagem
                        .ObterPorPlanejamentoAnualComponenteId(planejamentoAnualComponenteId);

                    foreach (var objetivo in componente.ObjetivosAprendizagem)
                    {
                        var planejamentoAnualObjetivoAprendizagem = await ObterPlanejamentoAnualObjetivoAprendizagem(listaPlanejamentoAnualObjetivoAprendizagemExistente,
                                                                                                                    objetivo.ObjetivoAprendizagemId,
                                                                                                                    planejamentoAnualComponenteId);
                        await repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarAsync(planejamentoAnualObjetivoAprendizagem);
                    }

                    var listaPlanejamentoAnualObjetivoAprendizagemRemover = listaPlanejamentoAnualObjetivoAprendizagemExistente
                        .Where(loa => !componente.ObjetivosAprendizagem.Select(oa => oa.ObjetivoAprendizagemId).Contains(loa.ObjetivoAprendizagemId))
                        .ToList();

                    listaPlanejamentoAnualObjetivoAprendizagemRemover
                        .ForEach(oa => repositorioPlanejamentoAnualObjetivosAprendizagem.RemoverLogico(oa.Id).Wait());
                }
            }

            return true;
        }

        private async Task<long> ObterIdPlanejamentoAnual(PlanejamentoAnual PlanejamentoAnual)
        {
            var planejamentoAnualId = await repositorioPlanejamentoAnual
                .ObterIdPorTurmaEComponenteCurricular(PlanejamentoAnual.TurmaId, PlanejamentoAnual.ComponenteCurricularId);

            if (planejamentoAnualId == 0)
                planejamentoAnualId = await repositorioPlanejamentoAnual.SalvarAsync(PlanejamentoAnual);
            return planejamentoAnualId;
        }

        private async Task<PlanejamentoAnualObjetivoAprendizagem> ObterPlanejamentoAnualObjetivoAprendizagem(IEnumerable<PlanejamentoAnualObjetivoAprendizagem> listaPlanejamentoAnualObjetivoAprendizagemExistente,
                                                                                                        long objetivoAprendizagemId, long planejamentoAnualComponenteId)
        {
            var planejamentoAnualObjetivoAprendizagem = listaPlanejamentoAnualObjetivoAprendizagemExistente
                              .FirstOrDefault(oa => oa.ObjetivoAprendizagemId.Equals(objetivoAprendizagemId));

            if (planejamentoAnualObjetivoAprendizagem.EhNulo())
                planejamentoAnualObjetivoAprendizagem = new PlanejamentoAnualObjetivoAprendizagem(planejamentoAnualComponenteId, objetivoAprendizagemId);
            else
                planejamentoAnualObjetivoAprendizagem.Excluido = false;
            return planejamentoAnualObjetivoAprendizagem;
        }

        private async Task<PlanejamentoAnualPeriodoEscolar> ObterPlanejamentoAnualPeriodoEscolar(long id, long periodoEscolarId)
        {
            var planejamentoAnualPeriodoEscolar = await repositorioPlanejamentoAnualPeriodoEscolar
                    .ObterPorPlanejamentoAnualIdEPeriodoId(id, periodoEscolarId, true);

            if (planejamentoAnualPeriodoEscolar.EhNulo())
                planejamentoAnualPeriodoEscolar = new PlanejamentoAnualPeriodoEscolar(periodoEscolarId, id);
            else
                planejamentoAnualPeriodoEscolar.Excluido = false;
            return planejamentoAnualPeriodoEscolar;
        }

        private async Task<PlanejamentoAnualComponente> ObterPlanejamentoAnualComponenteCurricular(long componenteCurricularId, 
                                                                                                   long planejamentoAnualPeriodoEscolarId, 
                                                                                                   string componenteCurricularDescricao)
        {
            var planejamentoAnualComponente = await repositorioPlanejamentoAnualComponente
                        .ObterPorPlanejamentoAnualPeriodoEscolarId(componenteCurricularId, planejamentoAnualPeriodoEscolarId, true);

            if (planejamentoAnualComponente.EhNulo())
                planejamentoAnualComponente = new PlanejamentoAnualComponente(componenteCurricularId, componenteCurricularDescricao, planejamentoAnualPeriodoEscolarId);
            else
            {
                planejamentoAnualComponente.Excluido = false;
                planejamentoAnualComponente.Descricao = componenteCurricularDescricao;
            }
            return planejamentoAnualComponente;
        }
    }
}
