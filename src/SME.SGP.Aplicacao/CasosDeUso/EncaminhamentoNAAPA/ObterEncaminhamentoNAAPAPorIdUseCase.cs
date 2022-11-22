using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAPorIdUseCase : IObterEncaminhamentoNAAPAPorIdUseCase
    {
        private readonly IMediator mediator;

        public ObterEncaminhamentoNAAPAPorIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<EncaminhamentoNAAPARespostaDto> Executar(long id)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAComTurmaPorIdQuery(id));

            if(encaminhamentoNAAPA == null)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(encaminhamentoNAAPA.AlunoCodigo, encaminhamentoNAAPA.Turma.AnoLetivo, true));

            return new EncaminhamentoNAAPARespostaDto()
            {
                Aluno = aluno,
                
                DreId = encaminhamentoNAAPA.Turma.Ue.Dre.Id,
                DreCodigo = encaminhamentoNAAPA.Turma.Ue.Dre.CodigoDre,
                DreNome = encaminhamentoNAAPA.Turma.Ue.Dre.Nome,
                
                UeId = encaminhamentoNAAPA.Turma.Ue.Id,
                UeCodigo = encaminhamentoNAAPA.Turma.Ue.CodigoUe,
                UeNome = encaminhamentoNAAPA.Turma.Ue.Nome,
                
                TurmaId = encaminhamentoNAAPA.Turma.Id,
                TurmaCodigo = encaminhamentoNAAPA.Turma.CodigoTurma,
                TurmaNome = encaminhamentoNAAPA.Turma.Nome,
                
                Anoletivo = encaminhamentoNAAPA.Turma.AnoLetivo,
            };
        }
    }
}
