using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterJustificativasAlunoPorComponenteCurricularUseCase : AbstractUseCase, IObterJustificativasAlunoPorComponenteCurricularUseCase
    {
        public ObterJustificativasAlunoPorComponenteCurricularUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<IEnumerable<JustificativaAlunoDto>> Executar(FiltroJustificativasAlunoPorComponenteCurricular dto)
        {
            var justificativas = await mediator.Send(new ObterMotivoPorTurmaAlunoComponenteCurricularQuery(dto.TurmaId, dto.ComponenteCurricularCodigo, dto.AlunoCodigo));
            MapearParaDto(justificativas);
            return justificativas;
        }

        private void MapearParaDto(IEnumerable<JustificativaAlunoDto> listaJustificativas)
        {
            foreach(var justificativa in listaJustificativas)
            {
                justificativa.Motivo = UtilRegex.RemoverTagsHtml(justificativa.Motivo);
            }
        }
    }
}
