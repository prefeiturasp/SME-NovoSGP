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
            var planejamentoAnualPeriodosEscolares = await repositorioPlanejamentoAnualPeriodoEscolar.ObterPorPlanejamentoAnualId(planejamentoAnual);


            foreach (var pape in planejamentoAnualPeriodosEscolares)
            {
                var planejamentoAnualComponente = await repositorioPlanejamentoAnualComponente.ObterPorPlanejamentoAnualPeriodoEscolarId(comando.ComponenteCurricularId, pape.Id);
                var planejamentoAnualObjetivosAprendizagem = await repositorioPlanejamentoAnualObjetivosAprendizagem.ObterPorPlanejamentoAnualComponenteId(planejamentoAnualComponente.Id);

                foreach(var paoa in planejamentoAnualObjetivosAprendizagem)
                {
                    repositorioPlanejamentoAnualObjetivosAprendizagem.Remover(paoa.Id);
                }

                repositorioPlanejamentoAnualComponente.Remover(planejamentoAnualComponente.Id);

                repositorioPlanejamentoAnualPeriodoEscolar.Remover(pape.Id);
            }

            repositorioPlanejamentoAnual.Remover(planejamentoAnual);
            
            return true;
        }
    }
}
