using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade
{
    public class ObterComponentesCurricularesPorAnosEModalidadeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorAnosEModalidadeQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IServicoEol servicoEol;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasDisciplina consultasDisciplina;

        public ObterComponentesCurricularesPorAnosEModalidadeQueryHandler(IServicoEol servicoEol, 
                                                                          IConsultasAbrangencia consultasAbrangencia,
                                                                          IConsultasDisciplina consultasDisciplina)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new System.ArgumentNullException(nameof(consultasAbrangencia));
            this.consultasDisciplina = consultasDisciplina ?? throw new System.ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorAnosEModalidadeQuery request, CancellationToken cancellationToken)
        {
            var componentes = (await servicoEol.ObterComponentesCurricularesPorAnosEModalidade(request.CodigoUe, request.Modalidade, request.AnosEscolares, request.AnoLetivo))?.ToList();
            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Nenhum componente localizado para a modalidade e anos informados.");
            }

            if(componentes.Any(componentes  => componentes.Regencia))
            {
                var turmasAbrangencia = await consultasAbrangencia.ObterTurmas(request.CodigoUe, request.Modalidade);

                var turmasNosAnos = turmasAbrangencia.Where(t => request.AnosEscolares.Contains(t.Ano) && t.AnoLetivo == request.AnoLetivo).DistinctBy(c => c.Ano);

                var lstComponentesRegencia = new List<DisciplinaResposta>();

                foreach (var regencia in componentes.Where(componentes => componentes.Regencia))
                {
                    var turmaNoAno = turmasNosAnos.FirstOrDefault(t => t.Ano == regencia.AnoTurma);

                    var componentesRegencia = await consultasDisciplina.ObterComponentesRegencia(ConverterParaTurma(turmaNoAno, request.CodigoUe), regencia.Codigo);

                    if (componentesRegencia != null || componentesRegencia.Any())
                        lstComponentesRegencia.AddRange(componentesRegencia);
                }

                if (lstComponentesRegencia != null && lstComponentesRegencia.Any())
                    componentes.AddRange(MapearParaComponenteCurricularDto(lstComponentesRegencia));
            }

            componentes = componentes.OrderBy(c => c.Descricao).ToList();
            componentes.Insert(0, new ComponenteCurricularEol
            {
                Codigo = -99,
                Descricao = "Todos"
            });
            return componentes;
        }

        private IEnumerable<ComponenteCurricularEol> MapearParaComponenteCurricularDto(IEnumerable<DisciplinaResposta> respostaEol)
        {
            foreach (var item in respostaEol)
            {
                yield return new ComponenteCurricularEol()
                {
                  
                };
            }
        }

        private Turma ConverterParaTurma(AbrangenciaTurmaRetorno turmaAbrangencia, string codigoUe)
        {
            return new Turma()
            {
                CodigoTurma = turmaAbrangencia.Codigo,
                ModalidadeCodigo = (Modalidade)turmaAbrangencia.CodigoModalidade,
                Ue = new Ue() { CodigoUe = codigoUe }
            };
        }
    }
}
