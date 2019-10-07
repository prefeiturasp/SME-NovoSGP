using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoEOL : IServicoEOL
    {
        private readonly HttpClient httpClient;

        public ServicoEOL(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<AlterarSenhaRespostaDto> AlterarSenha(string login, string novaSenha)
        {
            httpClient.DefaultRequestHeaders.Clear();

            var valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("usuario", login) },
                { new KeyValuePair<string, string>("senha", novaSenha) }};

            var resposta = await httpClient.PostAsync($"AutenticacaoSgp/AlterarSenha", new FormUrlEncodedContent(valoresParaEnvio));
            return new AlterarSenhaRespostaDto
            {
                Mensagem = resposta.IsSuccessStatusCode ? "" : await resposta.Content.ReadAsStringAsync(),
                StatusRetorno = (int)resposta.StatusCode,
                SenhaAlterada = resposta.IsSuccessStatusCode
            };
        }

        public async Task<UsuarioEolAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            httpClient.DefaultRequestHeaders.Clear();

            IList<KeyValuePair<string, string>> valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("login", login) },
                { new KeyValuePair<string, string>("senha", senha) }};

            var resposta = await httpClient.PostAsync($"AutenticacaoSgp/Autenticar", new FormUrlEncodedContent(valoresParaEnvio));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioEolAutenticacaoRetornoDto>(json);
            }
            else return null;
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorProfessorETurma(long codigoTurma, string rfProfessor)
        {
            var resposta = await httpClient.GetAsync($"professores/{rfProfessor}/turmas/{codigoTurma}/disciplinas");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DisciplinaResposta>>(json);
            }
            return null;
        }

        public IEnumerable<DreRespostaEolDto> ObterDres()
        {
            var resposta = httpClient.GetAsync("dres").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<DreRespostaEolDto>>(json);
            }
            return null;
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes)
        {
            httpClient.DefaultRequestHeaders.Clear();

            var resposta = httpClient.PostAsync("escolas", new StringContent(JsonConvert.SerializeObject(codigoUes), Encoding.UTF8, "application/json-patch+json")).Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<EscolasRetornoDto>>(json);
            }
            return null;
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId)
        {
            httpClient.DefaultRequestHeaders.Clear();

            var resposta = httpClient.GetAsync($"DREs/{dreId}/escolas").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<EscolasRetornoDto>>(json);
            }
            return null;
        }

        public IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string UeId, long cargoId)
        {
            var resposta = httpClient.GetAsync($"escolas/{UeId}/funcionarios/cargos/{cargoId}").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }
            return null;
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(string UeId)
        {
            var resposta = await httpClient.GetAsync($"escolas/{UeId}/funcionarios/");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }
            return null;
        }

        public IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf)
        {
            var resposta = httpClient.GetAsync($"professores/{codigoRf}/turmas").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ProfessorTurmaReposta>>(json);
            }
            return null;
        }

        public async Task<UsuarioEolAutenticacaoRetornoDto> ObterPerfisPorLogin(string login)
        {
            var resposta = await httpClient.GetAsync($"autenticacaoSgp/CarregarPerfisPorLogin/{login}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioEolAutenticacaoRetornoDto>(json);
            }
            return null;
        }

        public async Task<int[]> ObterPermissoesPorPerfil(Guid perfilGuid)
        {
            var resposta = await httpClient.GetAsync($"autenticacaoSgp/CarregarPermissoesPorPerfil/{perfilGuid}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int[]>(json);
            }
            return null;
        }

        public IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoSupervisores)
        {
            var resposta = httpClient.PostAsync("funcionarios/supervisores", new StringContent(JsonConvert.SerializeObject(codigoSupervisores), Encoding.UTF8, "application/json-patch+json")).Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<SupervisoresRetornoDto>>(json);
            }
            return null;
        }

        public IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId)
        {
            var resposta = httpClient.GetAsync($"dres/{dreId}/supervisores").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<SupervisoresRetornoDto>>(json);
            }
            return null;
        }

        public async Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo)
        {
            var resposta = await httpClient.GetAsync($"professores/{rfProfessor}/escolas/{codigoEscola}/turmas/anos_letivos/{anoLetivo}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(json);
            }
            return null;
        }
    }
}