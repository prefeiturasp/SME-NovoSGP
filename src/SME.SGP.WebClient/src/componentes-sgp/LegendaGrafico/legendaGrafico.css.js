import styled from 'styled-components';

export const ContainerLegendaGrafico = styled.div`
  font-size: 18px !important;
  font-family: 'Roboto' !important;
  font-weight: 700;
  color: #42474a;

  .legenda-container-conteudo {
    display: flex;
    align-items: center;
  }

  .legenda-container ul {
    margin: 0;
    margin-bottom: 5px;
    padding: 0;
    float: left;
    list-style: none;
    display: ${props => (props.orizontal ? 'flex' : 'block')};
  }

  .legenda-container ul li {
    font-size: 80%;
    list-style: none;
    margin-left: 0;
    line-height: 18px;
    margin-bottom: 2px;
  }

  .legenda-labels {
    text-align: left;
  }

  ul.legenda-labels li span {
    display: block;
    float: left;
    height: 16px;
    width: 30px;
    margin-right: 5px;
    margin-left: 0;
  }
`;
