using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoMediaRegistrosIndividuaisUseCase : AbstractUseCase, IConsolidacaoMediaRegistrosIndividuaisUseCase
    {
        public ConsolidacaoMediaRegistrosIndividuaisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroMediaRegistroIndividualTurmaDTO>();

                var alunosInfantilComRegistrosIndividuais = await mediator.Send(new ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery(filtro.AnoLetivo, filtro.TurmaId));

                if (alunosInfantilComRegistrosIndividuais != null && alunosInfantilComRegistrosIndividuais.Any())
                {
                    var mediaPorCriancaTurma = new List<MediaRegistoIndividualCriancaDTO>();
                    var totalMediasPorTurma = new List<int>();
                    foreach (var alunoRegistroIndividual in alunosInfantilComRegistrosIndividuais)
                    {
                        var registrosIndividuaisAluno = await mediator.Send(new ObterRegistrosIndividuaisPorTurmaAlunoQuery(alunoRegistroIndividual.TurmaId, alunoRegistroIndividual.AlunoCodigo));

                        if (registrosIndividuaisAluno != null && registrosIndividuaisAluno.Any())
                        {
                            var datasDeRegistro = ObterDatasDeRegistroIndividualAluno(registrosIndividuaisAluno);

                            // Media por aluno
                            var mediaEntreDatasPorAluno = ObterMediaRegistroIndividualPorAluno(alunoRegistroIndividual.TurmaId, alunoRegistroIndividual.AlunoCodigo, datasDeRegistro);

                            mediaPorCriancaTurma.Add(mediaEntreDatasPorAluno);
                        }
                    }
                    // Media Geral Turma
                    var mediaGeralPorTurma = (mediaPorCriancaTurma.Sum(m => m.Media) / alunosInfantilComRegistrosIndividuais.Count());

                    if (mediaGeralPorTurma > 0)
                        await mediator.Send(new RegistraConsolidacaoMediaRegistroIndividualCommand(alunosInfantilComRegistrosIndividuais.FirstOrDefault().TurmaId, mediaGeralPorTurma));
                }
                return true;
            }
            catch (System.Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private static List<DateTime> ObterDatasDeRegistroIndividualAluno(IEnumerable<RegistroIndividualAlunoDTO> registrosIndividuaisAluno)
        {
            var datasDeRegistro = new List<DateTime>();
            foreach (var item in registrosIndividuaisAluno)
                datasDeRegistro.Add(item.DataRegistro);
            return datasDeRegistro;
        }

        private static MediaRegistoIndividualCriancaDTO ObterMediaRegistroIndividualPorAluno(long turmaId, long alunoCodigo, List<DateTime> datasDeRegistro)
        {
            return new MediaRegistoIndividualCriancaDTO
            {
                TurmaId = turmaId,
                AlunoCodigo = alunoCodigo,
                Media = CalcularMediaRegistroIndividualPorCrianca(datasDeRegistro)
            };
        }

        private static int CalcularMediaRegistroIndividualPorCrianca(List<DateTime> datas)
        {
            if (datas.Count > 2)
                return ((int)datas.ToList().LastOrDefault().Subtract(datas.FirstOrDefault()).TotalDays / (datas.ToList().Count - 1));
            else
                return (int)datas.LastOrDefault().Subtract(datas.FirstOrDefault()).TotalDays / datas.Count;
        }
    }
}
