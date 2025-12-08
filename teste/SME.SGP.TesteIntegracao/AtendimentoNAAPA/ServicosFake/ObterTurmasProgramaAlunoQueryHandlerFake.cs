using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake
{
    public class ObterTurmasProgramaAlunoQueryHandlerFakeNAAPA : IRequestHandler<ObterTurmasProgramaAlunoQuery, IEnumerable<AlunoTurmaProgramaDto>>
    {

        public async Task<IEnumerable<AlunoTurmaProgramaDto>> Handle(ObterTurmasProgramaAlunoQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<AlunoTurmaProgramaDto>()
              {
                  new AlunoTurmaProgramaDto() {
                     DreUe = "Ue/Dre 01",
                     Turma = "Turma 09",
                     ComponenteCurricular = "0001"
                    }
            };

            return await Task.FromResult(alunos);
        }
    }
}