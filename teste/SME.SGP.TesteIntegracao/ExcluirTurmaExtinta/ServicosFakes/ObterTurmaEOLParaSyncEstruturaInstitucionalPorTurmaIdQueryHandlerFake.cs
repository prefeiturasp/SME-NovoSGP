using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ExcluirTurmaExtinta.ServicosFakes
{
    public class ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryFake : IRequestHandler<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery, TurmaParaSyncInstitucionalDto>
    {
        public async Task<TurmaParaSyncInstitucionalDto> Handle(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            var turmasParaSincronizacaoinstitucional = new List<TurmaParaSyncInstitucionalDto>()
            {
                new ()
                {
                    Ano = "4",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                    Codigo = 444,
                    TipoTurma = 1,
                    NomeTurma = "4A",
                    SerieEnsino = "4º ANO",
                    Extinta = true,
                    UeCodigo = "1",
                    NomeFiltro = "4A - 4º ANO",
                    Situacao = "E",
                    CodigoModalidade = Dominio.Modalidade.Medio,
                    DataStatusTurmaEscola = DateTimeExtension.HorarioBrasilia().AddYears(-1)
                },
                new ()
                {
                    Ano = "5",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                    Codigo = 555,
                    TipoTurma = 1,
                    NomeTurma = "5B",
                    SerieEnsino = "5º ANO",
                    Extinta = true,
                    UeCodigo = "1",
                    NomeFiltro = "5B - 5º ANO",
                    Situacao = "E",
                    CodigoModalidade = Dominio.Modalidade.Medio,
                    DataStatusTurmaEscola = DateTimeExtension.HorarioBrasilia().AddYears(-1)
                },
                new ()
                {
                    Ano = "6",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                    Codigo = 666,
                    TipoTurma = 1,
                    NomeTurma = "6B",
                    SerieEnsino = "6º ANO",
                    Extinta = true,
                    UeCodigo = "1",
                    NomeFiltro = "6B - 6º ANO",
                    Situacao = "E",
                    CodigoModalidade = Dominio.Modalidade.Medio,
                    DataStatusTurmaEscola = DateTimeExtension.HorarioBrasilia().AddYears(-1)
                },
                new ()
                {
                    Ano = "7",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                    Codigo = 777,
                    TipoTurma = 1,
                    NomeTurma = "7A",
                    SerieEnsino = "7º ANO",
                    Extinta = true,
                    UeCodigo = "1",
                    NomeFiltro = "7A - 7º ANO",
                    Situacao = "E",
                    CodigoModalidade = Dominio.Modalidade.Medio,
                    DataStatusTurmaEscola = DateTimeExtension.HorarioBrasilia().AddYears(-1)
                },
                new ()
                {
                    Ano = "8",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                    Codigo = 888,
                    TipoTurma = 1,
                    NomeTurma = "8B",
                    SerieEnsino = "8º ANO",
                    Extinta = true,
                    UeCodigo = "1",
                    NomeFiltro = "8B - 8º ANO",
                    Situacao = "E",
                    CodigoModalidade = Dominio.Modalidade.Medio,
                    DataStatusTurmaEscola = DateTimeExtension.HorarioBrasilia().AddYears(-1)
                },
                new ()
                {
                    Ano = "9",
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                    Codigo = 999,
                    TipoTurma = 1,
                    NomeTurma = "9A",
                    SerieEnsino = "9º ANO",
                    Extinta = true,
                    UeCodigo = "1",
                    NomeFiltro = "9A - 9º ANO",
                    Situacao = "E",
                    CodigoModalidade = Modalidade.Medio,
                    DataStatusTurmaEscola = DateTimeExtension.HorarioBrasilia().AddYears(-1)
                },
            };

            return await Task.FromResult(turmasParaSincronizacaoinstitucional.FirstOrDefault(f => f.Codigo == request.TurmaId));
        }
    }
}