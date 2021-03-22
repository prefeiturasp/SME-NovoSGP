import { store } from '~/redux';
import {
  setAcompanhamentoAprendizagemEmEdicao,
  setDadosAcompanhamentoAprendizagem,
  setExibirLoaderGeralAcompanhamentoAprendizagem,
} from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import { limparDadosRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';

const urlPadrao = '/v1/acompanhamento/alunos';

class ServicoAcompanhamentoAprendizagem {
  obterListaAlunos = (turmaCodigo, anoLetivo, periodo) => {
    const url = `v1/fechamentos/turmas/${turmaCodigo}/alunos/anos/${anoLetivo}/semestres/${periodo}`;
    return api.get(url);
  };

  obterListaSemestres = () => {
    return new Promise(resolve => {
      resolve({
        data: [
          {
            semestre: '1',
            descricao: '1º Semestre',
          },
          {
            semestre: '2',
            descricao: '2º Semestre',
          },
        ],
      });
    });
  };

  obterAcompanhamentoEstudante = async (turmaId, alunoId, semestre) => {
    const { dispatch } = store;
    dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(true));

    dispatch(limparDadosRegistroIndividual());

    const retorno = await api
      .get(
        `${urlPadrao}?turmaId=${turmaId}&alunoId=${alunoId}&semestre=${semestre}`
      )
      .catch(e => erros(e))
      .finally(() =>
        dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(false))
      );

    if (retorno?.data) {
      dispatch(setDadosAcompanhamentoAprendizagem({ ...retorno.data }));
      return retorno.data.acompanhamentoAlunoSemestreId;
    }

    dispatch(setDadosAcompanhamentoAprendizagem({}));
    return 0;
  };

  salvarAcompanhamentoAprendizagem = params => {
    return api.post(`${urlPadrao}/semestres`, params);
  };

  uploadFoto = (formData, configuracaoHeader) => {
    return api.post(
      `${urlPadrao}/semestres/upload`,
      formData,
      configuracaoHeader
    );
  };

  obterFotos = acompanhamentoAlunoSemestreId => {
    return api.get(
      `${urlPadrao}/semestres/${acompanhamentoAlunoSemestreId}/fotos`
    );
  };

  excluirFotos = (acompanhamentoAlunoSemestreId, codigoFoto) => {
    return api.delete(
      `${urlPadrao}/semestres/${acompanhamentoAlunoSemestreId}/fotos/${codigoFoto}`
    );
  };

  atualizarObservacoes = valorNovo => {
    const { dispatch } = store;
    const state = store.getState();

    const { acompanhamentoAprendizagem } = state;

    const { dadosAcompanhamentoAprendizagem } = acompanhamentoAprendizagem;

    const dadosAcompanhamentoAtual = dadosAcompanhamentoAprendizagem;
    dadosAcompanhamentoAtual.observacoes = valorNovo;
    dispatch(setDadosAcompanhamentoAprendizagem(dadosAcompanhamentoAtual));
  };

  salvarDadosAcompanhamentoAprendizagem = async semestreSelecionado => {
    const { dispatch } = store;
    const state = store.getState();

    const { acompanhamentoAprendizagem, usuario } = state;
    const { turmaSelecionada } = usuario;

    const {
      dadosAcompanhamentoAprendizagem,
      acompanhamentoAprendizagemEmEdicao,
      desabilitarCampos,
      dadosAlunoObjectCard,
    } = acompanhamentoAprendizagem;

    const { codigoEOL } = dadosAlunoObjectCard;

    const salvar = async () => {
      const params = {
        acompanhamentoAlunoId:
          dadosAcompanhamentoAprendizagem.acompanhamentoAlunoId,
        acompanhamentoAlunoSemestreId:
          dadosAcompanhamentoAprendizagem.acompanhamentoAlunoSemestreId,
        turmaId: turmaSelecionada?.id,
        semestre: semestreSelecionado,
        alunoCodigo: codigoEOL,
        observacoes: dadosAcompanhamentoAprendizagem.observacoes || '',
      };

      dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(true));
      const retorno = await this.salvarAcompanhamentoAprendizagem(params)
        .catch(e => erros(e))
        .finally(() =>
          dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(false))
        );

      if (retorno?.status === 200) {
        if (!dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId) {
          this.obterAcompanhamentoEstudante(
            turmaSelecionada?.id,
            codigoEOL,
            semestreSelecionado
          );
        } else {
          const dadosAcompanhamentoAtual = dadosAcompanhamentoAprendizagem;
          dadosAcompanhamentoAtual.auditoria = retorno.data;
          dispatch(
            setDadosAcompanhamentoAprendizagem(dadosAcompanhamentoAtual)
          );
        }

        dispatch(setAcompanhamentoAprendizagemEmEdicao(false));
        if (dadosAcompanhamentoAprendizagem.acompanhamentoAlunoId) {
          sucesso('Registro alterado com sucesso');
        } else {
          sucesso('Registro inserido com sucesso');
        }
        return true;
      }
      return false;
    };

    if (desabilitarCampos) {
      return true;
    }

    if (acompanhamentoAprendizagemEmEdicao) {
      return salvar();
    }

    return true;
  };
}

export default new ServicoAcompanhamentoAprendizagem();
