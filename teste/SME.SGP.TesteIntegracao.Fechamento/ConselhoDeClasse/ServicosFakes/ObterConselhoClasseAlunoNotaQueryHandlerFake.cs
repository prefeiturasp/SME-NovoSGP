using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterConselhoClasseAlunoNotaQueryHandlerFake : IRequestHandler<ObterConselhoClasseAlunoNotaQuery, IEnumerable<ConselhoClasseAlunoNotaDto>>
    {
        public async Task<IEnumerable<ConselhoClasseAlunoNotaDto>> Handle(ObterConselhoClasseAlunoNotaQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<ConselhoClasseAlunoNotaDto>()
            {
                new ConselhoClasseAlunoNotaDto
                {
                    AlunoCodigo = "3",
                    Nota = "8",
                    ComponenteCurricularId = 2,
                    Descricao = "MATEMATICA"
                },
                new ConselhoClasseAlunoNotaDto
                {
                    AlunoCodigo = "3",
                    Nota = "8",
                    ComponenteCurricularId = 6,
                    Descricao = "EDUCACAO FISICA"
                },
                new ConselhoClasseAlunoNotaDto
                {
                    AlunoCodigo = "2",
                    Nota = null,
                    ComponenteCurricularId = 7,
                    Descricao = "HISTORIA"
                },
                new ConselhoClasseAlunoNotaDto
                {
                    AlunoCodigo = "1",
                    Nota = null,
                    ComponenteCurricularId = 9,
                    Descricao = "INGLES"
                },
            };
            return await Task.FromResult(lista);
        }
    }
}