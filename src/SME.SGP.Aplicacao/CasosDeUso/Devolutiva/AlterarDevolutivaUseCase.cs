using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDevolutivaUseCase : AbstractUseCase, IAlterarDevolutivaUseCase
    {
        public AlterarDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AlterarDevolutivaDto param)
        {
            Devolutiva devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(param.Id));
            if (devolutiva == null)
                throw new NegocioException("Devolutiva informada não existe");

            var turma = await ObterTurma(param.TurmaCodigo);
            var bimestre = await mediator.Send(new ObterBimestreAtualQuery(DateTime.Today, turma));
            await ValidarBimestreEmAberto(turma, bimestre);

            await MoverRemoverExcluidos(param, devolutiva);

            devolutiva.Descricao = param.Descricao;
            return await mediator.Send(new AlterarDevolutivaCommand(devolutiva));
        }
        private async Task MoverRemoverExcluidos(AlterarDevolutivaDto alterarDevolutivaDto, Devolutiva devolutiva)
        {
            if (!string.IsNullOrEmpty(alterarDevolutivaDto.Descricao))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.Devolutiva, devolutiva.Descricao, alterarDevolutivaDto.Descricao));
                alterarDevolutivaDto.Descricao = moverArquivo;
            }
            if (!string.IsNullOrEmpty(devolutiva.Descricao))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(devolutiva.Descricao, alterarDevolutivaDto.Descricao, TipoArquivo.Devolutiva.Name()));
            }
        }
        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

            if (turma == null)
                throw new NegocioException("Turma informada não localizada!");

            return turma;
        }

        private async Task ValidarBimestreEmAberto(Turma turma, int bimestre)
        {
            var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, turma.AnoLetivo == DateTime.Today.Year));

            if (!periodoAberto)
                throw new NegocioException("Período dos diários de bordo não esta aberto");
        }

    }
}
