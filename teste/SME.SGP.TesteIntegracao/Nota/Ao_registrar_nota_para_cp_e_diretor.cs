using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_registrar_nota_para_cp_e_diretor : NotaBase
    {
        public Ao_registrar_nota_para_cp_e_diretor(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_lancar_nota_numerica_pelo_cp_com_avaliacoes_do_professor_titular()
        {
            await CrieDados(ObterPerfilCP(), TipoNota.Nota, ANO_5);
            await ExecuteTesteNota();
        }

        [Fact]
        public async Task Ao_lancar_nota_numerica_pelo_diretor_com_avaliacoes_do_professor_titular()
        {
            await CrieDados(ObterPerfilDiretor(), TipoNota.Nota, ANO_5);
            await ExecuteTesteNota();
        }

        [Fact]
        public async Task Ao_lancar_nota_conceito_pelo_cp_com_avaliacoes_do_professor_titular()
        {
            await CrieDados(ObterPerfilCP(), TipoNota.Conceito, ANO_2);
            await CriaConceito();
            await ExecuteTesteConceito();
        }

        [Fact]
        public async Task Ao_lancar_nota_conceito_pelo_diretor_com_avaliacoes_do_professor_titular()
        {
            await CrieDados(ObterPerfilDiretor(), TipoNota.Conceito, ANO_2);
            await CriaConceito();
            await ExecuteTesteConceito();
        }

        private async Task ExecuteTesteNota()
        {
            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_1,
                        Nota = 7,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    }
                }
            };

            await ExecuteTeste(TipoNota.Nota, dto);
        }

        private async Task ExecuteTesteConceito()
        {
            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_1,
                        Conceito = 1,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_2,
                        Conceito = 2,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                     new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_3,
                        Conceito = 3,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                }
            };

            await ExecuteTeste(TipoNota.Conceito, dto);
        }

        private async Task ExecuteTeste(TipoNota tipoNota, NotaConceitoListaDto dto)
        {
            var comando = ServiceProvider.GetService<IComandosNotasConceitos>();

            await comando.Salvar(dto);

            var notas = ObterTodos<NotaConceito>();

            notas.ShouldNotBeEmpty();
            notas.Count().ShouldBeGreaterThanOrEqualTo(1);
            notas.Exists(nota => nota.TipoNota == tipoNota).ShouldBe(true);
        }

        private async Task CrieDados(string perfil, TipoNota tipo, string anoTurma)
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }
    }
}
