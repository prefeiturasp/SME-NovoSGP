namespace SME.SGP.Infra
{
    public class ServicosEolConstants
    {
        protected ServicosEolConstants() { }

        public const string SERVICO = "servicoEOL";

        public const string URL_AUTENTICACAO_ALTERAR_SENHA = "v1/autenticacao/AlterarSenha/";

        public const string URL_AUTENTICACAO_RECUPERACAO_SENHA_USUARIO = "v1/autenticacao/RecuperarSenha/usuario";

        public const string URL_AUTENTICACAO_RECUPERACAO_SENHA_TOKEN_VALIDAR = "v1/autenticacao/RecuperarSenha/token/validar";

        public const string URL_AUTENTICACAO_SGP_DADOS = "AutenticacaoSgp/{0}/dados";

        public const string URL_PERFIS_SERVIDORES_PERFIL_ATRIBUIR_PERFIL = "perfis/servidores/{0}/perfil/{1}/atribuirPerfil";

        public const string URL_ABRANGENCIA_COMPACTA_VIGENTE_PERFIL = "abrangencia/compacta-vigente/{0}/perfil/{1}";

        public const string URL_ABRANGENCIA_CICLO_ENSINO = "abrangencia/ciclo-ensino";

        public const string URL_ABRANGENCIA_CODIGOS_DRES = "abrangencia/codigos-dres";

        public const string URL_ABRANGENCIA_ESTRUTURA_VIGENTE = "abrangencia/estrutura-vigente";

        public const string URL_AJUDA_DO_SISTEMA_SGP = "ajudadosistema/sgp";

        public const string URL_TURMAS_ALUNO_CONSIDERA_INATIVOS = "turmas/{0}/aluno/{1}/considera-inativos/{2}";

        public const string URL_TURMAS_ALUNOS_ATIVOS_DATA_AULA_TICKS = "Turmas/{0}/alunos-ativos/data-aula-ticks/{1}";

        public const string URL_TURMAS_DATA_MATRICULA_TICKS = "turmas/{0}/data-matricula-ticks/{1}";

        public const string URL_TURMAS_CONSIDERA_INATIVOS = "turmas/{0}/considera-inativos/{1}";

        public const string URL_TURMAS_CALCULO_FREQUENCIA = "turmas/{0}/calculo-frequencia";

        public const string URL_TURMAS_TODOS_ALUNOS = "turmas/{0}/todos-alunos";

        public const string URL_TURMAS_EDFISICA_2020 = "turmas/edfisica-2020";

        public const string URL_TURMAS_DADOS = "turmas/{0}/dados";

        public const string URL_TURMAS_TURMAS_PROGRAMA = "turmas/turmas-programa";

        public const string URL_TURMAS_ALUNOS = "/api/turmas/alunos/{0}";

        public const string URL_TURMAS_ANOS_LETIVOS_ALUNOS_REGULARES = "turmas/anos-letivos/{0}/alunos/{1}/regulares";

        public const string URL_TURMAS_ANOS_LETIVOS_UE_REGULARES = "turmas/anos-letivos/{0}/ue/{1}/regulares";

        public const string URL_TURMAS_UE_SINCRONIZACOES_ANO_LETIVO = "turmas/ue/{0}/sincronizacoes-institucionais/anosLetivos";

        public const string URL_TURMAS_ALUNO_MATRICULAS = "turmas/{0}/aluno/{1}/matriculas";

        public const string URL_TURMAS_UES_MODALIDADE_ANOS_COMPONENTES = "turmas/ues/{0}/modalidades/{1}/anos/{2}/componentes";

        public const string URL_TURMAS_ITINERARIO_ENSINO_MEDIO = "turmas/itinerario/ensino-medio";

        public const string URL_TURMAS_ANOS_LETIVOS_PROFESSOR_HISTORICAS_GERAL = "turmas/anos-letivos/{0}/professor/{1}/turmas-historicas-geral";

        public const string URL_TURMAS_TODOS_ALUNOS_ANO_MODALIDADE_ANO_LETIVO_DRE_INICIO_FIM = "turmas/todos-alunos/anoTurma/{0}/modalidade/{1}/anoLetivo/{2}/dre/{3}/inicio/{4}/fim/{5}";

        public const string URL_ALUNOS_UES_AUTOCOMPLETE_ATIVOS = "alunos/ues/{0}/autocomplete/ativos";

        public const string URL_ALUNOS_ALUNOS = "alunos/alunos";

        public const string URL_ALUNOS_ANO_LETIVO_ALUNOS = "alunos/anoLetivo/{0}/alunos";

        public const string URL_ALUNOS_TURMAS_ANOS_LETIVOS_HISTORICO_FILTRAR_SITUACAO = "alunos/{0}/turmas/anosLetivos/{1}/historico/{2}/filtrar-situacao/{3}/tipo-turma/{4}";

        public const string URL_ALUNOS_ANO_LETIVO_MATRICULADOS_QUANTIDADE = "alunos/ano-letivo/{0}/matriculados/quantidade";

        public const string URL_ALUNOS_ANO_LETIVO_MATRICULADOS = "alunos/ano-letivo/{0}/matriculados";

        public const string URL_ALUNOS_INFORMACOES = "alunos/{0}/informacoes";

        public const string URL_ALUNOS_ALUNOS_PAP = "alunos/alunos-pap/{0}";

        public const string URL_ALUNOS_TURMAS = "alunos/{0}/turmas";

        public const string URL_ALUNOS_SRM_PAEE_ALUNO = "alunos/srm-paee/aluno/{0}";

        public const string URL_ALUNOS_PAEE_TURMA_SRM_REGULAR_ALUNO = "alunos/paee/turma-srm-e-regular/aluno/{0}";

        public const string URL_ALUNOS_NECESSIDADES_ESPECIAIS = "alunos/{0}/necessidades-especiais";

        public const string URL_ALUNOS_TURMAS_ATIVOS = "alunos/turmas/{0}/ativos";

        public const string URL_ALUNOS_RESPONSAVEIS = "alunos/responsaveis/{0}";

        public const string URL_ALUNOS_ATUALIZAR_RESPONSAVEIS = "alunos/{0}/responsaveis/{1}";

        public const string URL_ALUNOS_TURMAS_ANO_LETIVO_MATRICULA_TURMA_TIPO_TURMA = "alunos/{0}/turmas/anosLetivos/{1}/matriculaTurma/{2}/tipoTurma/{3}";

        public const string URL_COMPONENTES_CURRICULARES_DADOS_AULA_TURMA = "/api/v1/componentes-curriculares/dados-aula-turma";

        public const string URL_COMPONENTES_CURRICULARES_TURMAS_REGULARES = "v1/componentes-curriculares/turmas/regulares";

        public const string URL_COMPONENTES_CURRICULARES_UES_TURMAS = "/api/v1/componentes-curriculares/ues/{0}/turmas";

        public const string URL_COMPONENTES_CURRICULARES_TURMAS = "v1/componentes-curriculares/turmas";

        public const string URL_COMPONENTES_CURRICULARES_TURMAS_FUNCIONARIOS_PERFIS_PLANEJAMENTO = "v1/componentes-curriculares/turmas/{0}/funcionarios/{1}/perfis/{2}/planejamento";

        public const string URL_COMPONENTES_CURRICULARES_ANOS_REGENCIA = "v1/componentes-curriculares/anos/{0}/regencia";

        public const string URL_COMPONENTES_CURRICULARES_TURMAS_FUNCIONARIOS_PERFIS = "v1/componentes-curriculares/turmas/{0}/funcionarios/{1}/perfis/{2}/agrupaComponenteCurricular/{3}";


        public const string URL_MATRICULAS_ANOS_ANTERIORES = "matriculas/anos-anteriores";

        public const string URL_MATRICULAS = "matriculas";

        public const string URL_FUNCIONARIOS_UNIDADE = "funcionarios/unidade/{0}";

        public const string URL_FUNCIONARIOS_PERFIS_DRES = "funcionarios/perfis/{0}/dres/{1}";

        public const string URL_FUNCIONARIOS_UE = "funcionarios/ue/{0}";

        public const string URL_FUNCIONARIOS_SUPERVISORES_POR_DRE = "funcionarios/supervisores/{0}";

        public const string URL_FUNCIONARIOS_TURMAS_DISCIPLINAS = "funcionarios/turmas/{0}/disciplinas";

        public const string URL_FUNCIONARIOS_BUSCAR_LISTA_LOGIN = "funcionarios/BuscarPorListaLogin";

        public const string URL_FUNCIONARIOS_BUSCAR_LISTA_RF = "funcionarios/BuscarPorListaRF";

        public const string URL_FUNCIONARIOS_ADMINS_SME = "/api/funcionarios/admins/sme";

        public const string URL_FUNCIONARIOS_TURMAS_DISCIPLINAS_ATRIBUICAO_VERIFICAR_DATA = "professores/{0}/turmas/{1}/disciplinas/{2}/atribuicao/verificar/datatick";

        public const string URL_ESCOLAS_FUNCIONARIOS_CARGOS = "escolas/{0}/funcionarios/cargos/{1}";

        public const string URL_ESCOLAS_FUNCIONARIOS_CARGOS_API = "/api/escolas/{0}/funcionarios/cargos";

        public const string URL_ESCOLAS_ALUNO_MATRICULAS = "escolas/{0}/aluno/{1}/matriculas";

        public const string URL_ESCOLAS_ALUNOS_QUANTIDADE = "escolas/{0}/alunos/quantidade";

        public const string URL_ESCOLAS_PROFESSORES = "/api/escolas/{0}/professores/{1}";

        public const string URL_ESCOLAS_DADOS = "/api/escolas/dados/{0}";

        public const string URL_ESCOLAS_TIPOS_ESCOLAS = "escolas/tiposEscolas";

        public const string URL_ESCOLAS_SINCRONIZACOES_INSTITUCIONAIS = "escolas/{0}/sincronizacoes-institucionais";

        public const string URL_ESCOLAS_FUNCIONARIOS_FUNCOES_EXTERNAS = "/api/escolas/{0}/funcionarios/funcoes-externas/{1}";

        public const string URL_ESCOLAS_FUNCIONARIOS_FUNCOES_ATIVIDADES = "/api/escolas/{0}/funcionarios/funcoes-atividades/{1}";

        public const string URL_PAINEL_EDUCACIONAL_INDICADORES_PAP = "/api/painel-educacional/indicadores-pap?codigoDre={0}&codigoUe={1}";

        public const string URL_PROFESSORES_TITULARES_TURMA = "professores/{0}/titulares/realizaAgrupamentoComponente/{1}";

        public const string URL_PROFESSORES_DISCIPLINA_TURMAS = "professores/{0}/disciplina/{1}/turmas";

        public const string URL_PROFESSORES_DISCIPLINAS_ATRIBUICAO_DATA = "professores/{0}/disciplinas/{1}/atribuicao/data";

        public const string URL_PROFESSORES_TURMAS_ATRIBUICAO_VERIFICAR_DATA = "professores/{0}/turmas/{1}/atribuicao/verificar/data";

        public const string URL_PROFESSORES_TURMAS_DISCIPLINAS_ATRIBUICAO_RECORRENCIA_VERIFICAR_DATA = "professores/{0}/turmas/{1}/disciplinas/{2}/atribuicao/recorrencia/verificar/datas";

        public const string URL_PROFESSORES_TURMAS_DISCIPLINAS_ATRIBUICAO_VERIFICAR_DATA = "professores/{0}/turmas/{1}/disciplinas/{2}/atribuicao/verificar/data";
       
        public const string URL_PROFESSORES_TITULAR_TURMAS_COMPONENTES = "professores/titular/turmas/{0}/componentes-curriculares/{1}";

        public const string URL_PROFESSORES_BUSCAR_RF = "professores/{0}/BuscarPorRf/{1}";

        public const string URL_PROFESSORES_TURMAS = "/api/professores/{0}/turmas";

        public const string URL_PROFESSORES_TITULARES = "professores/titulares";

        public const string URL_PROFESSORES_VALIDADE = "professores/{0}/validade";

        public const string URL_DRES_SUPERVISORES = "dres/{0}/supervisores";

        public const string URL_DRES_UES = "dres/{0}/ues";

        public const string URL_UES_TURMAS_SINCRONIZACOES = "ues/{0}/turmas/{1}/sincronizacoes-institucionais";

        public const string URL_COMPONENTES_CURRICULARES_AGRUPAMENTO_TERRITORIO_SABER = "v1/componentes-curriculares/territorio-saber/agrupamentos";

        public const string URL_COMPONENTES_CURRICULARES_AGRUPAMENTOS_TERRITORIO_SABER_CORRELACIONADOS = "v1/componentes-curriculares/territorio-saber/agrupamentos-correlacionados";
        
        public const string URL_AUTENTICACAO_SGP_ALTERAR_EMAIL = "AutenticacaoSgp/AlterarEmail";
        
        public const string URL_AUTENTICACAO_SGP_ALTERAR_SENHA = "AutenticacaoSgp/AlterarSenha";
        
        public const string URL_COMPONENTES_CURRICULARES = "v1/componentes-curriculares";
        
        public const string URL_COMPONENTES_CURRICULARES_TURMAS_FUNCIONARIOS_PERFIS_VALIDAR_PAP = "v1/componentes-curriculares/turmas/{0}/funcionarios/{1}/perfis/{2}/validar/pap";
        
        public const string URL_AUTENTICACAO = "v1/autenticacao";
        
        public const string URL_AUTENTICACAO_SGP_VALIDA_EMAIL_EXISTENTE = "autenticacaoSgp/{0}/ValidarEmailExistente/{1}";
        
        public const string URL_FUNCIONARIOS_PERFIS_TURMAS = "funcionarios/{0}/perfis/{1}/turmas";
        
        public const string URL_FUNCIONARIOS_TURMAS = "funcionarios/turmas";
        
        public const string URL_ESCOLAS_ADMINISTRADOR_SGP = "escolas/{0}/administrador-sgp";
        
        public const string URL_ALUNOS_UES_ANOS_LETIVOS_AUTOCOMPLETE = "alunos/ues/{0}/anosLetivos/{1}/autocomplete";
        
        public const string URL_ALUNOS_TURMAS_ANOS_LETIVOS_HISTORICO_FILTRAR_SITUACAO_TIPO_TURMA = "alunos/{0}/turmas/anosLetivos/{1}/historico/{2}/filtrar-situacao/{3}/tipo-turma/{4}";

        public const string URL_DADOS_RESPONSAVEIS_FILIACAO_ALUNO = "alunos/{0}/responsaveis/filiacao";

        public const string URL_DRES = "dres";
        
        public const string URL_FUNCIONARIOS = "funcionarios";
        
        public const string URL_PROFESSORES_BUSCAR_POR_LISTA_RF = "professores/{0}/BuscarPorListaRF";
        
        public const string URL_AUTENTICACAO_SGP_CARREGAR_PERFIS_POR_LOGIN = "autenticacaoSgp/CarregarPerfisPorLogin/{0}";
        
        public const string URL_PROFESSORES_AUTOCOMPLETE = "professores/{0}/AutoComplete/{1}";
        
        public const string URL_PROFESSORES = "professores/{0}";
        
        public const string URL_FUNCIONARIOS_SUPERVISORES = "funcionarios/supervisores";
        
        public const string URL_PROFESSORES_BUSCAR_POR_RF_DRE_UE = "professores/{0}/BuscarPorRfDreUe/{1}";

        public const string URL_PROFESSORES_ESCOLAS_TURMAS_ANOS_LETIVOS = "professores/{0}/escolas/{1}/turmas/anos_letivos/{2}";

        public const string URL_FUNCIONARIOS_BURCAR_TURMAS_ELEGIVEIS = "funcionarios/BuscarTurmasElegiveis";
        
        public const string URL_AUTENTICACAO_SGP_REINICIAR_SENHA = "AutenticacaoSgp/ReiniciarSenha";
        
        public const string URL_PROFESSORES_TURMAS_COMPONENTES_ATRIBUICAO_PERIODO_INICIO_FIM = "professores/{0}/turmas/{1}/componentes/{2}/atribuicao/periodo/inicio/{3}/fim/{4}";
        
        public const string URL_TURMAS_REGULARES = "turmas/turmas-regulares";
        
        public const string URL_PROFESSORES_TITULARES_UE = "professores/titulares/ue/{0}/{1}";
        
        public const string URL_AUTENTICACAO_AUTENTICAR_SEM_SENHA = "v1/autenticacao/AutenticarSemSenha/{0}";
        
        public const string URL_FUNCIONARIOS_PERFIS = "funcionarios/perfis/{0}";
        
        public const string URL_COMPONENTES_CURRICULARES_UES_MODALIDADES_ANOS = "v1/componentes-curriculares/ues/{0}/modalidades/{1}/anos/{2}";
        
        public const string URL_COMPONENTES_CURRICULARES_UES_MODALIDADES_ANOS_ANOS_ESCOLARES = "v1/componentes-curriculares/ues/{0}/modalidades/{1}/anos/{2}/anos-escolares";
        
        public const string URL_AUTENTICACAO_SGP_CARREGAR_DADOS_ACESSO_USUARIOS_PERFIS = "AutenticacaoSgp/CarregarDadosAcesso/usuarios/{0}/perfis/{1}";

        public const string URL_GRUPOS_USUARIOS = "grupos-usuarios/tipo-perfil/{0}";

        public const string URL_ABRANGENCIAS_PERFIS_USUARIO = "abrangencia/perfis/usuarios";
        
        public const string URL_COMPONENTES_CURRICULARES_TURMA_SEM_ATRIBUICAO_AULA = "v1/componentes-curriculares/turmas/{0}/sem-atribuicao/{1}";

        public const string URL_COMPONENTES_CURRICULARES_PROGRAMA_ALUNO = "alunos/{0}/turmas-programa/{1}/componentes-curriculares";
        public const string URL_TURMAS_TURMAS_PAP = "alunos/turmas-pap/{0}/ues/{1}";
        public const string URL_ALUNOS_TURMAS_PAP_ANO_CORRENTE = "alunos/pap/ano-corrente";
        public const string URL_ALUNOS_TURMAS_PAP_ANO_LETIVO = "alunos/pap/ano-letivo/{0}";
    }
}
