import styled from 'styled-components';
import { darken } from 'polished';

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
    fonte: 'white',
  },
};

export const TagEstilo = styled.div`
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
  }
`;
