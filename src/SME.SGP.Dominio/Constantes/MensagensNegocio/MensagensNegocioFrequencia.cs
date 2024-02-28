using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public static class MensagensNegocioFrequencia
    {
        public const string Lista_de_alunos_e_o_componente_devem_ser_informados = "A lista de alunos da turma e o componente curricular devem ser informados para calcular a frequência.";

        public const string A_aula_informada_nao_foi_encontrada = "A aula informada não foi encontrada";

        public const string Turma_informada_nao_foi_encontrada = "Turma informada não foi encontrada";

        public const string Nao_possui_permissão_para_inserir_neste_periodo = "Você não possui permissão para inserir registro de frequência neste período";

        public const string Nao_e_permitido_registro_de_frequencia_para_este_componente = "Não é permitido registro de frequência para este componente curricular.";

        public const string Nao_e_possível_registrar_a_frequência_o_componente_nao_permite_substituicao = "Não é possível registrar a frequência pois esse componente curricular não permite substituição.";

        public const string Crianca_nao_encontrada_anotacao = "Criança não encontrada.";

        public const string Crianca_nao_ativa = "Criança não ativa na turma.";

        public const string Aluno_nao_ativo = "Aluno não ativo na turma.";

        public const string Aluno_nao_encontrado_anotacao = "Aluno não encontrado.";

        public const string Aula_nao_encontrada_anotacao = "Aula não encontrada.";
        public const string Motivo_ausencia_nao_encontrado = "O motivo de ausência informado não foi localizado.";
        public const string Anotacao_nao_localizada_com_id_informado = "Anotação não localizada com o Id informado";
        public const string Nao_foi_possivel_registrar_a_frequencia_do_dia_x = "Não foi possível registrar a frequência do dia {0}";
        public const string TURMA_NAO_ENCONTRADA_POR_CODIGO = "Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.";
        public const string Nao_foi_localizado_usuario_pelo_login = "Não foi possível localizar o login {0}.";
    }
}
