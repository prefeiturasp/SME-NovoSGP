using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class SolicitarAlteracaoComunicadoEscolaAquiCommand : IRequest<string>
    {
        public long Id { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataEnvio { get; set; }

        public DateTime? DataExpiracao { get; set; }

        public List<int> GruposId { get; set; }

        public int AnoLetivo { get; set; }

        public string SeriesResumidas { get; set; }

        public string CodigoDre { get; set; }

        public string CodigoUe { get; set; }

        public IEnumerable<string> Turmas { get; set; }
        public long? TipoCalendarioId { get; }
        public long? EventoId { get; }
        public bool AlunosEspecificados { get; set; }

        public Modalidade? Modalidade { get; set; }

        public int Semestre { get; set; }

        public IEnumerable<string> Alunos { get; set; }

        public SolicitarAlteracaoComunicadoEscolaAquiCommand(long id, DateTime dataEnvio, DateTime? dataExpiracao, string descricao, List<int> gruposId, string titulo, int anoLetivo, string seriesResumidas, string codigoDre, string codigoUe, bool alunosEspecificados, Modalidade? modalidade, int semestre, IEnumerable<string> alunos, IEnumerable<string> turmas, long? tipoCalendarioId, long? eventoId)
        {
            Id = id;
            DataEnvio = dataEnvio;
            DataExpiracao = dataExpiracao;
            Descricao = descricao;
            GruposId = gruposId;
            Titulo = titulo;
            AnoLetivo = anoLetivo;
            SeriesResumidas = seriesResumidas;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            AlunosEspecificados = alunosEspecificados;
            Modalidade = modalidade;
            Semestre = semestre;
            Alunos = alunos;
            Turmas = turmas;
            TipoCalendarioId = tipoCalendarioId;
            EventoId = eventoId;
        }
    }
}
