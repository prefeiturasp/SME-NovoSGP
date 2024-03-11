using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralPorAlunosTurmaQueryHandler : IRequestHandler<ObterFrequenciaGeralPorAlunosTurmaQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaGeralPorAlunosTurmaQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciaGeralPorAlunosTurmaQuery request, CancellationToken cancellationToken)
        {         
            var frequenciaAlunosPeriodos = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralPorAlunosETurmas(request.CodigosAlunos, request.CodigoTurma);
            var frequenciaAlunosAgrupados = frequenciaAlunosPeriodos.GroupBy(fa => fa.CodigoAluno);
            List<FrequenciaAluno> frequenciaRetorno = new List<FrequenciaAluno>();

            foreach (var grupo in frequenciaAlunosAgrupados)
            {
                var codigoAluno = grupo.Key;
                var frequenciaAlunosDoAluno = grupo.ToList();

                var frequenciaAluno = new FrequenciaAluno()
                {
                    TotalAulas = grupo.Sum(f => f.TotalAulas),
                    TotalAusencias = grupo.Sum(f => f.TotalAusencias),
                    TotalCompensacoes = grupo.Sum(f => f.TotalCompensacoes),
                    CodigoAluno = codigoAluno
                };

                frequenciaRetorno.Add(frequenciaAluno);
            }

            return frequenciaRetorno;
        }
    }
}
