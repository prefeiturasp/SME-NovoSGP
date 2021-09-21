using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class SolicitarInclusaoComunicadoEscolaAquiCommand : IRequest<string>
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IEnumerable<string> Turmas { get; set; }
        public bool AlunosEspecificados { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public IEnumerable<string> Alunos { get; set; }
        public string SeriesResumidas { get; set; }
        public long? TipoCalendarioId { get; set; }
        public long? EventoId { get; set; }

        public SolicitarInclusaoComunicadoEscolaAquiCommand(DateTime dataEnvio, DateTime? dataExpiracao, string descricao, string titulo, int anoLetivo, string codigoDre, string codigoUe, bool alunosEspecificados, int[] modalidades, int semestre, IEnumerable<string> alunos, IEnumerable<string> turmas, string seriesResumidas, long? tipoCalendarioId, long? eventoId)
        {
            DataEnvio = dataEnvio;
            DataExpiracao = dataExpiracao;
            Descricao = descricao;
            Titulo = titulo;
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            AlunosEspecificados = alunosEspecificados;
            Modalidades = modalidades;
            Semestre = semestre;
            Alunos = alunos;
            Turmas = turmas;
            SeriesResumidas = seriesResumidas;
            TipoCalendarioId = tipoCalendarioId;
            EventoId = eventoId;
        }
    }
}
