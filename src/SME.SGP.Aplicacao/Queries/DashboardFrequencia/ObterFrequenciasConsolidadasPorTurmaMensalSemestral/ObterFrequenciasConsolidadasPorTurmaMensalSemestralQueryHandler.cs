using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasConsolidadasPorTurmaMensalSemestralQueryHandler : IRequestHandler<ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery, IEnumerable<FrequenciaGlobalMensalSemanalDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioFrequencia;

        public ObterFrequenciasConsolidadasPorTurmaMensalSemestralQueryHandler(IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public Task<IEnumerable<FrequenciaGlobalMensalSemanalDto>> Handle(ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioFrequencia.ObterFrequenciasConsolidadasPorTurmaMensalSemestral(request.AnoLetivo,
                                                                                                  request.DreId,
                                                                                                  request.UeId,
                                                                                                  request.Modalidade,
                                                                                                  request.TurmaIds,
                                                                                                  request.DataInicioSemmana,
                                                                                                  request.DataFimSemana,
                                                                                                  request.TipoConsolidadoFrequencia,
                                                                                                  request.VisaoDre);
        }
    }
}
