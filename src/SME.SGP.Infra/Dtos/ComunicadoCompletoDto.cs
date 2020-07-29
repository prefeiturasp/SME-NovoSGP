using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;

namespace SME.SGP.Dto
{
    public class ComunicadoCompletoDto : ComunicadoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }

        public static explicit operator ComunicadoCompletoDto(Comunicado comunicado)
             => comunicado == null ? null : new ComunicadoCompletoDto
             {
                 DataEnvio = comunicado.DataEnvio,
                 Alunos = comunicado.Alunos.Select(x => (ComunicadoAlunoDto)x),
                 AlteradoEm = comunicado.AlteradoEm,
                 AlteradoPor = comunicado.AlteradoPor,
                 AlteradoRF = comunicado.AlteradoRF,
                 AnoLetivo = comunicado.AnoLetivo,
                 CodigoDre = comunicado.CodigoDre,
                 CodigoUe = comunicado.CodigoUe,
                 CriadoEm = comunicado.CriadoEm,
                 CriadoPor  = comunicado.CriadoPor,
                 CriadoRF = comunicado.CriadoRF,
                 DataExpiracao = comunicado.DataExpiracao,
                 Descricao = comunicado.Descricao,
                 AlunoEspecificado = comunicado.AlunoEspecificado,
                 Modalidade = comunicado.Modalidade ?? default,
                 Id = comunicado.Id,
                 TipoComunicado = comunicado.TipoComunicado,
                 Semestre = comunicado.Semestre ?? default,
                 Titulo = comunicado.Titulo,
                 Turmas = comunicado.Turmas.Select(x => (ComunicadoTurmaDto)x),
             };
        
    }
}