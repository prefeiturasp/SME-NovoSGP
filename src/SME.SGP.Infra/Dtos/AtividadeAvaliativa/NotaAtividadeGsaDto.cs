﻿using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class NotaAtividadeGsaDto
    {
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AtividadeGoogleClassroomId { get; set; }
        public StatusGSA StatusGsa { get; set; }
        public double? Nota { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataEntregaAvaliacao { get; set; }
        public long CodigoAluno { get; set; }
        public string Titulo { get; set; }

        public NotaAtividadeGsaDto()
        {
        }
    }
}