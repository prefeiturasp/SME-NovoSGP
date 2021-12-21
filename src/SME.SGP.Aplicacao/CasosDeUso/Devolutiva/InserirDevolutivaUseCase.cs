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
    public class InserirDevolutivaUseCase : AbstractUseCase, IInserirDevolutivaUseCase
    {
        public InserirDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirDevolutivaDto param)
        {
            IEnumerable<Tuple<long, DateTime>> dados = await mediator.Send(new ObterDatasEfetivasDiariosQuery(param.TurmaCodigo, param.CodigoComponenteCurricular, param.PeriodoInicio.Date, param.PeriodoFim.Date));

            if (!dados.Any())
                throw new NegocioException("Diários de bordo não encontrados para aplicar Devolutiva.");

            DateTime inicioEfetivo = dados.Select(x => x.Item2.Date).Min();
            DateTime fimEfetivo = dados.Select(x => x.Item2.Date).Max();

            await ValidarDevolutivaNoPeriodo(param.TurmaCodigo, param.CodigoComponenteCurricular, param.PeriodoInicio.Date, param.PeriodoFim.Date);

            var turma = await ObterTurma(param.TurmaCodigo);
            var bimestre = await ValidarBimestreDiarios(turma, inicioEfetivo, fimEfetivo);
            await ValidarBimestreEmAberto(turma, bimestre);

            IEnumerable<long> idsDiarios = dados.Select(x => x.Item1);
            await MoverRemoverExcluidos(param);
            AuditoriaDto auditoria = await mediator.Send(new InserirDevolutivaCommand(param.CodigoComponenteCurricular, idsDiarios, inicioEfetivo, fimEfetivo, param.Descricao, turma.Id));

            bool diariosAtualizados = await mediator.Send(new AtualizarDiarioBordoComDevolutivaCommand(idsDiarios, auditoria.Id));



            return auditoria;
        }
        private async  Task MoverRemoverExcluidos(InserirDevolutivaDto devolutiva)
        {
            if (!string.IsNullOrEmpty(devolutiva.Descricao))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.Devolutiva, string.Empty, devolutiva.Descricao));
                devolutiva.Descricao = moverArquivo;
            }
        }
        private async Task ValidarDevolutivaNoPeriodo(string turmaCodigo, long codigoComponenteCurricular, DateTime periodoInicio, DateTime periodoFim)
        {
            var devolutivasIds = await mediator.Send(new ObterDevolutivaPorTurmaComponenteNoPeriodoQuery(turmaCodigo, codigoComponenteCurricular, periodoInicio.Date, periodoFim.Date));
            if (devolutivasIds != null && devolutivasIds.Any())
                throw new NegocioException("Já existe devolutiva criada para o período informado");
        }

        private async Task ValidarBimestreEmAberto(Turma turma, int bimestre)
        {
            var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, turma.AnoLetivo == DateTime.Today.Year));

            if (!periodoAberto)
                throw new NegocioException("Período dos diários de bordo não esta aberto");
        }

        private async Task<int> ValidarBimestreDiarios(Turma turma, DateTime inicioEfetivo, DateTime fimEfetivo)
        {
            var bimestreInicio = await mediator.Send(new ObterBimestreAtualQuery(inicioEfetivo, turma));
            var bimestreFim = await mediator.Send(new ObterBimestreAtualQuery(fimEfetivo, turma));

            if (bimestreInicio != bimestreFim)
            {
                if (inicioEfetivo.AddDays(30) <= fimEfetivo)
                    return bimestreFim;

                throw new NegocioException("Não é possível incluir diários de bordo de bimestres diferentes");
            }

            return bimestreInicio;
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));

            if (turma == null)
                throw new NegocioException("Turma informada não localizada!");

            return turma;
        }
    }
}
