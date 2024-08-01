using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryFake : IRequestHandler<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery, InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto>
    {
        private const long SRM = 1030;
        private const long PAEE_COLABORATIVO = 1310;

        public ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryFake()
        { }

        public Task<InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto> Handle(ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        => Task.Run(() => new InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto()
        {
            ComponentesPAP = new List<ComponenteCurricularSimplificadoDto> 
            { 
                new() { Id = ComponentesCurricularesConstants.CODIGO_PAP_RECUPERACAO_APRENDIZAGENS, Descricao = "Contraturno"},
                new() { Id = ComponentesCurricularesConstants.CODIGO_PAP_PROJETO_COLABORATIVO, Descricao = "Colaborativo"}
            },
            ComponentesSRMCEFAI = new List<ComponenteCurricularSimplificadoDto>
            {
                new() { Id = SRM, Descricao = SRM.ToString()},
                new() { Id = PAEE_COLABORATIVO, Descricao = PAEE_COLABORATIVO.ToString()}
            },
            ComponentesFortalecimentoAprendizagens = new List<ComponenteCurricularSimplificadoDto>
            {
                new() { Id = ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_PORTUGUES, Descricao = ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_PORTUGUES.ToString()},
                new() { Id = ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_MATEMATICA, Descricao = ComponentesCurricularesConstants.CODIGO_RECUPERACAO_PARALELA_AUTORAL_MATEMATICA.ToString()},
            }            
        });
    }
}
