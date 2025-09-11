﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PlanoAEEObservacao : EntidadeBase
    {
        public PlanoAEEObservacao() { }
        public PlanoAEEObservacao(long planoAEEId, string observacao)
        {
            PlanoAEEId = planoAEEId;
            Observacao = observacao;
        }

        public PlanoAEE PlanoAEE { get; set; }
        public long PlanoAEEId { get; set; }
        public string Observacao { get; set; }

        public bool Excluido { get; set; }
    }
}
