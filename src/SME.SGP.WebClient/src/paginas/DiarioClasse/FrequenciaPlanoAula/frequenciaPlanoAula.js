import React, { useEffect, useState } from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import PlanoAula from '../PlanoAula/plano-aula';

const FrequenciaPlanoAula = () => {
  const onClickVoltar = () => {
    console.log('onClickVoltar');
  };

  const onClickCancelar = () => {
    console.log('onClickCancelar');
  };

  const onClickSalvar = () => {
    console.log('onClickSalvar');
  };

  return (
    <>
      <Cabecalho pagina="Frequência/Plano de aula" />
      <Card>
        <div className="col-md-12 d-flex justify-content-end pb-4">
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
          />
          <Button
            label="Salvar"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickSalvar}
          />
        </div>
        <div className="col-sm-12 col-md-12 col-lg-12">
          <PlanoAula />
        </div>
      </Card>
    </>
  );
};

export default FrequenciaPlanoAula;
