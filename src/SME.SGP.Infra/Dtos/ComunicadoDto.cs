using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dto
{
    public class ComunicadoDto
    {
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public List<GrupoComunicacaoDto> Grupos { get; set; }
        public long Id { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public int[] Modalidades { get; set; }
        public int[] TiposEscolas { get; set; }
        public int Semestre { get; set; }
        public TipoComunicado TipoComunicado { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public bool AlunoEspecificado { get; set; }
        public IEnumerable<ComunicadoTurmaDto> Turmas { get; set; }
        public IEnumerable<ComunicadoAlunoDto> Alunos { get; set; }
        public long? TipoCalendarioId { get; set; }
        public long? EventoId { get; set; }
        public string[] AnosEscolares { get; set; }

        public bool EmEdicao { get; private set; } = true;

        public static explicit operator ComunicadoDto(Comunicado comunicado)
        => comunicado.EhNulo() ? null : new ComunicadoDto
        {
            AnoLetivo = comunicado.AnoLetivo,
            Turmas = comunicado.Turmas.Select(x => (ComunicadoTurmaDto)x),
            Alunos = comunicado.Alunos.Select(x => (ComunicadoAlunoDto)x),
            CodigoDre = comunicado.CodigoDre,
            CodigoUe = comunicado.CodigoUe,
            DataEnvio = comunicado.DataEnvio,
            DataExpiracao = comunicado.DataExpiracao,
            Descricao = comunicado.Descricao,
            Id = comunicado.Id,
            Modalidades = comunicado.Modalidades,
            Semestre = comunicado.Semestre ?? default,
            TipoComunicado = comunicado.TipoComunicado,
            Titulo = comunicado.Titulo,
            AlunoEspecificado = comunicado.AlunoEspecificado,
            TipoCalendarioId = comunicado.TipoCalendarioId,
            EventoId = comunicado.EventoId,
            AnosEscolares = comunicado.AnosEscolares,
            EmEdicao = true
        };
    }
}