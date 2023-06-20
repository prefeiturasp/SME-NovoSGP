using System.Collections.Generic;
using System.Linq;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_excluir_nota_conceito : NotaFechamentoTesteBase
    {
        public Ao_excluir_nota_conceito(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>),
                typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>),
                    typeof(SME.SGP.TesteIntegracao.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota conceito com professor titular no ensino fundamental")]
        public async Task Deve_permitir_excluir_nota_conceito_titular_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Conceito, ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalConceitoParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalConceitoParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota conceito com professor titular EJA")]
        public async Task Deve_permitir_excluir_nota_conceito_titular_eja()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Conceito, ANO_1,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                COMPONENTE_HISTORIA_ID_7);
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalConceitoParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalConceitoParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota conceito com professor fundamental")]
        public async Task Deve_permitir_excluir_nota_conceito_titular_regencia_classe_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Conceito, ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), false, true);
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalConceitoParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalConceitoParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota conceito com CP fundamental")]
        public async Task Deve_permitir_excluir_nota_conceito_cp_fundamental()
        {
           var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilCP(),
                TipoNota.Conceito, ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalConceitoParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalConceitoParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota conceito com Diretor fundamental")]
        public async Task Deve_permitir_excluir_nota_conceito_diretor_fundamental()
        {
           var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilDiretor(),
                TipoNota.Conceito, ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalConceitoParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalConceitoParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }
        private FechamentoFinalSalvarDto ObterFechamentoNotaFinalConceitoParaExcluir(FiltroNotaFechamentoDto filtroNotaFechamento)
        {
            var fechamentoAlunoInserido = ObterTodos<FechamentoAluno>();

            var fechamentoNotaFinalNumericaParaExcluir = new FechamentoFinalSalvarDto
            {
                DisciplinaId = filtroNotaFechamento.ComponenteCurricular,
                EhRegencia = filtroNotaFechamento.EhRegencia,
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
            };

            foreach (var fechamentoAluno in fechamentoAlunoInserido)
            {
                fechamentoNotaFinalNumericaParaExcluir.Itens.Add(new FechamentoFinalSalvarItemDto
                {
                    AlunoRf = fechamentoAluno.AlunoCodigo,
                    ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                    ConceitoId = null
                });
            }

            return fechamentoNotaFinalNumericaParaExcluir;
        }
        
        private FechamentoFinalSalvarDto ObterFechamentoNotaFinalConceitoParaSalvar(FiltroNotaFechamentoDto filtroNotaFechamento)
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
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        ConceitoId = (int)ConceitoValores.P
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        ConceitoId = (int)ConceitoValores.S
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_4,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        ConceitoId = (int)ConceitoValores.P
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_5,
                        ComponenteCurricularCodigo = long.Parse(filtroNotaFechamento.ComponenteCurricular),
                        ConceitoId = (int)ConceitoValores.S
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