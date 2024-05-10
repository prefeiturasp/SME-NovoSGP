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
using SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoBimestre.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_excluir_nota_numerica : NotaFechamentoTesteBase
    {
        public Ao_excluir_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
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
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>),
                typeof(ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterConselhoClassePorFechamentoIdQuery, ConselhoClasse>),
                  typeof(ObterConselhoClassePorFechamentoIdQueryHandlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota numérica com professor titular no ensino fundamental")]
        public async Task Deve_permitir_excluir_nota_numerica_titular_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalNumericaParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalNumericaParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && !w.NotaNova.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota numérica com professor titular no ensino médio")]
        public async Task Deve_permitir_excluir_nota_numerica_titular_medio()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_1,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalNumericaParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalNumericaParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && !w.NotaNova.HasValue).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota numérica com professor titular no EJA")]
        public async Task Deve_permitir_excluir_nota_numerica_titular_eja()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_3,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                COMPONENTE_HISTORIA_ID_7);
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalNumericaParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalNumericaParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && !w.NotaNova.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota numérica com CP - Fundamental")]
        public async Task Deve_permitir_excluir_nota_numerica_cp()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilCP(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalNumericaParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalNumericaParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && !w.NotaNova.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota numérica com Diretor - Fundamental")]
        public async Task Deve_permitir_excluir_nota_numerica_diretor()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilDiretor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalNumericaParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalNumericaParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && !w.NotaNova.HasValue).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir excluir nota numérica com Professor Titular - Regência")]
        public async Task Deve_permitir_excluir_nota_numerica_titular_regencia_classe_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilDiretor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), false, true);
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarParaInserir = ObterFechamentoNotaFinalNumericaParaSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaInserir);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(5);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(5);
            
            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            var fechamentoFinalSalvarParaExcluir = ObterFechamentoNotaFinalNumericaParaExcluir(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarParaExcluir);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(5);
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && !w.NotaNova.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && !w.NotaNova.HasValue).ShouldBeTrue();
        }

        private FechamentoFinalSalvarDto ObterFechamentoNotaFinalNumericaParaExcluir(FiltroNotaFechamentoDto filtroNotaFechamento)
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
                    Nota = null
                });
            }

            return fechamentoNotaFinalNumericaParaExcluir;
        }
        
        private FechamentoFinalSalvarDto ObterFechamentoNotaFinalNumericaParaSalvar(FiltroNotaFechamentoDto filtroNotaFechamento)
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
    }
}