import { Tooltip } from 'antd';
import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { DataTable, Label, Loader, SelectComponent } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import { MarcadorSituacaoAluno } from '../../dashboardEscolaAqui.css';

const LeituraDeComunicadosPorAlunos = () => {
  const [exibirLoader, setExibirLoader] = useState(false);
  const [listaAlunos, setListaAlunos] = useState(false);
  const [listaTurmas, setListaTurmas] = useState([]);

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

  const montarSelectTurmas = () => {
    return (
      <>
        <div className="d-flex align-items-center">
          <Label text="Selecione uma tuma" className="mr-1" />
          <SelectComponent
            style={{ width: '200px' }}
            id="select-turmas"
            lista={listaTurmas}
            valueOption="codigo"
            valueText="nome"
            placeholder="Turma"
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
          dataIndex: 'nome',
          colSpan: 2,
        },
        {
          title: 'Responsável',
          dataIndex: 'responsavel',
        },
        {
          title: 'Contato do responsável',
          dataIndex: 'contatoResponsavel',
        },
        {
          title: 'Possui Aplicativo',
          dataIndex: 'possuiAplicativo',
        },
        {
          title: 'Leu a mensagem',
          dataIndex: 'leuMensagem',
        },
        {
          title: 'Data da leitura',
          dataIndex: 'dataLeitura',
          render: data => {
            const dataFormatada = moment(data).format('DD/MM/YYYY HH:mm:ss');
            return <span>{dataFormatada}</span>;
          },
        },
      ],
    },
  ];

  const obterDadosLeituraDeComunicadosPorAlunos = useCallback(async () => {
    const codigoTrumas = dadosDeLeituraDeComunicadosPorTurmas.map(
      item => item.codigoTurma
    );

    setExibirLoader(true);
    const resposta = await ServicoDashboardEscolaAqui.obterDadosLeituraDeComunicadosPorAlunos(
      codigoTrumas
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (resposta?.data) {
      setListaAlunos(resposta.data);
    }
  }, [dadosDeLeituraDeComunicadosPorTurmas]);

  const obterTurmas = useCallback(async () => {
    const dados = dadosDeLeituraDeComunicadosPorTurmas.map(item => ({
      codigo: item.codigoTurma,
      nome: item.turma,
    }));

    setListaTurmas(dados);
  }, [dadosDeLeituraDeComunicadosPorTurmas]);

  useEffect(() => {
    if (dadosDeLeituraDeComunicadosPorTurmas?.length) {
      obterDadosLeituraDeComunicadosPorAlunos();
      obterTurmas();
    }
  }, [
    dadosDeLeituraDeComunicadosPorTurmas,
    obterDadosLeituraDeComunicadosPorAlunos,
    obterTurmas,
  ]);

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

export default LeituraDeComunicadosPorAlunos;
