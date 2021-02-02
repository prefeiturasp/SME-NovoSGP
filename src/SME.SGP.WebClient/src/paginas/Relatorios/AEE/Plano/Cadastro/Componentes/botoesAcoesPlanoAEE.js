import React from 'react';
import Button from '~/componentes/button';
import { Colors } from '~/componentes';
import { history } from '~/servicos';
import { RotasDto } from '~/dtos';

const BotoesAcoesPlanoAEE = () => {
  const onClickVoltar = async () => {
    history.push(RotasDto.RELATORIO_AEE_PLANO);
  };

  const onClickCancelar = async () => {
    history.push(RotasDto.RELATORIO_AEE_PLANO);
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
      />
    </>
  );
};

export default BotoesAcoesPlanoAEE;
