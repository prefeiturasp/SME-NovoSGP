import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { RotasDto } from '~/dtos';
import situacaoAEE from '~/dtos/situacaoAEE';
import {
  setExibirLoaderEncaminhamentoAEE,
  setExibirModalEncerramentoEncaminhamentoAEE,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import { confirmar, erros, sucesso } from '~/servicos';
import history from '~/servicos/history';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';

const BotoesAcoesEncaminhamentoAEE = props => {
  const { match } = props;

  const dispatch = useDispatch();

  const encaminhamentoAEEEmEdicao = useSelector(
    store => store.encaminhamentoAEE.encaminhamentoAEEEmEdicao
  );

  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  const dadosSecaoLocalizarEstudante = useSelector(
    store => store.encaminhamentoAEE.dadosSecaoLocalizarEstudante
  );

  const desabilitarCamposEncaminhamentoAEE = useSelector(
    store => store.encaminhamentoAEE.desabilitarCamposEncaminhamentoAEE
  );

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_ENCAMINHAMENTO];

  const onClickSalvar = async () => {
    const encaminhamentoId = match?.params?.id;
    let situacao = situacaoAEE.Rascunho;

    if (encaminhamentoId) {
      situacao = dadosEncaminhamento?.situacao;
    }

    let validarCamposObrigatorios = false;
    if (dadosEncaminhamento?.situacao === situacaoAEE.Encaminhado) {
      validarCamposObrigatorios = true;
    }

    const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
      encaminhamentoId,
      situacao,
      validarCamposObrigatorios
    );
    if (salvou) {
      let mensagem = 'Registro salvo com sucesso';
      if (encaminhamentoId) {
        mensagem = 'Registro alterado com sucesso';
      }
      sucesso(mensagem);
      history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
    }
  };

  const onClickEnviar = async () => {
    const encaminhamentoId = match?.params?.id;
    const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
      encaminhamentoId,
      situacaoAEE.Encaminhado,
      true
    );
    if (salvou) {
      sucesso('Encaminhamento enviado para validação do CP');
      history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
    }
  };

  const onClickVoltar = async () => {
    if (encaminhamentoAEEEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmou) {
        const encaminhamentoId = match?.params?.id;
        let situacao = situacaoAEE.Rascunho;

        if (encaminhamentoId) {
          situacao = dadosEncaminhamento?.situacao;
        }
        const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
          encaminhamentoId,
          situacao,
          false,
          false
        );
        if (salvou) {
          let mensagem = 'Registro salvo com sucesso';
          if (encaminhamentoId) {
            mensagem = 'Registro alterado com sucesso';
          }
          sucesso(mensagem);
          history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
        }
      } else {
        history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
      }
    } else {
      history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
    }
  };

  const onClickCancelar = async () => {
    if (!desabilitarCamposEncaminhamentoAEE && encaminhamentoAEEEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        ServicoEncaminhamentoAEE.resetarTelaDadosOriginais();
      }
    }
  };

  const onClickExcluir = async () => {
    const encaminhamentoId = match?.params?.id;
    if (permissoesTela.podeExcluir && encaminhamentoId) {
      const confirmado = await confirmar(
        'Excluir',
        '',
        'Você tem certeza que deseja excluir este registro?'
      );

      if (confirmado) {
        dispatch(setExibirLoaderEncaminhamentoAEE(true));
        const resposta = await ServicoEncaminhamentoAEE.excluirEncaminhamento(
          encaminhamentoId
        )
          .catch(e => erros(e))
          .finally(() => dispatch(setExibirLoaderEncaminhamentoAEE(false)));

        if (resposta?.status === 200) {
          sucesso('Registro excluído com sucesso');
          history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
        }
      }
    }
  };

  const onClickEncerrar = async () => {
    if (!desabilitarCamposEncaminhamentoAEE) {
      const encaminhamentoId = match?.params?.id;
      const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
        encaminhamentoId,
        situacaoAEE.Encaminhado,
        true,
        true
      );
      if (salvou) {
        dispatch(setExibirModalEncerramentoEncaminhamentoAEE(true));
      }
    }
  };

  const onClickEncaminharAEE = async () => {
    if (!desabilitarCamposEncaminhamentoAEE) {
      const encaminhamentoId = match?.params?.id;
      const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
        encaminhamentoId,
        situacaoAEE.Encaminhado,
        true,
        true
      );
      if (salvou) {
        dispatch(setExibirLoaderEncaminhamentoAEE(true));
        const resposta = await ServicoEncaminhamentoAEE.enviarParaAnaliseEncaminhamento(
          encaminhamentoId
        )
          .catch(e => erros(e))
          .finally(() => dispatch(setExibirLoaderEncaminhamentoAEE(false)));

        if (resposta?.status === 200) {
          sucesso('Encaminhamento enviado para a AEE');
          history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
        }
      }
    }
  };

  return (
    <>
      <Button
        id="btn-voltar"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-3"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-3"
        onClick={onClickCancelar}
        disabled={
          !encaminhamentoAEEEmEdicao || desabilitarCamposEncaminhamentoAEE
        }
      />
      <Button
        id="btn-excluir"
        label="Excluir"
        color={Colors.Vermelho}
        border
        className="mr-3"
        onClick={onClickExcluir}
        disabled={
          !permissoesTela.podeExcluir ||
          !match?.params?.id ||
          (match?.params?.id && !dadosEncaminhamento?.podeEditar)
        }
      />
      <Button
        id="btn-salvar"
        label={match?.params?.id ? 'Alterar' : 'Salvar'}
        color={Colors.Azul}
        border
        bold
        onClick={onClickSalvar}
        disabled={
          desabilitarCamposEncaminhamentoAEE ||
          !encaminhamentoAEEEmEdicao ||
          (match?.params?.id && !dadosEncaminhamento?.podeEditar)
        }
      />
      <Button
        id="btn-enviar"
        label="Enviar"
        color={Colors.Roxo}
        border
        bold
        className="ml-3"
        onClick={onClickEnviar}
        hidden={
          dadosEncaminhamento?.situacao &&
          dadosEncaminhamento?.situacao !== situacaoAEE.Rascunho
        }
        disabled={
          !dadosSecaoLocalizarEstudante?.codigoAluno ||
          desabilitarCamposEncaminhamentoAEE
        }
      />
      <Button
        id="btn-encerrar"
        label="Encerrar"
        color={Colors.Azul}
        border
        bold
        className="ml-3"
        onClick={onClickEncerrar}
        hidden={dadosEncaminhamento?.situacao === situacaoAEE.Rascunho}
        disabled={
          desabilitarCamposEncaminhamentoAEE ||
          encaminhamentoAEEEmEdicao ||
          !dadosEncaminhamento?.podeEditar ||
          dadosEncaminhamento?.situacao === situacaoAEE.Analise
        }
      />
      <Button
        id="btn-encaminhar-aee"
        label="Encaminhar AEE"
        color={Colors.Roxo}
        border
        bold
        className="ml-3"
        onClick={onClickEncaminharAEE}
        hidden={dadosEncaminhamento?.situacao === situacaoAEE.Rascunho}
        disabled={
          desabilitarCamposEncaminhamentoAEE ||
          encaminhamentoAEEEmEdicao ||
          !dadosEncaminhamento?.podeEditar ||
          dadosEncaminhamento?.situacao === situacaoAEE.Analise
        }
      />
    </>
  );
};

BotoesAcoesEncaminhamentoAEE.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

BotoesAcoesEncaminhamentoAEE.defaultProps = {
  match: {},
};

export default BotoesAcoesEncaminhamentoAEE;
