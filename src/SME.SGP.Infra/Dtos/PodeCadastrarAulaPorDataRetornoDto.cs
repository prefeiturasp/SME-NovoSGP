using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PodeCadastrarAulaPorDataRetornoDto
    {
        public PodeCadastrarAulaPorDataRetornoDto(bool podeCadastrar, string mensagemPeriodo = "", bool somenteReposicao = false)
        {
            PodeCadastrar = podeCadastrar;
            SomenteReposicao = somenteReposicao;
            MensagemPeriodo = mensagemPeriodo;
        }
        public bool PodeCadastrar { get; set; }
        public bool SomenteReposicao { get; set; }
        public string MensagemPeriodo { get; set; }
    }
}
