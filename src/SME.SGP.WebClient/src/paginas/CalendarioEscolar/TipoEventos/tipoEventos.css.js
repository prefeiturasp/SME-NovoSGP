import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Div = styled.div`
  .select-local {
  }
`;

export const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;

export const Icone = styled.i`
  color: ${Base.CinzaMako};
  cursor: pointer;
`;

export const Busca = styled(Icone)`
  left: 10px;
  max-height: 25px;
  max-width: 15px;
  padding: 1rem;
  right: 0;
  top: -2px;
`;

export const CampoTexto = styled.input`
  color: ${Base.CinzaBotao};
  font-size: 14px;
  padding-left: 40px;
  &[type='radio'] {
    background: ${Base.Branco};
    border: 1px solid ${Base.CinzaDesabilitado};
  }
`;
