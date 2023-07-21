using System;

namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioComuns
    {
        public const string Voce_nao_pode_criar_aulas_para_essa_turma = "Você não pode criar aulas para essa Turma.";
        
        public const string APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO = 
            "Apenas é possível consultar este registro pois o período não está em aberto.";

        public const string A_AUDITORIA_NAO_FOI_REGISTRADA_PELO_ADMINISTRADOR =
            "A auditoria não foi registrada pelo administrador.";

        public const string E_NECESSARIO_SELECIONAR_ESTUDANTE_ALUNO_PARA_COMPENSACAO = "É necessário selecionar um estudante para realizar a compensação de ausência";
        public const string USUARIO_SEM_ACESSO_TURMA_RESPECTIVA_AULA = "Usuario sem acesso a turma da respectiva aula";
        
        public const string Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data = "Você não pode fazer alterações ou inclusões nesta turma, componente e data.";

        public const string O_plano_aee_informado_nao_foi_encontrado = "O plano Aee informado não foi encontrado";

        public const string FORMATO_ARQUIVO_NAO_ACEITO = "O formato de arquivo enviado não é aceito";
        public const string NENHUMA_SECAO_ENCONTRADA = "Nenhuma seção foi encontrada";
        public const string NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X = "Nenhuma questão foi encontrada na Seção {0}";
        public const string ARQUIVO_INF0RMADO_NAO_ENCONTRADO = "O arquivo informado não foi encontrado";
        public const string NENHUM_ARQUIVO_ENCONTRADO = "Nenhum Arquivo foi Encontrado para Exclusão";
        public const string NAO_FOI_POSSIVEL_LOCALIZAR_USUARIO = "Não foi possível localizar o usuário.";
        public const string ACESSO_SUPORTE_INDISPONIVEL = "Acesso de suporte indisponível para este usuário por conta de informações sigilosas";
        public const string NAO_FOI_POSSIVEL_INICIAR_A_CONSOLIDACAO_DIARIA =  "Não foi possível iniciar a consolidação diária";
        public const string NAO_FOI_POSSIVEL_INSERIR_TURMA_X_NA_FILA_DE_CONSOLIDACAO_DIARIA_DE_FREQUENCIA = "Não foi possível inserir a turma: {0} na fila de consolidação de frequência.";
    }
}