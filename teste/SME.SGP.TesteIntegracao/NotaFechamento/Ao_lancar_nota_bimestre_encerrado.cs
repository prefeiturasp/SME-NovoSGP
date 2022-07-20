using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamento.Base;
using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamento
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
        public async Task Deve_permitir_lancamento_nota_para_professor_com_atribuicao()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await ExecutarTeste(filtroNotaFechamento);
        }

        [Fact]
        public async Task Nao_deve_permitir_lancamento_nota_para_professor_com_atribuicao_encerrada()
        {
        }

        [Fact]
        public async Task Nao_deve_permitir_lancamento_nota_com_periodo_escolar_no_4_bimestre_encerrada_sem_periodo_reabertura()
        {
        }
        
        [Fact]
        public async Task Deve_permitir_lancamento_nota_com_periodo_escolar_no_4_bimestre_encerrada_com_periodo_abertura()
        {
        }
        
        [Fact]
        public async Task Deve_permitir_lancamento_nota_com_periodo_escolar_no_4_bimestre_encerrada_com_periodo_reabertura()
        {
        }
        
        private async Task ExecutarTeste(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            await CriarDadosBase(filtroNotaFechamentoDto);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamentoDto);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
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