using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasNaSemanaPorProfessorComponenteCurricularQueryHandler: IRequestHandler<ObterQuantidadeAulasNaSemanaPorProfessorComponenteCurricularQuery, int>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterQuantidadeAulasNaSemanaPorProfessorComponenteCurricularQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<int> Handle(ObterQuantidadeAulasNaSemanaPorProfessorComponenteCurricularQuery request, CancellationToken cancellationToken)
            => request.ExperienciaPedagogica ?
                await repositorioAula.ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(request.TurmaCodigo, request.Semana) :
                await repositorioAula.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(request.TurmaCodigo, request.ComponenteCurricular.ToString(), request.Semana, request.ProfessorRf, request.DataExcecao);

    }
}
