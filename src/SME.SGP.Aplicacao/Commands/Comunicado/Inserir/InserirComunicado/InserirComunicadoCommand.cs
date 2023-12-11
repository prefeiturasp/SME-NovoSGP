using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoCommand : IRequest<bool>
    {
        public InserirComunicadoCommand(ComunicadoInserirDto comunicado)
        {
            Titulo = comunicado.Titulo;
            Descricao = comunicado.Descricao;
            DataEnvio = comunicado.DataEnvio;
            DataExpiracao = comunicado.DataExpiracao;
            AnoLetivo = comunicado.AnoLetivo;
            CodigoDre = comunicado.CodigoDre;
            CodigoUe = comunicado.CodigoUe;
            Turmas = comunicado.Turmas;
            AlunoEspecificado = comunicado.AlunoEspecificado;
            Modalidades = comunicado.Modalidades;
            Semestre = comunicado.Semestre;
            Alunos = comunicado.Alunos;
            SeriesResumidas = comunicado.SeriesResumidas;
            TipoCalendarioId = comunicado.TipoCalendarioId;
            EventoId = comunicado.EventoId;
            TiposEscolas = comunicado.TiposEscolas;
            AnosEscolares = comunicado.AnosEscolares;
        }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IEnumerable<string> Turmas { get; set; }
        public bool AlunoEspecificado { get; set; }
        public int[] Modalidades { get; set; }
        public int[] TiposEscolas { get; set; }
        public string[] AnosEscolares { get; set; }
        public int Semestre { get; set; }
        public IEnumerable<string> Alunos { get; set; }
        public string SeriesResumidas { get; set; }
        public long? TipoCalendarioId { get; set; }
        public long? EventoId { get; set; }
    }
}
