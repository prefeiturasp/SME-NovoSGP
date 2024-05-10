import React from 'react';
import styled from 'styled-components';
import t from 'prop-types';

// Componentes
import { Base } from '~/componentes/colors';

const Container = styled.div`
  width: 100%;
  display: flex;
  font-weight: bold;

  .topico {
    width: 50%;
    background-color: ${Base.CinzaFundo};
    border: 1px solid ${Base.CinzaDesabilitado};
    border-right: 1px solid ${Base.CinzaBadge};
    padding: 2rem 1rem;
    border-top-left-radius: 0.2rem;
		border-bottom-left-radius: 0.2rem;

		&::before {
			content: "Eixo",
			color: red;
			font-size: 200px;
			display: block;
			background-color: red;
		}
	
		&::after {
			content: 'Eixo',
			color: red;
			font-size: 20px;
		}
  }

  .pergunta {
    width: 50%;
    color: ${Base.Roxo};
    border: 1px solid ${Base.CinzaDesabilitado};
    display: flex;
    justify-content: center;
    align-items: center;
    border-top-right-radius: 0.2rem;
    border-bottom-right-radius: 0.2rem;
    padding: 0.7rem;
	}
`;

function EixoObjetivo({ eixo, objetivo }) {
  return (
    <Container>
      <div className="topico">{eixo.descricao}</div>
      <div className="pergunta">{objetivo.descricao}</div>
    </Container>
  );
}

EixoObjetivo.propTypes = {
  eixo: t.oneOfType([t.any]),
  objetivo: t.oneOfType([t.any]),
};

EixoObjetivo.defaultProps = {
  eixo: {},
  objetivo: {},
};

export default EixoObjetivo;
