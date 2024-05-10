using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_alterar_nota_numerica : NotaFechamentoBimestreTesteBase
    {
        public Ao_alterar_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
                typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>), typeof(ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota numérica lançada pelo Professor Titular em ano atual")]
        public async Task Deve_alterar_nota_numerico_lancada_professor_titular()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_7));

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            var fechamento = dto.FirstOrDefault();
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            retorno.ShouldNotBeNull();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).Nota = NOTA_9;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).Nota = NOTA_8;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).Nota = NOTA_7;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).Nota = NOTA_6;

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_7).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_9).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.NotaAnterior == NOTA_6 && w.NotaNova == NOTA_9).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_7 && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_8 && w.NotaNova == NOTA_7).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_9 && w.NotaNova == NOTA_6).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota numérica lançada pelo CP em ano atual")]
        public async Task Deve_alterar_nota_numerico_lancada_cp()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilCP(), ANO_7));

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            var fechamento = dto.FirstOrDefault();
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            retorno.ShouldNotBeNull();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).Nota = NOTA_9;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).Nota = NOTA_8;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).Nota = NOTA_7;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).Nota = NOTA_6;

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_7).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_9).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.NotaAnterior == NOTA_6 && w.NotaNova == NOTA_9).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_7 && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_8 && w.NotaNova == NOTA_7).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_9 && w.NotaNova == NOTA_6).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota numérica lançada pelo DIRETOR em ano atual")]
        public async Task Deve_alterar_nota_numerico_lancada_diretor()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilDiretor(), ANO_7));

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            var fechamento = dto.FirstOrDefault();
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            retorno.ShouldNotBeNull();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).Nota = NOTA_9;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).Nota = NOTA_8;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).Nota = NOTA_7;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).Nota = NOTA_6;

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            
            historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.NotaAnterior.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_6).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_7).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.NotaAnterior.HasValue && w.NotaNova == NOTA_9).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.NotaAnterior == NOTA_6 && w.NotaNova == NOTA_9).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.NotaAnterior == NOTA_7 && w.NotaNova == NOTA_8).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.NotaAnterior == NOTA_8 && w.NotaNova == NOTA_7).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.NotaAnterior == NOTA_9 && w.NotaNova == NOTA_6).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve alterar nota numérica em turma de ano anterior com WorkFlow")]
        public async Task Deve_alterar_nota_numerico_em_turma_do_ano_anterior()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_7, true));
            
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await comando.Salvar(dto);

            dto.FirstOrDefault().Id = retorno.FirstOrDefault().Id;
            dto.FirstOrDefault().NotaConceitoAlunos = new List<FechamentoNotaDto>()
                                                        {
                                                            new FechamentoNotaDto()
                                                            {
                                                                CodigoAluno = CODIGO_ALUNO_1,
                                                                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                Nota = (long)NOTA_8
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
            aprovacao.Nota.ShouldBe((long)NOTA_8);

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(0);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(0);

            retornoAlteracao.FirstOrDefault().MensagemConsistencia.ShouldBe(MensagensNegocioLancamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO);
        }

        private List<FechamentoNotaDto> ObterListaDeFechamentoNumerica(long disciplina)
        {
            return new List<FechamentoNotaDto>()
            {
                ObterNotaNumerica(CODIGO_ALUNO_1, disciplina, (long)NOTA_6),
                ObterNotaNumerica(CODIGO_ALUNO_2, disciplina, (long)NOTA_7),
                ObterNotaNumerica(CODIGO_ALUNO_3, disciplina, (long)NOTA_8),
                ObterNotaNumerica(CODIGO_ALUNO_4, disciplina, (long)NOTA_9),
            };
        }
    }
}
