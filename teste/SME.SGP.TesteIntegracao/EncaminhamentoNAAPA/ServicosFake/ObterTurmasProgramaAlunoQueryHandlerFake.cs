using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake
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