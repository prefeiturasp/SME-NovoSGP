using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using Xunit;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_lancar_nota_numerica : NotaFechamentoTesteBase
    {
        public Ao_lancar_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_fundamental()
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
        public async Task Deve_permitir_lancamento_nota_numerica_titular_medio()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_1,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await ExecutarTeste(filtroNotaFechamento);
        }
        
        [Fact]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_eja()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_3,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                COMPONENTE_HISTORIA_ID_7);
            
            await ExecutarTeste(filtroNotaFechamento);
        }
        
        [Fact]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_regencia_classe_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(),false, true);
            
            await ExecutarTeste(filtroNotaFechamento);
        }

        [Fact]
        public async Task Deve_permitir_lancamento_nota_numerica_cp()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilCP(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await ExecutarTeste(filtroNotaFechamento);
        }

        [Fact]
        public async Task Deve_permitir_lancamento_nota_numerica_diretor()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilDiretor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await ExecutarTeste(filtroNotaFechamento);
        }

        [Fact]
        public async Task Deve_permitir_alterar_nota_numerica_lancada_cp()
        {
            await ExecuteTesteNumericaAlteracao(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, TipoNota.Nota);
        }

        [Fact]
        public async Task Deve_permitir_alterar_nota_numerica_lancada_diretor()
        {
            await ExecuteTesteNumericaAlteracao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, TipoNota.Nota);
        }

        private async Task ExecuteTesteNumericaAlteracao(string perfil, long componenteCurricular, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, TipoNota tipoNota)
        {
            var filtroDto = ObterFiltroNotas(perfil, ANO_3, componenteCurricular.ToString(), tipoNota, modalidade, modalidadeTipoCalendario, false);
            var dtoSalvar = ObterFechamentoFinalSalvar(filtroDto);

            await CriarDadosBase(filtroDto);
            await ExecutarComandosFechamentoFinal(dtoSalvar);

            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_1).Nota = NOTA_3;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_2).Nota = NOTA_4;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_3).Nota = NOTA_5;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_4).Nota = NOTA_6;

            await ExecutarComandosFechamentoFinalComValidacaoNota(dtoSalvar);
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