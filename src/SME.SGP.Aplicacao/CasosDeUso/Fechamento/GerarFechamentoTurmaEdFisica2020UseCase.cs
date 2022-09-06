﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string codigoRfAluno = mensagem.Mensagem != null ? mensagem.Mensagem.ToString() : "";
            var valorParametro = await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.GerarFechamentoTurmasEdFisica2020));
            
            if(valorParametro != null)
            {
                if (valorParametro.Equals("true"))
                {
                    var dadosEolAlunoTurma = await mediator.Send(new ObterAlunosEdFisica2020Query());

                    if (String.IsNullOrEmpty(codigoRfAluno))
                    {
                        var dadosAgrupadosPorTurma = dadosEolAlunoTurma.GroupBy(d => d.CodigoTurma);

                        foreach (var alunosTurma in dadosAgrupadosPorTurma)
                        {
                            var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = alunosTurma.Key.ToString() });
                            if (dadosTurma != null && dadosTurma.TipoTurma == TipoTurma.EdFisica && dadosTurma.AnoLetivo == ANO_LETIVO_TURMAS_ED_FISICA)
                                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaGeracaoFechamentoEdFisica2020AlunosTurma,
                                    new { TurmaId = dadosTurma.Id, CodigoAlunos = alunosTurma.Select(a => a.CodigoAluno).ToArray() }, Guid.NewGuid(), null));
                        }
                    }
                    else
                    {
                        var dadosAlunoEdFisica = dadosEolAlunoTurma.Where(d => d.CodigoAluno.ToString().Equals(codigoRfAluno)).FirstOrDefault();
                       
                        if (dadosAlunoEdFisica != null)
                        {
                            var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = dadosAlunoEdFisica.CodigoTurma.ToString() });
                            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaGeracaoFechamentoEdFisica2020AlunosTurma,
                                                    new { TurmaId = dadosTurma.Id, CodigoAlunos = new long[] { dadosAlunoEdFisica.CodigoAluno } }, Guid.NewGuid(), null));
                        }
                            
                    }

                    return true;
                }
                
            }

            return false;
        }
    }
}
