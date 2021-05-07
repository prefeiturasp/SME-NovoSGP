import api from '~/servicos/api';

const urlPadrao = `/v1/fechamentos/acompanhamentos`;

class ServicoAcompanhamentoFechamento {
  obterTurmas = params => {
    return api.get(
      `${urlPadrao}?numeroPagina=${params.numeroPagina}&numeroRegistros=10`,
      { params }
    );
  };

  obterFechamentos = ({ turmaId, bimestre }) => {
    return api.get(
      `${urlPadrao}/turmas/${turmaId}/fechamentos/bimestres/${bimestre}`
    );
  };

  obterConselhoClasse = ({ turmaId, bimestre }) => {
    return api.get(
      `${urlPadrao}/turmas/${turmaId}/conselho-classe/bimestres/${bimestre}`
    );
  };
}

export default new ServicoAcompanhamentoFechamento();
