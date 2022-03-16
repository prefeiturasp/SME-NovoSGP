using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{ 
    public class ObterFechamentoTurmaDisciplinaPorIdQueryHandler : IRequestHandler<ObterFechamentoTurmaDisciplinaPorIdQuery, FechamentoTurmaDisciplina>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentoTurmaDisciplinaPorIdQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorio)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorio;
        }

        public async Task<FechamentoTurmaDisciplina> Handle(ObterFechamentoTurmaDisciplinaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplinaPorId(request.Id);
    }
}