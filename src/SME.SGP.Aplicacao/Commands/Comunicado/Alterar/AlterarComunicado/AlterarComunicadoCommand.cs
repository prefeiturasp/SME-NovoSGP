using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AlterarComunicadoCommand : IRequest<bool>
    {
        public AlterarComunicadoCommand(long id, ComunicadoAlterarDto comunicado)
        {
            Id = id;
            Titulo = comunicado.Titulo;
            Descricao = comunicado.Descricao;
            DataEnvio = comunicado.DataEnvio;
            DataExpiracao = comunicado.DataExpiracao;
            AnoLetivo = comunicado.AnoLetivo;
            SeriesResumidas = comunicado.SeriesResumidas;
            CodigoDre = comunicado.CodigoDre;
            CodigoUe = comunicado.CodigoUe;
            Turmas = comunicado.Turmas;
            TipoCalendarioId = comunicado.TipoCalendarioId;
            EventoId = comunicado.EventoId;
            AlunosEspecificados = comunicado.AlunosEspecificados;
            Modalidades = comunicado.Modalidades;
            Semestre = comunicado.Semestre;
            Alunos = comunicado.Alunos;
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
