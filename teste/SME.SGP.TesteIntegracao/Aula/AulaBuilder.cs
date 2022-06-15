using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class AulaBuilder
    {
        private readonly TesteBase _teste;
        private ItensBasicosBuilder itensBasicos;

        public AulaBuilder(TesteBase teste)
        {
            _teste = teste;
            itensBasicos = new ItensBasicosBuilder(_teste);
        }

        public async Task CriaItensComuns()
        {
            await itensBasicos.CriaItensComuns();
            await itensBasicos.CriarPeriodoEscolar();
            await itensBasicos.CriaComponenteCurricularSemFrequencia();
        }

        public void CriarClaimUsuario(string perfil)
        {
            var contextoAplicacao = _teste.ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "João Usuário");
            variaveis.Add("UsuarioLogado", "2222222");
            variaveis.Add("RF", "2222222");
            variaveis.Add("login", "2222222");

            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "2222222", Type = "rf" },
                new InternalClaim { Value = perfil, Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        public string ObtenhaPerfilEspecialista()
        {
            return "40e1e074-37d6-e911-abd6-f81654fe895d";
        }

        public string ObtenhaPerfilCJ()
        {
            return "41e1e074 - 37d6 - e911 - abd6 - f81654fe895d";
        }

        public async Task CriaUsuario()
        {
            await _teste.InserirNaBase(new Usuario
            {
                Id = 29,
                Login = "2222222",
                CodigoRf = "2222222",
                Nome = "João Usuário",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        public async Task CriaTurmaFundamental()
        {
            await _teste.InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = 2022,
                Semestre = 2,
                Nome = "Turma Nome 1"
            });
        }

        public async Task CriaTipoCalendario()
        {
            await _teste.InserirNaBase(new TipoCalendario
            {
                AnoLetivo = 2022,
                Nome = "Ano Letivo 202",
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false
            });
        }
    }
}
