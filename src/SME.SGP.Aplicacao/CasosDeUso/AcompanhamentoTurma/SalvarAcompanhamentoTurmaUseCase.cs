using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoTurmaUseCase : AbstractUseCase, ISalvarAcompanhamentoTurmaUseCase
    {
        private const int PRIMEIRO_SEMESTRE = 1;
        private const int SEGUNDO_BIMESTRE = 2;
        private const int QUARTO_BIMESTRE = 4;
        private const int QUANTIDADE_IMAGENS_PERMITIDAS_2 = 2;
        public SalvarAcompanhamentoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AcompanhamentoTurma> Executar(AcompanhamentoTurmaDto param)
        {
            if (param.ApanhadoGeral.ExcedeuQuantidadeImagensPermitidas(QUANTIDADE_IMAGENS_PERMITIDAS_2))
                throw new NegocioException(String.Format(MensagemAcompanhamentoTurma.QUANTIDADE_DE_IMAGENS_PERMITIDAS_EXCEDIDA,QUANTIDADE_IMAGENS_PERMITIDAS_2));
            
            var acompanhamentoTurma = await MapearAcompanhamentoTurma(param);

            return acompanhamentoTurma;
        }
        private async Task<bool> TurmaEmPeridoAberto(Turma turma, int semestre)
        {
            var bimestre = semestre == PRIMEIRO_SEMESTRE ? SEGUNDO_BIMESTRE : QUARTO_BIMESTRE;
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia().Date, bimestre, true));
        }

        private async Task<AcompanhamentoTurma> MapearAcompanhamentoTurma(AcompanhamentoTurmaDto dto)
        {
            var acompanhamentoTurma = dto.AcompanhamentoTurmaId > 0 ?
                await AtualizaApanhadoTurma(dto) :
                await GerarAcompanhamentoTurma(dto);

            return acompanhamentoTurma;
        }

        private async Task<AcompanhamentoTurma> AtualizaApanhadoTurma(AcompanhamentoTurmaDto dto)
        {
            var acompanhamento = await ObterAcompanhamentoTurmaPorId(dto.AcompanhamentoTurmaId);
            await MoverRemoverExcluidos(dto, acompanhamento);
            acompanhamento.ApanhadoGeral = dto.ApanhadoGeral;
            return await mediator.Send(new SalvarAcompanhamentoTurmaCommand(acompanhamento));
        }

        private async Task MoverRemoverExcluidos(AcompanhamentoTurmaDto dto, AcompanhamentoTurma acompanhamento)
        {
            if (!string.IsNullOrEmpty(dto.ApanhadoGeral))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.AcompanhamentoAluno, acompanhamento.ApanhadoGeral, dto.ApanhadoGeral));
                dto.ApanhadoGeral = moverArquivo;
            }
            if (!string.IsNullOrEmpty(acompanhamento.ApanhadoGeral))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(acompanhamento.ApanhadoGeral, dto.ApanhadoGeral, TipoArquivo.AcompanhamentoAluno.Name()));
            }
        }

        private async Task<AcompanhamentoTurma> ObterAcompanhamentoTurmaPorId(long acompanhamentoTurmaId)
            => await mediator.Send(new ObterAcompanhamentoTurmaPorIdQuery(acompanhamentoTurmaId));

        private async Task<AcompanhamentoTurma> GerarAcompanhamentoTurma(AcompanhamentoTurmaDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(dto.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException(MensagemAcompanhamentoTurma.TURMA_NAO_ENCONTRADA);

            var periodAberto = await TurmaEmPeridoAberto(turma, dto.Semestre);
            if (!periodAberto)
                throw new NegocioException(MensagemAcompanhamentoTurma.PERIODO_NAO_ESTA_ABERTO);

            await MoverRemoverExcluidos(dto, new AcompanhamentoTurma() { ApanhadoGeral = string.Empty });
            return await mediator.Send(new GerarAcompanhamentoTurmaCommand(turma.Id, dto.Semestre, dto.ApanhadoGeral));
        }
    }
}
