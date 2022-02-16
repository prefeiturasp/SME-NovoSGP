using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaQueryHandler : IRequestHandler<ObterFechamentoTurmaDisciplinaQuery, FechamentoTurmaDisciplina>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentoTurmaDisciplinaQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<FechamentoTurmaDisciplina> Handle(ObterFechamentoTurmaDisciplinaQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(request.TurmaCodigo, request.DisciplinaId);
    }
}
