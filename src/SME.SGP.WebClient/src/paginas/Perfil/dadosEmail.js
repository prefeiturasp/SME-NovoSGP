import React, {useState} from 'react';
import CampoTexto from '~/componentes/campoTexto';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import styled from 'styled-components';

const DadosEmail = () => {

  const [email, setEmail] = useState('teste@teste.com');
  const [senha, setSenha] = useState('123456');

  const Campos = styled.div`
    margin-right: 10px;
    margin-left: 40px;
    .campo{
      margin-top: 50px;
    }

    .botao{
      margin-top: 25px;
    }
  `;

  return (
    <Campos>
      <div className="row campo w-100">
        <div className="col-md-10">
          <CampoTexto
            desabilitado={true}
            label="E-mail"
            value={email}
            placeholder="Insira um e-mail"
            onChange={() => { }}
            type="email"
          />
        </div>
        <div className="col-md-2 botao">
          <Button
            label="Editar"
            color={Colors.Roxo}
            border
            bold
            onClick={() => { }}
          />
        </div>
      </div>
      <div className="row campo w-100">
        <div className="col-md-10">
          <CampoTexto
            desabilitado={true}
            label="Senha"
            value={senha}
            className="col-11 campo"
            placeholder="Insira uma senha"
            onChange={() => { }}
            type="password"
          />
        </div>
        <div className="col-md-2 botao">
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
