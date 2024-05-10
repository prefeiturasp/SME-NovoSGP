using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQueryHandler : IRequestHandler<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQueryHandler(IRepositorioAbrangencia repositorioAbrangencia )
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery request, CancellationToken cancellationToken)
        {
            var retorno = await repositorioAbrangencia.ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestreAnos(request.CodigoUe, request.AnoLetivo, request.Modalidade, request.Semestre, request.Anos);
            return retorno ?? throw new NegocioException("Não foi encontrada turmas para as informações enviadas");
        }
    }
}
