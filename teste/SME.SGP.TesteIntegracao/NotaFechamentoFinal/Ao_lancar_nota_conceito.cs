using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_lancar_nota_conceito : NotaFechamentoTesteBase
    {
        public Ao_lancar_nota_conceito(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

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
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo Professor Titular em ano atual para componentes diferentes de regência")]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_diferente_de_regencia()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_1, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));

            await ExecutarComandosFechamentoFinalComValidacaoNota(ObterFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, true));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo Professor Titular em ano atual para componentes regência")]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_Fundamental() 
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));

            await ExecutarComandosFechamentoFinalComValidacaoNota(ObterFechamentoFinalConceitoDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, true));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo Professor Titular em ano atual para componentes regência EJA")]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_EJA()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TipoNota.Conceito, Modalidade.EJA, ModalidadeTipoCalendario.EJA, false));
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(ObterFechamentoFinalConceitoDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114, true));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo CP em ano atual")]
        public async Task Deve_Lancar_nota_conceito_cp() 
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilCP(), ANO_1, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(ObterFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, true));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo DIRETOR em ano atual")]
        public async Task Deve_Lancar_nota_conceito_diretor()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilDiretor(), ANO_1, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));
            
            await ExecutarComandosFechamentoFinalComValidacaoNota(ObterFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve alterar nota conceito pelo Professor Titular em ano atual")]
        public async Task Deve_alterar_nota_conceito_lancada_professor_titular()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_1, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));

            await ExecutarComandosFechamentoFinal(ObterFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.P
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.S
                    },
                }
            };

            await ExecutarComandosFechamentoFinalComValidacaoNota(dto);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(6);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(6);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(6);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 4 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.S && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.P && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve alterar nota conceito pelo CP em ano atual")]
        public async Task Deve_alterar_nota_conceito_lancada_cp()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilCP(), ANO_1, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));

            await ExecutarComandosFechamentoFinal(ObterFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.P
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.S
                    },
                }
            };

            await ExecutarComandosFechamentoFinalComValidacaoNota(dto);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(6);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(6);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(6);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 4 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.S && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.P && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve alterar nota conceito pelo DIRETOR em ano atual")]
        public async Task Deve_alterar_nota_conceito_lancada_diretor()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilDiretor(), ANO_1, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));

            await ExecutarComandosFechamentoFinal(ObterFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(3);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(3);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(3);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.P
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.S
                    },
                }
            };

            await ExecutarComandosFechamentoFinalComValidacaoNota(dto);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(6);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(6);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(3);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(6);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 4 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.S && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.P && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre Final - Deve alterar nota conceito pelo Professor Titular em ano anterior")]
        public async Task Deve_alterar_nota_conceito_em_turma_do_ano_anterior()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_1, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, true));

            await ExecutarComandosFechamentoFinal(ObterFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(0);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(0);
            
            var wfAprovacaoNota = ObterTodos<WfAprovacaoNotaFechamento>();
            wfAprovacaoNota.Count.ShouldBe(3);
            
            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                EhRegencia = false,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.P
                    }
                }
            };

            var retorno = await ExecutarComandosFechamentoFinal(dto);

            var alunoFechamento = ObterTodos<FechamentoAluno>();
            alunoFechamento.ShouldNotBeNull();
            var aluno = alunoFechamento.FirstOrDefault(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_1);
            aluno.ShouldNotBeNull();
            
            var notas = ObterTodos<FechamentoNota>();
            notas.ShouldNotBeNull();
            var nota = notas.FirstOrDefault(nota => nota.FechamentoAlunoId == aluno.Id);
            nota.ShouldNotBeNull();
            
            var listaAprovacao = ObterTodos<WfAprovacaoNotaFechamento>();
            var aprovacao = listaAprovacao.FirstOrDefault(wf => wf.FechamentoNotaId == nota.Id);
            aprovacao.ShouldNotBeNull();
            aprovacao.ConceitoId.ShouldBe((int)ConceitoValores.P);

            retorno.MensagemConsistencia.ShouldBe(string.Format(MensagemNegocioFechamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO, TipoNota.Conceito.Name()));
            
            wfAprovacaoNota = ObterTodos<WfAprovacaoNotaFechamento>();
            wfAprovacaoNota.Count.ShouldBe(3);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(0);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(0);
        }
    }
}
