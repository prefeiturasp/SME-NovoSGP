using System;

namespace SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados
{
    public class DadosLeituraAlunosComunicadoPorTurmaDto
    {
        public Guid Id { get; set; }
        public long CodigoAluno { get; set; }
        public short NumeroChamada { get; set; }
        public string NomeAluno { get; set; }
        public string NomeResponsavel { get; set; }
        public string TelefoneResponsavel { get; set; }
        public bool PossueApp { get; set; }
        public bool EhAtendidoAEE { get; set; }
        public bool LeuComunicado { get; set; }
        public DateTime? DataLeitura { get; set; }
        public DateTime? DataSituacaoAluno { get; set; }
        public string SituacaoAluno { get; set; }
        public MarcadorFrequenciaDto Marcador { get; set; }

        public DadosLeituraAlunosComunicadoPorTurmaDto()
        {
            Id = Guid.NewGuid();
        }
    }
}