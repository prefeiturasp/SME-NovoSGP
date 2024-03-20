using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse.ServicosFakes
{
    public class ObterTurmasFechamentoConselhoPorAlunosQueryHandlerFake : IRequestHandler<ObterTurmasFechamentoConselhoPorAlunosQuery, IEnumerable<TurmaAlunoDto>>
    {
        public ObterTurmasFechamentoConselhoPorAlunosQueryHandlerFake()
        { }

        public async Task<IEnumerable<TurmaAlunoDto>> Handle(ObterTurmasFechamentoConselhoPorAlunosQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<TurmaAlunoDto>()
            {
                new TurmaAlunoDto()
                {
                    TurmaCodigo = "2",
                    TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                    AlunoCodigo = 1,
                    Modalidade = Dominio.Modalidade.Medio
                },

                new TurmaAlunoDto()
                {
                    TurmaCodigo = "3",
                    TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                    AlunoCodigo = 1,
                    Modalidade = Dominio.Modalidade.Medio
                }
            });
        }
    }
}
