using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarFechamentoTurmaEdFisica2020UseCase : AbstractUseCase, IGerarFechamentoTurmaEdFisica2020UseCase
    {
        public const int ANO_LETIVO_TURMAS_ED_FISICA = 2020;
        public GerarFechamentoTurmaEdFisica2020UseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            string codigoRfAluno = mensagem.Mensagem?.ToString() ?? "";
            if (await ParametroGeracaoFechamentoTurmasEdFisica2020Ativo())
            {
                var dadosEolAlunoTurma = await mediator.Send(ObterAlunosEdFisica2020Query.Instance);
                if (String.IsNullOrEmpty(codigoRfAluno))
                    return await TratarGeracaoFechamentoEdFisica2020PorTurma(dadosEolAlunoTurma);
                return await TratarGeracaoFechamentoEdFisica2020PorAluno(dadosEolAlunoTurma, codigoRfAluno);
            }
            return false;
        }

        private async Task<bool> TratarGeracaoFechamentoEdFisica2020PorAluno(IEnumerable<FechamentoAlunoComponenteDTO> fechamentosAluno, string codigoAluno)
        {
            var fechamentosAlunoTurma = fechamentosAluno.FirstOrDefault(d => d.CodigoAluno.ToString().Equals(codigoAluno));

            if (fechamentosAlunoTurma.NaoEhNulo())
            {
                var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = fechamentosAlunoTurma.CodigoTurma.ToString() });
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaGeracaoFechamentoEdFisica2020AlunosTurma,
                                                                new { TurmaId = dadosTurma.Id, 
                                                                      CodigoAlunos = new long[] { fechamentosAlunoTurma.CodigoAluno } }, 
                                                                      Guid.NewGuid(), null));
            }
            return true;
        }

        private async Task<bool> TratarGeracaoFechamentoEdFisica2020PorTurma(IEnumerable<FechamentoAlunoComponenteDTO> fechamentosAluno)
        {
            var fechamentosAlunoTurma = fechamentosAluno.GroupBy(d => d.CodigoTurma);

            foreach (var alunosTurma in fechamentosAlunoTurma)
            {
                var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = alunosTurma.Key.ToString() });
                if (dadosTurma.NaoEhNulo() && dadosTurma.TipoTurma == TipoTurma.EdFisica && dadosTurma.AnoLetivo == ANO_LETIVO_TURMAS_ED_FISICA)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaGeracaoFechamentoEdFisica2020AlunosTurma,
                                                                    new { TurmaId = dadosTurma.Id, 
                                                                          CodigoAlunos = alunosTurma.Select(a => a.CodigoAluno).ToArray() }, 
                                                                          Guid.NewGuid(), null));
            }

            return true;
        }

        private async Task<bool> ParametroGeracaoFechamentoTurmasEdFisica2020Ativo()
        {
            var valorParametro = await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.GerarFechamentoTurmasEdFisica2020));
            return (valorParametro.NaoEhNulo() && valorParametro.Equals("true"));
        }
    }
}
