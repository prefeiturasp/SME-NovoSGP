using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class OpcaoRespostaDto
    {
        public List<QuestaoDto> QuestoesComplementares { get; set; }
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
    }

    public static class OpcaoRespostaDtoExtensao
    {
        public static bool NaoNuloEContemRegistros(this IEnumerable<OpcaoRespostaDto> opcoes)
        {
            return opcoes.NaoEhNulo() && opcoes.Any();
        }
    }

}
