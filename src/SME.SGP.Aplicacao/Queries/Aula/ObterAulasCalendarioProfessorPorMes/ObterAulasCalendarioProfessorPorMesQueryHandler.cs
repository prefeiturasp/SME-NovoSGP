using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasCalendarioProfessorPorMesQueryHandler : IRequestHandler<ObterAulasCalendarioProfessorPorMesQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulasCalendarioProfessorPorMesQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }
        public async Task<IEnumerable<Aula>> Handle(ObterAulasCalendarioProfessorPorMesQuery request, CancellationToken cancellationToken)
        {
            return await  repositorioAula.ObterAulasProfessorCalendarioPorMes(request.TipoCalendarioId, request.TurmaCodigo, request.UeCodigo, request.Mes); 
        }
    }
}
