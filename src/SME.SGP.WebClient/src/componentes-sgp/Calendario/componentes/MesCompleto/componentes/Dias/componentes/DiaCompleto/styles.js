import styled from 'styled-components';

// Componentes
import { Base, Button } from '~/componentes';

export const DiaCompletoWrapper = styled.div`
  display: flex;
  overflow: hidden;
  max-height: 0;
  width: 100% !important;
  transition: all 0.9s;
  border-bottom: 1px solid ${Base.CinzaBordaCalendario};

  &.visivel {
    max-height: 1000px !important;
    height: auto !important;
    overflow: auto;
    padding: 0.1rem;
  }
`;

export const LinhaEvento = styled.div`
  display: flex;
  border-bottom: 1px solid ${Base.CinzaDesabilitado};
  padding: 0.4rem;
  cursor: pointer !important;
  border-radius: 0.2rem;
  transition: all 0.2s;

  .tituloEventoAula {
    display: flex;
    align-items: flex-start;
    padding-left: 0.5rem;
    font-weight: bold;
    flex: 1;
    flex-direction: column;

    .detalhesEvento {
      span {
        font-weight: normal;
      }
    }
  }

  .botoesEventoAula {
    display: flex;
    align-items: center;
  }

  &:hover {
    &:not(.evento) {
      background: ${Base.Roxo};
      color: white;
    }
  }
`;

export const Botao = styled(Button)`
  ${LinhaEvento}:not(.evento):hover & {
    background: transparent !important;
    border-color: ${Base.Branco} !important;
    color: ${Base.Branco} !important;

    &:hover {
      background: white !important;
      color: ${Base.Roxo} !important;
    }
  }
`;

export const BotoesAuxiliaresEstilo = styled.div`
  align-items: right;
  display: flex;
  justify-content: flex-end;
  padding: 0.7rem;
  width: 100%;
`;

export const Pilula = styled.div`
  font-size: 0.6rem;
  color: ${props => props.cor};
  background-color: ${props => props.fundo};
  border-radius: 0.2rem;
  padding: 0.1rem 0.3rem;
  margin: 0 5px;
  display: inline-flex;
`;
