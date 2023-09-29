using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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
        private readonly IMediator mediator;
        private readonly HttpClient httpClient;

        public ServicoEOL(HttpClient httpClient, IMediator mediator)
        {
            this.httpClient = httpClient;
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

        public async Task<AutenticacaoApiEolDto> Autenticar(string login, string senha)
        {
            var parametros = JsonConvert.SerializeObject(new { login, senha });
            var resposta = await httpClient.PostAsync($"v1/autenticacao", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AutenticacaoApiEolDto>(json);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<CicloRetornoDto>> BuscarCiclos()
        {
            var resposta = await httpClient.GetAsync("abrangencia/ciclo-ensino");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<CicloRetornoDto>>(json);
            }
            else
            {
                RegistrarLog(resposta, "escolas/tiposEscolas", string.Empty);
                throw new NegocioException("Ocorreu um erro na tentativa de buscar ciclos de ensino no EOL.");
            }
        }

        public async Task<IEnumerable<TipoEscolaRetornoDto>> BuscarTiposEscola()
        {
            var resposta = await httpClient.GetAsync("escolas/tiposEscolas");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
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

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorNomeCodigoEol(string anoLetivo, string codigoUe, long codigoTurma, string nome, long? codigoEol, bool? somenteAtivos)
        {
            var alunos = Enumerable.Empty<AlunoPorTurmaResposta>();
            var url = $"alunos/ues/{codigoUe}/anosLetivos/{anoLetivo}/autocomplete"
                + (codigoTurma > 0 ? $"?codigoTurma={codigoTurma}" : null)
                + (codigoEol.HasValue ? $"{(codigoTurma > 0 ? "&" : "?") + $"codigoEol={codigoEol}"}" : "")
                + (nome.NaoEhNulo() ? $"{(codigoEol.NaoEhNulo() || codigoTurma > 0 ? "&" : "?") + $"nomeAluno={nome}"}" : "")
                + (somenteAtivos == true ? $"&somenteAtivos={somenteAtivos}" : "");

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foi encontrado alunos ativos para UE {codigoUe}");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return alunos;

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codigoAluno, int anoLetivo, bool consideraHistorico, bool filtrarSituacao = true, bool verificarTipoTurma = true)
        {
            var alunos = Enumerable.Empty<AlunoPorTurmaResposta>();

            var resposta = await httpClient.GetAsync($"alunos/{codigoAluno}/turmas/anosLetivos/{anoLetivo}/historico/{consideraHistorico}/filtrar-situacao/{filtrarSituacao}/tipo-turma/{verificarTipoTurma}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
            }

            return alunos;
        }

        public async Task<IEnumerable<DreRespostaEolDto>> ObterDres()
        {
            var resposta = await httpClient.GetAsync("dres");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DreRespostaEolDto>>(json);
            }
            return Enumerable.Empty<DreRespostaEolDto>();
        }

        public async Task<IEnumerable<EscolasRetornoDto>> ObterEscolasPorCodigo(string[] codigoUes)
        {
            var resposta = await httpClient.PostAsync("escolas", new StringContent(JsonConvert.SerializeObject(codigoUes), Encoding.UTF8, "application/json-patch+json"));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<EscolasRetornoDto>>(json);
            }
            return Enumerable.Empty<EscolasRetornoDto>();
        }

        public async Task<IEnumerable<EscolasRetornoDto>> ObterEscolasPorDre(string dreId)
        {
            var resposta = await httpClient.GetAsync($"DREs/{dreId}/escolas");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<EscolasRetornoDto>>(json);
            }
            return Enumerable.Empty<EscolasRetornoDto>();
        }

        public async Task<EstruturaInstitucionalRetornoEolDTO> ObterEstruturaInstuticionalVigentePorDre()
        {
            EstruturaInstitucionalRetornoEolDTO resultado = null;
            var codigosDres = await ObterCodigosDres();
            string url = $"abrangencia/estrutura-vigente";

            if (codigosDres.NaoEhNulo() && codigosDres.Length > 0)
            {
                resultado = new EstruturaInstitucionalRetornoEolDTO();
                foreach (var item in codigosDres)
                {
                    var resposta = await httpClient.GetAsync($"{url}/{item}");

                    if (resposta.IsSuccessStatusCode)
                    {
                        var json = await resposta.Content.ReadAsStringAsync();
                        var parcial = JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);

                        if (parcial.NaoEhNulo())
                            resultado.Dres.AddRange(parcial.Dres);
                    }
                    else
                    {
                        var httpContentResult = await resposta.Content?.ReadAsStringAsync();
                        _ = mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente - HttpCode {resposta.StatusCode} - Body {httpContentResult ?? string.Empty}", LogNivel.Negocio, LogContexto.ApiEol, string.Empty)).Result;
                        throw new NegocioException($"Erro ao obter a estrutura organizacional vigente no EOL. URL base: {httpClient.BaseAddress}");
                    }
                }
            }

            return resultado;
        }

        public async Task<EstruturaInstitucionalRetornoEolDTO> ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null)
        {
            var filtroTurmas = new StringContent(JsonConvert.SerializeObject(codigosTurma ?? new string[] { }), UnicodeEncoding.UTF8, "application/json");

            string url = $"abrangencia/estrutura-vigente";

            var resposta = await httpClient.PostAsync(url, filtroTurmas);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);
            }
            else
            {
                var httpContentResult = await resposta.Content?.ReadAsStringAsync();
                _ = mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente - HttpCode {resposta.StatusCode} - Body {httpContentResult ?? string.Empty}", LogNivel.Negocio, LogContexto.ApiEol, string.Empty)).Result;

                return null;
            }
        }
        
        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorCargoUeAsync(string ueId, long cargoId)
        {
            var resposta = await httpClient.GetAsync($"escolas/{ueId}/funcionarios/cargos/{cargoId}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
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

        public async Task<IEnumerable<ProfessorTurmaReposta>> ObterListaTurmasPorProfessor(string codigoRf)
        {
            var resposta = await httpClient.GetAsync($"professores/{codigoRf}/turmas");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
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

        public async Task<PerfisApiEolDto> ObterPerfisPorLogin(string login)
        {
            var resposta = await httpClient.GetAsync($"autenticacaoSgp/CarregarPerfisPorLogin/{login}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PerfisApiEolDto>(json);
            }
            return null;
        }

        public async Task<RetornoDadosAcessoUsuarioSgpDto> CarregarDadosAcessoPorLoginPerfil(string login, Guid perfilGuid, AdministradorSuporteDto administradorSuporte = null)
        {
            HttpResponseMessage resposta;
            
            if (administradorSuporte.NaoEhNulo())
                resposta = await httpClient.GetAsync($"AutenticacaoSgp/CarregarDadosAcesso/usuarios/{login}/perfis/{perfilGuid}?loginAdm={administradorSuporte.Login}&nomeAdm={administradorSuporte.Nome}");
            else
                resposta = await httpClient.GetAsync($"AutenticacaoSgp/CarregarDadosAcesso/usuarios/{login}/perfis/{perfilGuid}");                
            
            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possivel obter os dados de acesso");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RetornoDadosAcessoUsuarioSgpDto>(json);
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

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorTurmas(IEnumerable<string> codigosTurmas)
        {
            StringBuilder url = new StringBuilder();

            url.Append($"professores/titulares?codigosTurmas={string.Join("&codigosTurmas=", codigosTurmas)}");

            var resposta = await httpClient.GetAsync(url.ToString());
            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorTitularDisciplinaEol>>(json);
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

        public async Task<ProfessorResumoDto> ObterProfessorPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos = false, bool buscarPorTodasDre = false)
        {
            if (string.IsNullOrWhiteSpace(codigoRF) || anoLetivo == 0 || (!buscarPorTodasDre && (string.IsNullOrWhiteSpace(dreId) || string.IsNullOrWhiteSpace(ueId))))
                throw new NegocioException("É necessario informar o codigoRF Dre, UE e o ano letivo");

            string paramUeId = string.Empty, paramDreId = string.Empty;

            if (!buscarPorTodasDre)
            {
                paramUeId = !string.IsNullOrWhiteSpace(ueId) ? $"ueId={ueId}&" : string.Empty;
                paramDreId = !string.IsNullOrWhiteSpace(dreId) ? $"dreId={dreId}&" : string.Empty;
            }            

            var resposta = await httpClient
                .GetAsync($"professores/{codigoRF}/BuscarPorRfDreUe/{anoLetivo}?{string.Concat(paramUeId, paramDreId)}buscarOutrosCargos={buscarOutrosCargos}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Ocorreu uma falha ao consultar o professor");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
            {
                var dadosUsuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
                var ehGestorEscolar = dadosUsuarioLogado.PossuiPerfilGestorEscolar();

                if (!dadosUsuarioLogado.EhProfessorCj() && !ehGestorEscolar)
                    throw new NegocioException($"Não foi encontrado professor com RF {codigoRF}");

                if (ehGestorEscolar)
                {
                    bool ehFuncionarioGestorEscolarDaUe = false;
                    if (codigoRF.EhLoginCpf())
                    {
                        var funcionariosExternosDaUe = await mediator.Send(new ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQuery(ueId,
                            new List<int> { (int)FuncaoExterna.AD, (int)FuncaoExterna.Diretor, (int)FuncaoExterna.CP }, dreId));
                        ehFuncionarioGestorEscolarDaUe = funcionariosExternosDaUe.Any(f => f.FuncionarioCpf == dadosUsuarioLogado.CodigoRf);
                    }
                    else
                    {
                        var funcionariosDaUe = await mediator.Send(new ObterFuncionariosCargosPorUeCargosQuery(ueId,
                            new List<int> { (int)Cargo.AD, (int)Cargo.Diretor, (int)Cargo.CP }, dreId));
                        ehFuncionarioGestorEscolarDaUe = funcionariosDaUe.Any(f => f.FuncionarioRF == dadosUsuarioLogado.CodigoRf);
                    }
                    

                    if (ehFuncionarioGestorEscolarDaUe)
                    {
                        return new ProfessorResumoDto
                            { CodigoRF = codigoRF, Nome = dadosUsuarioLogado.Nome, UsuarioId = dadosUsuarioLogado.Id };
                    }
                }     
                else
                {
                    var obterAtribuicoesCJAtivas = await mediator.Send(new ObterAtribuicoesCJAtivasQuery(codigoRF, false));

                    if (!obterAtribuicoesCJAtivas.Any())
                        throw new NegocioException($"Não foi encontrado professor com RF {codigoRF}");
                    
                    var possuiAtribuicaoNaUE = obterAtribuicoesCJAtivas.Any(a => a.UeId == ueId);

                    if (possuiAtribuicaoNaUE)
                    {
                        return new ProfessorResumoDto
                            { CodigoRF = codigoRF, Nome = dadosUsuarioLogado.Nome, UsuarioId = dadosUsuarioLogado.Id };
                    }
                }

                throw new NegocioException($"Não foi encontrado professor com RF {codigoRF}");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<ProfessorResumoDto>(json);

            var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(retorno.CodigoRF));

            if (usuario.EhNulo())
                throw new NegocioException("Usuário não localizado.");

            retorno.UsuarioId = usuario.Id;

            return retorno;
        }

        public async Task<IEnumerable<SupervisoresRetornoDto>> ObterSupervisoresPorCodigo(string[] codigoSupervisores)
        {
            var resposta = await httpClient.PostAsync("funcionarios/supervisores", new StringContent(JsonConvert.SerializeObject(codigoSupervisores), Encoding.UTF8, "application/json-patch+json"));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<SupervisoresRetornoDto>>(json);
            }
            return Enumerable.Empty<SupervisoresRetornoDto>();
        }

        public async Task<IEnumerable<SupervisoresRetornoDto>> ObterSupervisoresPorDre(string dreId)
        {
            var resposta = await httpClient.GetAsync($"dres/{dreId}/supervisores");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
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
            var turmas = Enumerable.Empty<TurmaParaCopiaPlanoAnualDto>();
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmas = JsonConvert.DeserializeObject<List<TurmaParaCopiaPlanoAnualDto>>(json);
            }
            return turmas;
        }

        public async Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data)
        {
            var dataString = data.ToString("s");

            var resposta = await httpClient.GetAsync($"professores/{professorRf}/turmas/{codigoTurma}/disciplinas/{disciplinaId}/atribuicao/verificar/data?={dataString}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL.");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        public async Task<AtribuicaoProfessorTurmaEOLDto> VerificaAtribuicaoProfessorTurma(string professorRf, string codigoTurma)
        {
            var resposta = await httpClient.GetAsync($"professores/{professorRf}/turmas/{codigoTurma}/atribuicao/status");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AtribuicaoProfessorTurmaEOLDto>(json);
            }
            else
            {
                throw new Exception("Não foi possível validar a atribuição do professor no EOL.");
            }
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
            else
            {
                return null;
            }
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

        private string ObterChaveCacheAlunosTurma(string turmaId) => $"alunos-turma:{turmaId}";

        private async Task<string[]> ObterCodigosDres()
        {
            string url = $"abrangencia/codigos-dres";

            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await  resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            else
            {
                var httpContentResult = await resposta.Content?.ReadAsStringAsync();
                _ = mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na tentativa de buscar os codigos das Dres no EOL - HttpCode {resposta.StatusCode} - Body {httpContentResult ?? string.Empty} - URL: {httpClient.BaseAddress}", LogNivel.Negocio, LogContexto.ApiEol, string.Empty)).Result;
                throw new NegocioException($"Erro ao obter os códigos de DREs no EOL. URL base: {httpClient.BaseAddress}");
            }
        }

        private async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurriculares(string url, string rotina, bool ignorarInfoPedagogicasSgp = false)
        {
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                await RegistrarLogAsync(resposta, rotina, url);
                throw new NegocioException("Ocorreu um erro na tentativa de buscar as disciplinas no EOL.");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
            if (!ignorarInfoPedagogicasSgp)
            {
                var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(retorno.ObterCodigos()));
                retorno.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
            }
            return retorno;
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

        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorAnosEModalidade(string codigoUe, Modalidade modalidade, int anoLetivo, string[] anosEscolares)
        {
            var url = string.Empty;
            if (anosEscolares.EhNulo() || !anosEscolares.Any())
                url = $@"v1/componentes-curriculares/ues/{codigoUe}/modalidades/{(int)modalidade}/anos/{anoLetivo}";
            else
              url = $@"v1/componentes-curriculares/ues/{codigoUe}/modalidades/{(int)modalidade}/anos/{anoLetivo}/anos-escolares?anosEscolares={string.Join("&anosEscolares=", anosEscolares)}";

            return await ObterComponentesCurriculares(url, "ObterComponentesCurricularesPorAnosEModalidade");
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

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
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
        
        public async Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> ListagemTurmasComComponente(string codigoUe, string modalidade, int bimestre, string codigoTurma, int anoLetivo, int qtdeRegistros, int qtdeRegistrosIgnorados)
        {
            var url = $@"turmas/{codigoUe}/{modalidade}/{bimestre}/{codigoTurma}/{anoLetivo}/{qtdeRegistros}/{qtdeRegistrosIgnorados}/listagem-turmas";

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foram encontrados dados da(s) turma(s) da UE {codigoUe} no EOL.");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>(json);
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorUe(string ueCodigo, DateTime dataReferencia)
        {
            var url = $"professores/titulares/ue/{ueCodigo}/{dataReferencia.ToString("yyyy-MM-dd")}";
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorTitularDisciplinaEol>>(json);
        }

        public async Task<IEnumerable<FuncionarioUnidadeDto>> ObterListaNomePorListaLogin(IEnumerable<string> logins)
        {
            var resposta = await httpClient.PostAsync($"funcionarios/BuscarPorListaLogin",
                new StringContent(JsonConvert.SerializeObject(logins), Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();

            return await Task.FromResult(JsonConvert.DeserializeObject<IEnumerable<FuncionarioUnidadeDto>>(json));
        }

        public async Task<AutenticacaoApiEolDto> ObtenhaAutenticacaoSemSenha(string login)
        {
            var url = $@"v1/autenticacao/AutenticarSemSenha/{login}";

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foram encontrados dados do usuário {login}");


            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AutenticacaoApiEolDto>(json);
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterUsuarioFuncionario(Guid perfil, FiltroFuncionarioDto filtroFuncionariosDto)
        {
            var resposta = await httpClient.GetAsync($@"funcionarios/perfis/{perfil}?CodigoDre={filtroFuncionariosDto.CodigoDRE}&CodigoUe={filtroFuncionariosDto.CodigoUE}&CodigoRf={filtroFuncionariosDto.CodigoRF}&NomeServidor={filtroFuncionariosDto.NomeServidor}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);

            }
            return Enumerable.Empty<UsuarioEolRetornoDto>();
        }
    }
}