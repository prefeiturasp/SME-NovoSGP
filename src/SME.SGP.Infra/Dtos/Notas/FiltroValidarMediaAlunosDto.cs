﻿using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroValidarMediaAlunosDto
    {
        public FiltroValidarMediaAlunosDto(IEnumerable<long> atividadesAvaliativasIds, IEnumerable<string> alunosIds, Usuario usuario, string disciplinaId, string codigoTurma, string hostAplicacao)
        {
            AtividadesAvaliativasIds = atividadesAvaliativasIds;
            AlunosIds = alunosIds;
            Usuario = usuario;
            DisciplinaId = disciplinaId;
            CodigoTurma = codigoTurma;
            HostAplicacao = hostAplicacao;
        }

        public IEnumerable<long> AtividadesAvaliativasIds { get; set; }
        public IEnumerable<string> AlunosIds { get; set; }
        public Usuario Usuario { get; set; }
        public string DisciplinaId { get; set; }
        public string CodigoTurma { get; set; }
        public string HostAplicacao { get; set; }
    }
}
