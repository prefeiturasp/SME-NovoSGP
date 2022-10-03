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
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_excluir_nota_numerica : NotaFechamentoBimestreTesteBase
    {
        public Ao_excluir_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
                typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_permitir_excluir_nota_titular_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_7);

            await ExecutarTesteInsercaoELimpeza(filtroNotaFechamento, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
        }

        [Fact]
        public async Task Deve_permitir_excluir_nota_cp_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilCP(), ANO_7);

            await ExecutarTesteInsercaoELimpeza(filtroNotaFechamento, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
        }

        [Fact]
        public async Task Deve_permitir_excluir_nota_diretor_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilDiretor(), ANO_7);

            await ExecutarTesteInsercaoELimpeza(filtroNotaFechamento, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
        }

        [Fact]
        public async Task Deve_permitir_excluir_nota_titular_regencia_classe_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_1);

            await ExecutarTesteInsercaoELimpeza(filtroNotaFechamento, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105);
        }

        private async Task ExecutarTesteInsercaoELimpeza(FiltroFechamentoNotaDto filtroNotaFechamentoDto, long disciplina)
        {
            await CriarDadosBase(filtroNotaFechamentoDto);

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(disciplina), disciplina);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            var fechamentoDto = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();

            fechamentoDto.Id = retorno.FirstOrDefault().Id;

            foreach (var fechamentoAluno in fechamentoDto.NotaConceitoAlunos)
            {
                fechamentoAluno.Nota = null;
            }

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
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