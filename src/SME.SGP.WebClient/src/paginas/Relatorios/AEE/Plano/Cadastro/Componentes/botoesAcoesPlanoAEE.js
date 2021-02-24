import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes';
import { confirmar, erros, history, sucesso } from '~/servicos';
import { RotasDto, situacaoPlanoAEE } from '~/dtos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import QuestionarioDinamicoFuncoes from '~/componentes-sgp/QuestionarioDinamico/Funcoes/QuestionarioDinamicoFuncoes';
import {
  limparDadosDevolutiva,
  setAtualizarPlanoAEEDados,
  setExibirLoaderPlanoAEE,
  setParecerCoordenacao,
  setDevolutivaEmEdicao,
} from '~/redux/modulos/planoAEE/actions';

const BotoesAcoesPlanoAEE = props => {
  const { match } = props;

  const questionarioDinamicoEmEdicao = useSelector(
    store => store.questionarioDinamico.questionarioDinamicoEmEdicao
  );

  const desabilitarCamposPlanoAEE = useSelector(
    store => store.planoAEE.desabilitarCamposPlanoAEE
  );

  const devolutivaEmEdicao = useSelector(
    store => store.planoAEE.devolutivaEmEdicao
  );

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];

  const situacaoDevolutivaCoordenacao =
    planoAEEDados?.situacao === situacaoPlanoAEE.DevolutivaCoordenacao;
  const planoAeeId = match?.params?.id;
  const labelBotaoSalvar =
    situacaoDevolutivaCoordenacao || !planoAeeId ? 'Salvar' : 'Alterar';

  const desabilitarBotaoSalvar = situacaoDevolutivaCoordenacao
    ? !devolutivaEmEdicao
    : desabilitarCamposPlanoAEE || !questionarioDinamicoEmEdicao;

  const desabilitarBotaoCancelar = situacaoDevolutivaCoordenacao
    ? !devolutivaEmEdicao
    : desabilitarCamposPlanoAEE || !questionarioDinamicoEmEdicao;

  console.log('planoAEEDados', planoAEEDados);
  console.log('desabilitarCamposPlanoAEE', desabilitarCamposPlanoAEE);

  const dispatch = useDispatch();

  const salvarDevolutivaCoordenacao = async () => {
    const resposta = await ServicoPlanoAEE.salvarDevolutivaCP().catch(e =>
      erros(e)
    );
    if (resposta?.data) {
      sucesso('Devolutiva registrada com sucesso');
      dispatch(setParecerCoordenacao(''));
      history.push(RotasDto.RELATORIO_AEE_PLANO);
    }
  };

  const limparDevolutiva = () => {
    dispatch(limparDadosDevolutiva());
    dispatch(setDevolutivaEmEdicao(false));
  };

  const onClickVoltar = async () => {
    if (questionarioDinamicoEmEdicao || devolutivaEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmou) {
        if (situacaoDevolutivaCoordenacao) {
          salvarDevolutivaCoordenacao();
          return;
        }
        const salvou = await ServicoPlanoAEE.salvarPlano();
        const planoId = match?.params?.id;

        if (salvou) {
          let mensagem = 'Registro salvo com sucesso';
          if (planoId) {
            mensagem = 'Registro alterado com sucesso';
          }
          sucesso(mensagem);
          history.push(RotasDto.RELATORIO_AEE_PLANO);
        }
      } else {
        if (situacaoDevolutivaCoordenacao) {
          limparDevolutiva();
        }
        history.push(RotasDto.RELATORIO_AEE_PLANO);
      }
    } else {
      history.push(RotasDto.RELATORIO_AEE_PLANO);
    }
  };

  const onClickCancelar = async () => {
    if (
      !desabilitarCamposPlanoAEE &&
      (questionarioDinamicoEmEdicao || devolutivaEmEdicao)
    ) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        if (situacaoDevolutivaCoordenacao) {
          limparDevolutiva();
          return;
        }
        QuestionarioDinamicoFuncoes.limparDadosOriginaisQuestionarioDinamico();
      }
    }
  };

  const onClickSalvar = async () => {
    if (situacaoDevolutivaCoordenacao) {
      salvarDevolutivaCoordenacao();
      return;
    }
    const salvou = await ServicoPlanoAEE.salvarPlano();
    const planoId = match?.params?.id;

    if (salvou) {
      let mensagem = 'Registro salvo com sucesso';
      if (planoId) {
        mensagem = 'Registro alterado com sucesso';
      }
      sucesso(mensagem);
      history.push(RotasDto.RELATORIO_AEE_PLANO);
    }
  };

  const onClickSolicitarEncerramento = async () => {
    if (!desabilitarCamposPlanoAEE && !questionarioDinamicoEmEdicao) {
      dispatch(setExibirLoaderPlanoAEE(true));

      const resposta = await ServicoPlanoAEE.encerrarPlano(planoAeeId)
        .catch(e => erros(e))
        .finally(() => dispatch(setExibirLoaderPlanoAEE(false)));
      if (resposta?.data) {
        dispatch(setAtualizarPlanoAEEDados(resposta?.data));
      }
    }
  };

  const onClickEncerrarPlano = async () => {
    const resposta = await ServicoPlanoAEE.salvarDevolutivaPAAI().catch(e =>
      erros(e)
    );
    if (resposta?.data) {
      sucesso('Devolutiva encerrada com sucesso');
      dispatch(setParecerCoordenacao(''));
      history.push(RotasDto.RELATORIO_AEE_PLANO);
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
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-3"
        onClick={onClickCancelar}
        disabled={desabilitarBotaoCancelar}
      />
      <Button
        id="btn-salvar"
        label={labelBotaoSalvar}
        color={Colors.Azul}
        border
        bold
        onClick={onClickSalvar}
        disabled={desabilitarBotaoSalvar}
      />

      <Button
        id="btn-solicitar-encerramento"
        label="Solicitar encerramento"
        color={Colors.Roxo}
        bold
        className="ml-3"
        onClick={onClickSolicitarEncerramento}
        hidden={
          !planoAEEDados?.situacao ||
          planoAEEDados?.situacao !== situacaoPlanoAEE.EmAndamento
        }
        disabled={
          desabilitarCamposPlanoAEE ||
          questionarioDinamicoEmEdicao ||
          !permissoesTela?.podeAlterar
        }
      />

      <Button
        id="btn-encerrar-plano"
        label="Encerrar plano"
        color={Colors.Roxo}
        bold
        className="ml-3"
        onClick={onClickEncerrarPlano}
        hidden={
          !planoAEEDados?.situacao ||
          planoAEEDados?.situacao !== situacaoPlanoAEE.DevolutivaPAAI
        }
        disabled={
          desabilitarCamposPlanoAEE ||
          questionarioDinamicoEmEdicao ||
          !devolutivaEmEdicao ||
          !permissoesTela?.podeAlterar
        }
      />
    </>
  );
};

BotoesAcoesPlanoAEE.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

BotoesAcoesPlanoAEE.defaultProps = {
  match: {},
};

export default BotoesAcoesPlanoAEE;
