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

  const dadosDevolutiva = useSelector(store => store.planoAEE.dadosDevolutiva);

  const devolutivaEmEdicao = useSelector(
    store => store.planoAEE.devolutivaEmEdicao
  );

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const dadosAtribuicaoResponsavel = useSelector(
    store => store.planoAEE.dadosAtribuicaoResponsavel
  );

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];

  const situacaoDevolutivaCoordenacao =
    planoAEEDados?.situacao === situacaoPlanoAEE.DevolutivaCoordenacao;
  const situacaoDevolutivaPAAI =
    planoAEEDados?.situacao === situacaoPlanoAEE.DevolutivaPAAI &&
    dadosDevolutiva?.podeEditarParecerPAAI;
  const situacaoAtribuicaoPAAI =
    planoAEEDados?.situacao === situacaoPlanoAEE.AtribuicaoPAAI;

  const situacaoDevolutiva =
    situacaoDevolutivaCoordenacao ||
    (situacaoAtribuicaoPAAI && !dadosAtribuicaoResponsavel?.codigoRF);

  const planoAeeId = match?.params?.id;
  const labelBotaoSalvar =
    situacaoDevolutiva || !planoAeeId ? 'Salvar' : 'Alterar';

  const desabilitarBotaoSalvar = situacaoDevolutiva
    ? !devolutivaEmEdicao
    : desabilitarCamposPlanoAEE || !questionarioDinamicoEmEdicao;

  const desabilitarBotaoCancelar =
    situacaoDevolutiva || situacaoDevolutivaPAAI
      ? !devolutivaEmEdicao
      : desabilitarCamposPlanoAEE || !questionarioDinamicoEmEdicao;

  const dispatch = useDispatch();

  const limparDevolutiva = () => {
    dispatch(limparDadosDevolutiva());
    dispatch(setDevolutivaEmEdicao(false));
  };

  const salvarDevolutiva = async funcao => {
    const resposta = await funcao().catch(e => erros(e));

    if (resposta?.data) {
      sucesso('Registro salvo com sucesso');
      limparDevolutiva();
      history.push(RotasDto.RELATORIO_AEE_PLANO);
    }
  };

  const escolherAcaoDevolutivas = () => {
    if (situacaoDevolutiva) {
      salvarDevolutiva(ServicoPlanoAEE.salvarDevolutivaCP);
      return;
    }
    if (situacaoAtribuicaoPAAI) {
      salvarDevolutiva(ServicoPlanoAEE.atribuirResponsavel);
      return;
    }
    if (situacaoDevolutivaPAAI) {
      salvarDevolutiva(ServicoPlanoAEE.salvarDevolutivaPAAI);
    }
  };

  const onClickVoltar = async () => {
    if (questionarioDinamicoEmEdicao || devolutivaEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );

      if (confirmou) {
        if (devolutivaEmEdicao) {
          escolherAcaoDevolutivas();
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
        limparDevolutiva();
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
        if (devolutivaEmEdicao) {
          limparDevolutiva();
          dispatch(setAtualizarPlanoAEEDados(true));
          return;
        }
        QuestionarioDinamicoFuncoes.limparDadosOriginaisQuestionarioDinamico();
      }
    }
  };

  const onClickSalvar = async () => {
    if (situacaoDevolutiva) {
      salvarDevolutiva(ServicoPlanoAEE.salvarDevolutivaCP);
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
        sucesso('Solicitação de encerramento realizada com sucesso');
        dispatch(setAtualizarPlanoAEEDados(resposta?.data));
      }
    }
  };

  const onClickEncerrarPlano = async () => {
    const resposta = await ServicoPlanoAEE.salvarDevolutivaPAAI().catch(e =>
      erros(e)
    );
    if (resposta?.data) {
      sucesso('Registro salvo com sucesso');
      limparDevolutiva();
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
          !dadosDevolutiva?.podeEditarParecerPAAI ||
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
