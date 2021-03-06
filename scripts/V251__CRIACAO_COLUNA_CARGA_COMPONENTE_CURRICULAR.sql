alter table componente_curricular 
add column if not exists descricao_sgp varchar(100) null;
   


update componente_curricular set descricao_sgp = 'Português' where id =    1;
update componente_curricular set descricao_sgp = 'Matemática' where id =    2;
update componente_curricular set descricao_sgp = 'Ed. Física' where id =    6;
update componente_curricular set descricao_sgp = 'História' where id =    7;
update componente_curricular set descricao_sgp = 'Geografia' where id =    8;
update componente_curricular set descricao_sgp = 'Inglês' where id =    9;
update componente_curricular set descricao_sgp = 'Mat.Comerc.Financ.' where id =   19;
update componente_curricular set descricao_sgp = 'Contab. e Custos' where id =   21;
update componente_curricular set descricao_sgp = 'Administração Geral' where id =   22;
update componente_curricular set descricao_sgp = 'Adm. de Materiais' where id =   25;
update componente_curricular set descricao_sgp = 'Direito e Legislação' where id =   29;
update componente_curricular set descricao_sgp = 'Adm. Financeira' where id =   31;
update componente_curricular set descricao_sgp = 'Mercadologia' where id =   34;
update componente_curricular set descricao_sgp = 'Publicidade' where id =   35;
update componente_curricular set descricao_sgp = 'Exportação/Import.' where id =   36;
update componente_curricular set descricao_sgp = 'Cont. Industrial' where id =   42;
update componente_curricular set descricao_sgp = 'Cont. e Custos' where id =   43;
update componente_curricular set descricao_sgp = 'Cont. Pública' where id =   45;
update componente_curricular set descricao_sgp = 'Física' where id =   51;
update componente_curricular set descricao_sgp = 'Química' where id =   52;
update componente_curricular set descricao_sgp = 'Biologia' where id =   53;
update componente_curricular set descricao_sgp = 'Estatística Aplicada' where id =   65;
update componente_curricular set descricao_sgp = 'Didat.Prat.Ens.' where id =   68;
update componente_curricular set descricao_sgp = 'Liter. Infantil' where id =   79;
update componente_curricular set descricao_sgp = 'Ciências' where id =   89;
update componente_curricular set descricao_sgp = 'Filosofia' where id =   98;
update componente_curricular set descricao_sgp = 'Economia e Mercados' where id =  100;
update componente_curricular set descricao_sgp = 'Sociologia' where id =  103;
update componente_curricular set descricao_sgp = 'História da Educação' where id =  106;
update componente_curricular set descricao_sgp = 'Teor.e Prótese Total' where id =  110;
update componente_curricular set descricao_sgp = 'Labor.Prótese Total' where id =  111;
update componente_curricular set descricao_sgp = 'Teor.Prótese Fixa' where id =  112;
update componente_curricular set descricao_sgp = 'Labor.Prótese Fixa' where id =  113;
update componente_curricular set descricao_sgp = 'Teor.Prot.Removível' where id =  114;
update componente_curricular set descricao_sgp = 'Labor.Prot.Removível' where id =  115;
update componente_curricular set descricao_sgp = 'Teor. de Ortodontia' where id =  116;
update componente_curricular set descricao_sgp = 'Organização de Empr.' where id =  119;
update componente_curricular set descricao_sgp = 'Higiene e Seg. Trab.' where id =  130;
update componente_curricular set descricao_sgp = 'Pesquisa de Mercado' where id =  133;
update componente_curricular set descricao_sgp = 'Língua Portuguesa' where id =  138;
update componente_curricular set descricao_sgp = 'Arte' where id =  139;
update componente_curricular set descricao_sgp = 'Mat.e Equip. Prótese' where id =  140;
update componente_curricular set descricao_sgp = 'Anatomia da Cabeça' where id =  141;
update componente_curricular set descricao_sgp = 'Comp.Ético e Organiz' where id =  142;
update componente_curricular set descricao_sgp = 'Contabilidade Geral' where id =  151;
update componente_curricular set descricao_sgp = 'Arquit.de Computador' where id =  154;
update componente_curricular set descricao_sgp = 'Estatis. Aplic. Educ' where id =  157;
update componente_curricular set descricao_sgp = 'Informática.Aplic. Educ' where id =  158;
update componente_curricular set descricao_sgp = 'Psicol.Geral e Educ' where id =  159;
update componente_curricular set descricao_sgp = 'Sociol.Geral e Educ' where id =  160;
update componente_curricular set descricao_sgp = 'Biologia Educacional' where id =  161;
update componente_curricular set descricao_sgp = 'Filosofia.Geral e Educ' where id =  162;
update componente_curricular set descricao_sgp = 'Ling. Port. Lit.Bras' where id =  163;
update componente_curricular set descricao_sgp = 'Estr.Func.E.F. E Inf' where id =  164;
update componente_curricular set descricao_sgp = 'Psic.Des.E da Aprend' where id =  165;
update componente_curricular set descricao_sgp = 'Met.Ens.Ling.Port' where id =  167;
update componente_curricular set descricao_sgp = 'Met.Ens.Hist. e Geog' where id =  168;
update componente_curricular set descricao_sgp = 'Met.Ens.Ciências' where id =  169;
update componente_curricular set descricao_sgp = 'Met.Ensino da Arte' where id =  170;
update componente_curricular set descricao_sgp = 'Met.Ens.Ed.Fis.Infan' where id =  171;
update componente_curricular set descricao_sgp = 'Met.Ens.Matemática' where id =  172;
update componente_curricular set descricao_sgp = 'Met.Ens.Educ.Infantil' where id =  173;
update componente_curricular set descricao_sgp = 'Adm. Comercial' where id =  174;
update componente_curricular set descricao_sgp = 'Adm. de Produção' where id =  175;
update componente_curricular set descricao_sgp = 'Adm. de Rec. Humanos' where id =  176;
update componente_curricular set descricao_sgp = 'An. Demons. Financ.' where id =  177;
update componente_curricular set descricao_sgp = 'Anatomia Dental' where id =  178;
update componente_curricular set descricao_sgp = 'Contábil.Comercial' where id =  180;
update componente_curricular set descricao_sgp = 'Contal. Inst. Financ' where id =  181;
update componente_curricular set descricao_sgp = 'Des. Publicitário' where id =  183;
update componente_curricular set descricao_sgp = 'Dir. Civil e Trab.' where id =  184;
update componente_curricular set descricao_sgp = 'Dir. Comerc. Tribut' where id =  186;
update componente_curricular set descricao_sgp = 'Encer. Progressivo' where id =  187;
update componente_curricular set descricao_sgp = 'Estru. Demo.Financ.' where id =  189;
update componente_curricular set descricao_sgp = 'Ética Comport. Org.' where id =  191;
update componente_curricular set descricao_sgp = 'Et. Prin. Fund.Cont.' where id =  192;
update componente_curricular set descricao_sgp = 'Gestão e Qualidade' where id =  194;
update componente_curricular set descricao_sgp = 'Info. Aplic. Gestão' where id =  195;
update componente_curricular set descricao_sgp = 'Inglês Instrumental' where id =  196;
update componente_curricular set descricao_sgp = 'Introd. a Economia' where id =  197;
update componente_curricular set descricao_sgp = 'Labor. Ortodontia' where id =  198;
update componente_curricular set descricao_sgp = 'Legislação' where id =  200;
update componente_curricular set descricao_sgp = 'Logística' where id =  202;
update componente_curricular set descricao_sgp = 'Mat Equip. Prot.Fixa' where id =  203;
update componente_curricular set descricao_sgp = 'Microb Parasitologia' where id =  205;
update componente_curricular set descricao_sgp = 'Mídia' where id =  206;
update componente_curricular set descricao_sgp = 'Org de Laboratório' where id =  208;
update componente_curricular set descricao_sgp = 'Parc Rem e Ortodont' where id =  209;
update componente_curricular set descricao_sgp = 'Prod Tec de Textos' where id =  212;
update componente_curricular set descricao_sgp = 'Promoção de Vendas' where id =  213;
update componente_curricular set descricao_sgp = 'Psico Consumidor' where id =  214;
update componente_curricular set descricao_sgp = 'Psico Organizacional' where id =  215;
update componente_curricular set descricao_sgp = 'Libras' where id =  218;
update componente_curricular set descricao_sgp = 'Informática (OIE)' where id =  219;
update componente_curricular set descricao_sgp = 'Leitura (OSL)' where id =  220;
update componente_curricular set descricao_sgp = 'Finanças e Custos' where id =  221;
update componente_curricular set descricao_sgp = 'Dir e Legisl P/ Info' where id =  222;
update componente_curricular set descricao_sgp = 'Leitura e Prod Texto' where id =  223;
update componente_curricular set descricao_sgp = 'Redes Computadores' where id =  224;
update componente_curricular set descricao_sgp = 'Tecnol Interativas' where id =  225;
update componente_curricular set descricao_sgp = 'Met Des Projetos' where id =  226;
update componente_curricular set descricao_sgp = 'Técnicas de Sistemas' where id =  227;
update componente_curricular set descricao_sgp = 'Bandas e Fanfarras' where id =  228;
update componente_curricular set descricao_sgp = 'Linguagens e Códigos' where id =  230;
update componente_curricular set descricao_sgp = 'Ciências da Nat/Mat' where id =  231;
update componente_curricular set descricao_sgp = 'Ciências Humanas' where id =  232;
update componente_curricular set descricao_sgp = 'Itinerário Informática' where id =  233;
update componente_curricular set descricao_sgp = 'Regência de Classe Fund I' where id =  508;
update componente_curricular set descricao_sgp = 'CJ Infantil (EMEI)' where id =  509;
update componente_curricular set descricao_sgp = 'Reg EMEI 4h' where id =  510;
update componente_curricular set descricao_sgp = 'Reg EMEI 4h' where id =  512;
update componente_curricular set descricao_sgp = 'Reg EMEI 2h' where id =  513;
update componente_curricular set descricao_sgp = 'CJ FUND I' where id =  514;
update componente_curricular set descricao_sgp = 'Reg CEI Parcial' where id =  515;
update componente_curricular set descricao_sgp = 'Reg CEI Integ/Manhã' where id =  517;
update componente_curricular set descricao_sgp = 'Reg CEI Integ/Tarde' where id =  518;
update componente_curricular set descricao_sgp = 'CJ Infantil (CEI)' where id =  519;
update componente_curricular set descricao_sgp = 'Laudo Médico' where id =  525;
update componente_curricular set descricao_sgp = 'CJ Língua Portugiesa' where id =  526;
update componente_curricular set descricao_sgp = 'CJ Matemática' where id =  527;
update componente_curricular set descricao_sgp = 'CJ Ciências' where id =  528;
update componente_curricular set descricao_sgp = 'CJ Geografia' where id =  529;
update componente_curricular set descricao_sgp = 'CJ História' where id =  530;
update componente_curricular set descricao_sgp = 'CJ Artes' where id =  531;
update componente_curricular set descricao_sgp = 'CJ Ed Física' where id =  532;
update componente_curricular set descricao_sgp = 'CJ Inglês' where id =  533;
update componente_curricular set descricao_sgp = 'Reg EMEI Int/Manhã' where id =  534;
update componente_curricular set descricao_sgp = 'Reg Emei Int/Tarde' where id =  535;
update componente_curricular set descricao_sgp = 'Língua Espanhola' where id =  537;
update componente_curricular set descricao_sgp = 'Acomod. Docente Cei' where id =  888;
update componente_curricular set descricao_sgp = 'Acomodação Docente' where id =  999;
update componente_curricular set descricao_sgp = 'Atend Escolar' where id = 1003;
update componente_curricular set descricao_sgp = 'Auxiliar Administrativo' where id = 1009;
update componente_curricular set descricao_sgp = 'Confeitaria' where id = 1010;
update componente_curricular set descricao_sgp = 'Corte Costura' where id = 1011;
update componente_curricular set descricao_sgp = 'Eletrônica' where id = 1012;
update componente_curricular set descricao_sgp = 'Espanhol' where id = 1013;
update componente_curricular set descricao_sgp = 'Informática (OIE)' where id = 1018;
update componente_curricular set descricao_sgp = 'Mecânica Automóveis' where id = 1019;
update componente_curricular set descricao_sgp = 'Movimento de Alfabetização' where id = 1020;
update componente_curricular set descricao_sgp = 'Panificação' where id = 1022;
update componente_curricular set descricao_sgp = 'Serigrafia' where id = 1027;
update componente_curricular set descricao_sgp = 'SRM' where id = 1030;
update componente_curricular set descricao_sgp = 'SAP' where id = 1032;
update componente_curricular set descricao_sgp = 'Rec Par Matemática' where id = 1033;
update componente_curricular set descricao_sgp = 'Artes Cênicas' where id = 1034;
update componente_curricular set descricao_sgp = 'Artes Plásticas' where id = 1035;
update componente_curricular set descricao_sgp = 'Capoeira' where id = 1036;
update componente_curricular set descricao_sgp = 'Circo' where id = 1037;
update componente_curricular set descricao_sgp = 'Danças' where id = 1038;
update componente_curricular set descricao_sgp = 'Direitos Humanos' where id = 1039;
update componente_curricular set descricao_sgp = 'Educação Ambiental' where id = 1040;
update componente_curricular set descricao_sgp = 'Educação Científica' where id = 1041;
update componente_curricular set descricao_sgp = 'Esportes' where id = 1042;
update componente_curricular set descricao_sgp = 'Iniciação Profissional' where id = 1043;
update componente_curricular set descricao_sgp = 'Língua Estrangeira' where id = 1044;
update componente_curricular set descricao_sgp = 'Língua Francesa' where id = 1045;
update componente_curricular set descricao_sgp = 'Língua Inglesa' where id = 1046;
update componente_curricular set descricao_sgp = 'Mais Educação' where id = 1047;
update componente_curricular set descricao_sgp = 'Música' where id = 1048;
update componente_curricular set descricao_sgp = 'Recreação' where id = 1049;
update componente_curricular set descricao_sgp = 'Rec Par Ciências' where id = 1051;
update componente_curricular set descricao_sgp = 'Rec Par Geografia' where id = 1052;
update componente_curricular set descricao_sgp = 'Rec Par Historia' where id = 1053;
update componente_curricular set descricao_sgp = 'Rec Par Português' where id = 1054;
update componente_curricular set descricao_sgp = 'Saúde' where id = 1055;
update componente_curricular set descricao_sgp = 'TIC' where id = 1056;
update componente_curricular set descricao_sgp = 'Escultura Dental' where id = 1058;
update componente_curricular set descricao_sgp = 'Oclusão' where id = 1059;
update componente_curricular set descricao_sgp = 'Informática - OIE' where id = 1060;
update componente_curricular set descricao_sgp = 'Leitura - OSL' where id = 1061;
update componente_curricular set descricao_sgp = 'Metodologia - EJA' where id = 1062;
update componente_curricular set descricao_sgp = 'Ed. Especial' where id = 1063;
update componente_curricular set descricao_sgp = 'Arte FI - Int/Manhã' where id = 1066;
update componente_curricular set descricao_sgp = 'Arte FI - Int/Tarde' where id = 1067;
update componente_curricular set descricao_sgp = 'Leitura (OSL) FI - Int/Manhã' where id = 1068;
update componente_curricular set descricao_sgp = 'Leitura (OSL) FI - Int/Tarde' where id = 1069;
update componente_curricular set descricao_sgp = 'Informática (OIE) FI - Int/Manhã' where id = 1070;
update componente_curricular set descricao_sgp = 'Informática (OIE) FI - Int/Tarde' where id = 1071;
update componente_curricular set descricao_sgp = 'Turno Trab Adi' where id = 1075;
update componente_curricular set descricao_sgp = 'EJA - Modular' where id = 1076;
update componente_curricular set descricao_sgp = 'Qual. Profissional EJA Modular' where id = 1077;
update componente_curricular set descricao_sgp = 'Língua Italiana' where id = 1078;
update componente_curricular set descricao_sgp = 'Caminhada e Alongamento' where id = 1079;
update componente_curricular set descricao_sgp = 'Alongamento' where id = 1080;
update componente_curricular set descricao_sgp = 'Hidroginástica' where id = 1081;
update componente_curricular set descricao_sgp = 'Atividade para Melhor Idade' where id = 1082;
update componente_curricular set descricao_sgp = 'Ginástica Melhor Idade' where id = 1083;
update componente_curricular set descricao_sgp = 'Jump' where id = 1084;
update componente_curricular set descricao_sgp = 'Mix Local' where id = 1085;
update componente_curricular set descricao_sgp = 'Caminhada' where id = 1086;
update componente_curricular set descricao_sgp = 'Libras - Proj Babel' where id = 1087;
update componente_curricular set descricao_sgp = 'Badminton' where id = 1088;
update componente_curricular set descricao_sgp = 'Atend Educ Especializado Convênio' where id = 1089;
update componente_curricular set descricao_sgp = 'Ativ Enriq Curricular Convênio' where id = 1090;
update componente_curricular set descricao_sgp = 'Iniciação ao Mundo do Trabalho Convênio' where id = 1091;
update componente_curricular set descricao_sgp = 'Hidroterapia' where id = 1092;
update componente_curricular set descricao_sgp = 'Yoga' where id = 1093;
update componente_curricular set descricao_sgp = 'Grêmio' where id = 1094;
update componente_curricular set descricao_sgp = 'Museu' where id = 1095;
update componente_curricular set descricao_sgp = 'Fanzine' where id = 1096;
update componente_curricular set descricao_sgp = 'Desenvolvendo Hab Textuais e Matemática' where id = 1097;
update componente_curricular set descricao_sgp = 'Língua Portuguesa - Libras' where id = 1098;
update componente_curricular set descricao_sgp = 'Oficina de Vídeo' where id = 1099;
update componente_curricular set descricao_sgp = 'Oficina' where id = 1100;
update componente_curricular set descricao_sgp = 'Vivência em Libras' where id = 1101;
update componente_curricular set descricao_sgp = 'Cursinho Preparatório' where id = 1102;
update componente_curricular set descricao_sgp = 'Projeto Atendimento Educacional Especializado' where id = 1103;
update componente_curricular set descricao_sgp = 'Regência de Classe Fund I - 5H' where id = 1105;
update componente_curricular set descricao_sgp = 'Língua Inglesa Compartilhada' where id = 1106;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 8H' where id = 1108;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 12H' where id = 1109;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 6H' where id = 1110;
update componente_curricular set descricao_sgp = 'Projetos' where id = 1111;
update componente_curricular set descricao_sgp = 'Regência de Classe Fund I - 4H' where id = 1112;
update componente_curricular set descricao_sgp = 'Regência de Classe EJA - Alfabetização' where id = 1113;
update componente_curricular set descricao_sgp = 'Regência de Classe EJA - Básica' where id = 1114;
update componente_curricular set descricao_sgp = 'Regência de Classe Especial - Diurno' where id = 1115;
update componente_curricular set descricao_sgp = 'Libras Compartilhada' where id = 1116;
update componente_curricular set descricao_sgp = 'Regência de Classe Especial - Noturno' where id = 1117;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 11 Aulas' where id = 1118;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 7 Aulas' where id = 1119;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 5 Aulas' where id = 1120;
update componente_curricular set descricao_sgp = 'Regência de Classe Alfabetização - Int/Tarde' where id = 1121;
update componente_curricular set descricao_sgp = 'Língua Inglesa Compartilhada - Manhã' where id = 1122;
update componente_curricular set descricao_sgp = 'Língua Inglesa Compartilhada - Tarde' where id = 1123;
update componente_curricular set descricao_sgp = 'Regência de Classe Alfabetização - Int/Manhã' where id = 1124;
update componente_curricular set descricao_sgp = 'Regência de Classe EJA - Especial' where id = 1125;
update componente_curricular set descricao_sgp = 'Português Mais Ed' where id = 1126;
update componente_curricular set descricao_sgp = 'Robótica' where id = 1127;
update componente_curricular set descricao_sgp = 'Arte Cultura' where id = 1128;
update componente_curricular set descricao_sgp = 'Orientação Estudos e Leitura' where id = 1129;
update componente_curricular set descricao_sgp = 'Trab. Conclusão de Curso' where id = 1130;
update componente_curricular set descricao_sgp = 'Direito Empresarial' where id = 1131;
update componente_curricular set descricao_sgp = 'Comércio Exterior' where id = 1132;
update componente_curricular set descricao_sgp = 'Anatomia Escult Dental Anteriores' where id = 1133;
update componente_curricular set descricao_sgp = 'Anatomia Escult Dental Posteriores' where id = 1134;
update componente_curricular set descricao_sgp = 'Prótese Dent Saúde Pública' where id = 1135;
update componente_curricular set descricao_sgp = 'Teoria de Prótese' where id = 1136;
update componente_curricular set descricao_sgp = 'Prótese Sobre Implante' where id = 1137;
update componente_curricular set descricao_sgp = 'Sistemas Econômicos' where id = 1138;
update componente_curricular set descricao_sgp = 'Gestão de Pessoas' where id = 1139;
update componente_curricular set descricao_sgp = 'Proces. Tec de Vendas' where id = 1140;
update componente_curricular set descricao_sgp = 'Aplic. Composição e Custos' where id = 1141;
update componente_curricular set descricao_sgp = 'Proc. Financ. e Orçamentário' where id = 1142;
update componente_curricular set descricao_sgp = 'Técnicas de Marketing' where id = 1143;
update componente_curricular set descricao_sgp = 'Estatística Aplicada ao Comércio' where id = 1144;
update componente_curricular set descricao_sgp = 'Comércio Digital' where id = 1145;
update componente_curricular set descricao_sgp = 'Gestão de Competências' where id = 1146;
update componente_curricular set descricao_sgp = 'Estratégia de Compra/Venda' where id = 1147;
update componente_curricular set descricao_sgp = 'Gestão Ambiental' where id = 1148;
update componente_curricular set descricao_sgp = 'Direito do Consumidor' where id = 1149;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 4 Aulas' where id = 1150;
update componente_curricular set descricao_sgp = 'Docência Compartilhada 3 Aulas' where id = 1151;
update componente_curricular set descricao_sgp = 'Saúde Coletiva' where id = 1152;
update componente_curricular set descricao_sgp = 'Educ para o Autocuidado' where id = 1153;
update componente_curricular set descricao_sgp = 'Bioética' where id = 1154;
update componente_curricular set descricao_sgp = 'Psicologia Aplicada Saúde' where id = 1155;
update componente_curricular set descricao_sgp = 'Biossegurança' where id = 1156;
update componente_curricular set descricao_sgp = 'Noções de Primeiros Socorros' where id = 1157;
update componente_curricular set descricao_sgp = 'Organização do Processo de Trabalho em Saúde' where id = 1158;
update componente_curricular set descricao_sgp = 'Politicas de Saúde' where id = 1159;
update componente_curricular set descricao_sgp = 'Dispensação de Medicamentos e Correlatos' where id = 1160;
update componente_curricular set descricao_sgp = 'Produção de Medicamentos e Cosméticos e Controle de Qualidade' where id = 1161;
update componente_curricular set descricao_sgp = 'Organização do Processo de Trabalho em Farmacia' where id = 1162;
update componente_curricular set descricao_sgp = 'Promoção do Uso Racional de Medicamentos' where id = 1163;
update componente_curricular set descricao_sgp = 'Química e Matemática Laboratorial' where id = 1164;
update componente_curricular set descricao_sgp = 'Fisiopatologia Aplicada às Análises Clínicas' where id = 1165;
update componente_curricular set descricao_sgp = 'Coleta e Manipulação de Amostras Biológicas' where id = 1166;
update componente_curricular set descricao_sgp = 'Morfologia e Fisiologia Aplicadas as Analises Clinicas' where id = 1167;
update componente_curricular set descricao_sgp = 'Biologia Molecular Aplicada às Análises Clínicas' where id = 1168;
update componente_curricular set descricao_sgp = 'Urinalise e Citologia Clínica' where id = 1169;
update componente_curricular set descricao_sgp = 'Legislação e Biossegurança nas Ações de Análises Clínicas' where id = 1170;
update componente_curricular set descricao_sgp = 'Parasitologia Clínica I' where id = 1171;
update componente_curricular set descricao_sgp = 'Hematologia Clínica' where id = 1172;
update componente_curricular set descricao_sgp = 'Bioquímica Clínica' where id = 1173;
update componente_curricular set descricao_sgp = 'Imunologia Clínica' where id = 1174;
update componente_curricular set descricao_sgp = 'Microbiologia Clínica' where id = 1175;
update componente_curricular set descricao_sgp = 'Parasitologia Clínica Ii' where id = 1176;
update componente_curricular set descricao_sgp = 'Metodologia Científica Aplicada ao Tcc em Análises Clínicas' where id = 1177;
update componente_curricular set descricao_sgp = 'Noções de Direito Aplicado Saúde' where id = 1178;
update componente_curricular set descricao_sgp = 'Sistemas de Informação' where id = 1179;
update componente_curricular set descricao_sgp = 'Gestão de Materiais' where id = 1180;
update componente_curricular set descricao_sgp = 'Adm de Serviço em Saúde' where id = 1181;
update componente_curricular set descricao_sgp = 'Gestão de Documentos' where id = 1182;
update componente_curricular set descricao_sgp = 'Abastecimento e Patrimônio' where id = 1183;
update componente_curricular set descricao_sgp = 'Gestão de Processos de Trabalho' where id = 1184;
update componente_curricular set descricao_sgp = 'Gestão de Serviço de Apoio' where id = 1185;
update componente_curricular set descricao_sgp = 'Gestão Financeira' where id = 1186;
update componente_curricular set descricao_sgp = 'Planejamento de Pessoal' where id = 1187;
update componente_curricular set descricao_sgp = 'Projeto Integrado em Saúde' where id = 1188;
update componente_curricular set descricao_sgp = 'Radiologia Odontológica I' where id = 1189;
update componente_curricular set descricao_sgp = 'Anatomofisiopatologia' where id = 1190;
update componente_curricular set descricao_sgp = 'Odontologia Preventiva' where id = 1191;
update componente_curricular set descricao_sgp = 'Biossegurança em Odontologia' where id = 1192;
update componente_curricular set descricao_sgp = 'Especialidades Odontológicas I' where id = 1193;
update componente_curricular set descricao_sgp = 'Educação em Saude Bucal I' where id = 1194;
update componente_curricular set descricao_sgp = 'Radiologia Odontológica Ii' where id = 1195;
update componente_curricular set descricao_sgp = 'Especialidades Odontológicas Ii' where id = 1196;
update componente_curricular set descricao_sgp = 'Saúde Coletiva em Odontologia Ii' where id = 1197;
update componente_curricular set descricao_sgp = 'Prática Odontológica Integrada' where id = 1198;
update componente_curricular set descricao_sgp = 'Projeto de Conclusão de Curso' where id = 1199;
update componente_curricular set descricao_sgp = 'Clube de Leitura' where id = 1200;
update componente_curricular set descricao_sgp = 'Aluno Monitor Docente' where id = 1201;
update componente_curricular set descricao_sgp = 'Vivência em Libras Docente' where id = 1202;
update componente_curricular set descricao_sgp = 'Arte Cultura Ed Patrimonial' where id = 1203;
update componente_curricular set descricao_sgp = 'Acomp Pedagogico- Português' where id = 1204;
update componente_curricular set descricao_sgp = 'Projeto Integral - Manhã' where id = 1209;
update componente_curricular set descricao_sgp = 'Projeto Integral - Tarde' where id = 1210;
update componente_curricular set descricao_sgp = 'Regência de Classe Interdisciplinar - Int/Manhã' where id = 1211;
update componente_curricular set descricao_sgp = 'Regência de Classe Interdisciplinar - Int/Tarde' where id = 1212;
update componente_curricular set descricao_sgp = 'Regência de Classe - SP Integral' where id = 1213;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 1' where id = 1214;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 2' where id = 1215;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 3' where id = 1216;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 4' where id = 1217;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 5' where id = 1218;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 6' where id = 1219;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 7' where id = 1220;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 8' where id = 1221;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 9' where id = 1222;
update componente_curricular set descricao_sgp = 'Territ Saber / Exp Pedag 10' where id = 1223;
update componente_curricular set descricao_sgp = 'Projovem Qualificação Profissional' where id = 1224;
update componente_curricular set descricao_sgp = 'Projovem Qualificação Profissional Compartilhada' where id = 1225;
update componente_curricular set descricao_sgp = 'Projovem Participação Cidadã' where id = 1226;
update componente_curricular set descricao_sgp = 'Projovem Matemática' where id = 1227;
update componente_curricular set descricao_sgp = 'Projovem Ciências Naturais' where id = 1228;
update componente_curricular set descricao_sgp = 'Projovem Ciências Humanas' where id = 1229;
update componente_curricular set descricao_sgp = 'Projovem Inglês' where id = 1230;
update componente_curricular set descricao_sgp = 'Projovem Português' where id = 1231;
update componente_curricular set descricao_sgp = 'Projovem Informática' where id = 1232;
update componente_curricular set descricao_sgp = 'Projovem Integração Curricular' where id = 1233;
update componente_curricular set descricao_sgp = 'Sala Acolhimento Projovem' where id = 1234;
update componente_curricular set descricao_sgp = 'Skate' where id = 1235;
update componente_curricular set descricao_sgp = 'Experiência Pedagógica Territorio Saber' where id = 1236;
update componente_curricular set descricao_sgp = 'Mancala' where id = 1237;
update componente_curricular set descricao_sgp = 'Empreendedorismo ( Org. Emp.)' where id = 1238;
update componente_curricular set descricao_sgp = 'Introdução Economia' where id = 1239;
update componente_curricular set descricao_sgp = 'Mat.Comercial e Financeira' where id = 1240;
update componente_curricular set descricao_sgp = 'Direito Tributário / Empresarial' where id = 1241;
update componente_curricular set descricao_sgp = 'Informática Aplicada a Gestão' where id = 1242;
update componente_curricular set descricao_sgp = 'Projeto e Orientação de TCC' where id = 1243;
update componente_curricular set descricao_sgp = 'Psicologia Organizacional' where id = 1244;
update componente_curricular set descricao_sgp = 'Produção Técnica de Textos' where id = 1245;
update componente_curricular set descricao_sgp = 'Proces Financ. e Orçamentários' where id = 1246;
update componente_curricular set descricao_sgp = 'Regência de Classe CIEJA - Alfabetização' where id = 1247;
update componente_curricular set descricao_sgp = 'Regência de Classe CIEJA - Básico' where id = 1248;
update componente_curricular set descricao_sgp = 'Informática (Qualificação Profissional )' where id = 1249;
update componente_curricular set descricao_sgp = 'Oficina CIEJA' where id = 1250;
update componente_curricular set descricao_sgp = 'Pilates' where id = 1251;
update componente_curricular set descricao_sgp = 'Beisebol' where id = 1252;
update componente_curricular set descricao_sgp = 'Português para Imigrantes' where id = 1253;
update componente_curricular set descricao_sgp = 'Brincadeiras Inclusivas' where id = 1254;
update componente_curricular set descricao_sgp = 'Acomp Pedagogico Matemática' where id = 1255;
update componente_curricular set descricao_sgp = 'Anatomia e Fisiologia Humana' where id = 1258;
update componente_curricular set descricao_sgp = 'Cuidados Alimentares e Aspectos da Fala e Deglutição' where id = 1259;
update componente_curricular set descricao_sgp = 'Ética Prof e Função Social do Cuidado do Idoso' where id = 1260;
update componente_curricular set descricao_sgp = 'Legislação e Políticas para Pessoa Idosa' where id = 1261;
update componente_curricular set descricao_sgp = 'Acessibilidade e Prevenção de Acidentes Domésticos' where id = 1262;
update componente_curricular set descricao_sgp = 'Cuidados, Higiene Pessoal e Rotina no Trabalho com o Idoso' where id = 1263;
update componente_curricular set descricao_sgp = 'Noções Básicas Farmacológicas' where id = 1264;
update componente_curricular set descricao_sgp = 'Projeto Interdisciplinar em Saúde' where id = 1265;
update componente_curricular set descricao_sgp = 'Noções Básicas Farmacológicas' where id = 1266;
update componente_curricular set descricao_sgp = 'Prática Profissional' where id = 1267;
update componente_curricular set descricao_sgp = 'Fundamentos de Gerontologia e Geriatria' where id = 1268;
update componente_curricular set descricao_sgp = 'Doenças Crônicas' where id = 1269;
update componente_curricular set descricao_sgp = 'Processo Biopsicossocial da Senescência e Senilidade' where id = 1270;
update componente_curricular set descricao_sgp = 'Cuidados Paliativos' where id = 1271;
update componente_curricular set descricao_sgp = 'Praticas Integrativas e Complementares em Saúde' where id = 1272;
update componente_curricular set descricao_sgp = 'Biossegurança' where id = 1273;
update componente_curricular set descricao_sgp = 'Biologia Celular, Molecular e Genética Aplicada' where id = 1274;
update componente_curricular set descricao_sgp = 'Fisiologia e Fisiopatologia do Sangue' where id = 1275;
update componente_curricular set descricao_sgp = 'Imuno Hematologia Básica' where id = 1276;
update componente_curricular set descricao_sgp = 'Controle de Qualidade em Hemoterapia' where id = 1277;
update componente_curricular set descricao_sgp = 'Captação de Doadores' where id = 1278;
update componente_curricular set descricao_sgp = 'Fundamentos em Imunologia e Sorologia' where id = 1279;
update componente_curricular set descricao_sgp = 'Legislação e Biossegurança em Hemoterapia' where id = 1280;
update componente_curricular set descricao_sgp = 'Estágio Profissional Supervisionado' where id = 1281;
update componente_curricular set descricao_sgp = 'Imuno Hematologia Clínica' where id = 1282;
update componente_curricular set descricao_sgp = 'Doenças Hematológicas' where id = 1283;
update componente_curricular set descricao_sgp = 'Doenças Infecciosas Transmitidas pelo Sangue' where id = 1284;
update componente_curricular set descricao_sgp = 'Metodologia Científica Aplicada ao Tcc Curso Tec Hemoterapia' where id = 1285;
update componente_curricular set descricao_sgp = 'Processamento e Armazenamento Hemocomponentes e Células Tronco Medu' where id = 1286;
update componente_curricular set descricao_sgp = 'Utilização de Hemocomponentes' where id = 1287;
update componente_curricular set descricao_sgp = 'Ed. Física Integral Manha' where id = 1288;
update componente_curricular set descricao_sgp = 'Ed. Física Integral Tarde' where id = 1289;
update componente_curricular set descricao_sgp = 'Regência de Classe EMEBS - SP Integral' where id = 1290;
update componente_curricular set descricao_sgp = 'Apoio Pedag SP Integral 5 Ano' where id = 1291;
update componente_curricular set descricao_sgp = 'Apoio Pedagógico SP Integral 9 Ano' where id = 1292;
update componente_curricular set descricao_sgp = 'Orientação de Estudos e Projetos' where id = 1293;
update componente_curricular set descricao_sgp = 'Parkour' where id = 1294;
update componente_curricular set descricao_sgp = 'Acompanhamento Pedagógico Ciências' where id = 1295;
update componente_curricular set descricao_sgp = 'Acompanhamento Clube de Informática Jogos' where id = 1296;
update componente_curricular set descricao_sgp = 'TIC História em Quadrinhos' where id = 1297;
update componente_curricular set descricao_sgp = 'Artes Cênicas Dança Zumba' where id = 1298;
update componente_curricular set descricao_sgp = 'Esporte Luta Hapkido' where id = 1299;
update componente_curricular set descricao_sgp = 'Língua Portuguesa em Libras' where id = 1300;
update componente_curricular set descricao_sgp = 'Regência de Classe Surdocegueira' where id = 1301;
update componente_curricular set descricao_sgp = 'Acomp Pedagogico Alfabetização' where id = 1302;
update componente_curricular set descricao_sgp = 'Experiências Motoras e Lúdicas Primeira Infancia' where id = 1303;
update componente_curricular set descricao_sgp = 'Reforço Acompanhamento Leitura' where id = 1304;
update componente_curricular set descricao_sgp = 'Futebol' where id = 1305;
update componente_curricular set descricao_sgp = 'Acoes de Apoio Pedagógico' where id = 1306;
update componente_curricular set descricao_sgp = 'Reg Classe Esc Partic Creche Manhã' where id = 1307;
update componente_curricular set descricao_sgp = 'Reg Classe Esc Partic Pre' where id = 1308;
update componente_curricular set descricao_sgp = 'Reg Classe Esc Partic Creche Tarde' where id = 1309;
update componente_curricular set descricao_sgp = 'PAEE Colaborativo' where id = 1310;
update componente_curricular set descricao_sgp = 'Cultura dos Países de Língua Espanhola' where id = 1311;
update componente_curricular set descricao_sgp = 'Tecnologias para Aprendizagem' where id = 1312;
update componente_curricular set descricao_sgp = 'Produção Textual' where id = 1313;
update componente_curricular set descricao_sgp = 'Investigação Científica e Processos Matemáticos' where id = 1314;
update componente_curricular set descricao_sgp = 'Ações de Apoio Pedagógico' where id = 1315;
update componente_curricular set descricao_sgp = 'Sociedade, Cultura e Multiculturalismo' where id = 1316;
update componente_curricular set descricao_sgp = 'Práticas Esportivas' where id = 1317;
update componente_curricular set descricao_sgp = 'Expressões Culturais Artísticas' where id = 1318;
update componente_curricular set descricao_sgp = 'Sala de Leitura' where id = 1319;
update componente_curricular set descricao_sgp = 'Laboratório de Educação Digital' where id = 1320;
update componente_curricular set descricao_sgp = 'Trabalho Colaborativo de Autoria - TCA' where id = 1321;
update componente_curricular set descricao_sgp = 'PAP - Recuperação de Aprendizagens' where id = 1322;
update componente_curricular set descricao_sgp = 'POSL Docente Ed Infantil EMEBS' where id = 1323;
update componente_curricular set descricao_sgp = 'POIE - Docente Ed Infantil EMEBS' where id = 1324;
update componente_curricular set descricao_sgp = 'Ética, Convivência e Protagonismo' where id = 1325;
update componente_curricular set descricao_sgp = 'Jogos' where id = 1326;
update componente_curricular set descricao_sgp = 'Arte e Cultura' where id = 1327;

