using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AlterarComunicadoCommand : IRequest<bool>
    {
        public AlterarComunicadoCommand(long id, string titulo, string descricao, DateTime dataEnvio, DateTime? dataExpiracao, int anoLetivo, string seriesResumidas, string codigoDre, string codigoUe, IEnumerable<string> turmas, long? tipoCalendarioId, long? eventoId, bool alunosEspecificados, int[] modalidades, int semestre, IEnumerable<string> alunos)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            DataEnvio = dataEnvio;
            DataExpiracao = dataExpiracao;
            AnoLetivo = anoLetivo;
            SeriesResumidas = seriesResumidas;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Turmas = turmas;
            TipoCalendarioId = tipoCalendarioId;
            EventoId = eventoId;
            AlunosEspecificados = alunosEspecificados;
            Modalidades = modalidades;
            Semestre = semestre;
            Alunos = alunos;
        }

        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public int AnoLetivo { get; set; }
        public string SeriesResumidas { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IEnumerable<string> Turmas { get; set; }
        public long? TipoCalendarioId { get; }
        public long? EventoId { get; }
        public bool AlunosEspecificados { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public IEnumerable<string> Alunos { get; set; }
    }
}
