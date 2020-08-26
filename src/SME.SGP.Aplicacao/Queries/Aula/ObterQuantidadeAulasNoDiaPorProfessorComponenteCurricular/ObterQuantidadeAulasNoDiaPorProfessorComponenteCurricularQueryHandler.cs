using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQueryHandler : IRequestHandler<ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQuery, int>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<int> Handle(ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQuery request, CancellationToken cancellationToken)
            => request.ExperienciaPedagogica ?
                await repositorioAula.ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(request.TurmaCodigo, request.Data) :
                await repositorioAula.ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(request.TurmaCodigo, request.ComponenteCurricular.ToString(), request.Data, request.ProfessorRf);

    }
}
