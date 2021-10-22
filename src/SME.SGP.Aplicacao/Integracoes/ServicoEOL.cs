using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoEOL : IServicoEol
    {
        private readonly IRepositorioCache cache;
        private readonly IMediator mediator;
        private readonly HttpClient httpClient;


        public ServicoEOL(HttpClient httpClient, IRepositorioCache cache, IMediator mediator)
        {
            this.httpClient = httpClient;
            this.cache = cache;
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task AlterarEmail(string login, string email)
        {


            var valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("usuario", login) },
                { new KeyValuePair<string, string>("email", email) }};

            var resposta = await httpClient.PostAsync($"AutenticacaoSgp/AlterarEmail", new FormUrlEncodedContent(valoresParaEnvio));

            if (resposta.IsSuccessStatusCode)
                return;

            var mensagem = await resposta.Content.ReadAsStringAsync();

            throw new NegocioException(mensagem);
        }

        public async Task<AlterarSenhaRespostaDto> AlterarSenha(string login, string novaSenha)
        {


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

        public async Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesCurriculares()
        {
            var url = $"v1/componentes-curriculares";

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                return null;

            var retorno = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularDto>>(retorno);
        }

        public async Task<bool> TurmaPossuiComponenteCurricularPAP(string codigoTurma, string login, Guid idPerfil)
        {


            var url = $"v1/componentes-curriculares/turmas/{codigoTurma}/funcionarios/{login}/perfis/{idPerfil}/validar/pap";

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                return false;

            var retorno = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<bool>(retorno);
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


            var parametros = JsonConvert.SerializeObject(new { login, senha });
            var resposta = await httpClient.PostAsync($"v1/autenticacao", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioEolAutenticacaoRetornoDto>(json);
            }
            else return null;
        }

        public IEnumerable<CicloRetornoDto> BuscarCiclos()
        {


            var resposta = httpClient.GetAsync("abrangencia/ciclo-ensino").Result;

            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<CicloRetornoDto>>(json);
            }
            else
            {
                RegistrarLog(resposta, "escolas/tiposEscolas", string.Empty);
                throw new NegocioException("Ocorreu um erro na tentativa de buscar ciclos de ensino no EOL.");
            }
        }

        public IEnumerable<TipoEscolaRetornoDto> BuscarTiposEscola()
        {


            var resposta = httpClient.GetAsync("escolas/tiposEscolas").Result;

            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<TipoEscolaRetornoDto>>(json);
            }
            else
            {
                RegistrarLog(resposta, "escolas/tiposEscolas", string.Empty);
                throw new NegocioException("Ocorreu um erro na tentativa de buscar tipos de escolas no EOL.");
            }
        }

        public async Task<bool> ExisteUsuarioComMesmoEmail(string login, string email)
        {
            var resposta = await httpClient.GetAsync($"autenticacaoSgp/{login}/ValidarEmailExistente/{email}/");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
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

            var resposta = await httpClient.PostAsync("funcionarios/turmas", json);

            if (resposta.IsSuccessStatusCode)
            {
                var jsonRetorno = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AbrangenciaRetornoEolDto>(jsonRetorno);
            }
            else throw new NegocioException("Houve erro ao tentar obter a abrangência do Eol");
        }

        public async Task<string[]> ObterAdministradoresSGP(string codigoDreOuUe)
        {
            var resposta = await httpClient.GetAsync($"escolas/{codigoDreOuUe}/administrador-sgp");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            return null;
        }

        public async Task<string[]> ObterAdministradoresSGPParaNotificar(string codigoDreOuUe)
        {
            var resposta = await httpClient.GetAsync($"escolas/{codigoDreOuUe}/administrador-sgp");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            return null;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosAtivosPorTurma(long turmaId)
        {
            var alunos = new List<AlunoPorTurmaResposta>();
            var resposta = await httpClient.GetAsync($"turmas/{turmaId}/alunos-ativos");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foi encontrado alunos ativos para a turma {turmaId}");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return alunos;

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId)
        {
            var alunos = new List<AlunoPorTurmaResposta>();

            var chaveCache = ObterChaveCacheAlunosTurma(turmaId);
            var cacheAlunos = cache.Obter(chaveCache);
            if (cacheAlunos != null)
            {
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(cacheAlunos);
            }
            else
            {
                var resposta = await httpClient.GetAsync($"turmas/{turmaId}");
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);

                    // Salva em cache por 5 min
                    await cache.SalvarAsync(chaveCache, json, 5);
                }
            }

            return alunos;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorNomeCodigoEol(string anoLetivo, string codigoUe, long codigoTurma, string nome, long? codigoEol, bool? somenteAtivos)
        {
            var alunos = new List<AlunoPorTurmaResposta>();
            var url = $"alunos/ues/{codigoUe}/anosLetivos/{anoLetivo}/autocomplete"
                + (codigoTurma > 0 ? $"?codigoTurma={codigoTurma}" : null)
                + (codigoEol.HasValue ? $"{(codigoTurma > 0 ? "&" : "?") + $"codigoEol={codigoEol}"}" : "")
                + (nome != null ? $"{(codigoEol != null || codigoTurma > 0 ? "&" : "?") + $"nomeAluno={nome}"}" : "")
                + (somenteAtivos == true ? $"&somenteAtivos={somenteAtivos}" : "");

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foi encontrado alunos ativos para UE {codigoUe}");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return alunos;

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
        }

        [Obsolete("não utilizar mais esse método, utilize o ObterAlunosPorTurma")]
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

        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil)
        {
            var url = $"v1/componentes-curriculares/turmas/{codigoTurma}/funcionarios/{login}/perfis/{perfil}";
            return await ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(url);
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(string codigoTurma, string login, Guid perfil)
        {
            var url = $"v1/componentes-curriculares/turmas/{codigoTurma}/funcionarios/{login}/perfis/{perfil}/planejamento";
            return await ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(url);
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codidoAluno, int anoLetivo)
        {
            var alunos = new List<AlunoPorTurmaResposta>();

            var resposta = await httpClient.GetAsync($"alunos/{codidoAluno}/turmas/anosLetivos/{anoLetivo}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
            }

            return alunos;
        }

        [Obsolete("Utilizar: ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento")]
        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasParaPlanejamento(long codigoTurma, string login, Guid perfil)
        {
            var url = $"funcionarios/{login}/perfis/{perfil}/turmas/{codigoTurma}/disciplinas/planejamento";
            return await ObterDisciplinas(url, "ObterDisciplinasParaPlanejamento");
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurma(string codigoTurma)
        {
            var url = $"funcionarios/turmas/{codigoTurma}/disciplinas";
            return await ObterDisciplinas(url, "ObterDisciplinasPorCodigoTurma");
        }

        [Obsolete("Utilizar: ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil", true)]
        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil)
        {
            var url = $"funcionarios/{login}/perfis/{perfil}/turmas/{codigoTurma}/disciplinas";

            return await ObterDisciplinas(url, "ObterDisciplinasPorCodigoTurmaLoginEPerfil");
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsSemAgrupamento(long[] ids)
        {


            var parametros = JsonConvert.SerializeObject(ids);
            var resposta = await httpClient.PostAsync("disciplinas/SemAgrupamento", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
            {
                await RegistrarLogAsync(resposta, "obter as disciplinas", parametros);
                return null;
            }

            var json = resposta.Content.ReadAsStringAsync().Result;

            var retorno = JsonConvert.DeserializeObject<IEnumerable<RetornoDisciplinaDto>>(json);

            return MapearParaDtoDisciplinas(retorno);
        }

        public IEnumerable<DreRespostaEolDto> ObterDres()
        {
            var resposta = httpClient.GetAsync("dres").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<DreRespostaEolDto>>(json);
            }
            return Enumerable.Empty<DreRespostaEolDto>();
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes)
        {


            var resposta = httpClient.PostAsync("escolas", new StringContent(JsonConvert.SerializeObject(codigoUes), Encoding.UTF8, "application/json-patch+json")).Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<EscolasRetornoDto>>(json);
            }
            return Enumerable.Empty<EscolasRetornoDto>();
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId)
        {


            var resposta = httpClient.GetAsync($"DREs/{dreId}/escolas").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<EscolasRetornoDto>>(json);
            }
            return Enumerable.Empty<EscolasRetornoDto>();
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
                    var resposta = httpClient.GetAsync($"{url}/{item}").Result;

                    if (resposta.IsSuccessStatusCode)
                    {
                        var json = resposta.Content.ReadAsStringAsync().Result;
                        var parcial = JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);

                        if (parcial != null)
                            resultado.Dres.AddRange(parcial.Dres);
                    }
                    else
                    {
                        _ = mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty}", LogNivel.Negocio, LogContexto.ApiEol, string.Empty)).Result;
                        throw new NegocioException($"Erro ao obter a estrutura organizacional vigente no EOL. URL base: {httpClient.BaseAddress}");
                    }
                }
            }

            return resultado;
        }

        public EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null)
        {
            var filtroTurmas = new StringContent(JsonConvert.SerializeObject(codigosTurma ?? new string[] { }), UnicodeEncoding.UTF8, "application/json");

            string url = $"abrangencia/estrutura-vigente";



            var resposta = httpClient.PostAsync(url, filtroTurmas).Result;

            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);
            }
            else
            {
                _ = mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty}", LogNivel.Negocio, LogContexto.ApiEol, string.Empty)).Result;

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
            return Enumerable.Empty<UsuarioEolRetornoDto>();
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorCargoUeAsync(string ueId, long cargoId)
        {
            var resposta = await httpClient.GetAsync($"escolas/{ueId}/funcionarios/cargos/{cargoId}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }
            return Enumerable.Empty<UsuarioEolRetornoDto>();
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
            return Enumerable.Empty<ProfessorTurmaReposta>();
        }

        public async Task<MeusDadosDto> ObterMeusDados(string login)
        {
            var url = $"AutenticacaoSgp/{login}/dados";
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
            {
                await RegistrarLogAsync(resposta, "ObterMeusDados", "login = " + login);
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

        public async Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string ueId, string nomeProfessor)
        {
            var resposta = await httpClient.GetAsync($"professores/{anoLetivo}/AutoComplete/{dreId}?nome={nomeProfessor}&ueId={ueId}");
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

            try
            {
                var resposta = await httpClient.GetAsync(url.ToString());
                if (!resposta.IsSuccessStatusCode)
                    return null;

                if (resposta.StatusCode == HttpStatusCode.NoContent)
                    return null;

                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ProfessorTitularDisciplinaEol>>(json);
            }
            catch(Exception ex)
            {
                var a = ex;
                return null;
            }
          
        }

        public async Task<string> ObterNomeProfessorPeloRF(string rfProfessor)
        {
            StringBuilder url = new StringBuilder();

            url.Append($"professores/{rfProfessor}");

            var resposta = await httpClient.GetAsync(url.ToString());

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json);
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

        public async Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo, bool buscarOutrosCargos = false)
        {
            var resposta = await httpClient.GetAsync($"professores/{codigoRF}/BuscarPorRf/{anoLetivo}?buscarOutrosCargos={buscarOutrosCargos}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Ocorreu uma falha ao consultar o professor");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException($"Não foi encontrado professor com RF {codigoRF}");

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ProfessorResumoDto>(json);
        }

        public async Task<ProfessorResumoDto> ObterProfessorPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId)
        {
            if (string.IsNullOrWhiteSpace(codigoRF) || anoLetivo == 0 || string.IsNullOrWhiteSpace(dreId) || string.IsNullOrWhiteSpace(ueId))
                throw new NegocioException("É necessario informar o codigoRF Dre, UE e o ano letivo");

            var resposta = await httpClient.GetAsync($"professores/{codigoRF}/BuscarPorRfDreUe/{anoLetivo}?ueId={ueId}&dreId={dreId}");

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
            return Enumerable.Empty<SupervisoresRetornoDto>();
        }

        public IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId)
        {
            var resposta = httpClient.GetAsync($"dres/{dreId}/supervisores").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<SupervisoresRetornoDto>>(json);
            }
            return Enumerable.Empty<SupervisoresRetornoDto>();
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

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanoAnual(string codigoRf, long componenteCurricularId, int codigoTurma)
        {


            var parametros = JsonConvert.SerializeObject(new
            {
                codigoRf,
                componenteCurricular = componenteCurricularId,
                codigoTurma
            });

            var resposta = await httpClient.PostAsync($"funcionarios/BuscarTurmasElegiveis", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
            var turmas = new List<TurmaParaCopiaPlanoAnualDto>();
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmas = JsonConvert.DeserializeObject<List<TurmaParaCopiaPlanoAnualDto>>(json);
            }
            return turmas;
        }

        public async Task<IEnumerable<TurmaPorUEResposta>> ObterTurmasPorUE(string ueId, string anoLetivo)
        {


            var resposta = await httpClient.GetAsync($"escolas/{ueId}/turmas/anos_letivos/{anoLetivo}");
            var turmas = new List<TurmaPorUEResposta>();
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                turmas = JsonConvert.DeserializeObject<List<TurmaPorUEResposta>>(json);
            }
            return turmas;
        }

        public async Task<bool> PodePersistirTurma(string professorRf, string codigoTurma, DateTime data)
        {


            var dataString = data.ToString("s");

            var resposta = await httpClient.GetAsync($"professores/{professorRf}/turmas/{codigoTurma}/atribuicao/verificar/data?dataConsulta={dataString}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL.");

            var json = resposta.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data)
        {


            var dataString = data.ToString("s");

            var resposta = await httpClient.GetAsync($"professores/{professorRf}/turmas/{codigoTurma}/disciplinas/{disciplinaId}/atribuicao/verificar/data?dataConsulta={dataString}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL.");

            var json = resposta.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<bool>(json);
        }
        public async Task<bool> ProfessorPodePersistirTurma(string professorRf, string codigoTurma, DateTime data)
        {
            var dataString = data.ToString("s");

            var resposta = await httpClient.GetAsync($"professores/{professorRf}/turmas/{codigoTurma}/atribuicao/verificar/data?dataConsulta={dataString}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<bool>(json);
            }
            else throw new Exception("Não foi possível validar a atribuição do professor no EOL.");
        }

        public async Task<AtribuicaoProfessorTurmaEOLDto> VerificaAtribuicaoProfessorTurma(string professorRf, string codigoTurma)
        {
            var resposta = await httpClient.GetAsync($"professores/{professorRf}/turmas/{codigoTurma}/atribuicao/status");
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<AtribuicaoProfessorTurmaEOLDto>(json);
            }
            else throw new Exception("Não foi possível validar a atribuição do professor no EOL.");
        }

        public async Task ReiniciarSenha(string login)
        {


            IList<KeyValuePair<string, string>> valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("login", login) }};

            var resposta = await httpClient.PostAsync($"AutenticacaoSgp/ReiniciarSenha", new FormUrlEncodedContent(valoresParaEnvio));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível reiniciar a senha deste usuário");
        }

        public async Task<UsuarioEolAutenticacaoRetornoDto> RelecionarUsuarioPerfis(string login)
        {


            IList<KeyValuePair<string, string>> valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("login", login) }};

            var resposta = await httpClient.PostAsync($"AutenticacaoSgp/RelacionarUsuarioPerfis", new FormUrlEncodedContent(valoresParaEnvio));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioEolAutenticacaoRetornoDto>(json);
            }
            else return null;
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
                Regencia = x.EhRegencia,
                Compartilhada = x.EhCompartilhada,
                RegistraFrequencia = x.RegistraFrequencia,
                TerritorioSaber = x.Territorio,
                LancaNota = x.LancaNota,
            });
        }

        private string ObterChaveCacheAlunosTurma(string turmaId) => $"alunos-turma:{turmaId}";

        private string[] ObterCodigosDres()
        {
            string url = $"abrangencia/codigos-dres";

            var resposta = httpClient.GetAsync(url).Result;

            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            else
            {
                _ = mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na tentativa de buscar os codigos das Dres no EOL - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty} - URL: {httpClient.BaseAddress}", LogNivel.Negocio, LogContexto.ApiEol, string.Empty)).Result;
                throw new NegocioException($"Erro ao obter os códigos de DREs no EOL. URL base: {httpClient.BaseAddress}");
            }
        }

        private async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(string url)
        {
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                await RegistrarLogAsync(resposta, "ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil", url);
                throw new NegocioException("Ocorreu um erro na tentativa de buscar as disciplinas no EOL.");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
        }

        private async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinas(string url, string rotina)
        {
            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DisciplinaResposta>>(json);
            }

            if (resposta.StatusCode == HttpStatusCode.BadRequest)
                throw new NegocioException("Ocorreu um erro na tentativa de buscar as disciplinas no EOL.");

            await RegistrarLogAsync(resposta, rotina, string.Empty);
            return null;
        }

        /// <summary>
        /// Registra log no sentry dos erros do EOL
        /// </summary>
        /// <param name="resposta">HttpResponse para registrar o request realizado</param>
        /// <param name="rotina">Nome da rotina executada, Ex: Obter Disciplinas</param>
        /// <param name="parametros">Parâmetros do requet caso utilize, Ex:Ids, Datas, Códigos</param>
        private void RegistrarLog(HttpResponseMessage resposta, string rotina, string parametros)
        {
            if (resposta.StatusCode != HttpStatusCode.NotFound)
            {
                var mensagem = resposta.Content.ReadAsStringAsync().Result;
                _ = mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao {rotina} no EOL, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"},Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, ", LogNivel.Negocio, LogContexto.ApiEol, string.Empty)).Result;

            }
        }

        private async Task RegistrarLogAsync(HttpResponseMessage resposta, string rotina, string parametros)
        {
            if (resposta.StatusCode != HttpStatusCode.NotFound)
            {
                var mensagem = await resposta.Content.ReadAsStringAsync();
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao {rotina} no EOL, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"},Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, ", LogNivel.Negocio, LogContexto.ApiEol, string.Empty));
            }
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorDre(Guid perfil, FiltroFuncionarioDto filtroFuncionariosDto)
        {
            var resposta = await httpClient.GetAsync($@"funcionarios/perfis/{perfil}/dres/{filtroFuncionariosDto.CodigoDRE}?CodigoUe={filtroFuncionariosDto.CodigoUE}&CodigoRf={filtroFuncionariosDto.CodigoRF}&NomeServidor={filtroFuncionariosDto.NomeServidor}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);

            }
            return Enumerable.Empty<UsuarioEolRetornoDto>();
        }


        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorAnosEModalidade(string codigoUe, Modalidade modalidade, string[] anosEscolares, int anoLetivo)
        {

            var url = $@"v1/componentes-curriculares/ues/{codigoUe}/modalidades/{(int)modalidade}/anos/{anoLetivo}/anos-escolares?anosEscolares={string.Join("&anosEscolares=", anosEscolares)}";

            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);

            }
            return Enumerable.Empty<ComponenteCurricularEol>();
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesTurmasProgramaPorAnoEModalidade(string codigoUe, Modalidade modalidade, int anoLetivo)
        {

            var url = $@"v1/componentes-curriculares/ues/{codigoUe}/modalidades/{modalidade}/anos/{anoLetivo}";

            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);

            }
            return Enumerable.Empty<ComponenteCurricularEol>();
        }

        public async Task AtribuirPerfil(string codigoRf, Guid perfil)
        {
            var resposta = await httpClient.GetAsync($"perfis/servidores/{codigoRf}/perfil/{perfil}/atribuirPerfil");

            if (resposta.IsSuccessStatusCode)
                return;

            var mensagem = await resposta.Content.ReadAsStringAsync();

            throw new NegocioException(mensagem);
        }

        public async Task<bool> PodePersistirTurmaNoPeriodo(string professorRf, string codigoTurma, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {


            var dataInicioString = dataInicio.ToString("s");
            var dataFimString = dataFim.ToString("s");

            var resposta = await httpClient.PostAsync($"professores/{professorRf}/turmas/{codigoTurma}/componentes/{componenteCurricularId}/atribuicao/periodo/inicio/{dataInicioString}/fim/{dataFimString}", new StringContent(""));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL.");

            var json = resposta.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsAgrupadas(long[] ids)
        {
            var parametros = JsonConvert.SerializeObject(ids);
            var resposta = await httpClient.PostAsync("disciplinas", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
            {
                await RegistrarLogAsync(resposta, "obter as disciplinas", parametros);
                return null;
            }

            var json = resposta.Content.ReadAsStringAsync().Result;

            var retorno = JsonConvert.DeserializeObject<IEnumerable<RetornoDisciplinaDto>>(json);

            return MapearParaDtoDisciplinas(retorno);
        }

        public async Task<InformacoesEscolaresAlunoDto> ObterNecessidadesEspeciaisAluno(string codigoAluno)
        {

            var url = $@"alunos/{codigoAluno}/necessidades-especiais";

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foram encontrados dados de necessidades especiais para o aluno no EOL");


            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<InformacoesEscolaresAlunoDto>(json);

        }

        public async Task<IEnumerable<string>> DefinirTurmasRegulares(string[] codigosTurmas)
        {
            var parametros = JsonConvert.SerializeObject(codigosTurmas);
            var resposta = await httpClient.PostAsync("turmas/turmas-regulares", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
            {
                await RegistrarLogAsync(resposta, "definir turmas regulares", parametros);
                return Enumerable.Empty<string>();
            }

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<string>>(json);
        }

        public async Task<DadosTurmaEolDto> ObterDadosTurmaPorCodigo(string codigoTurma)
        {
            var url = $@"turmas/{codigoTurma}/dados";

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foram encontrados dados da turma {codigoTurma} no EOL.");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DadosTurmaEolDto>(json);
        }
    }
}