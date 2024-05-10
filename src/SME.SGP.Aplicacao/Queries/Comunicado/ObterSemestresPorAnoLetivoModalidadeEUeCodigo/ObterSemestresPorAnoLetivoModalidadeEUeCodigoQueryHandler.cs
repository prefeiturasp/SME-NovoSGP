using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSemestresPorAnoLetivoModalidadeEUeCodigoQueryHandler : IRequestHandler<ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery, IEnumerable<int>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterSemestresPorAnoLetivoModalidadeEUeCodigoQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<IEnumerable<int>> Handle(ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery request, CancellationToken cancellationToken)
        {
            var semestres = await repositorioComunicado.ObterSemestresPorAnoLetivoModalidadeEUeCodigo(request.Login,
                                                                                                      request.Perfil,
                                                                                                      request.Modalidade,
                                                                                                      request.ConsideraHistorico,
                                                                                                      request.AnoLetivo,
                                                                                                      request.UeCodigo);
            return semestres;
        }
    }
}
