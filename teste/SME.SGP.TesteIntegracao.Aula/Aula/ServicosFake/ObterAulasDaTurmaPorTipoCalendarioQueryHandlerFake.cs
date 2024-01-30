using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.Aula.ServicosFake
{
    public class ObterAulasDaTurmaPorTipoCalendarioQueryHandlerFake : IRequestHandler<ObterAulasDaTurmaPorTipoCalendarioQuery, IEnumerable<Dominio.Aula>>
    {
        public Task<IEnumerable<Dominio.Aula>> Handle(ObterAulasDaTurmaPorTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Dominio.Aula>>(new List<Dominio.Aula>()
            {
                new Dominio.Aula()
                {
                    Id = 1,
                    DataAula = new DateTime(DateTime.Now.Year, 09,16),
                    TurmaId = "1",
                    DisciplinaId = "1105",
                    DadosComplementares = new AulaDadosComplementares() { PossuiFrequencia = true},
                    CriadoEm = DateTime.Now,
                    CriadoPor = "Sistema",
                    CriadoRF = "1",
                    Excluido = false,
                    UeId = "1",
                    ProfessorRf = "2222222",
                    TipoCalendarioId = 1,
                    Quantidade = 1,
                    RecorrenciaAula = RecorrenciaAula.AulaUnica,
                    TipoAula = TipoAula.Normal
                },
                 new Dominio.Aula()
                {
                    Id = 2,
                    DataAula = new DateTime(DateTime.Now.Year, 10,01),
                    TurmaId = "1",
                    DisciplinaId = "1105",
                    DadosComplementares = new AulaDadosComplementares() { PossuiFrequencia = false},
                    CriadoEm = DateTime.Now,
                    CriadoPor = "Sistema",
                    CriadoRF = "1",
                    Excluido = false,
                    UeId = "1",
                    ProfessorRf = "2222222",
                    TipoCalendarioId = 1,
                    Quantidade = 1,
                    RecorrenciaAula = RecorrenciaAula.AulaUnica,
                    TipoAula = TipoAula.Normal
                },
                  new Dominio.Aula()
                {
                    Id = 3,
                    DataAula = new DateTime(DateTime.Now.Year, 01,03),
                    TurmaId = "1",
                    DisciplinaId = "1105",
                    DadosComplementares = new AulaDadosComplementares() { PossuiFrequencia = false},
                    CriadoEm = DateTime.Now,
                    CriadoPor = "Sistema",
                    CriadoRF = "1",
                    Excluido = false,
                    UeId = "1",
                    ProfessorRf = "2222222",
                    TipoCalendarioId = 1,
                    Quantidade = 1,
                    RecorrenciaAula = RecorrenciaAula.AulaUnica,
                    TipoAula = TipoAula.Normal
                }
            });
        }
    }
}
