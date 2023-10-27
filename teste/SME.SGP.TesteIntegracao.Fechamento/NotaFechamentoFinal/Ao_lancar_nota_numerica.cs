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
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoBimestre.ServicosFakes;

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

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir lançamento de nota numérica pelo Professor Titular - Fundamental - Ano atual")]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
            
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
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir lançamento de nota numérica pelo Professor Titular - Ensino Médio - Ano atual")]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_medio()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_3,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
            
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
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir lançamento de nota numérica pelo Professor Titular - EJA - Ano atual")]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_eja()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_3,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                COMPONENTE_HISTORIA_ID_7);
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
            
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
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir lançamento de nota numérica pelo Professor Titular - Regência Fundamental - Ano atual")]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_regencia_classe_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(),false, true);
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
            
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
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir lançamento de nota numérica pelo CP - Fundamental - Ano atual")]
        public async Task Deve_permitir_lancamento_nota_numerica_cp()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilCP(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
            
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
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir lançamento de nota numérica pelo DIRETOR - Fundamental - Ano atual")]
        public async Task Deve_permitir_lancamento_nota_numerica_diretor()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilDiretor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            
            await CriarDadosBase(filtroNotaFechamento);

            var fechamentoFinalSalvarDto = ObterFechamentoFinalSalvar(filtroNotaFechamento);
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(fechamentoFinalSalvarDto);
            
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
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir alterar de nota numérica pelo CP - Fundamental - Ano atual")]
        public async Task Deve_permitir_alterar_nota_numerica_lancada_cp()
        {
            var filtroDto = ObterFiltroNotas(ObterPerfilCP(), ANO_7, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Nota, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
            var dtoSalvar = ObterFechamentoFinalSalvar(filtroDto);

            await CriarDadosBase(filtroDto);
            await ExecutarComandosFechamentoFinal(dtoSalvar);
            
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
            
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_1).Nota = NOTA_3;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_2).Nota = NOTA_4;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_3).Nota = NOTA_5;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_4).Nota = NOTA_6;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_5).Nota = NOTA_7;

            await ExecutarComandosFechamentoFinalComValidacaoNota(dtoSalvar);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(10);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && w.NotaNova == NOTA_3).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && w.NotaNova == NOTA_4).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && w.NotaNova == NOTA_7).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve permitir alterar de nota numérica pelo DIRETOR - Fundamental - Ano atual")]
        public async Task Deve_permitir_alterar_nota_numerica_lancada_diretor()
        {
            var filtroDto = ObterFiltroNotas(ObterPerfilDiretor(), ANO_7, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Nota, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
            var dtoSalvar = ObterFechamentoFinalSalvar(filtroDto);

            await CriarDadosBase(filtroDto);
            await ExecutarComandosFechamentoFinal(dtoSalvar);
            
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
            
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_1).Nota = NOTA_3;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_2).Nota = NOTA_4;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_3).Nota = NOTA_5;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_4).Nota = NOTA_6;
            dtoSalvar.Itens.FirstOrDefault(item => item.AlunoRf == ALUNO_CODIGO_5).Nota = NOTA_7;

            await ExecutarComandosFechamentoFinalComValidacaoNota(dtoSalvar);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(10);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(10);
            
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(5);
            historicoNotas.Count(w=> w.NotaNova.HasValue).ShouldBe(10);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_10).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_2).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_6 && w.NotaNova == NOTA_3).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_5 && w.NotaNova == NOTA_4).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_8 && w.NotaNova == NOTA_5).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 9 && w.NotaAnterior == NOTA_10 && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 10 && w.NotaAnterior == NOTA_2 && w.NotaNova == NOTA_7).ShouldBeTrue();
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