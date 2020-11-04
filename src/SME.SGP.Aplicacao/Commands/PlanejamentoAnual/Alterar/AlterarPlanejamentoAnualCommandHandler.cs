//using MediatR;
//using SME.SGP.Dominio;
//using SME.SGP.Dominio.Interfaces;
//using SME.SGP.Infra;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace SME.SGP.Aplicacao
//{
//    public class AlterarPlanejamentoAnualCommandHandler : IRequestHandler<AlterarPlanejamentoAnualCommand, AuditoriaDto>
//    {
//        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
//        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
//        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
//        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;

//        public AlterarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
//                                                      IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
//                                                      IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
//                                                      IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem)
//        {
//            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
//            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
//            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
//            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
//        }
//        public async Task<AuditoriaDto> Handle(AlterarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
//        {
//            if (comando.PlanejamentoAnualPeriodoEscolarId == 0)
//            {
//                var planejamentoPeriodoEscolar = new PlanejamentoAnualPeriodoEscolar(comando.PeriodoEscolarId)
//                {
//                    PlanejamentoAnualId = comando.Id
//                };
//                await repositorioPlanejamentoAnualPeriodoEscolar.SalvarAsync(planejamentoPeriodoEscolar);
//                comando.PlanejamentoAnualPeriodoEscolarId = planejamentoPeriodoEscolar.Id;
//            }



//            var planejamentoAnualComponentes = new List<PlanejamentoAnualComponente>();
//            foreach (var componente in comando.Componentes)
//            {
//                if (componente.PlanejamentoAnualComponenteCurricularId == 0)
//                {
//                    var planejamentoAnualComponente = new PlanejamentoAnualComponente
//                    {
//                        ComponenteCurricularId = componente.ComponenteCurricularId,
//                        Descricao = componente.Descricao,
//                        PlanejamentoAnualPeriodoEscolarId = comando.PlanejamentoAnualPeriodoEscolarId
//                    };
//                    await repositorioPlanejamentoAnualComponente.SalvarAsync(planejamentoAnualComponente);
//                    componente.PlanejamentoAnualComponenteCurricularId = planejamentoAnualComponente.Id;
//                    var objetivos = componente.ObjetivosAprendizagemId.Select(c => new PlanejamentoAnualObjetivoAprendizagem
//                    {
//                        ObjetivoAprendizagemId = c,
//                        PlanejamentoAnualComponenteId = componente.PlanejamentoAnualComponenteCurricularId
//                    });

//                    await Task.Run(() => repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarVarios(objetivos));
//                }
//                else
//                {
//                    var objetivosBase = await repositorioPlanejamentoAnualObjetivosAprendizagem.ObterObjetivosPorComponentePlanejamento()
//                }
//            }
//        }
//    }
//}
