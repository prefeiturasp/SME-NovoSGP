using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_registrar_nota_para_professor_cj : NotaBase
    {
        public Ao_registrar_nota_para_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_lancar_nota_numerica_pelo_professor_cj_com_avaliacoes_do_professor_titular_do_cj()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilCJ(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                TipoNota = TipoNota.Nota
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222);
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111);

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

            var comando = ServiceProvider.GetService<IComandosNotasConceitos>();

            await comando.Salvar(dto);

            var notas = ObterTodos<NotaConceito>();

            notas.ShouldNotBeEmpty();
            notas.Count().ShouldBeGreaterThanOrEqualTo(1);
            notas.Exists(nota => nota.TipoNota == TipoNota.Nota).ShouldBe(true);
        }

        [Fact]
        public async Task Ao_lancar_nota_conceito_pelo_professor_cj_com_avaliacoes_do_professor_titular_do_cj()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilCJ(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                TipoNota = TipoNota.Conceito
            };
   
            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222);
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111);
            await CriaConceito();

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

            var comando = ServiceProvider.GetService<IComandosNotasConceitos>();

            await comando.Salvar(dto);

            var notas = ObterTodos<NotaConceito>();

            notas.ShouldNotBeEmpty();
            notas.Count().ShouldBeGreaterThanOrEqualTo(1);
            notas.Exists(nota => nota.TipoNota == TipoNota.Conceito).ShouldBe(true);
        }
    }
}
