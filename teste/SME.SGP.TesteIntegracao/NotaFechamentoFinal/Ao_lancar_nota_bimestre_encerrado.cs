using System;
using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_lancar_nota_bimestre_encerrado : NotaFechamentoTesteBase
    {
        public Ao_lancar_nota_bimestre_encerrado(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Nao_deve_permitir_lancamento_nota_com_periodo_escolar_no_4_bimestre_encerrada_sem_periodo_reabertura()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            filtroNotaFechamento.CriarPeriodoEscolar = false;
            
            filtroNotaFechamento.CriarPeriodoAbertura = false;
            
            await InserirPeriodoEscolarCustomizado();
            
            await ExecutarTesteComExcecao(filtroNotaFechamento);
        }
        
        [Fact]
        public async Task Deve_permitir_lancamento_nota_com_periodo_escolar_no_4_bimestre_encerrada_com_periodo_abertura()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            filtroNotaFechamento.CriarPeriodoEscolar = false;
            
            filtroNotaFechamento.CriarPeriodoAbertura = false;
            
            await InserirPeriodoEscolarCustomizado();

            await InserirPeriodoAberturaCustomizado();
            
            await ExecutarTeste(filtroNotaFechamento);
        }
        
        [Fact]
        public async Task Deve_permitir_lancamento_nota_com_periodo_escolar_no_4_bimestre_encerrada_com_periodo_reabertura()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            filtroNotaFechamento.CriarPeriodoEscolar = false;
            
            filtroNotaFechamento.CriarPeriodoAbertura = false;
            
            await InserirPeriodoEscolarCustomizado();
            
            await ExecutarTeste(filtroNotaFechamento, true);
        }
        
        private async Task InserirPeriodoEscolarCustomizado(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();
            
            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_4, TIPO_CALENDARIO_1);
        }
        
        private async Task InserirPeriodoAberturaCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new PeriodoFechamento()
                { CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF });

            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia.AddDays(-209),
                FinalDoFechamento =  dataReferencia.AddDays(-205)
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento =  dataReferencia.AddDays(-116)
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia.AddDays(-120),
                FinalDoFechamento =  dataReferencia.AddDays(-116)
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia.AddDays(-38),
                FinalDoFechamento =  dataReferencia.AddDays(-34)
            });  
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                PeriodoFechamentoId = 1, 
                InicioDoFechamento = dataReferencia,
                FinalDoFechamento =  dataReferencia.AddDays(4)
            });  
        }
        
        private async Task InserirPeriodoReaberturaCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = DateTimeExtension.HorarioBrasilia().Date,
                Fim = DateTimeExtension.HorarioBrasilia().AddYears(1).Date,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = NUMERO_1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
        
        private async Task ExecutarTeste(FiltroNotaFechamentoDto filtroNotaFechamentoDto, bool criarPeriodoReabertura = false)
        {
            await CriarDadosBase(filtroNotaFechamentoDto);

            if (criarPeriodoReabertura)
                await InserirPeriodoReaberturaCustomizado();

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamentoDto);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
        }
        
        private async Task ExecutarTesteComExcecao(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            await CriarDadosBase(filtroNotaFechamentoDto);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamentoDto);
            
            await ExecutarComandosFechamentoFinalComExcecao(fechamentoFinalSalvarDto);
        }
        
        private FechamentoFinalSalvarDto ObterFechamentoFinalSalvar(FiltroNotaFechamentoDto filtroNotaFechamento)
        {
            return new FechamentoFinalSalvarDto()
            {
                DisciplinaId = filtroNotaFechamento.ComponenteCurricular,
                EhRegencia = filtroNotaFechamento.EhRegencia,
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_6
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_5
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_8
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_4,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_10
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_5,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        Nota = NOTA_2
                    }
                }
            };
        }

        private FiltroNotaFechamentoDto ObterFiltroNotasFechamento(string perfil, TipoNota tipoNota, string anoTurma,Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular , bool considerarAnoAnterior = false, bool ehRegencia = false)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = tipoNota,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                EhRegencia = ehRegencia
            };
        }
    }
}