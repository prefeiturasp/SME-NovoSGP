using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public  class Ao_excluir_nota_conceito : NotaFechamentoBimestreTesteBase
    {
        public Ao_excluir_nota_conceito(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
                typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve excluir nota conceito lançada pelo Professor Titular em ano atual")]
        public async Task Deve_permitir_excluir_nota_titular_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(),ANO_1);

            await CriarDadosBase(filtroNotaFechamento);

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            var fechamentoDto = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();

            fechamentoDto.Id = retorno.FirstOrDefault().Id;

            foreach (var fechamentoAluno in fechamentoDto.NotaConceitoAlunos)
            {
                fechamentoAluno.ConceitoId = null;
            }

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve excluir nota conceito lançada pelo CP em ano atual")]
        public async Task Deve_permitir_excluir_nota_cp_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilCP(),ANO_1);

            await CriarDadosBase(filtroNotaFechamento);

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            var fechamentoDto = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();

            fechamentoDto.Id = retorno.FirstOrDefault().Id;

            foreach (var fechamentoAluno in fechamentoDto.NotaConceitoAlunos)
            {
                fechamentoAluno.ConceitoId = null;
            }

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve excluir nota conceito lançada pelo DIRETOR em ano atual")]
        public async Task Deve_permitir_excluir_nota_diretor_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilDiretor(),ANO_1);

            await CriarDadosBase(filtroNotaFechamento);

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            var fechamentoDto = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();

            fechamentoDto.Id = retorno.FirstOrDefault().Id;

            foreach (var fechamentoAluno in fechamentoDto.NotaConceitoAlunos)
            {
                fechamentoAluno.ConceitoId = null;
            }

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve excluir nota conceito de regência de classe fundamental em ano atual")]
        public async Task Deve_permitir_excluir_nota_titular_regencia_classe_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_1);

            await CriarDadosBase(filtroNotaFechamento);

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            var fechamentoDto = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();

            fechamentoDto.Id = retorno.FirstOrDefault().Id;

            foreach (var fechamentoAluno in fechamentoDto.NotaConceitoAlunos)
            {
                fechamentoAluno.ConceitoId = null;
            }

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(8);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(8);
            
            historicoNotas.Count(w=> !w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            historicoNotas.Count(w=> w.ConceitoAnteriorId.HasValue).ShouldBe(4);
            
            historicoNotas.Any(w=> w.Id == 1 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.P).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 2 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 3 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.NS).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 4 && !w.ConceitoAnteriorId.HasValue && w.ConceitoNovoId == (long)ConceitoValores.S).ShouldBeTrue();
            
            historicoNotas.Any(w=> w.Id == 5 && w.ConceitoAnteriorId == (long)ConceitoValores.P && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 6 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 7 && w.ConceitoAnteriorId == (long)ConceitoValores.NS && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
            historicoNotas.Any(w=> w.Id == 8 && w.ConceitoAnteriorId == (long)ConceitoValores.S && !w.ConceitoNovoId.HasValue).ShouldBeTrue();
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
