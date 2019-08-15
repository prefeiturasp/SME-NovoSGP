import React from 'react';

import styled from 'styled-components';
import Button from '../../../componentes/button';
import Select from '../../../componentes/select';
import TextEditor from '../../../componentes/textEditor';

const BtnLink = styled.div`
  color: #686868;
  font-family: Roboto, FontAwesome;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  cursor: pointer;
  i {
    background-color: #6933ff;
    border-radius: 3px;
    color: white;
    font-size: 8px;
    padding: 2px;
    margin-left: 3px;
    position: fixed;
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
    cursor: pointer;
  }

  .btn-li-item {
    width: 30px;
    height: 30px;
    border: solid 0.8px #a4dafb;
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

  .opcao-selecionada {
    background-color: #a4dafb;
  }

  .id-item {
    padding-top: 3px;
  }
`;

export default function PlanoCiclo() {
  const urlPrefeitura = 'https://curriculo.prefeitura.sp.gov.br';
  const urlMatrizSaberes = `${urlPrefeitura}/matriz-de-saberes`;
  const urlODS = `${urlPrefeitura}/ods`;

  const listaTipo = [
    { label: 'Alfabetização', valor: '0' },
    { label: 'Interdiciplinar', valor: '1' },
    { label: 'Autoral', valor: '2' },
  ];

  const listaMatriz = [
    { id: 1, descricao: 'Pensamento Científico, Crítico e Criativo' },
    { id: 2, descricao: 'Resolução de Problemas' },
    { id: 3, descricao: 'Comunicação' },
    { id: 4, descricao: 'Autoconhecimento e Autocuidado' },
    { id: 5, descricao: 'Autonomia e Determinação' },
    { id: 6, descricao: 'Abertura à Diversidade' },
    { id: 7, descricao: 'Responsabilidade e Participação' },
    { id: 8, descricao: 'Empatia e Colaboração' },
    { id: 9, descricao: 'Repertório Cultural' },
  ];

  const listaMatrizSaberesSeleciondo = [
    { id: 3, descricao: 'Comunicação' },
    { id: 9, descricao: 'Repertório Cultural' },
  ];

  const listaODS = [
    { id: 1, descricao: 'Erradicação da Pobreza' },
    { id: 2, descricao: 'Fome zero e agricultura sustentável' },
    { id: 3, descricao: 'Saúde e Bem Estar' },
    { id: 4, descricao: 'Educação de Qualidade' },
    { id: 5, descricao: 'Igualdade de Genêro' },
    { id: 6, descricao: 'Água Potável e Saneamento' },
    { id: 7, descricao: 'Energia Limpa e Acessível' },
    { id: 8, descricao: 'Trabalho decente e crescimento econômico' },
    { id: 9, descricao: 'Indústria, inovação, e infraestrutura' },
    { id: 10, descricao: 'Redução das desigualdades' },
    { id: 11, descricao: 'Cidades e comunidades sustentáveis' },
    { id: 12, descricao: 'Consumo e produção responsáveis' },
    { id: 13, descricao: 'Ação contra a mudança global do clima' },
    { id: 14, descricao: 'Vida na água' },
    { id: 15, descricao: 'Vida terrestre' },
    { id: 16, descricao: 'Paz, justiça e instituições eficazes' },
    { id: 17, descricao: 'Parcerias e meios de implementação' },
  ];

  const listaODSSelecionada = [
    { id: 1, descricao: 'Erradicação da Pobreza' },
    { id: 9, descricao: 'Indústria, inovação, e infraestrutura' },
    { id: 17, descricao: 'Parcerias e meios de implementação' },
  ];

  function setTipo(event) {
    console.log(event.target.value);
  }

  function teste(value) {
    console.log(value);
  }

  function irParaLinkExterno(link) {
    window.open(link, '_blank');
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
                  lista={listaTipo}
                  value="valor"
                  label="label"
                  onChange={setTipo}
                />
              </div>
            </div>
          </div>
          <div className="col-md-6 d-flex justify-content-end">
            <Button
              label="Voltar"
              icon="arrow-left"
              className="mr-2 btn-outline-voltar"
              border
            />
            <Button
              label="Cancelar"
              className="mr-2 btn-outline-cancelar"
              border
            />
            <Button label="Salvar" className="btn-outline-salvar" border />
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
            <TextEditor onChange={teste} modules={modules} />
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
                        <React.Fragment key={item.id}>
                          <li>
                            {
                              <span
                                className={
                                  listaMatrizSaberesSeleciondo.find(
                                    matriz => matriz.id === item.id
                                  )
                                    ? 'btn-li-item btn-li-item-matriz opcao-selecionada'
                                    : 'btn-li-item btn-li-item-matriz'
                                }
                              >
                                <div className="id-item">{item.id}</div>
                              </span>
                            }
                            {item.descricao}
                          </li>
                        </React.Fragment>
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
                        <React.Fragment key={item.id}>
                          <li>
                            {
                              <span
                                className={
                                  listaODSSelecionada.find(
                                    ods => ods.id === item.id
                                  )
                                    ? 'btn-li-item btn-li-item-ods opcao-selecionada'
                                    : 'btn-li-item btn-li-item-ods'
                                }
                              >
                                <div className="id-item">{item.id}</div>
                              </span>
                            }
                            {item.descricao}
                          </li>
                        </React.Fragment>
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
