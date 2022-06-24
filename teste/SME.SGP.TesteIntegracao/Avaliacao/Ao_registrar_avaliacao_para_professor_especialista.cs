using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_registrar_avaliacao_para_professor_especialista : TesteAvaliacao
    {
        public Ao_registrar_avaliacao_para_professor_especialista(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Registrar_avaliacao_para_professor_especialista()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await ExecuteTesteResgistrarAvaliacaoPorPerfil(dto);
        }

        [Fact]
        public async Task Componente_curricular_nao_encontrado()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_INVALIDO, CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Inserir(dto));

            excecao.Message.ShouldBe("Componente curricular não encontrado no EOL.");
        }

        //[Fact]
        //public async Task Nao_pode_registrar_avaliacao_professor_com_permissao_encerrada()
        //{
        //    await CriarDadosBasicos(ObterCriacaoDeDadosDto());

        //    var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
        //    var dto = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);
        //    var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Inserir(dto));

        //    excecao.Message.ShouldBe("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        //}

        [Fact]
        public async Task Registrar_avaliacao_para_professor_especialista_multidiciplinar()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);
            dto.DisciplinasId = new string[] { COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() };

            await ExecuteTesteResgistrarAvaliacaoPorPerfil(dto);

            var atividadeAvaliativasDiciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeAvaliativasDiciplina.ShouldNotBeEmpty();
            atividadeAvaliativasDiciplina.FindAll(disciplina => disciplina.DisciplinaId == COMPONENTE_GEOGRAFIA_ID_8 || 
                                                                disciplina.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBe(2);
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto()
        {
            return new CriacaoDeDadosDto()
            {
                Perfil = ObterPerfilProfessor(),
                ModalidadeTurma = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoCalendarioId = TIPO_CALENDARIO_ID,
                DataInicio = DATA_02_05,
                DataFim = DATA_08_07,
                TipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Bimestre = BIMESTRE_2
            };
        }
    }
}
