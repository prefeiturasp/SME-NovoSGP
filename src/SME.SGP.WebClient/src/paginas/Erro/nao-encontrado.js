import React from 'react';
import Card from '~/componentes/card';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import styled from 'styled-components';

const NaoEncontrado = () => {
  const Corpo = styled.div`
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  min-height: 400px;
  font-size:16px;
  span{
    padding-bottom:10px !important
  }

  .msg-principal{
    font-size: 24px;
  }

  .not-found{
    font-size: 70px;
    padding-bottom: 10px;
  }

  `;

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  return (
    <Card>
      <Corpo className="col-md-12">
        <i className="far fa-frown not-found"></i>
        {/* <span className="not-found">404</span> */}
        <span className="msg-principal">Ocorreu um erro!</span>
        <span>A página que você tentou acessar não está disponível no momento.</span>
        <Button
          label="Voltar"
          icon="arrow-left"
          color={Colors.Azul}
          border
          className="mr-2"
          onClick={onClickVoltar}
        />
      </Corpo>
    </Card>
  )
}

export default NaoEncontrado;
