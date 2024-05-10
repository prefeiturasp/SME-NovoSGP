using SME.SGP.Dominio;
using System;

namespace SME.SGP.TesteIntegracao.DiarioBordo
{
    public class FiltroDiarioBordoDto
    {
        public long ComponenteCurricularId { get; set; }
        public bool ContemObservacoes { get; set; }
        public bool ContemDevolutiva { get; set; }

        public DateTime DataAulaDiarioBordo { get; set; } = DateTimeExtension.HorarioBrasilia().Date;
    }
} 