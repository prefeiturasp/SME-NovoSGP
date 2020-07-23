import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import ListaPaginada from '~/componentes/listaPaginada/listaPaginada';
import SelectComponent from '~/componentes/select';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import { erro, erros, sucesso } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoPendenciasFechamento from '~/servicos/Paginas/Fechamento/ServicoPendenciasFechamento';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import situacaoPendenciaDto from '~/dtos/situacaoPendenciaDto';
import {
  AprovadoList,
  PendenteList,
  ResolvidoList,
} from './situacaoFechamento.css';
import api from '~/servicos/api';
import RotasDto from '~/dtos/rotasDto';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import { BotaoImprimir } from './pendenciasFechamentoLista.css';
import ServicoRelatorioPendencias from '~/servicos/Paginas/Relatorios/Pendencias/ServicoRelatorioPendencias';

const PendenciasFechamentoLista = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const permissoesTela = usuario.permissoes[RotasDto.PENDENCIAS_FECHAMENTO];
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [lista, setLista] = useState([]);

  const [exibirLista, setExibirLista] = useState(false);
  const [carregandoDisciplinas, setCarregandoDisciplinas] = useState(false);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [pendenciasSelecionadas, setPendenciasSelecionadas] = useState([]);
  const [bimestreSelecionado, setBimestreSelecionado] = useState('');
  const [filtro, setFiltro] = useState({});
  const [listaBimestres, setListaBimestres] = useState([]);
  const [disciplinaIdSelecionada, setDisciplinaIdSelecionada] = useState(
    undefined
  );
  const [filtrouValoresRota, setFiltrouValoresRota] = useState(false);
  const [imprimindo, setImprimido] = useState(false);

  useEffect(() => {
    const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setSomenteConsulta(
      verificaSomenteConsulta(permissoesTela, naoSetarSomenteConsultaNoStore)
    );

    if (naoSetarSomenteConsultaNoStore) {
      resetarFiltro();
      setListaBimestres([]);
    }
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  const montaExibicaoSituacao = (situacaoId, pendencia) => {
    switch (situacaoId) {
      case situacaoPendenciaDto.Pendente:
        return (
          <PendenteList>
            <span>{pendencia.situacaoNome}</span>
          </PendenteList>
        );
      case situacaoPendenciaDto.Resolvida:
        return (
          <ResolvidoList>
            <span>{pendencia.situacaoNome}</span>
          </ResolvidoList>
        );
      case situacaoPendenciaDto.Aprovada:
        return (
          <AprovadoList>
            <span>{pendencia.situacaoNome}</span>
          </AprovadoList>
        );
      default:
        return '';
    }
  };

  const colunas = [
    {
      title: 'Componente curricular',
      dataIndex: 'componenteCurricular',
      width: '20%',
    },
    {
      title: 'Descrição',
      dataIndex: 'descricao',
      width: '65%',
    },
    {
      title: 'Situação',
      dataIndex: 'situacao',
      width: '8%',
      render: (situacaoId, dados) => montaExibicaoSituacao(situacaoId, dados),
    },
  ];

  const filtrar = useCallback(() => {
    const paramsFiltrar = {
      turmaCodigo: turmaSelecionada.turma,
      componenteCurricularId: disciplinaIdSelecionada,
      bimestre: bimestreSelecionado,
    };
    setPendenciasSelecionadas([]);
    setFiltro({ ...paramsFiltrar });
  }, [disciplinaIdSelecionada, bimestreSelecionado, turmaSelecionada.turma]);

  const resetarFiltro = () => {
    setListaDisciplinas([]);
    setDisciplinaIdSelecionada(undefined);
    setDesabilitarDisciplina(false);
    setBimestreSelecionado(undefined);
  };

  useEffect(() => {
    const montaBimestres = async () => {
      let listaBi = [];
      if (turmaSelecionada.modalidade == modalidade.EJA) {
        listaBi = [
          { valor: 1, descricao: 'Primeiro bimestre' },
          { valor: 2, descricao: 'Segundo bimestre' },
        ];
      } else {
        listaBi = [
          { valor: 1, descricao: 'Primeiro bimestre' },
          { valor: 2, descricao: 'Segundo bimestre' },
          { valor: 3, descricao: 'Terceiro bimestre' },
          { valor: 4, descricao: 'Quarto bimestre' },
        ];
      }
      setListaBimestres(listaBi);

      if (
        !filtrouValoresRota &&
        match &&
        match.params &&
        match.params.bimestre
      ) {
        const { bimestre } = match.params;
        const temBimestreNaLista = listaBi.find(item => item.valor == bimestre);
        if (temBimestreNaLista) {
          setBimestreSelecionado(String(bimestre));
        }
        setBreadcrumbManual(
          `${match.url}`,
          '',
          `${RotasDto.PENDENCIAS_FECHAMENTO}`
        );
        return true;
      } else {
        const bimestreAtual = await api
          .get(
            `v1/periodo-escolar/modalidades/${turmaSelecionada.modalidade}/bimestres/atual`
          )
          .catch(e => erros(e));

        if (bimestreAtual && bimestreAtual.data) {
          setBimestreSelecionado(String(bimestreAtual.data));
          return true;
        }
        return false;
      }
    };

    const obterDisciplinas = async temSugestaoBimestre => {
      setCarregandoDisciplinas(true);
      const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaSelecionada.turma
      ).catch(e => erros(e));

      if (disciplinas && disciplinas.data && disciplinas.data.length) {
        setListaDisciplinas(disciplinas.data);
      } else {
        setListaDisciplinas([]);
      }

      if (
        temSugestaoBimestre &&
        disciplinas &&
        disciplinas.data &&
        disciplinas.data.length === 1
      ) {
        const disciplina = disciplinas.data[0];
        setDisciplinaIdSelecionada(
          String(disciplina.codigoComponenteCurricular)
        );
        setDesabilitarDisciplina(true);
      }

      if (
        !filtrouValoresRota &&
        match &&
        match.params &&
        match.params.codigoComponenteCurricular
      ) {
        const { codigoComponenteCurricular } = match.params;
        const temNaLista = disciplinas.data.find(
          item => item.codigoComponenteCurricular == codigoComponenteCurricular
        );
        if (temNaLista) {
          setDisciplinaIdSelecionada(String(codigoComponenteCurricular));
          setFiltrouValoresRota(true);
        }
      }

      setCarregandoDisciplinas(false);
    };

    resetarFiltro();
    if (
      turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      montaBimestres().then(temSugestaoBimestre => {
        obterDisciplinas(temSugestaoBimestre);
      });
    } else {
      resetarFiltro();
    }
  }, [turmaSelecionada, modalidadesFiltroPrincipal]);

  useEffect(() => {
    if (bimestreSelecionado) {
      setExibirLista(true);
    } else {
      setExibirLista(false);
    }
    filtrar();
  }, [disciplinaIdSelecionada, bimestreSelecionado, filtrar]);

  const onChangeDisciplinas = disciplinaId => {
    setDisciplinaIdSelecionada(disciplinaId);
  };

  const onChangeBimestre = bimestre => {
    setBimestreSelecionado(bimestre);
    if (!bimestre) {
      setDisciplinaIdSelecionada(undefined);
    }

    if (bimestre && listaDisciplinas && listaDisciplinas.length === 1) {
      const disciplina = listaDisciplinas[0];
      setDisciplinaIdSelecionada(String(disciplina.codigoComponenteCurricular));
      setDesabilitarDisciplina(true);
    }
  };

  const onClickEditar = pendencia => {
    if (permissoesTela.podeConsultar) {
      history.push(
        `${RotasDto.PENDENCIAS_FECHAMENTO}/${pendencia.pendenciaId}`
      );
    }
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onSelecionarItems = items => {
    setPendenciasSelecionadas(items);
  };

  const onClickAprovar = async () => {
    const ids = pendenciasSelecionadas.map(e => e.pendenciaId);
    const retorno = await ServicoPendenciasFechamento.aprovar(ids).catch(e =>
      erros(e)
    );
    if (retorno && retorno.data) {
      const comErros = retorno.data.filter(item => !item.sucesso);
      if (comErros && comErros.length) {
        const mensagensErros = comErros.map(e => e.mensagemConsistencia);
        mensagensErros.forEach(msg => {
          erro(msg);
        });
      } else {
        if (ids && ids.length > 1) {
          sucesso(`Pendências aprovadas com sucesso`);
        } else {
          sucesso(`Pendência aprovada com sucesso`);
        }
        filtrar();
      }
    }
  };

  const gerarRelatorio = async () => {
    setImprimido(true);
    const params = {
      anoLetivo: turmaSelecionada.anoLetivo,
      dreCodigo: turmaSelecionada.dre,
      ueCodigo: turmaSelecionada.unidadeEscolar,
      modalidade: turmaSelecionada.modalidade,
      turmasCodigo: [turmaSelecionada.turma],
      bimestre: bimestreSelecionado,
      componentesCurriculares: disciplinaIdSelecionada
        ? [disciplinaIdSelecionada]
        : [],
      exibirDetalhamento: true,
    };
    await ServicoRelatorioPendencias.gerar(params)
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .catch(e => erros(e))
      .finally(setImprimido(false));
  };

  return (
    <>
      {!turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'pendencias-selecione-turma',
            mensagem: 'Você precisa escolher uma turma.',
            estiloTitulo: { fontSize: '18px' },
          }}
          className="mb-2"
        />
      ) : (
        ''
      )}
      <AlertaModalidadeInfantil />
      <Cabecalho pagina="Análise de Pendências" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <BotaoImprimir className="d-flex mr-2">
                <Loader loading={imprimindo}>
                  <Button
                    className="btn-imprimir"
                    icon="print"
                    color={Colors.Azul}
                    border
                    onClick={() => gerarRelatorio()}
                    disabled={lista.length === 0}
                    id="btn-imprimir-conselho-classe"
                  />
                </Loader>
              </BotaoImprimir>
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                label="Aprovar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickAprovar}
                disabled={
                  ehTurmaInfantil(
                    modalidadesFiltroPrincipal,
                    turmaSelecionada
                  ) ||
                  !turmaSelecionada.turma ||
                  somenteConsulta ||
                  !permissoesTela.podeAlterar ||
                  (turmaSelecionada.turma && listaDisciplinas.length < 1) ||
                  (pendenciasSelecionadas &&
                    pendenciasSelecionadas.length < 1) ||
                  pendenciasSelecionadas.filter(
                    item => item.situacao == situacaoPendenciaDto.Aprovada
                  ).length > 0
                }
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-3 mb-2">
              <SelectComponent
                id="bimestre"
                name="bimestre"
                onChange={onChangeBimestre}
                valueOption="valor"
                valueText="descricao"
                lista={listaBimestres}
                placeholder="Selecione o bimestre"
                valueSelect={bimestreSelecionado}
                disabled={ehTurmaInfantil(
                  modalidadesFiltroPrincipal,
                  turmaSelecionada
                )}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-3 mb-2">
              <Loader loading={carregandoDisciplinas} tip="">
                <SelectComponent
                  id="disciplina"
                  name="disciplinaId"
                  lista={listaDisciplinas}
                  valueOption="codigoComponenteCurricular"
                  valueText="nome"
                  valueSelect={disciplinaIdSelecionada}
                  onChange={onChangeDisciplinas}
                  placeholder="Selecione o componente curricular"
                  disabled={
                    ehTurmaInfantil(
                      modalidadesFiltroPrincipal,
                      turmaSelecionada
                    ) ||
                    desabilitarDisciplina ||
                    !bimestreSelecionado
                  }
                />
              </Loader>
            </div>
          </div>
        </div>
        {exibirLista ? (
          <div className="col-md-12 pt-2">
            <ListaPaginada
              url="v1/fechamentos/pendencias/listar"
              id="lista-pendencias-fechamento"
              colunaChave="pendenciaId"
              colunas={colunas}
              filtro={filtro}
              onClick={onClickEditar}
              multiSelecao={!somenteConsulta}
              selecionarItems={onSelecionarItems}
              setLista={dados => setLista(dados)}
            />
          </div>
        ) : (
          ''
        )}
      </Card>
    </>
  );
};

export default PendenciasFechamentoLista;
