import { store } from '~/redux';
import { setDesabilitarCampos } from '~/redux/modulos/conselhoClasse/actions';

import {
  atualizaDadosRegistroAtual,
  atualizarMarcadorDiasSemRegistroExibir,
  limparDadosRegistroIndividual,
  setAuditoriaNovoRegistro,
  setDadosPrincipaisRegistroIndividual,
  setExibirLoaderGeralRegistroIndividual,
  setPodeRealizarNovoRegistro,
  setRegistroIndividualEmEdicao,
} from '~/redux/modulos/registroIndividual/actions';

import {
  confirmar,
  erros,
  ServicoRegistroIndividual,
  sucesso,
} from '~/servicos';

class MetodosRegistroIndividual {
  dispatch = store.dispatch;

  obterDados = () => {
    const { registroIndividual, usuario } = store.getState();
    const { turmaSelecionada } = usuario;
    const turmaId = turmaSelecionada?.id || 0;

    return {
      registroIndividual,
      turmaId,
    };
  };

  escolheCadastrar = (atualizarDados, registroIndividual, turmaId, id) => {
    if (id) {
      this.editarRegistroIndividual(
        atualizarDados,
        registroIndividual,
        turmaId
      );
      return;
    }
    this.cadastrarRegistroIndividual(
      atualizarDados,
      registroIndividual,
      turmaId
    );
  };

  pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  salvarRegistroIndividual = async () => {
    const confirmado = await this.pergutarParaSalvar();
    const { registroIndividual, turmaId } = this.obterDados();
    const { id } = registroIndividual.dadosParaSalvarNovoRegistro;
    if (confirmado) {
      this.escolheCadastrar(false, registroIndividual, turmaId, id);
    }
    return true;
  };

  verificarSalvarRegistroIndividual = atualizarDados => {
    const { registroIndividual, turmaId } = this.obterDados();
    const { id } = registroIndividual.dadosParaSalvarNovoRegistro;

    if (registroIndividual.registroIndividualEmEdicao) {
      this.escolheCadastrar(atualizarDados, registroIndividual, turmaId, id);
    }
  };

  resetarInfomacoes = ehDataAnterior => {
    if (ehDataAnterior) {
      this.dispatch(limparDadosRegistroIndividual());
      return;
    }
    this.dispatch(setRegistroIndividualEmEdicao(false));
    this.dispatch(setDesabilitarCampos(false));
  };

  cadastrarRegistroIndividual = async (
    atualizarDados,
    registroIndividual,
    turmaId
  ) => {
    this.dispatch(setExibirLoaderGeralRegistroIndividual(true));
    const {
      dadosAlunoObjectCard,
      dadosParaSalvarNovoRegistro,
    } = registroIndividual;
    const { alunoCodigo, data, registro } = dadosParaSalvarNovoRegistro;

    const retorno = await ServicoRegistroIndividual.salvarRegistroIndividual({
      turmaId,
      componenteCurricularId:
        registroIndividual.componenteCurricularSelecionado,
      alunoCodigo,
      registro,
      data,
    })
      .catch(e => erros(e))
      .finally(() =>
        this.dispatch(setExibirLoaderGeralRegistroIndividual(false))
      );

    if (retorno?.status === 200) {
      sucesso('Registro cadastrado com sucesso.');

      const dataAtual = window.moment(window.moment().format('YYYY-MM-DD'));
      const ehDataAnterior = window.moment(dataAtual).isAfter(data);
      this.resetarInfomacoes(ehDataAnterior);
      if (!ehDataAnterior && atualizarDados) {
        this.dispatch(setAuditoriaNovoRegistro(retorno.data));
        this.dispatch(
          atualizaDadosRegistroAtual({
            id: retorno.data.id,
            registro,
            alunoCodigo,
            data,
          })
        );
      }
      if (dadosAlunoObjectCard?.marcadorDiasSemRegistroExibir) {
        this.dispatch(atualizarMarcadorDiasSemRegistroExibir(alunoCodigo));
      }
    }
  };

  editarRegistroIndividual = async (
    atualizarDados,
    registroIndividual,
    turmaId
  ) => {
    this.dispatch(setExibirLoaderGeralRegistroIndividual(true));

    const {
      componenteCurricularSelecionado,
      dadosAlunoObjectCard,
      dadosParaSalvarNovoRegistro,
    } = registroIndividual;
    const { id, alunoCodigo, data, registro } = dadosParaSalvarNovoRegistro;
    const retorno = await ServicoRegistroIndividual.editarRegistroIndividual({
      id,
      turmaId,
      componenteCurricularId: componenteCurricularSelecionado,
      alunoCodigo,
      registro,
      data,
    })
      .catch(e => erros(e))
      .finally(() =>
        this.dispatch(setExibirLoaderGeralRegistroIndividual(false))
      );

    if (retorno?.status === 200) {
      sucesso('Registro editado com sucesso.');
      if (atualizarDados) {
        this.dispatch(
          atualizaDadosRegistroAtual({
            id: retorno.data.id,
            registro,
            alunoCodigo,
            data,
          })
        );
      }
      if (dadosAlunoObjectCard?.marcadorDiasSemRegistroExibir) {
        this.dispatch(atualizarMarcadorDiasSemRegistroExibir(alunoCodigo));
      }
      this.dispatch(setAuditoriaNovoRegistro(retorno.data));
      const dataAtual = window.moment(window.moment().format('YYYY-MM-DD'));
      const ehDataAnterior = window.moment(dataAtual).isAfter(data);
      this.resetarInfomacoes(ehDataAnterior);
    }
  };

  obterRegistroIndividualPorData = async (
    dataFormatadaInicio,
    dataFimEscolhida,
    pagina,
    registros
  ) => {
    const { registroIndividual, turmaId } = this.obterDados();
    const {
      dadosAlunoObjectCard,
      componenteCurricularSelecionado,
    } = registroIndividual;
    const alunoCodigo = dadosAlunoObjectCard.codigoEOL;

    if (alunoCodigo) {
      const retorno = await ServicoRegistroIndividual.obterRegistroIndividualPorPeriodo(
        {
          alunoCodigo,
          componenteCurricular: componenteCurricularSelecionado,
          dataInicio: dataFormatadaInicio,
          dataFim: dataFimEscolhida,
          turmaCodigo: turmaId,
          numeroPagina: pagina,
          numeroRegistros: registros,
        }
      ).catch(e => erros(e));

      if (retorno?.data) {
        this.dispatch(setDadosPrincipaisRegistroIndividual(retorno.data));
        this.dispatch(
          setPodeRealizarNovoRegistro(retorno.data.podeRealizarNovoRegistro)
        );
      }
    }
  };
}
export default new MetodosRegistroIndividual();
