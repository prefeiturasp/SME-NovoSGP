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
    }
}
