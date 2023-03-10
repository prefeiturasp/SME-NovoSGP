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
        }
        
        [Fact(DisplayName = "Fechamento Bimestre Final - Deve lançar nota conceito pelo Professor Titular em ano atual para componentes diferentes de regência")]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_diferente_de_regencia()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_3, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));

            await ExecutarComandosFechamentoFinalComValidacaoNota(ObtenhaFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));
            
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
            await ExecuteTesteConceitoInsercao(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, true, TipoNota.Conceito);
            
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
            await ExecuteTesteConceitoInsercao(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114, Modalidade.EJA, ModalidadeTipoCalendario.EJA, true, TipoNota.Conceito);
            
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
            await ExecuteTesteConceitoInsercao(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
            
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
            await ExecuteTesteConceitoInsercao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
            
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
            await ExecuteTesteConceitoAlteracao(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
            
            //PAROU AQUI
            // var historicoNotas = ObterTodos<HistoricoNota>();
            // historicoNotas.Count.ShouldBe(6);
            //
            // var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            // historicoNotasNotaFechamentos.Count.ShouldBe(6);
            //
            // historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(6);
            // historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(6);
            //
            // historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            // historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            // historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            //
            // historicoNotas.Any(w=> w.Id == 4 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            // historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.S && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            // historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.P && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_cp()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
            
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

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_diretor()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
            
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

        [Fact]
        public async Task Deve_alterar_nota_conceito_em_turma_do_ano_anterior()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_3, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, true));

            await ExecutarComandosFechamentoFinal(ObtenhaFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                EhRegencia = false,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new FechamentoFinalSalvarItemDto()
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
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(0);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(0);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(0);
            historicoNotas.Count(w=> w.ConceitoNovoId.HasValue).ShouldBe(0);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }
    }
}
