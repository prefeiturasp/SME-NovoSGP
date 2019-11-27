import styled from 'styled-components';
import { darken } from 'polished';

const temas = {
  basico: {
    background: '#dadada',
    fonte: '#42474A',
  },
  informativo1: {
    background: '#086397',
    fonte: 'white',
  },
  informativo2: {
    background: '#10A3FB',
    fonte: 'white',
  },
  alerta: {
    background: '#D06D12',
    fonte: 'white',
  },
  erro: {
    background: '#B40C02',
    fonte: 'white',
  },
  atencao: {
    background: '#FFFF30',
    fonte: '#42474A',
  },
  sucesso: {
    background: '#297805',
    fonte: 'white',
  },
  cancelar: {
    background: '#040404',
    fonte: 'white',
  },
};

export const TagEstilo = styled.div`
  width: 100%;
  border-radius: 3px;
  border: 1px solid ${props => darken(0.05, temas[props.tipo].background)};
  background-color: ${props => temas[props.tipo].background};
  color: ${props => temas[props.tipo].fonte};
  padding: 10px;
  margin: 4px;
  text-align: center;
  font-weight: bold;
  &.inverted {
    border-color: ${props => temas[props.tipo].background};
    color: ${props => temas[props.tipo].background};
    background-color: transparent;
  }
`;
