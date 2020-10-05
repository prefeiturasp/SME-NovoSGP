using MediatR;
using SME.SGP.Dominio;
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

        public List<int> GruposId { get; set; }

        public int AnoLetivo { get; set; }

        public string CodigoDre { get; set; }

        public string CodigoUe { get; set; }

        public IEnumerable<string> Turmas { get; set; }

        public bool AlunosEspecificados { get; set; }

        public Modalidade? Modalidade { get; set; }

        public int Semestre { get; set; }

        public IEnumerable<string> Alunos { get; set; }

        public SolicitarInclusaoComunicadoEscolaAquiCommand(DateTime dataEnvio, DateTime? dataExpiracao, string descricao, List<int> gruposId, string titulo, int anoLetivo, string codigoDre, string codigoUe, bool alunosEspecificados, Modalidade? modalidade, int semestre, IEnumerable<string> turmas, IEnumerable<string> alunos)
        {
            DataEnvio = dataEnvio;
            DataExpiracao = dataExpiracao;
            Descricao = descricao;
            GruposId = gruposId;
            Titulo = titulo;
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            AlunosEspecificados = alunosEspecificados;
            Modalidade = modalidade;
            Semestre = semestre;
            Turmas = turmas;
            Alunos = alunos;
        }
    }
}
