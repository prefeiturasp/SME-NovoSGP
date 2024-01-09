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
        private const int ANO_LETIVO_INICIO_DEVOLUTIVA_UNIFICADA = 2024;
        private readonly IConsultasDisciplina consultasDisciplina;

        public InserirDevolutivaUseCase(IMediator mediator,
                                        IConsultasDisciplina consultasDisciplina) : base(mediator)
        {
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<AuditoriaDto> Executar(InserirDevolutivaDto param)
        {
            IEnumerable<(long Id, DateTime DataAula)> dados = await mediator.Send(new ObterDatasEfetivasDiariosQuery(param.TurmaCodigo, new long[] { param.CodigoComponenteCurricular }, param.PeriodoInicio.Date, param.PeriodoFim.Date));

            if (!dados.Any())
                throw new NegocioException("Diários de bordo não encontrados para aplicar Devolutiva.");

            DateTime inicioEfetivo = dados.Select(x => x.DataAula.Date).Min();
            DateTime fimEfetivo = dados.Select(x => x.DataAula.Date).Max();

            await ValidarDevolutivaNoPeriodo(param.TurmaCodigo, param.CodigoComponenteCurricular, param.PeriodoInicio.Date, param.PeriodoFim.Date);

            var turma = await ObterTurma(param.TurmaCodigo);
            var bimestre = await ValidarBimestreDiarios(turma, inicioEfetivo, fimEfetivo);
            await ValidarBimestreEmAberto(turma, bimestre);

            IEnumerable<long> idsDiarios = dados.Select(x => x.Id);
            await MoverRemoverExcluidos(param);
            AuditoriaDto auditoria = await mediator.Send(new InserirDevolutivaCommand(param.CodigoComponenteCurricular, idsDiarios, inicioEfetivo, fimEfetivo, param.Descricao, turma.Id));

            var idsDiariosAtualizacao = await ObterIdDiarioUnificado(idsDiarios, turma, param.CodigoComponenteCurricular, param.PeriodoInicio.Date, param.PeriodoFim.Date);
            await mediator.Send(new AtualizarDiarioBordoComDevolutivaCommand(idsDiariosAtualizacao, auditoria.Id));

            var filtro = new FiltroExclusaoPendenciasDevolutivaDto()
            { 
                TurmaId = turma.Id,
                ComponenteId = param.CodigoComponenteCurricular
            };

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasDevolutiva, filtro, Guid.NewGuid()));

            return auditoria;
        }

        private async Task<IEnumerable<long>> ObterIdDiarioUnificado(IEnumerable<long> idsDiarios, Turma turma, long codigoComponente, DateTime periodoInicio, DateTime periodoFim)
        {
            if (turma.AnoLetivo >= ANO_LETIVO_INICIO_DEVOLUTIVA_UNIFICADA)
            {
                var retorno = new List<long>();
                var disciplinas = await ObterComponentesCurricularesTurma(turma.CodigoTurma, codigoComponente);
                var dados = await mediator.Send(new ObterDatasEfetivasDiariosQuery(turma.CodigoTurma, disciplinas.Select(x => x.CodigoComponenteCurricular).ToArray(), periodoInicio, periodoFim));

                retorno.AddRange(idsDiarios);
                retorno.AddRange(dados.Select(x => x.Id));

                return retorno;
            }

            return idsDiarios;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesTurma(string turmaCodigo, long componentePai)
        {
            var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(turmaCodigo, false);

            return disciplinas.FindAll(item => item.CdComponenteCurricularPai == componentePai && item.CodigoComponenteCurricular != componentePai);
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
            if (devolutivasIds.NaoEhNulo() && devolutivasIds.Any())
                throw new NegocioException("Já existe devolutiva criada para o período informado");
        }

        private async Task ValidarBimestreEmAberto(Turma turma, int bimestre)
        {
            var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, turma.AnoLetivo == DateTime.Today.Year));

            if (!periodoAberto)
                throw new NegocioException("Apenas é possível consultar este registro pois o período não está em aberto.");
        }

        private async Task<int> ValidarBimestreDiarios(Turma turma, DateTime inicioEfetivo, DateTime fimEfetivo)
        {
            var bimestreInicio = await mediator.Send(new ObterBimestreAtualQuery(inicioEfetivo, turma));
            var bimestreFim = await mediator.Send(new ObterBimestreAtualQuery(fimEfetivo, turma));

            if (bimestreInicio != bimestreFim && bimestreFim > 2)
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

            if (turma.EhNulo())
                throw new NegocioException("Turma informada não localizada!");

            return turma;
        }
    }
}
