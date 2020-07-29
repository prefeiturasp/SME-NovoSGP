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
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQueryHandler : IRequestHandler<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQueryHandler(IRepositorioAbrangencia repositorioAbrangencia )
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery request, CancellationToken cancellationToken)
        {
            var retorno = await repositorioAbrangencia.ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestre(request.CodigoUe, request.AnoLetivo, request.Modalidade, request.Semestre);

            return retorno ?? throw new NegocioException("Não foi encontrada turmas para as informações enviadas");
        }
    }
}
