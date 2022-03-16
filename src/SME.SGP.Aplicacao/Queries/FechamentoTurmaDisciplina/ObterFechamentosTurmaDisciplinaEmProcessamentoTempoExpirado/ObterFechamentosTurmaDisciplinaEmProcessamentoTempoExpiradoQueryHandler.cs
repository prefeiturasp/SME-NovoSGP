using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosTurmaDisciplinaEmProcessamentoTempoExpiradoQueryHandler : IRequestHandler<ObterFechamentosTurmaDisciplinaEmProcessamentoTempoExpiradoQuery, IEnumerable<(long fechamentoTurmaDisciplinaId, long periodoEscolarId, string codigoRf)>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentosTurmaDisciplinaEmProcessamentoTempoExpiradoQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<(long fechamentoTurmaDisciplinaId, long periodoEscolarId, string codigoRf)>> Handle(ObterFechamentosTurmaDisciplinaEmProcessamentoTempoExpiradoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurmaDisciplina
                .ObterFechamentosTurmaDisciplinaEmProcessamentoComTempoExpirado(request.DataInicio, request.TempoConsideradoExpiradoEmMinutos);
        }
    }
}
