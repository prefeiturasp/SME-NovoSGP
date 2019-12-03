using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtribuicaoCJ : IComandosAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IServicoAtribuicaoCJ servicoAtribuicaoCJ;

        public ComandosAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoAtribuicaoCJ servicoAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoAtribuicaoCJ = servicoAtribuicaoCJ ?? throw new ArgumentNullException(nameof(servicoAtribuicaoCJ));
        }

        public async Task Salvar(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var atribuicaoCJs = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJPersistenciaDto.Modalidade, atribuicaoCJPersistenciaDto.TurmaId,
                atribuicaoCJPersistenciaDto.UeId, string.Empty, string.Empty, string.Empty);

            foreach (var atribuicaoDto in atribuicaoCJPersistenciaDto.Disciplinas)
            {
                var atribuicao = atribuicaoCJs.FirstOrDefault(a => a.DisciplinaId == atribuicaoDto.DisciplinaId);

                if (atribuicao == null)
                    atribuicao = TransformaDtoEmEntidade(atribuicaoCJPersistenciaDto, atribuicaoDto);
                else
                {
                    if (atribuicao.Substituir != atribuicaoDto.Substituir)
                        atribuicao.Substituir = atribuicaoDto.Substituir;
                }
                await servicoAtribuicaoCJ.Salvar(atribuicao);
            }
        }

        private AtribuicaoCJ TransformaDtoEmEntidade(AtribuicaoCJPersistenciaDto dto, AtribuicaoCJPersistenciaItemDto itemDto)
        {
            return new AtribuicaoCJ()
            {
                DreId = dto.DreId,
                Modalidade = dto.Modalidade,
                ProfessorRf = dto.UsuarioRf,
                Substituir = itemDto.Substituir,
                TurmaId = dto.TurmaId,
                UeId = dto.UeId,
                DisciplinaId = itemDto.DisciplinaId
            };
        }
    }
}