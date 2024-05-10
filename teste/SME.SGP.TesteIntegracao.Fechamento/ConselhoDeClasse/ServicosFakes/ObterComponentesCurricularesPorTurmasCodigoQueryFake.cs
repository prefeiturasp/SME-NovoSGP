using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesPorTurmasCodigoQueryFake: IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>
    {
        private const string TURMA_CODIGO_1 = "1";
        private const string MATEMATICA = "MATEMATICA";
        private const string EDUCACAO_FISICA = "ED. FISICA";
        private const string HISTORIA = "HISTORIA";
        private const string GEOGRAFIA = "GEOGRAFIA";
        private const string INGLES = "INGLES";
        private const string CIENCIAS = "CIENCIAS";
        private const string LINGUA_PORTUGUESA = "LINGUA PORTUGUESA";
        private const string ARTE = "ARTE";
        private const string INFORMATICA_OIE = "INFORMATICA - OIE";
        private const string LEITURA_OSL = "LEITURA - OSL";
        private const string CULTURAS_ARTE_E_MEMORIA_PERCUSSAO = "CULTURAS, ARTE E MEMÓRIA - PERCUSSÃO";
        private const string CULTURAS_ARTE_E_MEMORIA_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO = "CULTURAS, ARTE E MEMÓRIA - OUTRAS, DE ACORDO COM O PROJETO POLITICO - PEDAGOGICO";
        private const string ORIENTACAO_DE_ESTUDOS_E_INVENCAO_CRIATIVA_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO = "ORIENTAÇÃO DE ESTUDOS E INVENÇÃO CRIATIVA - OUTRAS, DE ACORDO COM O PROJETO POLITICO - PEDAGOGICO";
        private const string CONSCIENCIA_E_SUSTENTABILIDADE_SOCIOAMBIENTAL_ECON_SOLIDARIA_ED_FINAN_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO = "CONSCIÊNCIA E SUSTENTABILIDADE SOCIOAMBIENTAL,ECON SOLIDARIA ED FINAN - OUTRAS, DE ACORDO COM O PROJETO POLITICO - PEDAGOGICO";
        private const string ETICA_CONVIVENCIA_E_PROTAGONISMOS_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO = "ÉTICA, CONVIVÊNCIA E PROTAGONISMOS - OUTRAS, DE ACORDO COM O PROJETO POLITICO - PEDAGOGICO";
        
        public ObterComponentesCurricularesPorTurmasCodigoQueryFake()
        {
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<DisciplinaDto>()
            {
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 2,
                    CdComponenteCurricularPai = 0,
                    Nome = MATEMATICA,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 6,
                    CdComponenteCurricularPai = 0,
                    Nome = EDUCACAO_FISICA,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 7,
                    CdComponenteCurricularPai = 0,
                    Nome = HISTORIA,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 8,
                    CdComponenteCurricularPai = 0,
                    Nome = GEOGRAFIA,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 9,
                    CdComponenteCurricularPai = 0,
                    Nome = INGLES,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 89,
                    CdComponenteCurricularPai = 0,
                    Nome = CIENCIAS,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 138,
                    CdComponenteCurricularPai = 0,
                    Nome = LINGUA_PORTUGUESA,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 139,
                    CdComponenteCurricularPai = 0,
                    Nome = ARTE,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 1060,
                    CdComponenteCurricularPai = 0,
                    Nome = INFORMATICA_OIE,
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 1061,
                    CdComponenteCurricularPai = 0,
                    Nome = LEITURA_OSL,
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 13260119,//23579783260119
                    CdComponenteCurricularPai = null,
                    Nome = CULTURAS_ARTE_E_MEMORIA_PERCUSSAO,
                    TerritorioSaber = true,
                    LancaNota = true
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 131370119,//235797831370119
                    CdComponenteCurricularPai = null,
                    Nome = CULTURAS_ARTE_E_MEMORIA_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO,
                    TerritorioSaber = true,
                    LancaNota = true
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 141370119,//235797841370119
                    CdComponenteCurricularPai = null,
                    Nome = ORIENTACAO_DE_ESTUDOS_E_INVENCAO_CRIATIVA_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO,
                    TerritorioSaber = true,
                    LancaNota = true
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 151370119,//235797851370119
                    CdComponenteCurricularPai = null,
                    Nome = CONSCIENCIA_E_SUSTENTABILIDADE_SOCIOAMBIENTAL_ECON_SOLIDARIA_ED_FINAN_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO,
                    TerritorioSaber = true,
                    LancaNota = true
                },
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 161370119,//235797861370119
                    CdComponenteCurricularPai = null,
                    Nome = ETICA_CONVIVENCIA_E_PROTAGONISMOS_OUTRAS_DE_ACORDO_COM_O_PROJETO_POLITICO_PEDAGOGICO,
                    TerritorioSaber = true,
                    LancaNota = true
                },
            });
        }

    }
}