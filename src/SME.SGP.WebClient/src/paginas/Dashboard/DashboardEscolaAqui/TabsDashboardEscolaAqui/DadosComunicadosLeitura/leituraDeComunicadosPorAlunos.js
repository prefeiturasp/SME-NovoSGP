import { Tooltip } from 'antd';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { DataTable, Label, Loader, SelectComponent } from '~/componentes';
import { NomeEstudanteLista } from '~/componentes-sgp';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import { MarcadorSituacaoAluno } from '../../dashboardEscolaAqui.css';
import { obterDadosComunicadoSelecionado } from '../../../ComponentesDashboard/graficosDashboardUtils';

const LeituraDeComunicadosPorAlunos = props => {
  const { comunicado, listaComunicado } = props;

  const [exibirLoader, setExibirLoader] = useState(false);
  const [listaAlunos, setListaAlunos] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [codigoTurmaSelecionado, setCodigoTurmaSelecionado] = useState();

  const dadosDeLeituraDeComunicadosPorTurmas = useSelector(
    state => state.dashboardEscolaAqui.dadosDeLeituraDeComunicadosPorTurmas
  );

  const marcadorAlunos = aluno => {
    return (
      <div className="ml-3">
        {aluno.numeroChamada}
        {aluno?.marcador?.descricao ? (
          <Tooltip title={aluno.marcador.descricao} placement="top">
            <MarcadorSituacaoAluno className="fas fa-circle" />
          </Tooltip>
        ) : (
          ''
        )}
      </div>
    );
  };

  const obterDadosLeituraDeComunicadosPorAlunos = useCallback(
    async codigoTurma => {
      setExibirLoader(true);

      const dadosComunicado = obterDadosComunicadoSelecionado(
        comunicado,
        listaComunicado
      );

      const resposta = await ServicoDashboardEscolaAqui.obterDadosLeituraDeComunicadosPorAlunos(
        codigoTurma,
        dadosComunicado?.id
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data) {
        setListaAlunos(resposta.data);
      }
    },
    [comunicado, listaComunicado]
  );

  useEffect(() => {
    setListaAlunos([]);
    return () => {
      setListaAlunos([]);
    };
  }, [comunicado]);

  const montarSelectTurmas = () => {
    return (
      <>
        <div
          className="d-flex align-items-center"
          style={{ textTransform: 'none' }}
        >
          <Label text="Selecione uma tuma" className="mr-1" />
          <SelectComponent
            style={{ width: '200px' }}
            id="select-turmas"
            lista={listaTurmas}
            valueOption="codigo"
            valueText="nome"
            placeholder="Turma"
            valueSelect={codigoTurmaSelecionado}
            onChange={valor => {
              if (valor) {
                obterDadosLeituraDeComunicadosPorAlunos(valor);
              } else {
                setListaAlunos();
              }
              setCodigoTurmaSelecionado(valor);
            }}
            disabled={listaTurmas?.length === 1}
          />
        </div>
      </>
    );
  };

  const colunas = [
    {
      title: () => montarSelectTurmas(),
      className: 'text-left',
      children: [
        {
          title: '#',
          dataIndex: 'numeroChamada',
          render: (_, aluno) => {
            return marcadorAlunos(aluno);
          },
          colSpan: 0,
        },
        {
          title: 'Nome',
          dataIndex: 'nomeAluno',
          colSpan: 2,
          render: (_, record) => (
            <NomeEstudanteLista
              nome={record?.nomeAluno}
              exibirSinalizacao={record?.ehAtendidoAEE}
            />
          ),
        },
        {
          title: 'Responsável',
          dataIndex: 'nomeResponsavel',
        },
        {
          title: 'Contato do responsável',
          dataIndex: 'telefoneResponsavel',
        },
        {
          title: 'Possui aplicativo',
          dataIndex: 'possueApp',
          render: possueApp => {
            if (possueApp) {
              return 'Sim';
            }
            return 'Não';
          },
        },
        {
          title: 'Leu a mensagem',
          dataIndex: 'leuComunicado',
          render: leuComunicado => {
            if (leuComunicado) {
              return 'Sim';
            }
            return 'Não';
          },
        },
        {
          title: 'Data da leitura',
          dataIndex: 'dataLeitura',
          render: dataLeitura => {
            if (dataLeitura) {
              const dataFormatada = moment(dataLeitura).format(
                'DD/MM/YYYY HH:mm:ss'
              );
              return <span>{dataFormatada}</span>;
            }
            return '';
          },
        },
      ],
    },
  ];

  const obterTurmas = useCallback(async () => {
    const dados = dadosDeLeituraDeComunicadosPorTurmas.map(item => ({
      codigo: item.codigoTurma,
      nome: `${item.siglaModalidade} - ${item.turma}`,
    }));

    if (dados?.length === 1) {
      setCodigoTurmaSelecionado(dados[0].codigo);
      obterDadosLeituraDeComunicadosPorAlunos(dados[0].codigo);
    } else {
      setCodigoTurmaSelecionado();
      setListaAlunos();
    }
    setListaTurmas(dados);
  }, [
    dadosDeLeituraDeComunicadosPorTurmas,
    obterDadosLeituraDeComunicadosPorAlunos,
  ]);

  useEffect(() => {
    setListaTurmas([]);
    setListaAlunos([]);
    if (dadosDeLeituraDeComunicadosPorTurmas?.length) {
      obterTurmas();
    }
  }, [dadosDeLeituraDeComunicadosPorTurmas, comunicado, obterTurmas]);

  return dadosDeLeituraDeComunicadosPorTurmas.length ? (
    <div className="col-md-12 pt-2">
      <Loader loading={exibirLoader}>
        <DataTable
          id="lista-dados-leitura-aluno"
          columns={colunas}
          dataSource={listaAlunos}
          pagination={false}
          pageSize={9999}
        />
      </Loader>
    </div>
  ) : (
    ''
  );
};

LeituraDeComunicadosPorAlunos.propTypes = {
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
};

LeituraDeComunicadosPorAlunos.defaultProps = {
  comunicado: '',
  listaComunicado: [],
};

export default LeituraDeComunicadosPorAlunos;
