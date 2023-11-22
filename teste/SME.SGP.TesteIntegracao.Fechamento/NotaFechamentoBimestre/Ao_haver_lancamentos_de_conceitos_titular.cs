using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using System;
using Moq;
using System.Threading;
using SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_haver_lancamentos_de_conceitos_titular : NotaFechamentoBimestreTesteBase
    {
        public Ao_haver_lancamentos_de_conceitos_titular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
                typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerAnoAnteriorFake), ServiceLifetime.Scoped));
           
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>),
                typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), 
                    typeof(SME.SGP.TesteIntegracao.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>), 
                typeof(ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task deve_haver_lancamento_com_mais_de_50_porcento_NS_para_inserir_justificativa()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDtoFundamental(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito(ehPorcentagem);
        }

        [Fact]
        public async Task deve_haver_lancamento_com_mais_de_50_porcento_NS_para_inserir_justificativa_regencia()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDtoFundamental(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceitoRegencia(ehPorcentagem);
        }

        [Fact]
        public async Task deve_haver_lancamento_de_conceito_para_componente_fundamental()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDtoFundamental(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito();
        }
        [Fact]
        public async Task deve_haver_lancamento_de_conceito_para_componente_EJA()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDtoEJA(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito();
        }
        [Fact]
        public async Task deve_haver_lancamento_para_regencia_de_classe_fundamental()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDtoFundamental(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito();
        }

        [Fact]
        public async Task deve_haver_lancamento_para_regencia_de_classe_EJA()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDtoEJA(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito();
        }

        private async Task ExecutarComandoConceito(bool ehPorcentagem = false)
        {
            var fechamentoNotaDto = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CodigoAluno = ALUNO_CODIGO_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= (int) ConceitoValores.NS,
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CodigoAluno = ALUNO_CODIGO_2,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= (int) ConceitoValores.NS,
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.P,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CodigoAluno = ALUNO_CODIGO_3,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= (int) ConceitoValores.P,
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                }
            };

            var fechamentoTurmaDisciplinaDto = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = 1 ,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Justificativa = "Teste" ,
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentoNotaDto
                }
            };

            if (ehPorcentagem)
                await ExecutarTesteMaiorQue50Porcento(fechamentoTurmaDisciplinaDto);

            await ExecutarTeste(fechamentoTurmaDisciplinaDto);
        }

        private async Task ExecutarComandoConceitoRegencia(bool ehPorcentagem = false)
        {
            var fechamentoNotaDto = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_1,
                    DisciplinaId = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                    ConceitoId= (int) ConceitoValores.NS,
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = "",
                    AlteradoRf = "",
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_2,
                    DisciplinaId = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                    ConceitoId= (int) ConceitoValores.NS,
                    CriadoPor= "",
                    CriadoRf= "",
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.P,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = "",
                    AlteradoRf = "",
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_3,
                    DisciplinaId = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                    ConceitoId= (int) ConceitoValores.P,
                    CriadoPor= "",
                    CriadoRf= "",
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                }
            };

            var fechamentoTurmaDisciplinaDto = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = 1 ,
                    DisciplinaId = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                    Justificativa = "teste" ,
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentoNotaDto
                }
            };

            if (ehPorcentagem)
                await ExecutarTesteMaiorQue50Porcento(fechamentoTurmaDisciplinaDto);

            await ExecutarTeste(fechamentoTurmaDisciplinaDto);
        }



        private async Task ExecutarTesteMaiorQue50Porcento(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            await comando.Salvar(fechamentoTurma);
            var notasFechamento = ObterTodos<FechamentoNota>();

            notasFechamento.ShouldNotBeNull();
            notasFechamento.ShouldNotBeEmpty();
            notasFechamento.Count(x => x.ConceitoId == (long)ConceitoValores.NS).ShouldBeGreaterThan((notasFechamento.Count() / 2));
        }

        private async Task ExecutarTeste(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            await comando.Salvar(fechamentoTurma);
            var notasFechamento = ObterTodos<FechamentoNota>();

            notasFechamento.ShouldNotBeNull();
            notasFechamento.ShouldNotBeEmpty();
        }



        private FiltroFechamentoNotaDto ObterFiltroFechamentoNotaDtoFundamental(string perfil, string anoTurma, bool consideraAnorAnterior = false)
        {
            return new FiltroFechamentoNotaDto()
            {
                Perfil = perfil,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = consideraAnorAnterior,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina
            };
        }

        private FiltroFechamentoNotaDto ObterFiltroFechamentoNotaDtoEJA(string perfil, string anoTurma, bool consideraAnorAnterior = false)
        {
            return new FiltroFechamentoNotaDto()
            {
                Perfil = perfil,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = consideraAnorAnterior,
                Modalidade = Modalidade.EJA,
                TipoCalendario = ModalidadeTipoCalendario.EJA,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina
            };
        }
    }
}