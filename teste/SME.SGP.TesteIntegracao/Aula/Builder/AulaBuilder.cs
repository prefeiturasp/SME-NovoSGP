using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

        private const string USUARIO_CHAVE = "NomeUsuario";
        private const string USUARIO_RF_CHAVE = "RF";
        private const string USUARIO_LOGIN_CHAVE = "login";

        private const string USUARIO_LOGADO_CHAVE = "UsuarioLogado";

        private const string USUARIO_CLAIMS_CHAVE = "Claims";

        private const string USUARIO_CLAIM_TIPO_RF = "rf";

        private const string USUARIO_CLAIM_TIPO_PERFIL = "perfil";

        private const string USUARIO_PROFESSOR_LOGIN_2222222 = "2222222";
        private const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        private const string USUARIO_PROFESSOR_NOME_2222222 = "Nome do usuario 2222222";

        private const string SISTEMA_NOME = "Sistema";
        private const string SISTEMA_CODIGO_RF = "1";

        private const string TURMA_CODIGO_1 = "1";
        private const string TURMA_NOME_1 = "Turma Nome 1";
        private const string TURMA_ANO_2 = "2";

        private const int ANO_LETIVO_2022_NUMERO = 2022;
        private const string ANO_LETIVO_2022_NOME = "Ano Letivo 2022";

        private const int SEMESTRE_2 = 2;

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
            var variaveis = new Dictionary<string, object>
            {
                { USUARIO_CHAVE, USUARIO_PROFESSOR_NOME_2222222 },
                { USUARIO_LOGADO_CHAVE, USUARIO_PROFESSOR_LOGIN_2222222 },
                { USUARIO_RF_CHAVE, USUARIO_PROFESSOR_LOGIN_2222222 },
                { USUARIO_LOGIN_CHAVE, USUARIO_PROFESSOR_LOGIN_2222222 },

                {
                   USUARIO_CLAIMS_CHAVE,
                    new List<InternalClaim> {
                        new InternalClaim { Value = USUARIO_PROFESSOR_LOGIN_2222222, Type = USUARIO_CLAIM_TIPO_RF },
                        new InternalClaim { Value = perfil, Type = USUARIO_CLAIM_TIPO_PERFIL }
                    }
                }
            };
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        protected string ObtenhaPerfilEspecialista()
        {
            return Guid.Parse(PerfilUsuario.PROFESSOR.Name()).ToString();
        }

        protected string ObtenhaPerfilCJ()
        {
            return Guid.Parse(PerfilUsuario.CJ.Name()).ToString();
        }

        public async Task CriaUsuario()
        {
            await _teste.InserirNaBase(new Usuario
            {
                Login = USUARIO_PROFESSOR_LOGIN_2222222,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Nome = USUARIO_PROFESSOR_NOME_2222222,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        public async Task CriaTurmaFundamental()
        {
            await _teste.InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = true,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = ANO_LETIVO_2022_NUMERO,
                Semestre = SEMESTRE_2,
                Nome = TURMA_NOME_1
            });
        }

        public async Task CriaTipoCalendario()
        {
            await _teste.InserirNaBase(new TipoCalendario
            {
                AnoLetivo = ANO_LETIVO_2022_NUMERO,
                Nome = ANO_LETIVO_2022_NOME,
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false
            });
        }
    }
}
