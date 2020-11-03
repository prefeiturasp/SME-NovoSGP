import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerTabsDashboardEscolaAqui = styled.div`
  .ant-tabs-nav {
    width: 33.33% !important;
  }
`;

export const DataUltimaAtualizacao = styled.span`
  background-color: ${Base.Roxo};
  border: solid 0.5px ${Base.Roxo};
  border-radius: 3px;
  color: ${Base.Branco};
  font-weight: bold;
  padding: 0px 5px 0px 5px;
`;

export const LegendaGrafico = styled.div`
  font-size: 18px !important;
  font-family: 'Roboto' !important;
  font-weight: 700;
  color: #42474a;

  .legend-scale ul {
    margin: 0;
    margin-bottom: 5px;
    padding: 0;
    float: left;
    list-style: none;
  }

  .legend-scale ul li {
    font-size: 80%;
    list-style: none;
    margin-left: 0;
    line-height: 18px;
    margin-bottom: 2px;
  }

  .legend-labels {
    text-align: left;
  }

  ul.legend-labels li span {
    display: block;
    float: left;
    height: 16px;
    width: 30px;
    margin-right: 5px;
    margin-left: 0;
  }
`;
