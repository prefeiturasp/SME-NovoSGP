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
    }
}
