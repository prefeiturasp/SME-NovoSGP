import styled from 'styled-components';
import { Base } from '~/componentes';

export const Corpo = styled.div`
  .negrito{
    font-weight: bold;
  }

  th, td {
    border: solid 1px ${Base.CinzaDesabilitado};
  }

  .fundo-cinza{
    background-color: ${Base.CinzaFundo};
  }
`;

export const CampoDesabilitado = styled.div`
  border-radius: 4px;
  border: 1px solid ${Base.CinzaDesabilitado};
  color: ${Base.CinzaDesabilitado} !important;
  width: 65px;
  height: 32px;
  text-align: center;
  span{
    margin-top: 10px;
  }
`;
