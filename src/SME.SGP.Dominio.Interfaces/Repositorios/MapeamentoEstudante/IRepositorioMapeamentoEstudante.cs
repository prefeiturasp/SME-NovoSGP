﻿using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioMapeamentoEstudante : IRepositorioBase<MapeamentoEstudante>
    {
        Task<MapeamentoEstudante> ObterMapeamentoEstudantePorId(long id);
        Task<long?> ObterIdentificador(string codigoAluno, long turmaId, int bimestre);
        Task<IEnumerable<string>> ObterCodigoArquivoPorMapeamentoEstudanteId(long id);
        Task<IEnumerable<long>> ObterIdentificadoresDosMapeamentosDoBimestreAtual(DateTime dataBase);
        Task<InformacoesAtualizadasMapeamentoEstudanteAlunoDto> ObterInformacoesAtualizadasAlunoMapeamentoEstudante(string codigoAluno, int anoLetivo, int bimestre);
        Task<string[]> ObterCodigosAlunosComMapeamentoEstudanteBimestre(long turmaId, int bimestre);
        Task<long> ObterTurmaIdMapeamentoEstudante(long mapeamentoEstudanteId);
    }
}
