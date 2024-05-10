using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesPorTurmaEProfessorQuery : IRequest<IEnumerable<AtribuicaoCJ>>
    {
        public ObterAtribuicoesPorTurmaEProfessorQuery(AtribuicoesPorTurmaEProfessorDto dto)
        {
            Modalidade = dto.Modalidade;
            TurmaId = dto.TurmaId;
            UeId = dto.UeId;
            ComponenteCurricularId = dto.ComponenteCurricularId;
            UsuarioRf = dto.UsuarioRf;
            UsuarioNome = dto.UsuarioNome;
            Substituir = dto.Substituir;
            DreCodigo = dto.DreCodigo;
            TurmaIds = dto.TurmaIds;
            AnoLetivo = dto.AnoLetivo;
            Historico = dto.Historico;
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
        public int AnoLetivo { get; set; }
        public bool? Historico { get; set; }
    }
}
