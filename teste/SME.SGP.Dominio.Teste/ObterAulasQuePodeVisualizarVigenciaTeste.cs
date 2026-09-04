using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class ObterAulasQuePodeVisualizarVigenciaTeste
    {
        private const string RF_PROFESSOR = "7912366";
        private const string RF_COLEGA = "8088772";
        private const string INGLES = "9";
        private const string LEITURA = "1061";

        private static readonly DateTime FIM_ATRIBUICAO_LEITURA = new DateTime(2026, 2, 23);

        [Fact]
        public void Sem_considerar_vigencia_o_componente_encerrado_revela_o_ano_inteiro()
        {
            var aulas = new[]
            {
                Aula(LEITURA, new DateTime(2026, 2, 12)),
                Aula(LEITURA, new DateTime(2026, 3, 10))
            };

            var visiveis = Professor().ObterAulasQuePodeVisualizar(aulas, new List<ComponenteCurricularEol> { LeituraEncerrada() });

            Assert.Equal(2, visiveis.Count());
        }

        [Fact]
        public void Considerando_vigencia_o_componente_encerrado_revela_apenas_o_proprio_periodo()
        {
            var dentro = Aula(LEITURA, new DateTime(2026, 2, 12));
            var fora = Aula(LEITURA, new DateTime(2026, 3, 10));

            var visiveis = Professor().ObterAulasQuePodeVisualizar(new[] { dentro, fora },
                                                                   new List<ComponenteCurricularEol> { LeituraEncerrada() },
                                                                   considerarVigenciaAtribuicao: true);

            Assert.Single(visiveis);
            Assert.Equal(dentro.Id, visiveis.First().Id);
        }

        [Fact]
        public void Componente_com_atribuicao_ativa_revela_o_ano_inteiro()
        {
            var aulas = new[]
            {
                Aula(INGLES, new DateTime(2026, 2, 6)),
                Aula(INGLES, new DateTime(2026, 11, 30))
            };

            var visiveis = Professor().ObterAulasQuePodeVisualizar(aulas,
                                                                   new List<ComponenteCurricularEol> { InglesAtivo() },
                                                                   considerarVigenciaAtribuicao: true);

            Assert.Equal(2, visiveis.Count());
        }

        [Fact]
        public void Aula_do_proprio_professor_permanece_visivel_fora_da_vigencia()
        {
            var aulaDoProfessor = Aula(LEITURA, new DateTime(2026, 3, 10), RF_PROFESSOR);

            var visiveis = Professor().ObterAulasQuePodeVisualizar(new[] { aulaDoProfessor },
                                                                   new List<ComponenteCurricularEol> { LeituraEncerrada() },
                                                                   considerarVigenciaAtribuicao: true);

            Assert.Single(visiveis);
        }

        [Fact]
        public void Componente_sem_vigencia_conhecida_nao_e_recortado()
        {
            var aulas = new[] { Aula(LEITURA, new DateTime(2026, 3, 10)) };

            var componenteSemVigencia = new ComponenteCurricularEol { Codigo = long.Parse(LEITURA) };

            var visiveis = Professor().ObterAulasQuePodeVisualizar(aulas,
                                                                   new List<ComponenteCurricularEol> { componenteSemVigencia },
                                                                   considerarVigenciaAtribuicao: true);

            Assert.Single(visiveis);
        }

        private static Usuario Professor()
        {
            var usuario = new Usuario { CodigoRf = RF_PROFESSOR, PerfilAtual = Perfis.PERFIL_PROFESSOR };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { Tipo = TipoPerfil.UE, CodigoPerfil = Perfis.PERFIL_PROFESSOR }
            });
            return usuario;
        }

        private static ComponenteCurricularEol LeituraEncerrada()
            => new ComponenteCurricularEol
            {
                Codigo = long.Parse(LEITURA),
                AtribuicaoAtiva = false,
                InicioAtribuicao = new DateTime(2025, 12, 23),
                FimAtribuicao = FIM_ATRIBUICAO_LEITURA
            };

        private static ComponenteCurricularEol InglesAtivo()
            => new ComponenteCurricularEol
            {
                Codigo = long.Parse(INGLES),
                AtribuicaoAtiva = true,
                InicioAtribuicao = new DateTime(2026, 2, 19),
                FimAtribuicao = null
            };

        private static int proximoId = 1;

        private static Aula Aula(string disciplinaId, DateTime dataAula, string professorRf = RF_COLEGA)
            => new Aula
            {
                Id = proximoId++,
                DisciplinaId = disciplinaId,
                DataAula = dataAula,
                ProfessorRf = professorRf,
                TurmaId = "3019147"
            };
    }
}
