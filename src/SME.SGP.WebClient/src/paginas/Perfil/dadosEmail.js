import React from 'react';
import CampoTexto from '~/componentes/campoTexto';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import styled from 'styled-components';

const DadosEmail = () => {

  const Campos = styled.div`
    margin-right: 10px;
  `;

  return (
    <Campos>
      <div className="row">
        <div className="col-md-10">
          <CampoTexto
            label="E-mail"
            placeholder="Insira um e-mail"
            onChange={() => { }}
            type="email"
          />
        </div>
        <div className="col-md-2">
          <Button
            label="Editar"
            color={Colors.Roxo}
            border
            bold
            onClick={() => { }}
          />
        </div>
      </div>
      <div className="row">
        <div className="col-md-10">
          <CampoTexto
            label="Senha"
            className="col-11 campo"
            placeholder="Insira uma senha"
            onChange={() => { }}
          />
        </div>
        <div className="col-md-2">
          <Button
            label="Editar"
            color={Colors.Roxo}
            border
            bold
            onClick={() => { }}
          />
        </div>
      </div>
    </Campos>
  );
}

export default DadosEmail;
