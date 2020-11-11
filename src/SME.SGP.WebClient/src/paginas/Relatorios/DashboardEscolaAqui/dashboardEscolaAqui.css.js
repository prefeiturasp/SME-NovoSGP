import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerTabsDashboardEscolaAqui = styled.div`
  .ant-tabs-nav {
    width: 33.33% !important;
  }

  .scrolling-chart {
    display: flex;
    flex-wrap: nowrap;
    overflow-x: auto;

    ::-webkit-scrollbar-track {
      background-color: #f4f4f4 !important;
    }

    ::-webkit-scrollbar {
      width: 4px !important;
      background-color: rgba(229, 237, 244, 0.71) !important;
      border-radius: 2.5px !important;
    }

    ::-webkit-scrollbar-thumb {
      background: #a8a8a8 !important;
      border-radius: 3px !important;
    }
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

export const TituloGrafico = styled.div`
  text-align: center;
  font-size: 24px;
  color: #000000;
  font-weight: 700;
  margin: 10px;
`;

export const ContainerGraficoBarras = styled.div`
  height: 500px;
  min-width: 900px;
`;
