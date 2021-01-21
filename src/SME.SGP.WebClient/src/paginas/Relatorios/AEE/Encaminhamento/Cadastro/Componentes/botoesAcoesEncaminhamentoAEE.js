import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { RotasDto } from '~/dtos';
import { confirmar, erros, sucesso } from '~/servicos';
import history from '~/servicos/history';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';

const BotoesAcoesEncaminhamentoAEE = props => {
  const { match } = props;

  const encaminhamentoAEEEmEdicao = useSelector(
    store => store.encaminhamentoAEE.encaminhamentoAEEEmEdicao
  );

  const onClickSalvar = async () => {
    const encaminhamentoId = match?.params?.id;
    ServicoEncaminhamentoAEE.salvarEncaminhamento(encaminhamentoId);
  };

  const onClickEnviar = async () => {
    const encaminhamentoId = match?.params?.id;
    ServicoEncaminhamentoAEE.salvarEncaminhamento(encaminhamentoId, true);
  };

  const onClickVoltar = async () => {
    if (encaminhamentoAEEEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja voltar para tela de listagem agora?'
      );
      if (confirmou) history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
    } else {
      history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
    }
  };

  const onClickCancelar = async () => {
    // TODO
    console.log('onClickCancelar');
  };

  const onClickExcluir = async () => {
    const encaminhamentoId = match?.params?.id;
    if (encaminhamentoId) {
      const confirmado = await confirmar(
        'Excluir',
        '',
        'Você tem certeza que deseja excluir este registro?'
      );

      if (confirmado) {
        const resposta = await ServicoEncaminhamentoAEE.excluirEncaminhamento(
          encaminhamentoId
        ).catch(e => erros(e));

        if (resposta?.status === 200) {
          sucesso('Registro excluído com sucesso');
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
        disabled={!encaminhamentoAEEEmEdicao}
      />
      <Button
        id="btn-excluir"
        label="Excluir"
        color={Colors.Vermelho}
        border
        className="mr-3"
        onClick={onClickExcluir}
        disabled={!match?.params?.id}
      />
      <Button
        id="btn-salvar"
        label="Salvar"
        color={Colors.Azul}
        border
        bold
        className="mr-3"
        onClick={onClickSalvar}
        disabled={!encaminhamentoAEEEmEdicao}
      />
      <Button
        id="btn-enviar"
        label="Enviar"
        color={Colors.Roxo}
        border
        bold
        onClick={onClickEnviar}
        disabled={!encaminhamentoAEEEmEdicao}
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
