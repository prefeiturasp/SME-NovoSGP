using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterTurmasComplementaresPorAlunoQueryHandlerFake : IRequestHandler<ObterTurmasComplementaresPorAlunoQuery, IEnumerable<TurmaComplementarDto>>
    {
        public async Task<IEnumerable<TurmaComplementarDto>> Handle(ObterTurmasComplementaresPorAlunoQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<TurmaComplementarDto>()
            {
                new TurmaComplementarDto
                {
                    Ano = "1A",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTurma = "1",
                    TipoTurma = TipoTurma.Regular,
                    Id = 1,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "Turma Teste",
                    Semestre = 1
                }
            };
           return await Task.FromResult(lista);
        }
    }
}