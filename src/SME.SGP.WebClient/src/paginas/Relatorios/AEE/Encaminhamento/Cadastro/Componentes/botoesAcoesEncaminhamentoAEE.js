import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import QuestionarioDinamicoFuncoes from '~/componentes-sgp/QuestionarioDinamico/Funcoes/QuestionarioDinamicoFuncoes';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { RotasDto } from '~/dtos';
import situacaoAEE from '~/dtos/situacaoAEE';
import {
  setExibirLoaderEncaminhamentoAEE,
  setExibirModalDevolverAEE,
  setExibirModalEncerramentoEncaminhamentoAEE,
  setListaSecoesEmEdicao,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import { confirmar, erros, sucesso } from '~/servicos';
import history from '~/servicos/history';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';

const BotoesAcoesEncaminhamentoAEE = props => {
  const { match } = props;

  const dispatch = useDispatch();

  const questionarioDinamicoEmEdicao = useSelector(
    store => store.questionarioDinamico.questionarioDinamicoEmEdicao
  );

  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
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

    const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
      encaminhamentoId,
      situacao,
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
    if (questionarioDinamicoEmEdicao) {
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
    if (!desabilitarCamposEncaminhamentoAEE && questionarioDinamicoEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        QuestionarioDinamicoFuncoes.limparDadosOriginaisQuestionarioDinamico();
        dispatch(setListaSecoesEmEdicao([]));
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

  const onClickDevolver = () => {
    dispatch(setExibirModalDevolverAEE(true));
  };

  const onClickEncerrar = async () => {
    if (!desabilitarCamposEncaminhamentoAEE) {
      const encaminhamentoId = match?.params?.id;
      const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
        encaminhamentoId,
        situacaoAEE.Encaminhado,
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

  const onClickConcluirParecer = async () => {
    if (!desabilitarCamposEncaminhamentoAEE) {
      const encaminhamentoId = match?.params?.id;
      const salvou = await ServicoEncaminhamentoAEE.salvarEncaminhamento(
        encaminhamentoId,
        situacaoAEE.Analise,
        true
      );
      if (salvou) {
        dispatch(setExibirLoaderEncaminhamentoAEE(true));
        const resposta = await ServicoEncaminhamentoAEE.concluirEncaminhamento(
          encaminhamentoId
        )
          .catch(e => erros(e))
          .finally(() => dispatch(setExibirLoaderEncaminhamentoAEE(false)));

        if (resposta?.status === 200) {
          sucesso('Encaminhamento concluído');
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
          !questionarioDinamicoEmEdicao || desabilitarCamposEncaminhamentoAEE
        }
      />
      <Button
        id="btn-excluir"
        label="Excluir"
        color={Colors.Vermelho}
        border
        className="mr-3"
        onClick={onClickExcluir}
        hidden={
          (dadosEncaminhamento?.situacao !== situacaoAEE.Encaminhado &&
            dadosEncaminhamento?.situacao !== situacaoAEE.Rascunho) ||
          !(permissoesTela.podeExcluir && dadosEncaminhamento?.podeEditar)
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
          !questionarioDinamicoEmEdicao ||
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
          dadosEncaminhamento?.situacao !== situacaoAEE.Rascunho &&
          dadosEncaminhamento?.situacao !== situacaoAEE.Devolvido
        }
        disabled={
          !dadosCollapseLocalizarEstudante?.codigoAluno ||
          desabilitarCamposEncaminhamentoAEE
        }
      />
      <Button
        id="btn-devolver"
        label="Devolver"
        color={Colors.Azul}
        border
        bold
        className="ml-3"
        onClick={onClickDevolver}
        hidden={dadosEncaminhamento?.situacao !== situacaoAEE.Encaminhado}
        disabled={
          desabilitarCamposEncaminhamentoAEE || !dadosEncaminhamento?.podeEditar
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
        hidden={
          !dadosEncaminhamento?.situacao ||
          dadosEncaminhamento?.situacao !== situacaoAEE.Encaminhado
        }
        disabled={
          desabilitarCamposEncaminhamentoAEE || !dadosEncaminhamento?.podeEditar
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
        hidden={
          !dadosEncaminhamento?.situacao ||
          dadosEncaminhamento?.situacao !== situacaoAEE.Encaminhado
        }
        disabled={
          desabilitarCamposEncaminhamentoAEE || !dadosEncaminhamento?.podeEditar
        }
      />
      <Button
        id="btn-concluir-parecer"
        label="Concluir parecer"
        color={Colors.Roxo}
        border
        bold
        className="ml-3"
        onClick={onClickConcluirParecer}
        hidden={
          !dadosEncaminhamento?.situacao ||
          dadosEncaminhamento?.situacao !== situacaoAEE.Analise
        }
        disabled={
          desabilitarCamposEncaminhamentoAEE || !dadosEncaminhamento?.podeEditar
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
