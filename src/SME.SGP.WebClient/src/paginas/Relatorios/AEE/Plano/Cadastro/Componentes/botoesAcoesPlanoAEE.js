import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes';
import { confirmar, erros, history, sucesso } from '~/servicos';
import { RotasDto, situacaoPlanoAEE } from '~/dtos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import QuestionarioDinamicoFuncoes from '~/componentes-sgp/QuestionarioDinamico/Funcoes/QuestionarioDinamicoFuncoes';

const BotoesAcoesPlanoAEE = props => {
  const { match } = props;

  const questionarioDinamicoEmEdicao = useSelector(
    store => store.questionarioDinamico.questionarioDinamicoEmEdicao
  );

  const desabilitarCamposPlanoAEE = useSelector(
    store => store.planoAEE.desabilitarCamposPlanoAEE
  );

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];
  const planoAeeId = match?.params?.id;
  console.log('planoAEEDados', planoAEEDados);

  const onClickVoltar = async () => {
    if (questionarioDinamicoEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmou) {
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
        history.push(RotasDto.RELATORIO_AEE_PLANO);
      }
    } else {
      history.push(RotasDto.RELATORIO_AEE_PLANO);
    }
  };

  const onClickCancelar = async () => {
    if (!desabilitarCamposPlanoAEE && questionarioDinamicoEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        QuestionarioDinamicoFuncoes.limparDadosOriginaisQuestionarioDinamico();
      }
    }
  };

  const onClickSalvar = async () => {
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
      await ServicoPlanoAEE.encerrarPlano(planoAeeId).catch(e => erros(e));
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
        disabled={desabilitarCamposPlanoAEE || !questionarioDinamicoEmEdicao}
      />
      <Button
        id="btn-salvar"
        label={match?.params?.id ? 'Alterar' : 'Salvar'}
        color={Colors.Azul}
        border
        bold
        onClick={onClickSalvar}
        disabled={desabilitarCamposPlanoAEE || !questionarioDinamicoEmEdicao}
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
