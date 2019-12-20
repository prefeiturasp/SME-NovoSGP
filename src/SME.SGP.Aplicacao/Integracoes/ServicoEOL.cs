using Newtonsoft.Json;
using Sentry;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task AtribuirCJSeNecessario(string codigoRf)
        {
            var resumo = await ObterResumoCore(codigoRf);

            await AtribuirCJSeNecessario(resumo.Id);
        }

        public async Task AtribuirCJSeNecessario(Guid usuarioId)
        {
            var parametros = JsonConvert.SerializeObject(usuarioId.ToString());

            var resposta = await httpClient.PostAsync("autenticacaoSgp/AtribuirPerfilCJ", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode)
                return;

            var mensagem = await resposta.Content.ReadAsStringAsync();

            throw new NegocioException(mensagem);
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

        public async Task<AbrangenciaRetornoEolDto> ObterAbrangencia(string login, Guid perfil)
        {
            var resposta = await httpClient.GetAsync($"funcionarios/{login}/perfis/{perfil.ToString()}/turmas");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AbrangenciaRetornoEolDto>(json);
            }
            return null;
        }

        public async Task<AbrangenciaCompactaVigenteRetornoEOLDTO> ObterAbrangenciaCompactaVigente(string login, Guid perfil)
        {
            var resposta = await httpClient.GetAsync($"abrangencia/compacta-vigente/{login}/perfil/{perfil.ToString()}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AbrangenciaCompactaVigenteRetornoEOLDTO>(json);
            }
            return null;
        }

        public async Task<AbrangenciaRetornoEolDto> ObterAbrangenciaParaSupervisor(string[] uesIds)
        {
            var json = new StringContent(JsonConvert.SerializeObject(uesIds), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(httpClient.BaseAddress.AbsoluteUri + "funcionarios/turmas"),
                Content = json
            };

            var resposta = await httpClient.SendAsync(request);

            if (resposta.IsSuccessStatusCode)
            {
                var jsonRetorno = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AbrangenciaRetornoEolDto>(jsonRetorno);
            }
            else throw new NegocioException("Houve erro ao tentar obter a abrangência do Eol");
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId)
        {
            var alunos = new List<AlunoPorTurmaResposta>();
            var resposta = await httpClient.GetAsync($"turmas/{turmaId}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
            }

            return alunos;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId, int anoLetivo)
        {
            var alunos = new List<AlunoPorTurmaResposta>();
            var resposta = await httpClient.GetAsync($"turmas/{turmaId}/alunos/anosLetivos/{anoLetivo}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
            }

            return alunos;
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasParaPlanejamento(long codigoTurma, string login, Guid perfil)
        {
            var url = $"funcionarios/{login}/perfis/{perfil}/turmas/{codigoTurma}/disciplinas/planejamento";
            return await ObterDisciplinas(url);
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil)
        {
            var url = $"funcionarios/{login}/perfis/{perfil}/turmas/{codigoTurma}/disciplinas";

            return await ObterDisciplinas(url);
        }

        public IEnumerable<DisciplinaDto> ObterDisciplinasPorIds(long[] ids)
        {
            httpClient.DefaultRequestHeaders.Clear();

            var resposta = httpClient.PostAsync("disciplinas", new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json-patch+json")).Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                var retorno = JsonConvert.DeserializeObject<IEnumerable<RetornoDisciplinaDto>>(json);
                return MapearParaDtoDisciplinas(retorno);
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

        public EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null)
        {
            var filtroTurmas = new StringContent(JsonConvert.SerializeObject(codigosTurma ?? new string[] { }), UnicodeEncoding.UTF8, "application/json");

            string url = $"abrangencia/estrutura-vigente";

            httpClient.DefaultRequestHeaders.Clear();

            var resposta = httpClient.PostAsync(url, filtroTurmas).Result;

            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);
            }
            else
            {
                SentrySdk.AddBreadcrumb($"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty}");
                return null;
            }
        }

        public EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorDre()
        {
            EstruturaInstitucionalRetornoEolDTO resultado = null;
            var codigosDres = ObterCodigosDres();
            string url = $"abrangencia/estrutura-vigente";

            if (codigosDres != null && codigosDres.Length > 0)
            {
                resultado = new EstruturaInstitucionalRetornoEolDTO();
                foreach (var item in codigosDres)
                {
                    httpClient.DefaultRequestHeaders.Clear();

                    var resposta = httpClient.GetAsync($"{url}/{item}").Result;

                    if (resposta.IsSuccessStatusCode)
                    {
                        var json = resposta.Content.ReadAsStringAsync().Result;
                        var parcial = JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);

                        if (parcial != null)
                            resultado.Dres.AddRange(parcial.Dres);
                    }
                    else
                        SentrySdk.AddBreadcrumb($"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente por Dre: {item} - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty}");
                }
            }

            return resultado;
        }

        private string[] ObterCodigosDres()
        {
            string url = $"abrangencia/codigos-dres";

            httpClient.DefaultRequestHeaders.Clear();

            var resposta = httpClient.GetAsync(url).Result;

            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            else
            {
                SentrySdk.AddBreadcrumb($"Ocorreu um erro na tentativa de buscar os codigos das Dres no EOL - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty}");
                return null;
            }
        }

        public IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string ueId, long cargoId)
        {
            var resposta = httpClient.GetAsync($"escolas/{ueId}/funcionarios/cargos/{cargoId}").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }
            return null;
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto)
        {
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(buscaFuncionariosFiltroDto), UnicodeEncoding.UTF8, "application/json");

            var resposta = await httpClient.PostAsync("funcionarios/", jsonParaPost);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }

            return null;
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterListaNomePorListaRF(IEnumerable<string> codigosRF)
        {
            var resposta = await httpClient.PostAsync($"funcionarios/BuscarPorListaRF",
                new StringContent(JsonConvert.SerializeObject(codigosRF),
                Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterListaResumosPorListaRF(IEnumerable<string> codigosRF, int anoLetivo)
        {
            var resposta = await httpClient.PostAsync($"professores/{anoLetivo}/BuscarPorListaRF",
                new StringContent(JsonConvert.SerializeObject(codigosRF),
                Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
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

        public async Task<MeusDadosDto> ObterMeusDados(string login)
        {
            var url = $"AutenticacaoSgp/{login}/dados";
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
            {
                throw new NegocioException("Não foi possível obter os dados do usuário");
            }
            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MeusDadosDto>(json);
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

        public async Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor)
        {
            var resposta = await httpClient.GetAsync($"professores/{anoLetivo}/AutoComplete/{dreId}?nome={nomeProfessor}");

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei)
        {
            var resposta = await httpClient.GetAsync($"professores/{anoLetivo}/AutoComplete/{dreId}/{incluirEmei}?nome={nomeProfessor}");

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesDisciplinas(string turmaCodigo, string professorRf = null)
        {
            StringBuilder url = new StringBuilder();

            url.Append($"professores/{turmaCodigo}/titulares");

            //Ao passar o RF do professor, o endpoint retorna todas as disciplinas que o professor não é titular para evitar
            //que o professor se atribua como CJ da própria da turma que ele é titular da disciplina
            if (!string.IsNullOrEmpty(professorRf))
                url.Append($"?codigoRf={professorRf}");

            var resposta = await httpClient.GetAsync(url.ToString());

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorTitularDisciplinaEol>>(json);
        }

        public async Task<UsuarioResumoCoreDto> ObterResumoCore(string login)
        {
            var resposta = await httpClient.GetAsync($"AutenticacaoSgp/{login}/obter/resumo");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possivel obter os dados do usuário");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException("Usuário não encontrado no EOL");

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UsuarioResumoCoreDto>(json);
        }

        public async Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo)
        {
            var resposta = await httpClient.GetAsync($"professores/{codigoRF}/BuscarPorRf/{anoLetivo}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Ocorreu uma falha ao consultar o professor");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException($"Não foi encontrado professor com RF {codigoRF}");

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ProfessorResumoDto>(json);
        }

        public async Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo, bool incluirEmei)
        {
            var resposta = await httpClient.GetAsync($"professores/{codigoRF}/BuscarPorRf/{anoLetivo}/{incluirEmei}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Ocorreu uma falha ao consultar o professor");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException($"Não foi encontrado professor com RF {codigoRF}");

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ProfessorResumoDto>(json);
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

        public async Task<IEnumerable<TurmaPorUEResposta>> ObterTurmasPorUE(string ueId, string anoLetivo)
        {
            httpClient.DefaultRequestHeaders.Clear();

            var resposta = await httpClient.GetAsync($"escolas/{ueId}/turmas/anos_letivos/{anoLetivo}");
            var turmas = new List<TurmaPorUEResposta>();
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                turmas = JsonConvert.DeserializeObject<List<TurmaPorUEResposta>>(json);
            }
            return turmas;
        }

        public async Task ReiniciarSenha(string codigoRf)
        {
            httpClient.DefaultRequestHeaders.Clear();

            IList<KeyValuePair<string, string>> valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("login", codigoRf) }};

            var resposta = await httpClient.PostAsync($"AutenticacaoSgp/ReiniciarSenha", new FormUrlEncodedContent(valoresParaEnvio));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível reiniciar a senha deste usuário");
        }

        public async Task RemoverCJSeNecessario(Guid usuarioId)
        {
            var parametros = JsonConvert.SerializeObject(usuarioId.ToString());

            var resposta = await httpClient.PostAsync("autenticacaoSgp/RemoverPerfilCJ", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode)
                return;

            var mensagem = await resposta.Content.ReadAsStringAsync();

            throw new NegocioException(mensagem);
        }

        public async Task<bool> ValidarProfessor(string professorRf)
        {
            var resposta = await httpClient.GetAsync($"professores/{professorRf}/validade");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }

        private IEnumerable<DisciplinaDto> MapearParaDtoDisciplinas(IEnumerable<RetornoDisciplinaDto> disciplinas)
        {
            return disciplinas.Select(x => new DisciplinaDto
            {
                CodigoComponenteCurricular = x.CdComponenteCurricular,
                Nome = x.Descricao,
                Regencia = x.EhRegencia
            });
        }

        private async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinas(string url)
        {
            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DisciplinaResposta>>(json);
            }
            return null;
        }

        public Task<IEnumerable<TurmaInativaRetornoEolDto>> ObterTurmasFinalizadas(DateTime? dataFim, string[] turmasFiltro)
        {
            return Task.Factory.StartNew(() =>
            {
                var filtroTurmas = new StringContent(JsonConvert.SerializeObject(turmasFiltro ?? new string[] { }), UnicodeEncoding.UTF8, "application/json");
                string filtroDataFim = string.Empty;

                if (dataFim.HasValue)
                    filtroDataFim = $"?dataFim = {dataFim.Value.ToString("yyyy-MM-dd")}";

                string url = $"abrangencia/turmas-finalizadas{filtroDataFim}";

                httpClient.DefaultRequestHeaders.Clear();

                var resposta = httpClient.PostAsync(url, filtroTurmas).Result;

                if (resposta.IsSuccessStatusCode)
                {
                    var json = resposta.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<IEnumerable<TurmaInativaRetornoEolDto>>(json);
                }
                return Enumerable.Empty<TurmaInativaRetornoEolDto>();
            });
        }

        public Task<IEnumerable<VinculoTurmaFinalizadoRetornoEolDto>> ObterVinculosFinalizados(string login)
        {
            return Task.Factory.StartNew(() =>
            {
                var resposta = httpClient.GetAsync($"abrangencia/vinculos-finalizados/{login}").Result;

                if (resposta.IsSuccessStatusCode)
                {
                    var json = resposta.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<IEnumerable<VinculoTurmaFinalizadoRetornoEolDto>>(json);
                }
                return Enumerable.Empty<VinculoTurmaFinalizadoRetornoEolDto>();
            });
        }
    }
}