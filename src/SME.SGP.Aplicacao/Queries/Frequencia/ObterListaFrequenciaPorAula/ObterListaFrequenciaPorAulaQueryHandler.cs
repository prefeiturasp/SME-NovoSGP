using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterListaFrequenciaPorAulaQueryHandler : IRequestHandler<ObterListaFrequenciaPorAulaQuery, IEnumerable<RegistroAusenciaAluno>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequenciaConsulta;

        public ObterListaFrequenciaPorAulaQueryHandler(IRepositorioFrequenciaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFrequenciaConsulta = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<RegistroAusenciaAluno>> Handle(ObterListaFrequenciaPorAulaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaConsulta.ObterListaFrequenciaPorAula(request.AulaId);
    }
}