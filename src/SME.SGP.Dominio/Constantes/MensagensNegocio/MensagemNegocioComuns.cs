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
        public const string NENHUMA_SECAO_ENCONTRADA = "Nenhuma seção foi encontrada";
        public const string NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X = "Nenhuma questão foi encontrada na Seção {0}";
    }
}