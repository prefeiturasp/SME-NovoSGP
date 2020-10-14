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
                                                     IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem, IMediator mediator) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }

        public async Task<bool> Handle(SalvarCopiaPlanejamentoAnualCommand request, CancellationToken cancellationToken)
        {
            var planejamentoAnualId = await repositorioPlanejamentoAnual.SalvarAsync(request.PlanejamentoAnual);

            foreach(var periodo in request.PlanejamentoAnual.PeriodosEscolares)
            {
                var copiaPeriodo = new PlanejamentoAnualPeriodoEscolar(periodo.PeriodoEscolarId, planejamentoAnualId);
                var planejamentoAnualPeriodoEscolarId = await repositorioPlanejamentoAnualPeriodoEscolar.SalvarAsync(copiaPeriodo);

                foreach(var componente in periodo.ComponentesCurriculares)
                {
                    var copiaComponente = new PlanejamentoAnualComponente(componente.ComponenteCurricularId, componente.Descricao, planejamentoAnualPeriodoEscolarId);
                    var planejamentoAnualComponenteId = await repositorioPlanejamentoAnualComponente.SalvarAsync(copiaComponente);

                    foreach(var objetivo in componente.ObjetivosAprendizagem)
                    {
                        var copiaObjetivo = new PlanejamentoAnualObjetivoAprendizagem(planejamentoAnualComponenteId, objetivo.ObjetivoAprendizagemId);
                        await repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarAsync(copiaObjetivo);
                    }
                }
            }

            return true;
        }
    }
}
