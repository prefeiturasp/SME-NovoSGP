import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import TabsComponent from '~/componentes/tabs/tabs';
import Avaliacao from '~/componentes-sgp/avaliacao/avaliacao';

const Notas = () => {

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [listaTabs, setListaTabs] = useState([]);

  useEffect(() => {
    const obterDisciplinas = async () => {
      const url = `v1/professores/123/turmas/${turmaId}/disciplinas`;
      const disciplinas = await api.get(url);

      setListaDisciplinas(disciplinas.data);
      if (disciplinas.data && disciplinas.data.length == 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
        setDesabilitarDisciplina(true);
        montaListaTabs();
      }
    };

    if (turmaId) {
      setDisciplinaSelecionada(undefined);
      obterDisciplinas();
      // TODO - TESTE
    } else {
      // TODO - Resetar tela
      setListaDisciplinas([]);
      setDesabilitarDisciplina(false);
      setDisciplinaSelecionada(undefined);
    }
  }, [turmaSelecionada.turma]);

  const onClickVoltar = ()=> {
    console.log('onClickVoltar');
  }

  const onClickCancelar = ()=> {
    console.log('onClickCancelar');
  }

  const onClickSalvar = ()=> {
    console.log('onClickSalvar');
  }

  const onChangeDisciplinas =  disciplinaId => {
    setDisciplinaSelecionada(disciplinaId);
  };

  const onChangeTab = (item) => {
    console.log(item);
  }

  const dadosBimentreUm =
  {
      avaliacoes: [
        {codigo: 1, nome: 'Avaliação 01', podeEditar: true, tipoDescricao: 'Pesquisa', data: '07/10/2019'},
        {codigo: 2, nome: 'Avaliação 02', podeEditar: true, tipoDescricao: 'Seminário', data: '28/10/2019'},
        {codigo: 3, nome: 'Avaliação 03', podeEditar: true, tipoDescricao: 'Trabalho em grupo', data: '01/11/2019'},
        {codigo: 4, nome: 'Avaliação 04', podeEditar: true, tipoDescricao: 'Teste', data: '09/11/2019'},
      ],
      alunos: [
        {
          codigo: 1,
          nome: 'Alvaro Ramos Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 2 },
            { nota: 10, conceito: 'S', tipoNota: 2, ausencia: true},
            { nota: 10, conceito: 'NS', tipoNota: 2 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 2,
          nome: 'Aline Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 2 },
            { nota: 10, conceito: 'S', tipoNota: 2},
            { nota: 10, conceito: 'NS', tipoNota: 2 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 3,
          nome: 'Bianca Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 2 },
            { nota: 10, conceito: 'S', tipoNota: 2},
            { nota: 10, conceito: 'NS', tipoNota: 2 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 4,
          nome: 'José Ramos Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 2 },
            { nota: 10, conceito: 'S', tipoNota: 2},
            { nota: 10, conceito: 'NS', tipoNota: 2 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 5,
          nome: 'Valentina Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 2 },
            { nota: 10, conceito: 'S', tipoNota: 2},
            { nota: 10, conceito: 'NS', tipoNota: 2 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 6,
          nome: 'Laura Ramos Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 2 },
            { nota: 10, conceito: 'S', tipoNota: 2},
            { nota: 10, conceito: 'NS', tipoNota: 2 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 7,
          nome: 'Angela Ramos Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 1 },
            { nota: 10, conceito: 'S', tipoNota: 1, ausencia: true },
            { nota: 10, conceito: 'NS', tipoNota: 1 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 8,
          nome: 'Marcos Ramos Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 1 },
            { nota: 10, conceito: 'S', tipoNota: 1},
            { nota: 10, conceito: 'NS', tipoNota: 1 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 9,
          nome: 'Jefferson Ramos Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 1 },
            { nota: 10, conceito: 'S', tipoNota: 1},
            { nota: 10, conceito: 'NS', tipoNota: 1 },
            { nota: 10, conceito: undefined },
          ]
        },
        {
          codigo: 10,
          nome: 'Júlio Ramos Grassi',
          notas: [
            { nota: 10, conceito: 'P', tipoNota: 1 },
            { nota: 10, conceito: 'S', tipoNota: 1},
            { nota: 10, conceito: 'NS', tipoNota: 1 },
            { nota: 10, conceito: undefined },
          ]
        },
      ]
    }
  const montaListaTabs = ()=> {

    const teste = [
      {
        nome: '1° Bimestre',
        conteudo: (
          <Avaliacao dados={dadosBimentreUm}></Avaliacao>
        )
      },
      {
        nome: '2° Bimestre',
        conteudo: (
          <Avaliacao dados={dadosBimentreUm}></Avaliacao>
        )
      },
    ]

    setListaTabs(teste);
  }

  return (
    <>
      <Cabecalho pagina="Lançamento de notas" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                label="Cancelar"
                color={Colors.Roxo}
                border
                className="mr-2"
                onClick={onClickCancelar}
              />
              <Button
                label="Salvar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickSalvar}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                lista={listaDisciplinas}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={disciplinaSelecionada}
                onChange={onChangeDisciplinas}
                placeholder="Disciplina"
                disabled={desabilitarDisciplina}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
              <TabsComponent onChangeTab={onChangeTab} listaTabs={listaTabs}>

              </TabsComponent>
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default Notas;
