using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.ConselhoDeClasse;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse
{
    public class Ao_obter_pareceres_conclusivos_turma : ConselhoDeClasseTesteBase
    {
        public Ao_obter_pareceres_conclusivos_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Conselho Classe - Deve retornar todos os pareceres conclusivos da turma")]
        public async Task Ao_obter_pareceres_conclusivos()
        {
            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_4,
                AnoTurma = "5",
                ComponenteCurricular = COMPONENTE_CURRICULAR_512.ToString(),
                DataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                CriarPeriodoReabertura = false,
            };

            await CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(filtroNota);

            var useCase = ServiceProvider.GetService<IObterPareceresConclusivosTurmaUseCase>();
            var retorno = await useCase.Executar(TURMA_ID_1, false);

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            retorno.ToList().Exists(parecer => parecer.Nome == "Retido por frequência");
            retorno.ToList().Exists(parecer => parecer.Nome == "Continuidade dos estudos");
        }


        [Fact(DisplayName = "Conselho Classe - Deve retornar todos os pareceres conclusivos da turma ano anterior")]
        public async Task Ao_obter_pareceres_conclusivos_ano_anterior()
        {
            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_4,
                AnoTurma = "5",
                ComponenteCurricular = COMPONENTE_CURRICULAR_512.ToString(),
                DataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                CriarPeriodoReabertura = false,
                AnoParecerAnterior = true
            };

            await CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(filtroNota);

            var useCase = ServiceProvider.GetService<IObterPareceresConclusivosTurmaUseCase>();
            var retorno = await useCase.Executar(TURMA_ID_1, true);

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            retorno.ToList().Exists(parecer => parecer.Nome == "Retido por frequência");
            retorno.ToList().Exists(parecer => parecer.Nome == "Continuidade dos estudos");
        }

        [Fact(DisplayName = "Conselho Classe - Deve retornar pareceres EM para turmas com ano P")]
        public async Task Ao_obter_pareceres_conclusivos_ensino_medio_turmas_ano_p()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var inicioVigencia = new DateTime(2014, 01, 01);
            var dre = new Dre() { Id = 1, CodigoDre = "100000", Abreviacao = "DRE", Nome = "DRE TESTE", DataAtualizacao = dataAtual.Date };
            var ue = new Ue { Id = 1, CodigoUe = "123456", DreId = 1, Nome = "UE TESTE", TipoEscola = TipoEscola.EMEFM, DataAtualizacao = dataAtual.Date };
            var turma = new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                Nome = "PA",
                Ano = "P",
                AnoLetivo = dataAtual.Year,
                ModalidadeCodigo = Modalidade.Medio,
                Semestre = 0,
                QuantidadeDuracaoAula = 4,
                TipoTurno = 3,
                DataAtualizacao = dataAtual,
                EtapaEJA = 0,
                SerieEnsino = "2º MODULO",
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                NomeFiltro = "PA - 2º MODULO"
            };
            var conselhoClasseParecerPromovidoConselho = new ConselhoClasseParecer()
            {
                Nome = "Promovido pelo conselho",
                Aprovado = true,
                Frequencia = false,
                Conselho = true,
                InicioVigencia = inicioVigencia,
                FimVigencia = null,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia,
                Nota = false
            };
            var conselhoClasseParecerRetido = new ConselhoClasseParecer()
            {
                Nome = "Retido",
                Aprovado = false,
                Frequencia = false,
                Conselho = true,
                InicioVigencia = inicioVigencia,
                FimVigencia = null,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia,
                Nota = true
            };
            var conselhoClasseParecerRetidoFrequencia = new ConselhoClasseParecer()
            {
                Nome = "Retido por frequência",
                Aprovado = false,
                Frequencia = true,
                Conselho = false,
                InicioVigencia = inicioVigencia,
                FimVigencia = null,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia,
                Nota = false
            };
            var conselhoClasseParecerPromovido = new ConselhoClasseParecer()
            {
                Nome = "Promovido",
                Aprovado = true,
                Frequencia = true,
                Conselho = false,
                InicioVigencia = inicioVigencia,
                FimVigencia = null,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia,
                Nota = true
            };
            var conselhoClasseParecerAnoParecerRetidoFrequencia = new ConselhoClasseParecerAno()
            {
                ParecerId = 3,
                AnoTurma = 1,
                Modalidade = (int)Modalidade.Medio,
                InicioVigencia = inicioVigencia,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia
            };
            var conselhoClasseParecerAnoParecerPromovido = new ConselhoClasseParecerAno()
            {
                ParecerId = 4,
                AnoTurma = 1,
                Modalidade = (int)Modalidade.Medio,
                InicioVigencia = inicioVigencia,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia
            };
            var conselhoClasseParecerAnoParecerPromovidoConselho = new ConselhoClasseParecerAno()
            {
                ParecerId = 1,
                AnoTurma = 1,
                Modalidade = (int)Modalidade.Medio,
                InicioVigencia = inicioVigencia,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia
            };
            var conselhoClasseParecerAnoParecerRetido = new ConselhoClasseParecerAno()
            {
                ParecerId = 2,
                AnoTurma = 1,
                Modalidade = (int)Modalidade.Medio,
                InicioVigencia = inicioVigencia,
                CriadoPor = "SISTEMA",
                CriadoRF = "0",
                CriadoEm = inicioVigencia
            };

            await InserirNaBase(dre);
            await InserirNaBase(ue);
            await InserirNaBase(turma);
            await InserirNaBase(conselhoClasseParecerPromovidoConselho);
            await InserirNaBase(conselhoClasseParecerRetido);
            await InserirNaBase(conselhoClasseParecerRetidoFrequencia);
            await InserirNaBase(conselhoClasseParecerPromovido);
            await InserirNaBase(conselhoClasseParecerAnoParecerRetidoFrequencia);
            await InserirNaBase(conselhoClasseParecerAnoParecerPromovido);
            await InserirNaBase(conselhoClasseParecerAnoParecerPromovidoConselho);
            await InserirNaBase(conselhoClasseParecerAnoParecerRetido);

            var repositorioParecer = ServiceProvider.GetService<IRepositorioConselhoClasseParecerConclusivo>();
            var mediator = ServiceProvider.GetService<IMediator>();
            var query = new ObterPareceresConclusivosPorTurmaQueryHandler(repositorioParecer, mediator);

            var resultado = await query.Handle(new ObterPareceresConclusivosPorTurmaQuery(turma), new System.Threading.CancellationToken());

            resultado.ShouldContain(x => x.Nome == conselhoClasseParecerPromovidoConselho.Nome);
            resultado.ShouldContain(x => x.Nome == conselhoClasseParecerRetido.Nome);
            resultado.ShouldContain(x => x.Nome == conselhoClasseParecerRetidoFrequencia.Nome);
            resultado.ShouldContain(x => x.Nome == conselhoClasseParecerPromovido.Nome);
        }
    }
}
