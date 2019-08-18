import React, { useEffect, useState } from 'react';

import styled from 'styled-components';
import Button from '../../../componentes/button';
import Select from '../../../componentes/select';
import TextEditor from '../../../componentes/textEditor';
import { Colors, Base } from '../../../componentes/colors';

import api from '../../../servicos/api';

const BtnLink = styled.div`
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

const ListaItens = styled.div`
  ul {
    list-style: none;
    columns: 2;
    -webkit-columns: 2;
    -moz-columns: 2;
  }

  li {
    margin-bottom: 5px;
  }

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

const Badge = styled.span`
  cursor: pointer;
  padding-top: 3px;

  &[opcao-selecionada='true'] {
    background: ${Base.AzulAnakiwa} !important;
  }
`;

export default function PlanoCiclo() {
  const urlPrefeitura = 'https://curriculo.prefeitura.sp.gov.br';
  const urlMatrizSaberes = `${urlPrefeitura}/matriz-de-saberes`;
  const urlODS = `${urlPrefeitura}/ods`;

  const [listaMatriz, setListaMatriz] = useState([]);
  const [listaODS, setListaODS] = useState([]);
  const [listaCiclos, setListaCiclos] = useState([]);
  const [listaMatrizSelecionda, setListaMatrizSelecionda] = useState([]);
  const [listaODSSelecionado, setListaODSSelecionado] = useState([]);

  useEffect(() => {
    async function carregarListas() {
      const matrizes = await api.get('v1/matrizes-saber');
      setListaMatriz(matrizes.data);

      const ods = await api.get('v1/objetivos-desenvolvimento-sustentavel');
      setListaODS(ods.data);

      const ciclos = await api.get('v1/ciclos');
      setListaCiclos(ciclos.data);
    }

    carregarListas();
  }, []);

  function addRemoverMatriz(event, matrizSelecionada) {
    event.target.setAttribute(
      'opcao-selecionada',
      event.target.getAttribute('opcao-selecionada') == 'true'
        ? 'false'
        : 'true'
    );

    let adicionarNovo = true;
    listaMatrizSelecionda.forEach((item, index) => {
      if (item.id === matrizSelecionada.id) {
        listaMatrizSelecionda.splice(index);
        adicionarNovo = false;
      }
    });
    if (adicionarNovo) {
      listaMatrizSelecionda.push(matrizSelecionada);
    }
    setListaMatrizSelecionda(listaMatrizSelecionda);
    console.log(listaMatrizSelecionda);
  }

  function addRemoverODS(event, odsSelecionado) {
    event.target.setAttribute(
      'opcao-selecionada',
      event.target.getAttribute('opcao-selecionada') == 'true'
        ? 'false'
        : 'true'
    );

    let adicionarNovo = true;
    listaODSSelecionado.forEach((item, index) => {
      if (item.id === odsSelecionado.id) {
        listaODSSelecionado.splice(index);
        adicionarNovo = false;
      }
    });
    if (adicionarNovo) {
      listaODSSelecionado.push(odsSelecionado);
    }
    setListaODSSelecionado(listaODSSelecionado);
    console.log(listaODSSelecionado);
  }

  function setTipo(event) {
    console.log(event.target.value);
  }

  function teste(value) {
    console.log(value);
  }

  function irParaLinkExterno(link) {
    window.open(link, '_blank');
  }

  function validaMatrizSelecionada() {
    listaMatriz.forEach(item => {
      const jaSelecionado = listaMatrizSelecionda.find(
        matriz => matriz.id == item.id
      );
      if (jaSelecionado) {
        return true;
      }
      return false;
    });
  }

  function validaODSSelecionado() {
    listaODS.forEach(item => {
      const jaSelecionado = listaODSSelecionado.find(
        matriz => matriz.id == item.id
      );
      if (jaSelecionado) {
        return true;
      }
      return false;
    });
  }

  const toolbarOptions = [
    ['bold', 'italic', 'underline'],
    [{ list: 'bullet' }, { list: 'ordered' }],
  ];

  const modules = {
    toolbar: toolbarOptions,
  };

  return (
    <>
      <div className="col-md-12">
        <div className="row mb-3">
          <div className="col-md-6">
            <div className="row">
              <div className="col-md-6">
                <Select
                  name="filtro-tipo"
                  id="filtro-tipo"
                  className="custom-select"
                  optionClass="custom-select-option"
                  lista={listaCiclos}
                  value="id"
                  label="descricao"
                  onChange={setTipo}
                />
              </div>
            </div>
          </div>
          <div className="col-md-6 d-flex justify-content-end">
            <Button
              label="Voltar"
              icon="arrow-left"
              color={Colors.Azul}
              border
              className="mr-3"
            />
            <Button
              label="Cancelar"
              color={Colors.Roxo}
              border
              bold
              className="mr-3"
            />
            <Button label="Salvar" color={Colors.Roxo} border bold disabled />
          </div>
        </div>

        <div className="row mb-3">
          <div className="col-md-6">
            Este é um espaço para construção coletiva. Considerem os diversos
            ritmos de aprendizagem para planejar e traçar o percurso de cada
            Ciclo de Aprendizagem.
          </div>
          <div className="col-md-6">
            Considerando as especificidades de cada Ciclo dessa Unidade Escolar
            e o Currículo da Cidade, selecione os itens da Matriz do Saber e dos
            Objetivos de Desenvolvimento e Sustentabilidade que contemplam as
            propostas que planejaram:
          </div>
        </div>

        <div className="row mb-3">
          <div className="col-md-6">
            <TextEditor
              onChange={teste}
              className="form-control"
              modules={modules}
              height={515}
            />
          </div>
          <div className="col-md-6 btn-link-plano-ciclo">
            <div className="col-md-12">
              <div className="row mb-3">
                <BtnLink onClick={() => irParaLinkExterno(urlMatrizSaberes)}>
                  Matriz de saberes
                  <i className="fas fa-share" />
                </BtnLink>
              </div>

              <div className="row">
                <ListaItens>
                  <ul>
                    {listaMatriz.map(item => {
                      return (
                        <li key={item.id}>
                          {
                            <Badge
                              className="btn-li-item btn-li-item-matriz"
                              opcao-selecionada={validaMatrizSelecionada}
                              onClick={e => addRemoverMatriz(e, item)}
                            >
                              {item.id}
                            </Badge>
                          }
                          {item.descricao}
                        </li>
                      );
                    })}
                  </ul>
                </ListaItens>
              </div>

              <hr className="row mb-3 mt-3" />

              <div className="row mb-3">
                <BtnLink onClick={() => irParaLinkExterno(urlODS)}>
                  Objetivos de Desenvolvimento Sustentável
                  <i className="fas fa-share" />
                </BtnLink>
              </div>
              <div className="row">
                <ListaItens>
                  <ul>
                    {listaODS.map(item => {
                      return (
                        <li key={item.id}>
                          {
                            <Badge
                              className="btn-li-item btn-li-item-ods"
                              opcao-selecionada={validaODSSelecionado}
                              onClick={e => addRemoverODS(e, item)}
                            >
                              {item.id}
                            </Badge>
                          }
                          {item.descricao}
                        </li>
                      );
                    })}
                  </ul>
                </ListaItens>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
