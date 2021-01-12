import React from 'react';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { RotasDto } from '~/dtos';
import history from '~/servicos/history';

const BotoesAcoesEncaminhamentoAEE = () => {
  const onClickSalvar = async () => {
    // TODO
    console.log('onClickSalvar');
  };

  const onClickVoltar = async () => {
    // TODO
    console.log('onClickVoltar');
    history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
  };

  const onClickCancelar = async () => {
    // TODO
    console.log('onClickCancelar');
  };

  const onClickExcluir = async () => {
    // TODO
    console.log('onClickExcluir');
  };

  const onClickEnviar = async () => {
    // TODO
    console.log('onClickEnviar');
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
      />
      <Button
        id="btn-excluir"
        label="Excluir"
        color={Colors.Vermelho}
        border
        className="mr-3"
        onClick={onClickExcluir}
      />
      <Button
        id="btn-salvar"
        label="Salvar"
        color={Colors.Azul}
        border
        bold
        className="mr-3"
        onClick={onClickSalvar}
      />
      <Button
        id="btn-enviar"
        label="Enviar"
        color={Colors.Roxo}
        border
        bold
        onClick={onClickEnviar}
      />
    </>
  );
};

export default BotoesAcoesEncaminhamentoAEE;
