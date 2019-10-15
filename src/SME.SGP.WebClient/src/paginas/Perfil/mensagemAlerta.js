import React from 'react';
import styled from 'styled-components';
import { Colors, Base } from '~/componentes/colors';
import Button from '~/componentes/button';

const MensagemAlerta = props => {
  const {
    oculto,
    confirmar,
    cancelar
  } = props;

  const Corpo = styled.div`
    border: 2px solid ${Base.VermelhoAlerta};
    border-radius: 4px;
    font-size: 16px;
    color: ${Base.VermelhoAlerta};
    padding: 15px;
    width: 100%;

    .titulo{
      font-size: 24px;
      margin-bottom: 10px;
    }
  `;

  return (
    <Corpo hidden={oculto}>
      <span className="titulo">Alerta</span><br />
      <span>Suas alterações não foram salvas, deseja salvar agora?</span>
      <div className="d-flex justify-content-end p-t-20">
        <Button
          key="btn-confirma-alteracao"
          label="Não"
          color={Colors.Azul}
          bold
          border
          className="mr-2 padding-btn-confirmacao"
          onClick={cancelar}
        />
        <Button
          key="btn-cancela-alteracao"
          label="Sim"
          color={Colors.Azul}
          bold
          className="padding-btn-confirmacao"
          onClick={confirmar}
        />
      </div>
    </Corpo>
  );
}

MensagemAlerta.defaultProps = {
  oculto: true
};

export default MensagemAlerta;
