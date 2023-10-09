using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler : IRequestHandler<ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery, IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno;
        private readonly IRepositorioCache repositorioCache;

        public ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno,
                                                                          IRepositorioCache repositorioCache)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> Handle(ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery request, CancellationToken cancellationToken)
        {
            var idsRegistrosFrequenciaAlunoDesconsiderados = new List<long>();

            foreach (var turma in request.TurmasId)
            {
                var chaveCache = string.Format(NomeChaveCache.REGISTROS_FREQUENCIA_ALUNO_EXCLUIDOS_TURMA, turma);
                var registrosCache = await repositorioCache.ObterAsync(chaveCache);
                if (registrosCache != null && registrosCache.Any())
                    idsRegistrosFrequenciaAlunoDesconsiderados.AddRange(JsonConvert.DeserializeObject<long[]>(registrosCache));
            }

            return await repositorioRegistroFrequenciaAluno
                .ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(request.DataAula, request.TurmasId, request.Alunos, professor: request.Professor, idsRegistrosFrequenciaAlunoDesconsiderados.ToArray());
        }
    }
}
