import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { CampoTexto, Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import ListaPaginada from '~/componentes/listaPaginada/listaPaginada';
import SelectComponent from '~/componentes/select';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoCompensacaoAusencia from '~/servicos/Paginas/DiarioClasse/ServicoCompensacaoAusencia';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';

const CompensacaoAusenciaLista = () => {
  const usuario = useSelector(store => store.usuario);

  const { turmaSelecionada } = usuario;

  const [carregandoDisciplinas, setCarregandoDisciplinas] = useState(false);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [compensacoesSelecionadas, setCompensacoesSelecionadas] = useState([]);
  const [bimestreSelecionado, setBimestreSelecionado] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [nomeAtividade, setNomeAtividade] = useState('');
  const [nomeAluno, setNomeAluno] = useState('');
  const [listaBimestres, setListaBimestres] = useState([]);
  const [disciplinaIdSelecionada, setDisciplinaIdSelecionada] = useState(
    undefined
  );

  const montaExibicaoAlunos = dados => {
    // TODO Ver como vai vir a lista de alunos do back para montar conforme protótipo!
    return dados;
  };

  const colunas = [
    {
      title: 'Bimestre',
      dataIndex: 'bimestre',
      width: '10%',
    },
    {
      title: 'Atividade',
      dataIndex: 'atividade',
      width: '30%',
    },
    {
      title: 'Alunos',
      dataIndex: 'alunos',
      width: '60%',
      render: dados => montaExibicaoAlunos(dados),
    },
  ];

  const filtrar = useCallback(() => {
    const paramsFiltrar = {
      disciplina: disciplinaIdSelecionada,
      nomeAluno,
      nomeAtividade,
      bimestre: bimestreSelecionado,
    };
    setFiltro({ ...paramsFiltrar });
    console.log(paramsFiltrar);
  }, [disciplinaIdSelecionada, nomeAluno, nomeAtividade, bimestreSelecionado]);

  const resetarFiltro = () => {
    setListaDisciplinas([]);
    setDisciplinaIdSelecionada(undefined);
    setNomeAluno('');
    setNomeAtividade('');
    setDesabilitarDisciplina(false);
    setBimestreSelecionado(undefined);
  };

  useEffect(() => {
    const obterDisciplinas = async () => {
      setCarregandoDisciplinas(true);
      const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaSelecionada.turma
      );
      if (disciplinas.data && disciplinas.data.length) {
        setListaDisciplinas(disciplinas.data);
      } else {
        setListaDisciplinas([]);
      }
      if (disciplinas.data && disciplinas.data.length === 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaIdSelecionada(
          String(disciplina.codigoComponenteCurricular)
        );
        setDesabilitarDisciplina(true);
      }
      setCarregandoDisciplinas(false);
    };

    if (turmaSelecionada.turma) {
      resetarFiltro();
      obterDisciplinas(turmaSelecionada.turma);
    } else {
      resetarFiltro();
    }

    let listaBi = [];
    if (turmaSelecionada.modalidade == modalidade.EJA) {
      listaBi = [
        { valor: 1, descricao: '1° Bimestre' },
        { valor: 2, descricao: '2° Bimestre' },
      ];
    } else {
      listaBi = [
        { valor: 1, descricao: '1° Bimestre' },
        { valor: 2, descricao: '2° Bimestre' },
        { valor: 3, descricao: '3° Bimestre' },
        { valor: 4, descricao: '4° Bimestre' },
      ];
    }
    setListaBimestres(listaBi);
  }, [turmaSelecionada.turma, turmaSelecionada.modalidade]);

  useEffect(() => {
    filtrar();
  }, [disciplinaIdSelecionada, filtrar]);

  const onChangeDisciplinas = disciplinaId => {
    if (!disciplinaId) {
      setNomeAluno('');
      setNomeAtividade('');
      setBimestreSelecionado(undefined);
    }
    setDisciplinaIdSelecionada(disciplinaId);
  };

  const onChangeNomeAtividade = e => {
    setNomeAtividade(e.target.value);
  };

  const onChangeNomeAluno = e => {
    setNomeAluno(e.target.value);
  };

  const onChangeBimestre = bimestre => {
    setBimestreSelecionado(bimestre);
  };

  const onClickEditar = compoensacao => {
    history.push(`compensacao-ausencia/editar/${compoensacao.id}`);
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  // TODO Testar quando tiver o back pronto!
  const onClickExcluir = async () => {
    if (compensacoesSelecionadas && compensacoesSelecionadas.length > 0) {
      const listaExcluir = compensacoesSelecionadas.map(
        item => item.nomeAtividade
      );
      const confirmadoParaExcluir = await confirmar(
        'Excluir compensação',
        listaExcluir,
        `Deseja realmente excluir ${
          compensacoesSelecionadas.length > 1
            ? 'estas compensações'
            : 'esta compensação'
        }?`,
        'Excluir',
        'Cancelar'
      );
      if (confirmadoParaExcluir) {
        const idsDeletar = compensacoesSelecionadas.map(c => c.id);
        const excluir = await ServicoCompensacaoAusencia.deletar(
          idsDeletar
        ).catch(e => erros(e));
        if (excluir && excluir.status === 200) {
          const mensagemSucesso = `${
            compensacoesSelecionadas.length > 1
              ? 'Compensações excluídas'
              : 'Compensação excluída'
          } com sucesso.`;
          sucesso(mensagemSucesso);
          setCompensacoesSelecionadas([]);
          filtrar();
        }
      }
    }
  };

  const onSelecionarItems = items => {
    setCompensacoesSelecionadas(items);
  };

  const onClickNovo = () => {
    history.push(`compensacao-ausencia/novo`);
  };

  return (
    <>
      {usuario && turmaSelecionada.turma ? (
        ''
      ) : (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'compensacao-selecione-turma',
            mensagem: 'Você precisa escolher uma turma.',
            estiloTitulo: { fontSize: '18px' },
          }}
          className="mb-2"
        />
      )}
      <Cabecalho pagina="Compensação de Ausência" />
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
                label="Excluir"
                color={Colors.Vermelho}
                border
                className="mr-2"
                onClick={onClickExcluir}
                disabled={
                  compensacoesSelecionadas &&
                  compensacoesSelecionadas.length < 1
                }
              />
              <Button
                label="Novo"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickNovo}
                disabled={
                  !turmaSelecionada.turma ||
                  (turmaSelecionada.turma && listaDisciplinas.length < 1)
                }
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
              <Loader loading={carregandoDisciplinas} tip="">
                <SelectComponent
                  id="disciplina"
                  name="disciplinaId"
                  lista={listaDisciplinas}
                  valueOption="codigoComponenteCurricular"
                  valueText="nome"
                  valueSelect={disciplinaIdSelecionada}
                  onChange={onChangeDisciplinas}
                  placeholder="Disciplina"
                  disabled={desabilitarDisciplina}
                />
              </Loader>
            </div>
            {disciplinaIdSelecionada ? (
              <>
                <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                  <SelectComponent
                    id="bimestre"
                    name="bimestre"
                    onChange={onChangeBimestre}
                    valueOption="valor"
                    valueText="descricao"
                    lista={listaBimestres}
                    placeholder="Bimestre"
                    valueSelect={bimestreSelecionado}
                  />
                </div>
                <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                  <CampoTexto
                    name="nomeAtividade"
                    id="nomeAtividade"
                    placeholder="Nome da Atividade"
                    iconeBusca
                    allowClear
                    onChange={onChangeNomeAtividade}
                    value={nomeAtividade}
                  />
                </div>
                <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                  <CampoTexto
                    name="nomeAtividade"
                    id="nomeAtividade"
                    placeholder="Nome do Aluno"
                    iconeBusca
                    allowClear
                    onChange={onChangeNomeAluno}
                    value={nomeAluno}
                  />
                </div>
              </>
            ) : (
              ''
            )}
          </div>
        </div>
        <div className="col-md-12 pt-2">
          {disciplinaIdSelecionada ? (
            <ListaPaginada
              // TODO MUdal URL
              url="v1/compensacoes"
              id="lista-compensacao"
              colunaChave="id"
              colunas={colunas}
              filtro={filtro}
              onClick={onClickEditar}
              multiSelecao
              selecionarItems={onSelecionarItems}
            />
          ) : (
            ''
          )}
        </div>
      </Card>
    </>
  );
};

export default CompensacaoAusenciaLista;
