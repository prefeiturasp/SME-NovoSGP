using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteAulaNaDataTurmaDisciplinaProfessorRfQueryHandler : IRequestHandler<ExisteAulaNaDataTurmaDisciplinaProfessorRfQuery, bool>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ExisteAulaNaDataTurmaDisciplinaProfessorRfQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<bool> Handle(ExisteAulaNaDataTurmaDisciplinaProfessorRfQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ExisteAulaNaDataDataTurmaDisciplinaProfessorRfAsync(request.DataAula, request.TurmaCodigo, request.DisciplinaId, request.ProfessorRf, request.TipoAula);
    }
}
