using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AcompanhamentoAlunoSemestreAuditoriaDto
    {
        public long AcompanhamnetoAlunoId { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }
        public AuditoriaDto Auditoria { get; set; }

        public static explicit operator AcompanhamentoAlunoSemestreAuditoriaDto(AcompanhamentoAlunoSemestre acompanhamento)
            => acompanhamento == null ? null :
            new AcompanhamentoAlunoSemestreAuditoriaDto()
            {
                AcompanhamnetoAlunoId = acompanhamento.AcompanhamentoAlunoId,
                AcompanhamentoAlunoSemestreId = acompanhamento.Id,
                Auditoria = (AuditoriaDto)acompanhamento
            };
    }
}
