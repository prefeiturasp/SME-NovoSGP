import styled from 'styled-components';
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

  font-size: 12px;
  color: #42474a;

  .btn-li-item {
    width: 30px;
    height: 30px;
    border: solid 0.8px ${Base.AzulAnakiwa};
    display: inline-block;
    font-weight: bold;
    margin-right: 5px;
    text-align: center;
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
  padding-top: 5.5px;

  &[opcao-selecionada='true'] {
    background: ${Base.AzulAnakiwa} !important;
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

export const Container = styled.div``;
