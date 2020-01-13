import styled from 'styled-components';
import { Base } from '~/componentes';

const Div = styled.div`
  .select-local {
    max-width: 185px;
  }
`;

const Icone = styled.i`
  color: ${Base.CinzaMako};
  cursor: pointer;
`;

const Busca = styled(Icone)`
  left: 10px;
  max-height: 25px;
  max-width: 15px;
  padding: 1rem;
  right: 0;
  top: -2px;
`;

const CampoTexto = styled.input`
  color: ${Base.CinzaBotao};
  font-size: 14px;
  padding-left: 40px;
  &[type='radio'] {
    background: ${Base.Branco};
    border: 1px solid ${Base.CinzaDesabilitado};
  }
`;

const Row = styled.div`
  [class*='col-'] {
    padding: 0 8px !important;
  }

  [class*='col-']:first-child {
    padding-left: 0px !important;
  }

  [class*='col-']:last-child {
    padding-right: 0px !important;
  }
`;

export { Div, Icone, Busca, CampoTexto, Row };
