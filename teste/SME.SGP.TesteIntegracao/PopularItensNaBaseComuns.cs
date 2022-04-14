using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class PopularItensNaBaseComuns : TesteBase
    {
        public PopularItensNaBaseComuns(TestFixture testFixture) : base(testFixture)
        {
        }

        public async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 1,
                TipoCalendarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                PeriodoInicio = new DateTime(2022, 01, 01),
                PeriodoFim = new DateTime(2022, 02, 01)
            });
        }

        public async Task CrieTurma(string ano)
        {
            await InserirNaBase(new Turma
            {
                Id = 1,
                UeId = 1,
                Ano = ano,
                CodigoTurma = "1"
            });
        }

        public async Task CrieAula()
        {
            await InserirNaBase(new Aula
            {
                Id = 1,
                CriadoPor = "",
                CriadoRF = "",
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                ProfessorRf = "",
                TipoCalendarioId = 1,
                DataAula = new DateTime(2022, 01, 15),
                Quantidade = 4
            });
        }
    }
}
