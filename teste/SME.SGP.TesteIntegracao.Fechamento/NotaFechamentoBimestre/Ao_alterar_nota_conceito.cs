using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_alterar_nota_conceito : NotaFechamentoBimestreTesteBase
    {
        public Ao_alterar_nota_conceito(CollectionFixture collectionFixture) : base(collectionFixture)
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
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota conceito lançada pelo Professor Titular em ano atual")]
        public async Task Deve_alterar_nota_conceito_lancada_professor_titular()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_1, false));

            var conceitosParaPersistir = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_ARTES_ID_139), COMPONENTE_CURRICULAR_ARTES_ID_139);
            
            var retorno = await ExecutarTesteComValidacaoNota(conceitosParaPersistir, TipoNota.Nota);
            retorno.ShouldNotBeNull();

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            var fechamento = conceitosParaPersistir.FirstOrDefault();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).ConceitoId = (long)ConceitoValores.NS;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).ConceitoId = (long)ConceitoValores.P;

            await ExecutarTesteComValidacaoNota(conceitosParaPersistir, TipoNota.Conceito);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);

            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.P && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota conceito lançada pelo CP em ano atual")]
        public async Task Deve_alterar_nota_conceito_lancada_cp()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilCP(), ANO_1, false));

            var conceitosParaPersistir = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_ARTES_ID_139), COMPONENTE_CURRICULAR_ARTES_ID_139);
            
            var retorno = await ExecutarTesteComValidacaoNota(conceitosParaPersistir, TipoNota.Nota);
            retorno.ShouldNotBeNull();

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            var fechamento = conceitosParaPersistir.FirstOrDefault();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).ConceitoId = (long)ConceitoValores.NS;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).ConceitoId = (long)ConceitoValores.P;

            await ExecutarTesteComValidacaoNota(conceitosParaPersistir, TipoNota.Conceito);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);

            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.P && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota conceito lançada pelo DIRETOR em ano atual")]
        public async Task Deve_alterar_nota_conceito_lancada_diretor_ano_atual()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilDiretor(), ANO_1, false));

            var conceitosParaPersistir = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_ARTES_ID_139), COMPONENTE_CURRICULAR_ARTES_ID_139);
            
            var retorno = await ExecutarTesteComValidacaoNota(conceitosParaPersistir, TipoNota.Nota);
            retorno.ShouldNotBeNull();

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            var fechamento = conceitosParaPersistir.FirstOrDefault();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).ConceitoId = (long)ConceitoValores.NS;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).ConceitoId = (long)ConceitoValores.P;

            await ExecutarTesteComValidacaoNota(conceitosParaPersistir, TipoNota.Conceito);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);

            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota conceito em turma de ano anterior com WorkFlow")]
        public async Task Deve_alterar_nota_conceito_em_turma_do_ano_anterior()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_1, true));

            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_ARTES_ID_139), COMPONENTE_CURRICULAR_ARTES_ID_139);
            var retorno = await comando.Salvar(dto);

            dto.FirstOrDefault().Id = retorno.FirstOrDefault().Id;
            dto.FirstOrDefault().NotaConceitoAlunos = new List<FechamentoNotaDto>()
                                                        {
                                                            new FechamentoNotaDto()
                                                            {
                                                                CodigoAluno = CODIGO_ALUNO_1,
                                                                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139,
                                                                ConceitoId = (int)ConceitoValores.NS
                                                            }
                                                        };
            var retornoAlteracao = await comando.Salvar(dto);
            retornoAlteracao.ShouldNotBeNull();
            var alunoFechamento = ObterTodos<FechamentoAluno>();
            
            alunoFechamento.ShouldNotBeNull();
            var aluno = alunoFechamento.LastOrDefault(aluno => aluno.AlunoCodigo == CODIGO_ALUNO_1);
            aluno.ShouldNotBeNull();
            
            var notas = ObterTodos<FechamentoNota>();
            notas.ShouldNotBeNull();
            var nota = notas.FirstOrDefault(nota => nota.FechamentoAlunoId == aluno.Id);
            nota.ShouldNotBeNull();
            
            var listaAprovacao = ObterTodos<WfAprovacaoNotaFechamento>();
            var aprovacao = listaAprovacao.FirstOrDefault(wf => wf.FechamentoNotaId == nota.Id);
            aprovacao.ShouldNotBeNull();
            aprovacao.ConceitoId.ShouldBe((int)ConceitoValores.NS);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(0);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(0);

            retornoAlteracao.FirstOrDefault().MensagemConsistencia.ShouldBe(MensagensNegocioLancamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO);
        }

        private List<FechamentoNotaDto> ObterListaDeFechamentoConceito(long disciplina)
        {
            return new List<FechamentoNotaDto>()
            {
                ObterNotaConceito(CODIGO_ALUNO_1, disciplina, (long)ConceitoValores.P),
                ObterNotaConceito(CODIGO_ALUNO_2, disciplina, (long)ConceitoValores.NS),
                ObterNotaConceito(CODIGO_ALUNO_3, disciplina, (long)ConceitoValores.NS),
                ObterNotaConceito(CODIGO_ALUNO_4, disciplina, (long)ConceitoValores.S),
            };
        }
    }
}
