import React from 'react';
import { Button, Colors } from '~/componentes';

const BotoesAcoes = () => {
  const onClickVoltar = () => {};
  const onClickCancelar = () => {};
  const onClickCadastrar = () => {};

  return (
    <>
      <Button
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-2"
        onClick={onClickCancelar}
        // disabled={!modoEdicao || somenteConsulta}
      />
      <Button
        label="Cadastrar"
        color={Colors.Roxo}
        bold
        className="mr-2"
        onClick={onClickCadastrar}
        // disabled={!modoEdicao || somenteConsulta}
      />
    </>
  );
};

export default BotoesAcoes;
