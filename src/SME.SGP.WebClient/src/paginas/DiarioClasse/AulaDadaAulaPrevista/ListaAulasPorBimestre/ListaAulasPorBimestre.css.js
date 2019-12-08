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
  justify-content:center;
  align-items: center;
  display:flex;
  
  span{
    justify-content:center;
    align-items: center;
    display:flex;
    border-radius: 4px;
    border: 1px solid ${Base.CinzaDesabilitado};
    color: ${Base.CinzaDesabilitado} !important;
    width: 65px;
    height: 32px;
  }
`;

export const CampoEditavel = styled.div`
  justify-content:center;
  align-items: center;
  display:flex;
  padding-left: 1px;
  div{
    height: 32px !important;
  }
  input{
    justify-content:center;
    align-items: center;
    display:flex;
    width: 65px;
    height: 32px !important;
  }
`;

export const CampoAlerta = styled.div`
  justify-content: left;
  align-items: center;
  display:flex;
  background-color: ${Base.LaranjaAlerta};
  border-radius: 4px;
  width: 105px;
  height: 34px;
  .icone{
    align-items: center;
    i{
      font-size: 12px;
      color: ${Base.Branco};
    }
  }
`
