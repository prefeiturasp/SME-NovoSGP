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
                ComponenteCurricular = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString()
            };

            await CriarDadosBase(filtroNota);

            await CriarAula(filtroNota.ComponenteCurricular, DATA_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);

            await CriarAtividadeAvaliativa(DATA_INICIO_BIMESTRE_2, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), USUARIO_PROFESSOR_LOGIN_2222222);
            await CriarAtividadeAvaliativa(DATA_INICIO_BIMESTRE_2, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), USUARIO_PROFESSOR_LOGIN_1111111);

            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = "1",
                        Nota = 7,
                        AtividadeAvaliativaId = 1
                    }
                }
            };

            var comando = ServiceProvider.GetService<IComandosNotasConceitos>();

            await comando.Salvar(dto);

            var notas = ObterTodos<NotaConceito>();

            notas.ShouldNotBeEmpty();
            notas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

    
    }
}
