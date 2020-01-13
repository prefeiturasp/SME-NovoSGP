import styled from 'styled-components';
import { Base } from '../../../componentes/colors';

export const Titulo = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 24px;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  letter-spacing: normal;
  color: #42474a;
  margin-bottom: 10px;
`;
export const TituloAno = styled.span`
  font-weight: bold;
  font-size: 16px;
  color: ${Base.Roxo};
`;

export const Planejamento = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 11px;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  letter-spacing: normal;
  color: #c8c8c8;
  padding-top: 6px;
`;

export const ContainerBimestres = styled.div`
  .ant-collapse-item {
    background-color: #fff;
    padding-bottom: 10px;
    border-top-right-radius: 0.25rem !important;
    border: 0px !important;
  }

  .ant-collapse-content-active {
    border-bottom-left-radius: 0.25rem !important;
    border-bottom-right-radius: 0.25rem !important;
    border-left: 1px solid ${Base.CinzaDesabilitado} !important;
    border-bottom: 1px solid ${Base.CinzaDesabilitado} !important;
    border-right: 1px solid ${Base.CinzaDesabilitado} !important;
  }

  .ant-collapse-header {
    border-top: 1px solid ${Base.CinzaDesabilitado} !important;
    border-right: 1px solid ${Base.CinzaDesabilitado} !important;
    border-bottom: 1px solid ${Base.CinzaDesabilitado} !important;
    border-left: 8px solid ${Base.AzulBordaCard} !important;
    border-radius: 0.25rem !important;
    height: 75px;
    color: ${Base.CinzaBadge};
    font-size: 16px !important;
    font-family: FontAwesome, Roboto, sans-serif !important;
    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;
    &#text {
      margin-top: 20px;
    }
  }
`;
