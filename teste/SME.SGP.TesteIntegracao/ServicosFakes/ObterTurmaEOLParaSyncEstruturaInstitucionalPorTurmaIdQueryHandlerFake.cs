using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryHandlerFake : IRequestHandler<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery, TurmaParaSyncInstitucionalDto>
    {
        public async Task<TurmaParaSyncInstitucionalDto> Handle(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new TurmaParaSyncInstitucionalDto()
            {
                Ano = "4",
                AnoLetivo = 2022,
                Codigo = 444,
                TipoTurma = 1,
                NomeTurma = "4A",
                SerieEnsino = "4º ANO",
                Extinta = true,
                UeCodigo = "094765",
                NomeFiltro = "4A - 4º ANO",
                Situacao = "E",
                CodigoModalidade = Dominio.Modalidade.Medio,
                DataStatusTurmaEscola = System.DateTime.Now.AddYears(-1)
            });
        }
    }
}