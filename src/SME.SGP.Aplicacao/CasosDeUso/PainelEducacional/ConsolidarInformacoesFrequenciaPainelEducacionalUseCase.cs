using MediatR;
using Microsoft.Win32;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesFrequenciaPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesFrequenciaPainelEducacionalUseCase
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ConsolidarInformacoesFrequenciaPainelEducacionalUseCase(IMediator mediator, IRepositorioFrequenciaConsulta repositorioFrequencia) : base(mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registrosFrequencia = await repositorioFrequencia.ObterInformacoesFrequenciaPainelEducacional(DateTime.Now.Year);

            var outroMes = registrosFrequencia.Where(f => f.Mes != 2)?.ToList();

            var agrupadoMensal = registrosFrequencia
                    .GroupBy(r => new { r.Mes, r.ModalidadeCodigo })
                    .Select(g => new
                    {
                        Mes = g.Key.Mes,
                        Modalidade = g.Key.ModalidadeCodigo,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        TotalCompensacoes = g.Sum(x => x.QuantidadeCompensacoes),
                        MediaPercentual = g.Average(x => x.Percentual),
                        Alunos = g.Select(x => x.CodigoAluno).Distinct().ToList(),
                    })
                    .OrderBy(x => x.Modalidade)
                    .ThenBy(x => x.Mes).ToList();

            var agrupadoGlobal = registrosFrequencia
                  .GroupBy(r => new { r.ModalidadeCodigo })
                  .Select(g => new
                  {
                      Modalidade = g.Key.ModalidadeCodigo,
                      TotalAulas = g.Sum(x => x.QuantidadeAulas),
                      TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                      TotalCompensacoes = g.Sum(x => x.QuantidadeCompensacoes),
                      MediaPercentual = g.Average(x => x.Percentual),
                      Alunos = g.Select(x => x.CodigoAluno).Distinct().ToList()
                  })
                  .OrderBy(x => x.Modalidade).ToList();

            var agrupadoEscola = registrosFrequencia
                    .GroupBy(r => new { r.CodigoUe })
                    .Select(g => new
                    {
                        CodigoUe = g.Key.CodigoUe,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        TotalCompensacoes = g.Sum(x => x.QuantidadeCompensacoes),
                        MediaPercentual = g.Average(x => x.Percentual),
                        Alunos = g.Select(x => x.CodigoAluno).Distinct().ToList(),
                        UE = g.Select(x => x.Ue).Distinct().ToList()
                    })
                    .OrderBy(x => x.CodigoUe).ToList();

            return true;
        }
    }
}
