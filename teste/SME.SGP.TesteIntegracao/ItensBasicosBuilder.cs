using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class ItensBasicosBuilder
    {
        private readonly TesteBase _teste;
        public ItensBasicosBuilder(TesteBase teste)
        {
            _teste = teste;
        }

        public async Task CriaItensComuns()
        {
            await _teste.InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1",
                Abreviacao = "DRE AB",
                Nome = "DRE AB"
            });
            await _teste.InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                Nome = "Nome da UE",
            });

            await _teste.InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = "5",
                CodigoTurma = "1",
                Historica = false,
                ModalidadeCodigo = Modalidade.Fundamental,
                TipoTurma = TipoTurma.Regular,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Turma Teste 1",
                TipoTurno = 2
            });

            await _teste.InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = "9",
                CodigoTurma = "9",
                Historica = false,
                ModalidadeCodigo = Modalidade.Fundamental,
                TipoTurma = TipoTurma.Programa,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Turma Programa",
                TipoTurno = 2
            });

            await _teste.InserirNaBase(new PrioridadePerfil
            {
                Id = 1,
                CodigoPerfil = Perfis.PERFIL_PROFESSOR,
                NomePerfil = "Professor",
                Ordem = 290,
                Tipo = TipoPerfil.UE,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await _teste.InserirNaBase(new PrioridadePerfil
            {
                Id = 2,
                CodigoPerfil = Perfis.PERFIL_CJ,
                NomePerfil = "Professor CJ",
                Ordem = 320,
                Tipo = TipoPerfil.UE,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        public async Task CriaItensComunsEja()
        {
            await CriaItensComunsEja(false);
        }

        public async Task CriaItensComunsEja(bool incluirAdm, int semestre = 2)
        {
            CriarClaimRegenciaEja(incluirAdm);

            await CriaItensComuns();
            await CriarPeriodoEscolar();
            await CriaAtribuicaoCJ();

            await _teste.InserirNaBase(new Usuario
            {
                Id = 29,
                Login = TesteBaseComuns.USUARIO_LOGADO_RF,
                CodigoRf = TesteBaseComuns.USUARIO_LOGADO_RF,
                Nome = TesteBaseComuns.USUARIO_LOGADO_NOME,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await _teste.InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                ModalidadeCodigo = Modalidade.EJA,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Semestre = semestre,
                Nome = "Turma Nome 1",
                TipoTurno = 2
            });

            await _teste.InserirNaBase(new TipoCalendario
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Ano Letivo Ano Atual",
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.EJA,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                Semestre = semestre
            });

            await _teste.InserirNaBase(new Dominio.Aula
            {
                UeId = "1",
                DisciplinaId = "1114",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = TesteBaseComuns.USUARIO_LOGADO_RF,
                Quantidade = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });

            await _teste.InserirNaBase(new Dominio.Abrangencia
            {
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = true,
                Perfil = Perfis.PERFIL_CJ
            });
        }

        public async Task CriaAulaSemFrequencia()
        {
            await _teste.InserirNaBase(new Dominio.Aula
            {
                UeId = "1",
                DisciplinaId = "1106",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = TesteBaseComuns.USUARIO_LOGADO_RF,
                Quantidade = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });
        }

        public async Task CriaComponenteCurricularSemFrequencia()
        {
            await _teste.InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await _teste.InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await _teste.InserirNaBase("componente_curricular", "1106", "1106", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");
        }

        public async Task CriarPeriodoEscolar()
        {
            await _teste.InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 10, 1),
                PeriodoFim = DateTime.Now.AddYears(1),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        private void CriarClaimRegenciaEja(bool incluirAdm)
        {
            var contextoAplicacao = _teste.ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", TesteBaseComuns.USUARIO_LOGADO_NOME);
            variaveis.Add("UsuarioLogado", TesteBaseComuns.USUARIO_LOGADO_RF);
            variaveis.Add("RF", TesteBaseComuns.USUARIO_LOGADO_RF);
            variaveis.Add("login", TesteBaseComuns.USUARIO_LOGADO_RF);

            if (incluirAdm)
            {
                variaveis.Add("Administrador", TesteBaseComuns.USUARIO_ADMIN_RF);
            }

            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = TesteBaseComuns.USUARIO_LOGADO_RF, Type = "rf" },
                new InternalClaim { Value = "41e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        private async Task CriaAtribuicaoCJ()
        {
            await _teste.InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = "1",
                DreId = "1",
                UeId = "1",
                ProfessorRf = TesteBaseComuns.USUARIO_LOGADO_RF,
                DisciplinaId = 1106,
                Modalidade = Modalidade.CIEJA,
                Substituir = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }
    }
}
