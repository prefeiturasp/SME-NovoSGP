import styled from 'styled-components';
import { darken } from 'polished';

<<<<<<< HEAD
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
=======
const cores = {
  basico: {
    fundo: '#dadada',
    fonte: '#42474A',
  },
  informativo1: {
    fundo: '#086397',
    fonte: 'white',
  },
  informativo2: {
    fundo: '#10A3FB',
    fonte: 'white',
  },
  alerta: {
    fundo: '#D06D12',
    fonte: 'white',
  },
  erro: {
    fundo: '#B40C02',
    fonte: 'white',
  },
  atencao: {
    fundo: '#FFFF30',
    fonte: '#42474A',
  },
  sucesso: {
    fundo: '#297805',
    fonte: 'white',
  },
  cancelar: {
    fundo: '#040404',
>>>>>>> 1d7bad66a617c18e28055710b061c8afbe92c1ce
    fonte: 'white',
  },
};

export const TagEstilo = styled.div`
<<<<<<< HEAD
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
=======
  .ant-tag {
    font-weight: bold;
    display: inline-flex;
    align-items: center;
    font-size: 12px;
    justify-content: center;
    margin: 0;
    background-color: ${props => cores[props.tipo].fundo};
    border-color: ${props => darken(0.1, cores[props.tipo].fundo)};
    color: ${props => cores[props.tipo].fonte};
  }

  .ant-tag-hidden {
    display: none !important;
  }

  &.fluido {
    .ant-tag {
      display: flex;
      justify-content: space-between;
    }
  }

  &.fluido.centralizado {
    .ant-tag {
      justify-content: center;
    }
  }

  &.medio {
    .ant-tag {
      padding: 6px;
      font-size: 13px;
    }
  }

  &.grande {
    .ant-tag {
      padding: 8px;
      font-size: 14px;
    }
>>>>>>> 1d7bad66a617c18e28055710b061c8afbe92c1ce
  }
`;
