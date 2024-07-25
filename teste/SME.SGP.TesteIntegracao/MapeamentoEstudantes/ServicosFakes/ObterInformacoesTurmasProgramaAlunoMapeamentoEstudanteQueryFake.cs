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
        private const long RECUPERACAO_PARALELA_AUTORAL_PORTUGUES = 1663;

        private const long SRM = 1030;
        private const long PAEE_COLABORATIVO = 1310;
        private const long ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA = 1255;
        private const long ACOMPANHAMENTO_PEDAGOGICO_PORTUGUES = 1204;

        public ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryFake()
        { }

        public Task<InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto> Handle(ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        => Task.Run(() => new InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto()
        {
            ComponentesPAP = new List<ComponenteCurricularSimplificadoDto> 
            { 
                new() { Id = RECUPERACAO_PARALELA_AUTORAL_PORTUGUES, Descricao = RECUPERACAO_PARALELA_AUTORAL_PORTUGUES.ToString()},
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
                new() { Id = ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA, Descricao = ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA.ToString()},
                new() { Id = ACOMPANHAMENTO_PEDAGOGICO_PORTUGUES, Descricao = ACOMPANHAMENTO_PEDAGOGICO_PORTUGUES.ToString()}
            },
        });
    }
}
