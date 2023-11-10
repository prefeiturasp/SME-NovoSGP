using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoBimestre.ServicosFakes
{
    public class ObterConselhoClassePorFechamentoIdQueryHandlerFake : IRequestHandler<ObterConselhoClassePorFechamentoIdQuery, ConselhoClasse>
    {
        public ObterConselhoClassePorFechamentoIdQueryHandlerFake()
        {
        }

        public async Task<ConselhoClasse> Handle(ObterConselhoClassePorFechamentoIdQuery request, CancellationToken cancellationToken)
        {

            return new ConselhoClasse()
            {
                FechamentoTurmaId = 2,
                Situacao = SituacaoConselhoClasse.EmAndamento,
                FechamentoTurma = new FechamentoTurma()
                {
                    Id = 2,
                    PeriodoEscolarId = 1,
                    TurmaId = 1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = "Sistema",
                    CriadoRF = "0"
                },
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0",
            };
        }
    }
}
