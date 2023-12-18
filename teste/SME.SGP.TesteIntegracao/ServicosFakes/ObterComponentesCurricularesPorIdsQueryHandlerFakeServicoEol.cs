using MediatR;
using Nest;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterComponentesCurricularesPorIdsQueryHandlerFakeServicoEol : IRequestHandler<ObterComponentesCurricularesPorIdsQuery, IEnumerable<DisciplinaDto>>
    {
        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<DisciplinaDto>
            {
                new DisciplinaDto
                {
                    Id = 1217,
                    CodigoComponenteCurricular = 1217,
                    GrupoMatrizId = 4,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 1",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 1",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 138,
                    CodigoComponenteCurricular = 138,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 2",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = true,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 6,
                    CodigoComponenteCurricular = 6,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 3",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"

                },
                new DisciplinaDto
                {
                    Id = 2,
                    CodigoComponenteCurricular = 2,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 4",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"

                },
                new DisciplinaDto
                {
                    Id = 7,
                    CodigoComponenteCurricular = 7,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 5",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"

                },
                new DisciplinaDto
                {
                    Id = 139,
                    CodigoComponenteCurricular = 139,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 6",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1105,
                    CodigoComponenteCurricular = 1105,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 7",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = true,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1114,
                    CodigoComponenteCurricular = 1114,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 8",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = true,
                    RegistraFrequencia = true,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1061,
                    CodigoComponenteCurricular = 1061,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Leitura - OSL",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = true,
                    RegistraFrequencia = true,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 512,
                    CodigoComponenteCurricular = 512,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Regência de Classe Infantil",
                    NomeComponenteInfantil = "REGÊNCIA INFANTIL EMEI 4H",
                    Regencia = true,
                    RegistraFrequencia = true,
                    GrupoMatrizNome = "Base Nacional Comum",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 513,
                    CodigoComponenteCurricular = 513,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Regência de Classe Infantil",
                    NomeComponenteInfantil = "REGÊNCIA INFANTIL EMEI 2H",
                    Regencia = true,
                    RegistraFrequencia = true,
                    GrupoMatrizNome = "Base Nacional Comum",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1214,
                    CodigoComponenteCurricular = 1214,
                    GrupoMatrizId = 4,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 11",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 11",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 89,
                    CodigoComponenteCurricular = 89,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Ciências",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 89",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 8,
                    CodigoComponenteCurricular = 8,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Geografia",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 8",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1060,
                    CodigoComponenteCurricular = 1060,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 1060",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 1060",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 9,
                    CodigoComponenteCurricular = 9,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 9",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 9",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1213,
                    CodigoComponenteCurricular = 1213,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 1213",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = true,
                    RegistraFrequencia = true,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 1213",
                    TurmaCodigo = "1"
                }
            }.Where(x => request.Ids.Contains(x.Id)));
        }
    }
}
