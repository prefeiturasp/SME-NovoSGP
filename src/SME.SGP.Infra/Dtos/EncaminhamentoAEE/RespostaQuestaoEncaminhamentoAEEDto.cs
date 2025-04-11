﻿using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RespostaQuestaoEncaminhamentoAEEDto
    {
        public long Id { get; set; }
        public long QuestaoId { get; set; }
        public long? RespostaId { get; set; }
        public string Texto { get; set; }
        public Arquivo Arquivo { get; set; }
    }
}
