import styled, { css } from 'styled-components';
import { Base } from '../../../componentes/colors';

export const BtnLink = styled.div`
  color: #686868;
  font-family: Roboto, FontAwesome;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  cursor: pointer;
  i {
    background-color: ${Base.Roxo};
    border-radius: 3px;
    color: white;
    font-size: 8px;
    padding: 2px;
    margin-left: 3px;
    position: absolute;
    margin-top: 3px;
  }
`;

export const ListaItens = styled.div`
  ul {
    list-style: none;
    columns: 2;
    -webkit-columns: 2;
    -moz-columns: 2;
  }

  li {
    margin-bottom: 5px;
  }

  .aling-center {
    align-items: center;
  }

  font-size: 12px;
  color: #42474a;

  .btn-li-item {
    width: 32px;
    height: 32px;
    border: solid 0.8px ${Base.AzulAnakiwa};
    display: inline-block;
    font-weight: bold;
    margin-right: 5px;
    text-align: center;
    font-size: 15px;
  }

  .btn-li-item-matriz {
    border-radius: 50%;
  }

  .btn-li-item-ods {
    border-radius: 0.25rem !important;
  }
`;

export const Badge = styled.span`
  cursor: pointer;
  padding-top: 6px;

  &[opcao-selecionada='true'] {
    background: ${Base.AzulAnakiwa};
  }
`;

export const TextArea = styled.div`
  textarea {
    height: 600px !important;
  }
`;

export const InseridoAlterado = styled.div`
  object-fit: contain;
  font-weight: bold;
  font-style: normal;
  font-size: 10px;
  color: #42474a;
  p {
    margin: 0px;
  }
`;

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
  margin-bottom: 3px;
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

export const Container = styled.div``;

export const IframeStyle = css`
  body {
    min-height: 500px !important;
  }
`;
