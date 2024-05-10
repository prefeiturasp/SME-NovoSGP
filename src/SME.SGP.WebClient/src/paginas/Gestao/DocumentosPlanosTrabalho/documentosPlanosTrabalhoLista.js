import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { ListaPaginada, Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes';
import { RotasDto } from '~/dtos';
import { erros, verificaSomenteConsulta } from '~/servicos';
import AbrangenciaServico from '~/servicos/Abrangencia';
import ServicoArmazenamento from '~/servicos/Componentes/ServicoArmazenamento';
import history from '~/servicos/history';
import ServicoDocumentosPlanosTrabalho from '~/servicos/Paginas/Gestao/DocumentosPlanosTrabalho/ServicoDocumentosPlanosTrabalho';
import { downloadBlob } from '~/utils/funcoes/gerais';
import FiltroHelper from '~componentes-sgp/filtro/helper';

const DocumentosPlanosTrabalhoLista = () => {
  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.DOCUMENTOS_PLANOS_TRABALHO];

  const [carregandoAnos, setCarregandoAnos] = useState(false);
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);

  const [carregandoDres, setCarregandoDres] = useState(false);
  const [listaDres, setListaDres] = useState([]);

  const [carregandoUes, setCarregandoUes] = useState(false);
  const [listaUes, setListaUes] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);

  const [listaTipoDocumento, setListaTipoDocumento] = useState([]);
  const [tipoDocumentoId, setTipoDocumentoId] = useState();

  const [listaClassificacao, setListaClassificacao] = useState([]);
  const [classificacaoId, setClassificacaoId] = useState();

  const [filtro, setFiltro] = useState({});

  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const NOME_BTN_DOWNLOAD = 'BOTAO-DOWNLOAD-ARQUIVO';

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnos(true);
    let anosLetivos = [];

    const anosLetivoComHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: true,
    });
    const anosLetivoSemHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: false,
    });

    anosLetivos = anosLetivos.concat(anosLetivoComHistorico);

    anosLetivoSemHistorico.forEach(ano => {
      if (!anosLetivoComHistorico.find(a => a.valor === ano.valor)) {
        anosLetivos.push(ano);
      }
    });

    if (!anosLetivos.length) {
      anosLetivos.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    if (anosLetivos && anosLetivos.length) {
      const temAnoAtualNaLista = anosLetivos.find(
        item => String(item.valor) === String(anoAtual)
      );
      if (temAnoAtualNaLista) setAnoLetivo(anoAtual);
      else setAnoLetivo(anosLetivos[0].valor);
    }

    setListaAnosLetivo(anosLetivos);
    setCarregandoAnos(false);
  }, [anoAtual]);

  const obterTiposDocumento = async () => {
    const resposta = await ServicoDocumentosPlanosTrabalho.obterTiposDeDocumentos().catch(
      e => erros(e)
    );

    if (resposta?.data?.length) {
      setListaTipoDocumento(resposta.data);
      if (resposta.data.length === 1) {
        const tipo = resposta.data[0];
        setTipoDocumentoId(String(tipo.id));

        if (tipo.classificacoes.length === 1) {
          setListaClassificacao(tipo.classificacoes);
          const classificacao = tipo.classificacoes[0];
          setClassificacaoId(String(classificacao.id));
        }
      }
    }
  };

  useEffect(() => {
    if (ueId && listaUes?.length) {
      const ueSelecionada = listaUes.find(
        item => String(item.valor) === String(ueId)
      );
      const params = {
        ueId: ueSelecionada ? ueSelecionada.id : '',
        tipoDocumentoId,
        classificacaoId,
      };
      setFiltro({ ...params });
    }
  }, [ueId, tipoDocumentoId, classificacaoId, listaUes]);

  useEffect(() => {
    obterAnosLetivos();
    obterTiposDocumento();
  }, [obterAnosLetivos]);

  const onChangeAnoLetivo = async valor => {
    setDreId();
    setUeId();
    setAnoLetivo(valor);
  };

  const onChangeDre = valor => {
    setDreId(valor);
    setUeId(undefined);
  };

  const onChangeUe = valor => {
    setUeId(valor);
  };

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const resposta = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/false/dres?anoLetivo=${anoLetivo}`
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoDres(false));

      if (resposta?.data && resposta?.data?.length) {
        const lista = resposta.data
          .map(item => ({
            desc: item.nome,
            valor: String(item.codigo),
            abrev: item.abreviacao,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setDreId(lista[0].valor);
        }
      } else {
        setListaDres([]);
        setDreId(undefined);
      }
    }
  }, [anoLetivo]);

  useEffect(() => {
    obterDres();
  }, [obterDres]);

  const obterUes = useCallback(async (dre, ano) => {
    if (dre) {
      setCarregandoUes(true);
      const resposta = await AbrangenciaServico.buscarUes(
        dre,
        `v1/abrangencias/false/dres/${dre}/ues?anoLetivo=${ano}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoUes(false));

      if (resposta?.data?.length) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
          id: item.id,
        }));

        if (lista && lista.length && lista.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, []);

  useEffect(() => {
    if (dreId) {
      obterUes(dreId, anoLetivo);
    } else {
      setUeId();
      setListaUes([]);
    }
  }, [dreId, anoLetivo, obterUes]);

  const onClickNovo = () => {
    if (!somenteConsulta && permissoesTela.podeIncluir) {
      history.push(`${RotasDto.DOCUMENTOS_PLANOS_TRABALHO}/novo`);
    }
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeTipoDocumento = tipo => {
    let classificacaoPorTipo = [];
    if (tipo) {
      const lista = listaTipoDocumento.find(
        item => String(item.id) === String(tipo)
      );
      classificacaoPorTipo = lista.classificacoes;
    }
    setTipoDocumentoId(tipo);
    setListaClassificacao(classificacaoPorTipo);
    setClassificacaoId();
  };

  const formatarCampoDataGrid = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('DD/MM/YYYY');
    }
    return <span> {dataFormatada}</span>;
  };

  const onClickDownload = linha => {
    ServicoArmazenamento.obterArquivoParaDownload(linha.codigoArquivo)
      .then(resposta => {
        downloadBlob(resposta.data, linha.nomeArquivo);
      })
      .catch(e => erros(e));
  };

  const colunas = [
    {
      title: 'Tipo',
      dataIndex: 'tipoDocumento',
    },
    {
      title: 'Cassificação',
      dataIndex: 'classificacao',
    },
    {
      title: 'Usuário',
      dataIndex: 'usuario',
    },
    {
      title: 'Data de inclusão',
      dataIndex: 'dataUpload',
      render: data => formatarCampoDataGrid(data),
    },
    {
      title: 'Anexo',
      dataIndex: 'anexo',
      width: '10%',
      render: (texto, linha) => {
        return (
          <Button
            icon={`fas fa-arrow-down ${NOME_BTN_DOWNLOAD}`}
            label="Download"
            color={Colors.Azul}
            className={`ml-2 text-center ${NOME_BTN_DOWNLOAD}`}
            onClick={() => onClickDownload(linha)}
          />
        );
      },
    },
  ];

  const onClickEditar = (linha, colunaClicada) => {
    let executarClick = true;
    if (colunaClicada?.target?.className) {
      const clicouNoBotao = colunaClicada.target.className.includes(
        NOME_BTN_DOWNLOAD
      );
      executarClick = !clicouNoBotao;
    }

    if (executarClick) {
      history.push(
        `${RotasDto.DOCUMENTOS_PLANOS_TRABALHO}/editar/${linha.documentoId}`
      );
    }
  };

  return (
    <>
      <Cabecalho pagina="Upload de documentos e planos de trabalho" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4 justify-itens-end">
              <Button
                id="btn-voltar"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                id="btn-novo"
                label="Novo"
                color={Colors.Roxo}
                border
                bold
                onClick={onClickNovo}
                disabled={somenteConsulta || !permissoesTela.podeIncluir}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-2 mb-2">
              <Loader loading={carregandoAnos} tip="">
                <SelectComponent
                  id="select-ano-letivo"
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo && listaAnosLetivo.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Ano letivo"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-5 mb-2">
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  id="select-dre"
                  label="Diretoria Regional de Educação (DRE)"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!anoLetivo || (listaDres && listaDres.length === 1)}
                  onChange={onChangeDre}
                  valueSelect={dreId}
                  placeholder="Diretoria Regional De Educação (DRE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-5 mb-2">
              <Loader loading={carregandoUes} tip="">
                <SelectComponent
                  id="select-ue"
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!dreId || (listaUes && listaUes.length === 1)}
                  onChange={onChangeUe}
                  valueSelect={ueId}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
              <SelectComponent
                id="select-tipos-documento"
                label="Tipo de documento"
                lista={listaTipoDocumento}
                valueOption="id"
                valueText="tipoDocumento"
                onChange={onChangeTipoDocumento}
                valueSelect={tipoDocumentoId}
                placeholder="Tipo de documento"
                disabled={listaTipoDocumento.length === 1}
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
              <SelectComponent
                id="select-classificacao-documento"
                label="Classificação"
                lista={listaClassificacao}
                valueOption="id"
                valueText="classificacao"
                onChange={setClassificacaoId}
                valueSelect={classificacaoId}
                placeholder="Classificação do documento"
                disabled={listaClassificacao.length === 1}
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
              {ueId ? (
                <ListaPaginada
                  url="v1/armazenamento/documentos"
                  id="lista-tipo-documento"
                  colunaChave="documentoId"
                  colunas={colunas}
                  filtro={filtro}
                  onClick={(linha, colunaClicada) =>
                    onClickEditar(linha, colunaClicada)
                  }
                  filtroEhValido={!!filtro.ueId}
                />
              ) : (
                ''
              )}
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default DocumentosPlanosTrabalhoLista;
