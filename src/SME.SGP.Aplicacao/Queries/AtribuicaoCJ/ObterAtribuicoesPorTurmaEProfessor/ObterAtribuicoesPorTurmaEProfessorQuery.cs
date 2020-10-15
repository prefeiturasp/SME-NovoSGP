using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesPorTurmaEProfessorQuery : IRequest<IEnumerable<AtribuicaoCJ>>
    {
        public ObterAtribuicoesPorTurmaEProfessorQuery(Modalidade? modalidade, string turmaId, string ueId, long componenteCurricularId, string usuarioRf, string usuarioNome, bool? substituir, string dreCodigo = "", string[] turmaIds = null, int? anoLetivo = null)
        {
            Modalidade = modalidade;
            TurmaId = turmaId;
            UeId = ueId;
            ComponenteCurricularId = componenteCurricularId;
            UsuarioRf = usuarioRf;
            UsuarioNome = usuarioNome;
            Substituir = substituir;
            DreCodigo = dreCodigo;
            TurmaIds = turmaIds;
            AnoLetivo = anoLetivo;
        }

        public Modalidade? Modalidade { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string UsuarioRf { get; set; }
        public string UsuarioNome { get; set; }
        public bool? Substituir { get; set; }
        public string DreCodigo { get; set; }
        public string[] TurmaIds { get; set; }
        public int? AnoLetivo { get; set; }
    }
}
