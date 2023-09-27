using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AnotacaoFechamentoAluno
{
    public class Ao_registrar_anotacao_fechamento : TesteBase
    {
        public Ao_registrar_anotacao_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_salvar_anotacao()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await CarregarDados();

            var anotacaoFechamento = new Dominio.AnotacaoFechamentoAluno()
            {
                FechamentoAlunoId = 1,
                Anotacao = "Anotação teste",
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = ""
            };

            await mediator.Send(new SalvarAnotacaoFechamentoAlunoCommand(anotacaoFechamento));

            var retorno = ObterTodos<Dominio.AnotacaoFechamentoAluno>();

            retorno.ShouldNotBeEmpty();
            retorno.First().FechamentoAlunoId.ShouldBe(1, "Anotação salva com sucesso!");
        }

        public async Task CarregarDados()
        {
            await InserirNaBase(new TipoCalendario() 
            { 
                Id = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Calendário Teste Ano Atual",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1,1),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1,12),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                CodigoDre = "11",
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = "22"
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1234",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new FechamentoTurma()
            {
                Id = 1,
                PeriodoEscolarId = 1,
                TurmaId = 1,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                Id = 1,
                FechamentoTurmaId = 1,
                DisciplinaId = 1,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoAluno()
            {
                Id = 1,
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = "123123",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

        }
    }
}
