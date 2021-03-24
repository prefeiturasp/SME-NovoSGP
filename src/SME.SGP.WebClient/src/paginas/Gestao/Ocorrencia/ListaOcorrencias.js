import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import {
  Button,
  CampoData,
  Card,
  Colors,
  InputBusca,
  ListaPaginada,
  Alert,
} from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import AlertaPermiteSomenteTurmaInfantil from '~/componentes-sgp/AlertaPermiteSomenteTurmaInfantil/alertaPermiteSomenteTurmaInfantil';
import { RotasDto } from '~/dtos';
import modalidade from '~/dtos/modalidade';
import {
  erros,
  ServicoOcorrencias,
  sucesso,
  history,
  confirmar,
  verificaSomenteConsulta,
  ehTurmaInfantil,
} from '~/servicos';

const ListaOcorrencias = () => {
  const [dataInicial, setDataInicial] = useState();
  const [dataFinal, setDataFinal] = useState();
  const [nomeCrianca, setNomeCrianca] = useState();
  const [tituloOcorrencia, setTituloOcorrencia] = useState();
  const [filtro, setFiltro] = useState();
  const [itenSelecionados, setItensSelecionados] = useState([]);
  const [ehFiltroValido, setEhFiltroValido] = useState(false);
  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada, permissoes } = usuario;
  const { podeExcluir, podeIncluir } = permissoes[RotasDto.OCORRENCIAS];

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  useEffect(() => {
    const ehInfantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );

    const soConsulta = verificaSomenteConsulta(
      permissoes[RotasDto.OCORRENCIAS],
      !ehInfantil
    );
    setSomenteConsulta(soConsulta);
  }, [permissoes, modalidadesFiltroPrincipal, turmaSelecionada]);

  const ehModalidadeInfantil = () => {
    return turmaSelecionada?.turma
      ? turmaSelecionada.modalidade.toString() ===
          modalidade.INFANTIL.toString()
      : false;
  };

  const colunas = [
    {
      title: 'Data da ocorrência',
      dataIndex: 'dataOcorrencia',
      width: '20%',
    },
    {
      title: 'Criança',
      dataIndex: 'alunoOcorrencia',
      width: '40%',
    },
    {
      title: 'Título da ocorrência',
      dataIndex: 'titulo',
      width: '40%',
    },
  ];

  const onSetFiltro = async () => {
    if (turmaSelecionada?.turma) {
      setFiltro({
        DataOcorrenciaInicio:
          dataInicial !== '' ? dataInicial?.format('YYYY-MM-DD') : null || '',
        DataOcorrenciaFim:
          dataFinal !== '' ? dataFinal?.format('YYYY-MM-DD') : null || '',
        AlunoNome: nomeCrianca || '',
        titulo: tituloOcorrencia || '',
        turmaId: turmaSelecionada?.id || '',
      });
      setEhFiltroValido(true);
    } else setEhFiltroValido(false);
  };

  const onClickVoltar = () => {
    history.push('/');
  };

  const onClickExcluir = async () => {
    if (itenSelecionados?.length) {
      const confirmado = await confirmar(
        'Atenção',
        itenSelecionados?.length > 1
          ? 'Deseja realmente excluir estes registros?'
          : 'Deseja realmente excluir este registro?'
      );
      if (confirmado) {
        const parametros = { data: itenSelecionados };
        ServicoOcorrencias.excluir(parametros)
          .then(resp => {
            const mensagemSucesso = `${
              itenSelecionados.length > 1
                ? 'Registros excluídos'
                : 'Registro excluído'
            } com sucesso.`;
            sucesso(mensagemSucesso);
            setItensSelecionados([]);
            onSetFiltro();
            if (resp.existemErros) {
              erros(resp.mensagens);
            }
          })
          .catch(e => erros(e));
      }
    }
  };

  const onClickNovo = () => {
    history.push(`${RotasDto.OCORRENCIAS}/novo`);
  };

  const onSelecionarItems = items => {
    setItensSelecionados([...items.map(item => String(item.id))]);
  };

  const onChangeDataInicial = valor => {
    setDataInicial(valor);
  };
  const onChangeDataFinal = valor => {
    setDataFinal(valor);
  };

  useEffect(() => {
    if (dataInicial && dataFinal) {
      onSetFiltro();
    }
    if (!dataInicial && !dataFinal) {
      onSetFiltro();
    }
  }, [dataInicial, dataFinal]);

  useEffect(() => {
    onSetFiltro();
  }, []);

  useEffect(() => {
    onSetFiltro();
  }, [turmaSelecionada]);

  const desabilitarCampos = () => {
    return !turmaSelecionada?.turma || !ehModalidadeInfantil();
  };

  const ehTurmaAnoAnterior = () => {
    return (
      turmaSelecionada.anoLetivo.toString() !== window.moment().format('YYYY')
    );
  };

  return (
    <>
      {turmaSelecionada.turma ? <AlertaPermiteSomenteTurmaInfantil /> : ''}
      {turmaSelecionada?.turma ? (
        ''
      ) : (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'plano-ciclo-selecione-turma',
            mensagem: 'Você precisa escolher uma turma.',
            estiloTitulo: { fontSize: '18px' },
          }}
          className="mb-0"
        />
      )}
      <Cabecalho pagina="Ocorrências" />
      <Card>
        <div className="col-md-12 d-flex justify-content-end pb-4">
          <Button
            id={shortid.generate()}
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-2"
            onClick={onClickVoltar}
          />
          <Button
            id={shortid.generate()}
            label="Excluir"
            color={Colors.Vermelho}
            border
            className="mr-2"
            onClick={onClickExcluir}
            disabled={
              !itenSelecionados?.length ||
              ehTurmaAnoAnterior() ||
              somenteConsulta ||
              !podeExcluir
            }
          />
          <Button
            id={shortid.generate()}
            label="Nova"
            color={Colors.Roxo}
            border
            bold
            disabled={
              !turmaSelecionada?.turma ||
              !ehModalidadeInfantil() ||
              ehTurmaAnoAnterior() ||
              somenteConsulta ||
              !podeIncluir
            }
            className="mr-2"
            onClick={onClickNovo}
          />
        </div>
        <div className="col-sm-12 col-md-3">
          <CampoData
            label="Data da ocorrência"
            valor={dataInicial}
            onChange={onChangeDataInicial}
            placeholder="Data inicial"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitarCampos()}
          />
        </div>
        <div className="col-sm-12 col-md-3" style={{ marginTop: '25px' }}>
          <CampoData
            valor={dataFinal}
            onChange={onChangeDataFinal}
            placeholder="Data final"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitarCampos()}
          />
        </div>
        <div className="col-sm-12 col-md-6">
          <InputBusca
            placeholder="Procure pelo nome da criança"
            label="Criança"
            onPressEnter={onSetFiltro}
            onClick={onSetFiltro}
            valor={nomeCrianca}
            onChange={valor => setNomeCrianca(valor.currentTarget.value)}
            disabled={desabilitarCampos()}
          />
        </div>
        <div className="col-sm-12 col-md-6 mt-2">
          <InputBusca
            placeholder="Procure pelo título da ocorrência"
            label="Título da ocorrência"
            onPressEnter={onSetFiltro}
            onClick={onSetFiltro}
            valor={tituloOcorrencia}
            onChange={valor => setTituloOcorrencia(valor.currentTarget.value)}
            disabled={desabilitarCampos()}
          />
        </div>
        <div className="col-md-12 pt-4">
          <ListaPaginada
            url="v1/ocorrencias"
            id="lista-ocorrencias"
            colunaChave="id"
            colunas={colunas}
            filtro={filtro}
            onClick={ocorrencia =>
              history.push(`${RotasDto.OCORRENCIAS}/editar/${ocorrencia.id}`)
            }
            multiSelecao
            selecionarItems={onSelecionarItems}
            filtroEhValido={ehFiltroValido}
            desabilitado={desabilitarCampos()}
          />
        </div>
      </Card>
    </>
  );
};

export default ListaOcorrencias;
