using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes
{
    public class ObterTurmasPorProfessorRfQueryHandlerFakeFundamental1AAno2 : IRequestHandler<ObterTurmasPorProfessorRfQuery, IEnumerable<ProfessorTurmaDto>>
    {

        private const string ESCOLA_CODIGO_1 = "1";
        private const string ANO_2 = "2";
        private const int CODIGO_TURMA_1 = 1;
        private const string NOME_TURMA_1 = "1A";
        private const int CODIGO_MODALIDADE = 5;
        private const string DRE_CODIGO_1 = "1";
        private const string DRE_NOME_1 = "NOME DRE 1";
        private const string DRE_ABREVIACAO_1 = "DRE-1";
        private const string MODALIDADE_FUNDAMENTAL = "FUNDAMENTAL";
        private const int SEMESTRE_0 = 0;
        private const string UNIDADE_ADMINISTRATIVA = "UNIDADE ADMINISTRATIVA";
        private const int UE_CODIGO_TIPO_3 = 3;
        private const string UE_NOME_1 = "NOME UE 1";
        private const string UE_ABREVIACAO_1 = "UE-1";
        private const string TIPO_ESCOLA_CEU_EMEF = "CEU EMEF";
        private const string TIPO_ESCOLA_CODIGO_16 = "16";
        public async Task<IEnumerable<ProfessorTurmaDto>> Handle(ObterTurmasPorProfessorRfQuery request, CancellationToken cancellationToken)
        {
            var professoresTurmaDTO = MapearParaDto(ObterListaTurmasPorProfessor(request.CodigoRf));
            return await Task.Run(() => professoresTurmaDTO);
        }

        private IEnumerable<ProfessorTurmaDto> MapearParaDto(IEnumerable<ProfessorTurmaReposta> turmas)
        {
            return turmas?.Select(m => new ProfessorTurmaDto()
            {
                Ano = m.Ano,
                AnoLetivo = m.AnoLetivo,
                CodDre = m.CodDre,
                CodEscola = m.CodEscola,
                CodModalidade = m.CodModalidade,
                CodTipoEscola = m.CodTipoEscola,
                CodTipoUE = m.CodTipoUE,
                CodTurma = m.CodTurma,
                Dre = m.Dre,
                DreAbrev = m.DreAbrev,
                Modalidade = m.Modalidade,
                NomeTurma = m.NomeTurma,
                TipoEscola = m.TipoEscola,
                Semestre = m.Semestre,
                TipoUE = m.TipoUE,
                Ue = m.Ue,
                UeAbrev = m.UeAbrev
            });
        }

        public IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf)
        {
            return new List<ProfessorTurmaReposta>()
            {
                new ProfessorTurmaReposta(){
                    CodEscola = ESCOLA_CODIGO_1,
                    Ano = ANO_2,
                    CodTurma = CODIGO_TURMA_1,
                    NomeTurma = NOME_TURMA_1,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    CodModalidade = CODIGO_MODALIDADE,
                    CodDre = DRE_CODIGO_1,
                    Dre = DRE_NOME_1,
                    DreAbrev = DRE_ABREVIACAO_1,
                    Modalidade = MODALIDADE_FUNDAMENTAL,
                    Semestre = SEMESTRE_0,
                    TipoUE = UNIDADE_ADMINISTRATIVA,
                    CodTipoUE = UE_CODIGO_TIPO_3,
                    Ue = UE_NOME_1,
                    UeAbrev = UE_ABREVIACAO_1,
                    TipoEscola = TIPO_ESCOLA_CEU_EMEF,
                    CodTipoEscola = TIPO_ESCOLA_CODIGO_16,
                }
            };
        }
    }
}