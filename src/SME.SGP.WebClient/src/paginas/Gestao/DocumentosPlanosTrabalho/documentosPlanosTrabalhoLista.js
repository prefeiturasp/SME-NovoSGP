import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import { ListaPaginada, Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes';
import { RotasDto } from '~/dtos';
import { erros } from '~/servicos';
import AbrangenciaServico from '~/servicos/Abrangencia';
import history from '~/servicos/history';
import FiltroHelper from '~componentes-sgp/filtro/helper';

const DocumentosPlanosTrabalhoLista = () => {
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
  const [tipoDocumento, setTipoDocumento] = useState();

  const [listaClassificacao, setListaClassificacao] = useState([]);
  const [classificacao, setClassificacao] = useState();

  const [filtro, setFiltro] = useState({});

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnos(true);
    const anosLetivo = await AbrangenciaServico.buscarTodosAnosLetivos()
      .catch(e => erros(e))
      .finally(() => setCarregandoAnos(false));

    if (anosLetivo && anosLetivo.data) {
      const a = [];
      anosLetivo.data.forEach(ano => {
        a.push({ desc: ano, valor: ano });
      });
      setAnoLetivo(a[0].valor);
      setListaAnosLetivo(a);
    } else {
      setListaAnosLetivo([]);
    }
  }, []);

  const obterTiposDocumento = () => {
    // TODO MOCK!
    const mockTipoDocumento = [
      {
        valor: '1',
        desc: 'Documento',
        classificacao: [
          { valor: 'PEA ', desc: 'PEA ' },
          { valor: 'PPP', desc: 'PPP' },
        ],
      },
      {
        valor: '2',
        desc: 'Plano de Trabalho',
        classificacao: [
          { valor: 'PAEE', desc: 'PAEE' },
          { valor: 'PAP', desc: 'PAP' },
          { valor: 'POA', desc: 'POA' },
          { valor: 'POED', desc: 'POED' },
          { valor: 'POEI', desc: 'POEI' },
          { valor: 'POSL', desc: 'POSL' },
        ],
      },
    ];
    setListaTipoDocumento(mockTipoDocumento);
  };

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
      const { data } = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/false/dres?anoLetivo=${anoLetivo}`
      );
      if (data && data.length) {
        const lista = data
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
      setCarregandoDres(false);
    }
  }, [anoLetivo]);

  useEffect(() => {
    obterDres();
  }, [obterDres]);

  const obterUes = useCallback(async (dre, ano) => {
    if (dre) {
      setCarregandoUes(true);
      const { data } = await AbrangenciaServico.buscarUes(
        dre,
        `v1/abrangencias/false/dres/${dre}/ues?anoLetivo=${ano}`,
        true
      );
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista && lista.length && lista.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
      setCarregandoUes(false);
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

  const onClickNovo = () => {};

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeTipoDocumento = tipo => {
    let classificacaoPorTipo = [];
    if (tipo) {
      const lista = listaTipoDocumento.find(item => item.valor === tipo);
      classificacaoPorTipo = lista.classificacao;
    }
    setTipoDocumento(tipo);
    setListaClassificacao(classificacaoPorTipo);
    setClassificacao();
  };

  const formatarCampoDataGrid = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('DD/MM/YYYY');
    }
    return <span> {dataFormatada}</span>;
  };

  const onClickDownload = linha => {
    console.log(linha);
  };

  const colunas = [
    {
      title: 'Tipo',
      dataIndex: 'tipo',
    },
    {
      title: 'Cassificação',
      dataIndex: 'cassificacao',
    },
    {
      title: 'Usuário',
      dataIndex: 'usuario',
    },
    {
      title: 'Data de inclusão',
      dataIndex: 'dataInclusao',
      render: data => formatarCampoDataGrid(data),
    },
    {
      title: 'Anexo',
      dataIndex: 'anexo',
      width: '10%',
      render: (texto, linha) => {
        return (
          <Button
            icon="fas fa-arrow-down"
            label="Download"
            color={Colors.Azul}
            className="ml-2 text-center"
            onClick={() => onClickDownload(linha)}
          />
        );
      },
    },
  ];

  const onClickEditar = linha => {
    history.push(`${RotasDto.DOCUMENTOS_PLANOS_TRABALHO}/editar/${linha.id}`);
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
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
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
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
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
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
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
                valueOption="valor"
                valueText="desc"
                onChange={onChangeTipoDocumento}
                valueSelect={tipoDocumento}
                placeholder="Tipo de documento"
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
              <SelectComponent
                id="select-classificacao-documento"
                label="Classificação"
                lista={listaClassificacao}
                valueOption="valor"
                valueText="desc"
                onChange={setClassificacao}
                valueSelect={classificacao}
                placeholder="Classificação do documento"
              />
            </div>
            <ListaPaginada
              url="v1/calendarios/eventos/tipos/listar"
              id="lista-tipo-documento"
              colunaChave="id"
              colunas={colunas}
              filtro={filtro}
              onClick={onClickEditar}
            />
          </div>
        </div>
      </Card>
    </>
  );
};

export default DocumentosPlanosTrabalhoLista;
