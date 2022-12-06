﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPADto
    {
        public EncaminhamentoNAAPADto()
        {
            Secoes = new List<EncaminhamentoNAAPASecaoDto>();
        }
        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public List<EncaminhamentoNAAPASecaoDto> Secoes { get; set; }
    }
}
