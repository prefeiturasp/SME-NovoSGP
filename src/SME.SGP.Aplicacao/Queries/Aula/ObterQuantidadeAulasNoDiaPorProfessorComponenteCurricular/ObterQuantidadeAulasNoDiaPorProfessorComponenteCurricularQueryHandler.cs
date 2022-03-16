using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQueryHandler : IRequestHandler<ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQuery, int>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<int> Handle(ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQuery request, CancellationToken cancellationToken)
            => request.ExperienciaPedagogica ?
                await repositorioAula.ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(request.TurmaCodigo, request.Data) :
                await repositorioAula.ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(request.TurmaCodigo, request.ComponenteCurricular.ToString(), request.Data, request.ProfessorRf, request.EhGestor);

    }
}
