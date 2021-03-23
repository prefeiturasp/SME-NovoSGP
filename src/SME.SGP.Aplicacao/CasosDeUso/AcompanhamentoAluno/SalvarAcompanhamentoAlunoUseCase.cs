using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoAlunoUseCase : AbstractUseCase, ISalvarAcompanhamentoAlunoUseCase
    {
        public SalvarAcompanhamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AcompanhamentoAlunoDto dto)
        {
            var acompanhamentoSemestre = await MapearAcompanhamentoSemestre(dto);

            return (AuditoriaDto)acompanhamentoSemestre;
        }

        private async Task<AcompanhamentoAlunoSemestre> MapearAcompanhamentoSemestre(AcompanhamentoAlunoDto dto)
        {
            var acompanhamentoSemestre = dto.AcompanhamentoAlunoSemestreId > 0 ?
                await AtualizaObservacoesAcompanhamento(dto.AcompanhamentoAlunoSemestreId, dto.Observacoes) :
                await GerarAcompanhamentoSemestre(dto);

            return acompanhamentoSemestre;
        }

        private async Task<AcompanhamentoAlunoSemestre> AtualizaObservacoesAcompanhamento(long acompanhamentoAlunoSemestreId, string observacoes)
        {
            var acompanhamento = await ObterAcompanhamentoSemestrePorId(acompanhamentoAlunoSemestreId);
            acompanhamento.Observacoes = observacoes;

            return await mediator.Send(new SalvarAcompanhamentoAlunoSemestreCommand(acompanhamento));
        }

        private async Task<AcompanhamentoAlunoSemestre> ObterAcompanhamentoSemestrePorId(long acompanhamentoAlunoSemestreId)
            => await mediator.Send(new ObterAcompanhamentoAlunoSemestrePorIdQuery(acompanhamentoAlunoSemestreId));

        private async Task<AcompanhamentoAlunoSemestre> GerarAcompanhamentoSemestre(AcompanhamentoAlunoDto dto)
        {
            var acompanhamentoAlunoId = dto.AcompanhamentoAlunoId > 0 ?
                dto.AcompanhamentoAlunoId :
                await mediator.Send(new GerarAcompanhamentoAlunoCommand(dto.TurmaId, dto.AlunoCodigo));

            return await mediator.Send(new GerarAcompanhamentoAlunoSemestreCommand(acompanhamentoAlunoId, dto.Semestre, dto.Observacoes));
        }
    }
}
