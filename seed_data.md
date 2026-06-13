# SEED DATA - Modelo Stock Mantenimiento UCC

> Este archivo contiene la representación completa del Excel de clasificación de productos para generar el seed de base de datos.


## 00_README

| Modelo seed de stock - Mantenimiento UCC | Unnamed: 1 |
|---|---|
|  |  |
| Contenido |  |
| 01_Rubros | Rubros sugeridos con IDs y criterio de clasificación. |
| 02_Familias | Familias sugeridas por rubro, con cantidad de productos clasificados y ejemplos. |
| 03_Atributos | Catálogo maestro de atributos únicos. |
| 04_Familia_Atributos | Tabla relacional para seed: cada familia con sus atributos obligatorios/opcionales. |
| 05_Productos_Clasificados | Referencia de clasificación sugerida para los 290 productos del archivo original. |
| 06_Resumen | Resumen de cantidades y observaciones de limpieza. |
|  |  |
| Regla de uso |  |
| Los atributos se asignan a nivel familia y luego replican en productos. |  |
| Cuando un atributo tiene condición de aplicación y está marcado como Obligatorio, debe cargarse obligatoriamente si esa condición se cumple. |  |
| No se recomienda crear familias por marca, medida, potencia, color o modelo; esos datos deben vivir como atributos. |  |

## 01_Rubros

| rubro_id | rubro | descripcion | criterio_clasificacion | productos_referencia |
|---|---|---|---|---|
| ELEC | Electricidad | Materiales eléctricos de conexión, protección, maniobra, conductores, canalización y gabinetes. | Uso técnico eléctrico; no incluye luminarias/lámparas salvo accesorios eléctricos de soporte. | 140 |
| ILUM | Iluminación | Lámparas, luminarias, iluminación de emergencia, señalización luminosa y sistemas LED. | Productos cuya función principal es iluminar, señalizar o alimentar sistemas de iluminación. | 54 |
| SAN | Sanitario / Plomería | Materiales para agua, desagüe, grifería y repuestos sanitarios. | Productos asociados a instalaciones sanitarias, grifería, descargas, conexiones y cañerías. | 47 |
| CLIM | Climatización y ventilación | Refrigerantes, extractores, ventiladores y controles de aire. | Productos asociados a aire acondicionado, refrigeración y ventilación. | 6 |
| EQEM | Equipos electromecánicos | Bombas y equipos/controles asociados a operación hídrica o electromecánica. | Equipos que combinan función eléctrica y mecánica dentro del mantenimiento edilicio. | 3 |
| SEG | Seguridad edilicia | Elementos de seguridad contra incendio o señalización edilicia no eléctrica. | Productos vinculados a seguridad física del edificio. | 2 |
| CERR | Cerrajería y accesos | Cerraduras, cierrapuertas, destrabas y accesorios de acceso. | Control físico o electromecánico de puertas, muebles y accesos. | 8 |
| PINT | Pintura y terminaciones | Pinturas, recubrimientos, herramientas de aplicación, selladores y adhesivos. | Terminaciones superficiales, aplicación de pintura, sellado y reparación menor. | 28 |
| EQUI | Equipamiento edilicio | Elementos funcionales del edificio o aulas que no encajan en rubros técnicos anteriores. | Bienes físicos de uso operativo/aula que se stockean como insumos o repuestos. | 2 |

## 02_Familias

| familia_id | rubro_id | rubro | familia | descripcion | productos_referencia | ejemplos_productos |
|---|---|---|---|---|---|---|
| MTF | ELEC | Electricidad | Mecanismos, tomas y fichas | Bastidores, módulos, tomas, fichas, teclas, zapatillas, tótems y estaciones de carga. | 29 | E1: BASTIDOR SCHNEIDER RODA / E2: BASTIDOR SCHNEIDER  BASE / E3: TAPA MARCO SCHNEIDER BASE / E4: MODULO PUNTO SCHNEIDER BASE / E5: MODULO CIEGO SCHNEIDER BASE |
| PROT | ELEC | Electricidad | Protección eléctrica | Térmicas, disyuntores, fusibles, guardamotores, repartidores, borneras y barras. | 28 | E19: DISYUNTOR  4P 25A 30mA  SCHNEIDER  EZ9R36425 / E20: TERMICA  UNIPOLAR  10A  SCHNEIDER / E21: TERMICA UNIPOLAR 16A SCHNEIDER / E22: TERMICA UNIPOLAR 20A SCHNEIDER / E23: TERMICA TETRAPOLAR  4P 40A SCHNEIDER |
| MANI | ELEC | Electricidad | Maniobra y control eléctrico | Contactores, timers, fotocélulas, detectores, Sonoff, condensadores y dispositivos de control. | 14 | E15: MODULO DETECTOR DE MOVIMIENTO  SCHNEIDER RODA WDA56101 / E16: ZOCALO PARA FOTOCELULA / E17: FOTOCELULA / E25: CONTACTOR ESCHNEIDER  TRIFASICO 32A  BOBINA 24V NA  17899 / E26: CONTACTOR MODULAR SCHNEIDER  ICT 25A 250V |
| COND | ELEC | Electricidad | Conductores eléctricos | Cables unipolares y otros conductores eléctricos. | 12 | E110: CABLE DE 1,5mm UNIPOLAR  ROJO 100 mts / E111: CABLE UNIPOLAR DE 1.5mm  CELESTE  100 mts / E112: CABLE UNIPOLAR DE 1.5mm VERDE AMARILLO 100 mts / E113: CABLE UNIPOLAR DE 1.5 mm ROJO  50 mts / E114: CABLE UNIPOLAE DE 1.5 mm CELESTE 50 mts |
| CAN | ELEC | Electricidad | Canalización eléctrica | Caños, cablecanal, bandejas portacable, curvas, acoples, grampas, conectores y prensa cables. | 33 | E72: ACOPLE  TUBE ELECTRIC   0,16 / E73: CURVA  TUBE ELECTRIC 22 mm / E74: GRAMPA  TUBE ELECTRIC  22mm / E75: CONECTOR TUBE ELECTRIC 22mm / E76: ACOPLE  TUBE ELECTRIC   22 mm |
| CG | ELEC | Electricidad | Cajas y gabinetes eléctricos | Cajas, gabinetes, tapas ciegas, porta bastidores y envolventes eléctricos. | 20 | E61: CAJA OCTOGONAL DE EMBUTIR PVC   CTR33 100mm / E62: CAJA OCTOGONAL DE EMBUTIR PVC   90mm / E63: CAJA OCTOGONAL DE CHAPA  PARA EMBUTIR 80mm / E64: TAPA CIEGA 10X5 PVC / E65: TAPA CIEGA 5X5 PVC |
| ACC | ELEC | Electricidad | Accesorios eléctricos generales | Jabalinas, cintas aisladoras, portalámparas, zócalos, accesorios menores y consumibles eléctricos. | 4 | E59: ZOCALO CERAMICO PARA AR111 / E95: PORTA LAMPARA CERAMICA E40 / E197: JABALINA DE 1,5M / E198: CINTA AISLADORA 19MM X 20MTS 3M |
| LAM | ILUM | Iluminación | Lámparas y tubos | Lámparas LED/halógenas/halogenuro, dicroicas, AR111 y tubos LED. | 17 | E37: LAMPARA 80W E40 LUZ FRIA  LEDVANCE / E40: LAMPARA LED E27 27W / E43: AR111 LAMPARA  LED 15W AKAI ENERGY LUZ FRIA / E44: AR111 12W LUZ CALIDA  OSRAM / E45: AR111 11W 12VLUZ FRIA |
| LUM | ILUM | Iluminación | Luminarias | Plafones, reflectores, paneles, spots, apliques, farolas y bases de luminarias. | 27 | E31: REFLECTOR LED  30W LUZ CALIDA AKAI ENERGY / E32: REFLECTOR LED 100W LUZ CALIDA ENERGENERATION / E36: REFLECTOR  LED 125W LEDVANCE / E38: PLAFON LED CUADRADO PARA APLICAR 6W LUZ FRIA LUMENAC POLO P6 / E39: REFLECTOR LED 250W PROLITE |
| EME | ILUM | Iluminación | Iluminación de emergencia y señalización | Luces de emergencia y carteles de salida/señalización luminosa. | 4 | E34: LUZ DE EMERGENCIA  60 LEDS  LCI-ME60 / E35: LUZ DE EMERGENCIA 90LEDS GAMASONIC / E100: CARTEL DE SEÑALIZACION DE SALIDA  13 LED 250V ALIC / E101: LUZ DE EMERGENCIA  48 LED LCI  (MIKEY) |
| SLED | ILUM | Iluminación | Sistemas LED y rieles | Tiras LED, fuentes para tira LED, tracklights y rieles. | 6 | E153: LEDVANCE TRACKLIGHT PARA 30 E27  MAX31W / E154: LEDVANCE TRACKLIGHT  AR111 GU10 MAX 14W / E155: LEDVANCE RIEL  1M BLACK  PARA APLICAR / E181: FUENTE PARA TIRA DE LED 220V 2,1AMPER  A13985 / E182: FUENTE DE ALIMENT. P/TIRA DE LED   60W 220V ENTRADA/ SALIDA 12V 5A TBC |
| CON | SAN | Sanitario / Plomería | Conexiones y flexibles | Flexibles, niples, uniones dobles, acoples y accesorios de conexión. | 12 | S1: FLEXIBLE METALICO  3/4"  50CM LARGO / S2: FLEXIBLE METALICO 1/2" 5O CM LARGO / S18: ACOPLE RAPIDO  PVC  DUKE 1 1/4" / S19: ACOPLE RAPIDO  PVC  DUKE 1 " / S20: UNION DOBLE DE POLIPROPILENO DE 1 1/4" |
| DES | SAN | Sanitario / Plomería | Desagües y cañerías | Codos, curvas, ramales, T, manguitos, sifones, piletas de patio y descargas. | 12 | S7: PILETA DE PATIO POLIANGULAR DE DE 40X63 AWADUCT / S8: CURVA DE PVC  45° 110 AWADUCT / S9: SIFON PAR APILETA DE PATIO AWADUCT / S10: SIFON DE BACHA PVC / S11: RAMAL Y DE 40 AWADUCT |
| GRI | SAN | Sanitario / Plomería | Grifería y llaves de paso | Canillas, grifos y llaves de paso. | 4 | S6: LLAVE DE PASO FUSION 1 1/4" / S23: GRIFO  P/PARED DECAMATIC / S32: Canilla de 1/2"de bronce / S33: Canilla automatica par alavatorio VASSER 50/1013,01 |
| REP | SAN | Sanitario / Plomería | Repuestos sanitarios | Pistones, retenes, botones, asientos, aros base, cadenas y repuestos FV/Ferrum. | 15 | S21: ARO BASE DE INDORO / S22: ARO BASE DE INDORO  DESPLAZADO / S24: REPUESTO FV TORNILLOS Y ESPARRAGOS  DE BRONCE PAR AVALVULA DE DESCARGA. 0367,15,0-B CONJ.N/5 / S25: REPUESTO FV  RESORTE Y MANIJA REGULADORA CONJ.N°5 / S26: REPUESTO FV PISTON COMPETO CON GRAMPA  PARA  VALVULA DE DESCARGA |
| TAP | SAN | Sanitario / Plomería | Tapas, rejillas y accesorios sanitarios | Tapas ciegas, rejillas, marcos y accesorios sanitarios de terminación. | 4 | S3: TAPA CIEGA CON MARCO  14,5X14,5 CM / S4: TAPA CIEGA CON MARCO 11X11 CM / S5: REJILLA CON MARCO / S34: SOPAPAPA REJILLA DE 40 PAR AVACHA |
| REF | CLIM | Climatización y ventilación | Refrigerantes | Gases refrigerantes y envases asociados. | 2 | R1: REFRIGERANTE FREON 410A  11,35 KG / R2: REFRIGERANTE  R22 13,5 KG |
| VENT | CLIM | Climatización y ventilación | Ventilación y controles HVAC | Extractores, ventiladores y controles programables/universales de aire. | 4 | E42: EXTRACTOR DE AIRE HY-VF 100 BLANCO / E103: CONTROL DE AIRE PROGRAMABLE UNIVERSAL   COOLTECH / E189: EXTRACTOR DE AIRE HY-VF 100 BLANCO / E190: EXTRACTOR DE AIRE  Y VENTILADOR 50W 220V HIDRA HY-VF250B |
| BOM | EQEM | Equipos electromecánicos | Bombas y control hídrico | Bombas, flotantes eléctricos y cicladores/controladores de bombas. | 3 | E41: BOMBA SUMERGIBLE DE DESAGOTE RW DRAIN Q400F / E188: FLOTANTE ELECTRICO 1,5MTS  VIYILANT / E204: CICLADOR  RBC SITEL 220V 3 AMP. PARA 2 BOMBAS |
| HID | SEG | Seguridad edilicia | Hidrantes | Mangueras, gabinetes y accesorios de hidrantes. | 2 | E108: MANGUERAS DE HIDRANTES / E109: GABINETE  ROJO P/ HIDRANTE |
| CER | CERR | Cerrajería y accesos | Cerraduras | Cerraduras de muebles, cilíndricas, de pomo, Kallay, Prive y similares. | 5 | C1: CERRRADURA DE PULSO PARA MUEBLES / C2: CERRADURA PRIVE 200 / C3: CERRADURA KALLAY 4003 / C4: CERRADURA KALLAY 5003 / C7: CERRADURA CILINDRICA CON POMO FPV |
| CIE | CERR | Cerrajería y accesos | Cierrapuertas | Cierrapuertas hidráulicos y dispositivos libre/ocupado. | 2 | C5: CIERRAPUERTA HIDRAULICO OCB / C6: CIERRRA PUERTA /BAÑO LIBRE OCUPADO |
| ACC | CERR | Cerrajería y accesos | Control de acceso y destrabas | Destrabas eléctricas y elementos eléctricos de acceso. | 1 | E180: DESTRABA ELECTRICO  BOBINA 12V 5W ROA |
| HERR | PINT | Pintura y terminaciones | Herramientas de aplicación | Rodillos, mini rodillos, pinceles y pinceletas. | 9 | P1: RODILLOS PARA SINTETICO DE POLIESTER Y TELA 10 CM / P2: MINI RODILLO PARA EPOXI 10CM / P3: MINI RODILLO PARA EPOXI 5CM / P4: RODILLO PELO CORTO  22CM ROSAPIN / P5: RODILLO DE LANA  22CM  CACIQUE |
| REC | PINT | Pintura y terminaciones | Pinturas y recubrimientos | Látex, sintéticos, epoxi, barniz, convertidores, impermeabilizantes y revestimientos. | 15 | P13: IMPERMEABILIZANTE QUIMEX  X 20KG / P14: SINTETICO BRILLANTE GRIS ESPACIAL X 1LITRO / P15: BARNIZ MARINO X 1 LITRO / P16: CONVERTIDOR OXIDO GRIS X 1 LITRO / P17: SINTETICO SATINADO MARRON TOBAGO X 1 LITRO |
| SELL | PINT | Pintura y terminaciones | Selladores y adhesivos | Siliconas, selladores, SikaFlex y sellarosca. | 4 | S46: Sella rosca 125 cc / P10: SILICONA ANTIHONGOS  BLANCO / P11: SILICONA  TRANSPARENTE / P12: SELLADOR SICAFLEX 1A PLUS |
| AULA | EQUI | Equipamiento edilicio | Equipamiento de aula y presentación | Pizarrones, rotafolios y elementos de presentación. | 2 | E106: PIZARRON MOBIL / E107: ROTAFOLIO |

## 03_Atributos

| atributo_id | atributo | descripcion | tipo_dato_default | unidad_sugerida | valores_sugeridos |
|---|---|---|---|---|---|
| ATR-001 | Código interno | Identificador/código del producto dentro del stock. | STRINGo |  |  |
| ATR-002 | Nombre normalizado | Nombre técnico limpio y consistente del producto. | STRINGo |  |  |
| ATR-003 | Unidad de medida | Unidad con la que se controla el stock. | Lista |  | Ud; metro; rollo; litro; kg; envase |
| ATR-004 | Marca | Marca. | STRINGo |  |  |
| ATR-005 | Modelo / código fabricante | Modelo. | STRINGo |  |  |
| ATR-006 | Observaciones | Información complementaria no estructurada. | STRINGo largo |  |  |
| ATR-007 | Tipo de pieza | Tipo de pieza. | Lista |  | tapa ciega; rejilla; marco; accesorio de terminación |
| ATR-008 | Línea / gama | Línea comercial o gama del mecanismo. | STRINGo |  |  |
| ATR-009 | Corriente nominal | Corriente nominal. | Número | A |  |
| ATR-010 | Tensión nominal | Tensión. | Número | V |  |
| ATR-011 | Cantidad de módulos / bocas | Capacidad modular. | Número |  |  |
| ATR-012 | Tipo de instalación | Instalación. | STRINGo |  |  |
| ATR-013 | Color | Color. | STRINGo |  |  |
| ATR-014 | Grado IP | Protección IP. | STRINGo |  |  |
| ATR-015 | Tipo de dispositivo | Tipo. | Lista |  | destraba eléctrica; electroimán; lector; otro |
| ATR-016 | Cantidad de polos | Cantidad de polos o vías. | Lista |  | 1P; 2P; 3P; 4P; bipolar; tetrapolar |
| ATR-017 | Sensibilidad diferencial | Sensibilidad de disparo. | Número | mA |  |
| ATR-018 | Curva / clase | Curva o clase de protección. | STRINGo |  |  |
| ATR-019 | Capacidad de corte | Capacidad de interrupción. | STRINGo |  |  |
| ATR-020 | Montaje | Forma de montaje. | Lista |  | riel DIN; embutir; aplicar; zócalo; otro |
| ATR-021 | Tensión de bobina | Tensión. | Número | V |  |
| ATR-022 | Cantidad de polos / contactos | Cantidad de polos o contactos. | STRINGo |  |  |
| ATR-023 | Tipo de contacto | Tipo de contacto. | Lista |  | NA; NC; NA+NC; no informado |
| ATR-024 | Rango de temporización / programación | Detalle de programación o temporización. | STRINGo |  |  |
| ATR-025 | Función de control | Función específica del dispositivo. | STRINGo |  |  |
| ATR-026 | Tipo de conductor | Tipo de cable/conductor. | Lista |  | unipolar; multipolar; desnudo; otro |
| ATR-027 | Sección | Sección nominal. | Número | mm² |  |
| ATR-028 | Color del conductor | Color normalizado del cable. | STRINGo |  |  |
| ATR-029 | Material del conductor | Material conductor. | STRINGo |  |  |
| ATR-030 | Largo de presentación | Largo de tramo o presentación. | Número | m |  |
| ATR-031 | Presentación | Presentación. | STRINGo | cc / ml / unidad |  |
| ATR-032 | Material | Material. | STRINGo |  |  |
| ATR-033 | Medida / diámetro / sección | Medida técnica principal. | STRINGo | mm |  |
| ATR-034 | Marca / sistema | Sistema compatible. | STRINGo |  |  |
| ATR-035 | Tipo de caja / gabinete | Tipo de envolvente. | Lista |  | caja octogonal; caja exterior; gabinete DIN; gabinete estanco; tapa ciega; gabinete metálico |
| ATR-036 | Dimensiones | Medida. | STRINGo | cm / mm |  |
| ATR-037 | Tapa incluida | Indica si incluye tapa. | Booleano |  | Sí; No |
| ATR-038 | Tipo de accesorio | Tipo de accesorio eléctrico. | Lista |  | jabalina; cinta aisladora; portalámpara; zócalo; accesorio menor; otro |
| ATR-039 | Medida | Medida. | Número | cm / pulgadas |  |
| ATR-040 | Compatibilidad | Compatibilidad técnica. | STRINGo |  |  |
| ATR-041 | Tipo de lámpara | Tipo de lámpara. | Lista |  | lámpara; tubo; dicroica; AR111; halógena; halogenuro; vintage |
| ATR-042 | Tecnología | Tecnología. | Lista |  | LED; otra |
| ATR-043 | Potencia | Potencia. | Número | W |  |
| ATR-044 | Rosca / base | Base o rosca. | STRINGo |  |  |
| ATR-045 | Temperatura de color | Color de luz. | Lista |  | cálida; neutra; fría; no informado |
| ATR-046 | Flujo luminoso | Lúmenes. | Número | lm |  |
| ATR-047 | Formato / forma | Formato físico. | STRINGo |  |  |
| ATR-048 | Dimerizable | Apto para dimmer. | Booleano |  | Sí; No |
| ATR-049 | Tipo de luminaria | Tipo de luminaria. | Lista |  | plafón; reflector; panel; spot; aplique; farola; base de luminaria |
| ATR-050 | Forma / formato | Forma. | STRINGo |  |  |
| ATR-051 | Color de carcasa | Color visible. | STRINGo |  |  |
| ATR-052 | Incluye base / zócalo | Indica si incluye base o zócalo. | Booleano |  | Sí; No |
| ATR-053 | Tipo de elemento | Tipo de elemento. | Lista |  | manguera; gabinete; accesorio hidrante |
| ATR-054 | Cantidad de LEDs | Cantidad de LEDs. | Número |  |  |
| ATR-055 | Autonomía | Autonomía de batería. | STRINGo | h |  |
| ATR-056 | Tipo de montaje | Montaje. | STRINGo |  |  |
| ATR-057 | Mensaje / señal | STRINGo o pictograma indicado. | STRINGo |  |  |
| ATR-058 | Tipo de sistema LED | Tipo de sistema. | Lista |  | tira LED; fuente; tracklight; riel; accesorio |
| ATR-059 | Tensión de entrada | Entrada eléctrica. | Número | V |  |
| ATR-060 | Tensión de salida | Salida eléctrica. | Número | V |  |
| ATR-061 | Corriente | Corriente. | Número | A |  |
| ATR-062 | Longitud | Largo. | Número | m |  |
| ATR-063 | Rosca / base compatible | Base compatible. | STRINGo |  |  |
| ATR-064 | Color / terminación | Terminación. | STRINGo |  |  |
| ATR-065 | Tipo de conexión | Tipo de conexión. | Lista |  | flexible; niple; unión doble; acople; conexión; junta |
| ATR-066 | Diámetro / rosca | Diámetro o rosca. | STRINGo | pulgadas |  |
| ATR-067 | Largo | Largo. | Número | cm |  |
| ATR-068 | Tipo de extremo | Tipo de conexión en extremos. | STRINGo |  |  |
| ATR-069 | Presión de trabajo | Presión admitida. | STRINGo |  |  |
| ATR-070 | Diámetro | Diámetro. | Número | mm |  |
| ATR-071 | Ángulo | Ángulo. | Número | ° |  |
| ATR-072 | Tipo de unión | Tipo de unión. | STRINGo |  |  |
| ATR-073 | Tipo de grifería | Tipo de grifería. | Lista |  | canilla; grifo; llave de paso; automática |
| ATR-074 | Accionamiento | Tipo de accionamiento. | Lista |  | manual; automática; válvula; otro |
| ATR-075 | Aplicación | Uso. | STRINGo |  |  |
| ATR-076 | Marca / modelo | Marca/modelo. | STRINGo |  |  |
| ATR-077 | Tipo de repuesto | Tipo de repuesto. | Lista |  | pistón; retenes; tornillos; resorte; manija; aro; asiento; botón; cadena; conjunto descarga; válvula |
| ATR-078 | Compatible con | Elemento compatible. | STRINGo |  |  |
| ATR-079 | Marca compatible | Marca compatible. | STRINGo |  |  |
| ATR-080 | Modelo / código compatible | Código compatible. | STRINGo |  |  |
| ATR-081 | Tipo de refrigerante | Tipo de gas. | Lista |  | R410A; R22; otro |
| ATR-082 | Compatibilidad / uso | Uso compatible. | STRINGo |  |  |
| ATR-083 | Tipo de envase | Envase. | STRINGo |  |  |
| ATR-084 | Observación de seguridad | Advertencia o seguridad. | STRINGo largo |  |  |
| ATR-085 | Tipo de equipo | Tipo de equipo. | Lista |  | bomba; flotante eléctrico; ciclador; controlador de bomba |
| ATR-086 | Caudal / capacidad | Caudal de aire si se conoce. | STRINGo |  |  |
| ATR-087 | Diámetro / medida | Medida. | STRINGo | mm |  |
| ATR-088 | Modo de control / compatibilidad | Compatibilidad o modo de control. | STRINGo |  |  |
| ATR-089 | Cantidad de bombas controladas | Cantidad de bombas. | Número |  |  |
| ATR-090 | Largo de cable | Largo. | Número | m |  |
| ATR-091 | Compatibilidad / norma | Compatibilidad o norma. | STRINGo |  |  |
| ATR-092 | Tipo de cerradura | Tipo de cerradura. | Lista |  | mueble; cilíndrica; pomo; pulso; embutir; exterior |
| ATR-093 | Medida / cilindro | Medida o cilindro. | STRINGo |  |  |
| ATR-094 | Terminación | Terminación. | Lista |  | brillante; satinado; mate; no informado |
| ATR-095 | Sentido de apertura | Sentido. | Lista |  | derecha; izquierda; reversible; no aplica |
| ATR-096 | Tipo de cierrapuerta | Tipo. | Lista |  | hidráulico; libre/ocupado; otro |
| ATR-097 | Fuerza / tamaño | Fuerza o tamaño. | STRINGo |  |  |
| ATR-098 | Tipo de herramienta | Tipo. | Lista |  | rodillo; mini rodillo; pincel; pinceleta |
| ATR-099 | Material / pelo | Material o pelo. | STRINGo |  |  |
| ATR-100 | Uso recomendado | Uso recomendado. | STRINGo |  |  |
| ATR-101 | Tipo de pintura / recubrimiento | Tipo. | Lista |  | látex; sintético; epoxi; barniz; convertidor; impermeabilizante; revestimiento; piso deportivo |
| ATR-102 | Uso | Uso. | STRINGo |  |  |
| ATR-103 | Base / composición | Base. | STRINGo |  |  |
| ATR-104 | Rendimiento | Rendimiento. | STRINGo | m²/l |  |
| ATR-105 | Tipo de producto | Tipo. | Lista |  | silicona; sellador; sellarosca; adhesivo |
| ATR-106 | Base química | Base química. | STRINGo |  |  |
| ATR-107 | Tipo de equipamiento | Tipo. | Lista |  | pizarrón; rotafolio; accesorio aula; otro |
| ATR-108 | Movilidad | Indica si es móvil. | Booleano |  | Sí; No |

## 04_Familia_Atributos

| familia_atributo_id | rubro_id | rubro | familia_id | familia | atributo_id | atributo | obligatoriedad | condicion_aplicacion | tipo_dato | unidad_sugerida | valores_sugeridos | ejemplo | categoria | orden |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| FAMATR-0001 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0002 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0003 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0004 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0005 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0006 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0007 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0008 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0009 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0010 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0011 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0012 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0013 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0014 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0015 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0016 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0017 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0018 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0019 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0020 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0021 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0022 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0023 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0024 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0025 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0026 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0027 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0028 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0029 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0030 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0031 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0032 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0033 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0034 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0035 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0036 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0037 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0038 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0039 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0040 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0041 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0042 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0043 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0044 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0045 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0046 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0047 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0048 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0049 | ILUM | Iluminación | LUM | Luminarias | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0050 | ILUM | Iluminación | LUM | Luminarias | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0051 | ILUM | Iluminación | LUM | Luminarias | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0052 | ILUM | Iluminación | LUM | Luminarias | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0053 | ILUM | Iluminación | LUM | Luminarias | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0054 | ILUM | Iluminación | LUM | Luminarias | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0055 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0056 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0057 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0058 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0059 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0060 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0061 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0062 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0063 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0064 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0065 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0066 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0067 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0068 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0069 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0070 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0071 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0072 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0073 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0074 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0075 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0076 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0077 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0078 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0079 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0080 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0081 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0082 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0083 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0084 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0085 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0086 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0087 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0088 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0089 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0090 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0091 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0092 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0093 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0094 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0095 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0096 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0097 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0098 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0099 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0100 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0101 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0102 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0103 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0104 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0105 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0106 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0107 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0108 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0109 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0110 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0111 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0112 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0113 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0114 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0115 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0116 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0117 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0118 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0119 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0120 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0121 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0122 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0123 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0124 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0125 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0126 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0127 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0128 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0129 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0130 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0131 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0132 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0133 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0134 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0135 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0136 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0137 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0138 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0139 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0140 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0141 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0142 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0143 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0144 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0145 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0146 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0147 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0148 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0149 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0150 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0151 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0152 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0153 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0154 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0155 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0156 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0157 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-001 | Código interno | Obligatorio |  | STRINGo |  |  |  | Base común | 1 |
| FAMATR-0158 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-002 | Nombre normalizado | Obligatorio |  | STRINGo |  |  |  | Base común | 2 |
| FAMATR-0159 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-003 | Unidad de medida | Obligatorio |  | Lista |  | Ud; metro; rollo; litro; kg; envase |  | Base común | 3 |
| FAMATR-0160 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-004 | Marca | Opcional |  | STRINGo |  |  |  | Base común | 4 |
| FAMATR-0161 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Base común | 5 |
| FAMATR-0162 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-006 | Observaciones | Opcional |  | STRINGo largo |  |  |  | Base común | 6 |
| FAMATR-0163 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-007 | Tipo de pieza | Obligatorio |  | Lista |  | bastidor; módulo; toma; ficha; tecla; estación de carga; tótem; zapatilla | toma doble | Específico | 7 |
| FAMATR-0164 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-008 | Línea / gama | Obligatorio |  | STRINGo |  |  | Roda / Base / Cambre | Específico | 8 |
| FAMATR-0165 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-009 | Corriente nominal | Obligatorio | Para tomas, fichas, teclas y estaciones de carga. | Número | A |  | 10 A / 20 A | Específico | 9 |
| FAMATR-0166 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-010 | Tensión nominal | Obligatorio | Para dispositivos eléctricos activos o de conexión. | Número | V |  | 220 V | Específico | 10 |
| FAMATR-0167 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-011 | Cantidad de módulos / bocas | Obligatorio | Para bastidores, cajas de mecanismos, tomas, tótems o estaciones. | Número |  |  | 2 bocas / 3 tomas | Específico | 11 |
| FAMATR-0168 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-012 | Tipo de instalación | Obligatorio |  | Lista |  | embutir; aplicar; exterior; industrial; portátil | embutir | Específico | 12 |
| FAMATR-0169 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-013 | Color | Opcional |  | STRINGo |  |  | blanco / gris | Específico | 13 |
| FAMATR-0170 | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | ATR-014 | Grado IP | Opcional |  | STRINGo |  |  | IP40 / IP69 | Específico | 14 |
| FAMATR-0171 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-015 | Tipo de dispositivo | Obligatorio |  | Lista |  | térmica; disyuntor diferencial; fusible; guardamotor; bornera; repartidor; barra | térmica | Específico | 7 |
| FAMATR-0172 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-009 | Corriente nominal | Obligatorio |  | Número | A |  | 16 A / 40 A / 100 A | Específico | 8 |
| FAMATR-0173 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-016 | Cantidad de polos | Obligatorio | Para térmicas, disyuntores, repartidores y borneras. | Lista |  | 1P; 2P; 3P; 4P; bipolar; tetrapolar | 4P | Específico | 9 |
| FAMATR-0174 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-010 | Tensión nominal | Obligatorio | Para protecciones y repartidores eléctricos. | Número | V |  | 250 V / 400 V / 1000 V | Específico | 10 |
| FAMATR-0175 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-017 | Sensibilidad diferencial | Obligatorio | Solo si tipo de dispositivo = disyuntor diferencial. | Número | mA |  | 30 mA | Específico | 11 |
| FAMATR-0176 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-018 | Curva / clase | Obligatorio | Para térmicas, fusibles y guardamotores cuando se informe técnicamente. | STRINGo |  |  | gG-GL / clase GL | Específico | 12 |
| FAMATR-0177 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-019 | Capacidad de corte | Opcional |  | STRINGo |  |  | 6 kA / 10 kA | Específico | 13 |
| FAMATR-0178 | ELEC | Electricidad | PROT | Protección eléctrica | ATR-020 | Montaje | Opcional |  | Lista |  | riel DIN; tablero; borne; otro | riel DIN | Específico | 14 |
| FAMATR-0179 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-015 | Tipo de dispositivo | Obligatorio |  | Lista |  | contactor; timer; fotocélula; detector; módulo inteligente; condensador; zócalo | contactor | Específico | 7 |
| FAMATR-0180 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-009 | Corriente nominal | Obligatorio | Para contactores, módulos y dispositivos de maniobra. | Número | A |  | 25 A / 32 A | Específico | 8 |
| FAMATR-0181 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-010 | Tensión nominal | Obligatorio | Para dispositivos eléctricos activos. | Número | V |  | 220 V / 250 V / 380 V | Específico | 9 |
| FAMATR-0182 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-021 | Tensión de bobina | Obligatorio | Solo si el dispositivo posee bobina. | Número | V |  | 24 V / 220 V | Específico | 10 |
| FAMATR-0183 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-022 | Cantidad de polos / contactos | Obligatorio | Para contactores, detectores, borneras o módulos de conexión/control. | STRINGo |  |  | trifásico / NA | Específico | 11 |
| FAMATR-0184 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-023 | Tipo de contacto | Obligatorio | Para contactores, relés o dispositivos con contactos. | Lista |  | NA; NC; NA+NC; no informado |  | Específico | 12 |
| FAMATR-0185 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-020 | Montaje | Obligatorio | Para dispositivos instalables en tablero o riel. | Lista |  | riel DIN; embutir; aplicar; zócalo; otro | riel DIN | Específico | 13 |
| FAMATR-0186 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-024 | Rango de temporización / programación | Obligatorio | Solo si tipo de dispositivo = timer o programador. | STRINGo |  |  | digital / analógico mecánico | Específico | 14 |
| FAMATR-0187 | ELEC | Electricidad | MANI | Maniobra y control eléctrico | ATR-025 | Función de control | Opcional |  | STRINGo |  |  | fotocélula / detector movimiento / control remoto | Específico | 15 |
| FAMATR-0188 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-026 | Tipo de conductor | Obligatorio |  | Lista |  | unipolar; multipolar; desnudo; otro | unipolar | Específico | 7 |
| FAMATR-0189 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-027 | Sección | Obligatorio |  | Número | mm² |  | 1,5 mm² / 2,5 mm² / 4 mm² | Específico | 8 |
| FAMATR-0190 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-028 | Color del conductor | Obligatorio |  | STRINGo |  |  | rojo / celeste / verde amarillo | Específico | 9 |
| FAMATR-0191 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-029 | Material del conductor | Obligatorio | Cuando se informe o sea relevante para compra/reposición. | STRINGo |  |  | cobre | Específico | 10 |
| FAMATR-0192 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-030 | Largo de presentación | Obligatorio |  | Número | m |  | 50 m / 100 m | Específico | 11 |
| FAMATR-0193 | ELEC | Electricidad | COND | Conductores eléctricos | ATR-031 | Presentación | Obligatorio |  | Lista |  | rollo; metro; bobina; unidad | rollo 100 m | Específico | 12 |
| FAMATR-0194 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-007 | Tipo de pieza | Obligatorio |  | Lista |  | caño; curva; acople; conector; grampa; bandeja; cablecanal; accesorio; prensa cable; espiral | curva | Específico | 7 |
| FAMATR-0195 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-032 | Material | Obligatorio |  | STRINGo |  |  | PVC / chapa / plástico | Específico | 8 |
| FAMATR-0196 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-033 | Medida / diámetro / sección | Obligatorio |  | STRINGo | mm |  | 22 mm / 100x50 mm / 30 cm | Específico | 9 |
| FAMATR-0197 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-034 | Marca / sistema | Obligatorio | Cuando la pieza pertenece a un sistema compatible específico. | STRINGo |  |  | Tube Electric / Zoloda / Cambre | Específico | 10 |
| FAMATR-0198 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-012 | Tipo de instalación | Obligatorio |  | Lista |  | embutir; aplicar; exterior; bandeja; otro | aplicar | Específico | 11 |
| FAMATR-0199 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-030 | Largo de presentación | Obligatorio | Para caños, cablecanal, bandejas o espirales que se compran por tramo/rollo. | Número | m |  | 1 m / 10 m | Específico | 12 |
| FAMATR-0200 | ELEC | Electricidad | CAN | Canalización eléctrica | ATR-013 | Color | Opcional |  | STRINGo |  |  | gris / blanco | Específico | 13 |
| FAMATR-0201 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-035 | Tipo de caja / gabinete | Obligatorio |  | Lista |  | caja octogonal; caja exterior; gabinete DIN; gabinete estanco; tapa ciega; gabinete metálico | gabinete DIN | Específico | 7 |
| FAMATR-0202 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-032 | Material | Obligatorio |  | STRINGo |  |  | PVC / chapa / metal | Específico | 8 |
| FAMATR-0203 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-036 | Dimensiones | Obligatorio |  | STRINGo | mm / cm |  | 310x400x110 mm | Específico | 9 |
| FAMATR-0204 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-012 | Tipo de instalación | Obligatorio |  | Lista |  | embutir; sobreponer; exterior; tablero; otro | sobreponer | Específico | 10 |
| FAMATR-0205 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-011 | Cantidad de módulos / bocas | Obligatorio | Para cajas de módulos, gabinetes DIN o porta módulos. | Número |  |  | 24 módulos / 12 bocas | Específico | 11 |
| FAMATR-0206 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-014 | Grado IP | Obligatorio | Para gabinetes, cajas exteriores o estancas. | STRINGo |  |  | IP41 / IP65 | Específico | 12 |
| FAMATR-0207 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-013 | Color | Opcional |  | STRINGo |  |  | gris / blanco / rojo | Específico | 13 |
| FAMATR-0208 | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos | ATR-037 | Tapa incluida | Opcional |  | Booleano |  | Sí; No | Sí | Específico | 14 |
| FAMATR-0209 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-038 | Tipo de accesorio | Obligatorio |  | Lista |  | jabalina; cinta aisladora; portalámpara; zócalo; accesorio menor; otro | cinta aisladora | Específico | 7 |
| FAMATR-0210 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-032 | Material | Obligatorio | Cuando el material define compatibilidad o seguridad. | STRINGo |  |  | cerámica / cobre / PVC | Específico | 8 |
| FAMATR-0211 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-039 | Medida | Obligatorio | Cuando la medida define reposición o compatibilidad. | STRINGo | mm / m |  | 19 mm x 20 m / 1,5 m | Específico | 9 |
| FAMATR-0212 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-010 | Tensión nominal | Obligatorio | Para portalámparas, zócalos u otros accesorios energizados. | Número | V |  | 220 V | Específico | 10 |
| FAMATR-0213 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-009 | Corriente nominal | Obligatorio | Para accesorios de conexión donde aplique. | Número | A |  | 10 A / 16 A | Específico | 11 |
| FAMATR-0214 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-040 | Compatibilidad | Opcional |  | STRINGo |  |  | E40 / AR111 | Específico | 12 |
| FAMATR-0215 | ELEC | Electricidad | ACC | Accesorios eléctricos generales | ATR-031 | Presentación | Opcional |  | STRINGo |  |  | unidad / rollo | Específico | 13 |
| FAMATR-0216 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-041 | Tipo de lámpara | Obligatorio |  | Lista |  | lámpara; tubo; dicroica; AR111; halógena; halogenuro; vintage | AR111 | Específico | 7 |
| FAMATR-0217 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-042 | Tecnología | Obligatorio |  | Lista |  | LED; halógena; halogenuro metálico; otra | LED | Específico | 8 |
| FAMATR-0218 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-043 | Potencia | Obligatorio |  | Número | W |  | 9 W / 80 W / 2000 W | Específico | 9 |
| FAMATR-0219 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-044 | Rosca / base | Obligatorio | Para lámparas con base/rosca identificable. | STRINGo |  |  | E27 / E40 / GU10 | Específico | 10 |
| FAMATR-0220 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-045 | Temperatura de color | Obligatorio | Para lámparas LED o cuando el color de luz define la compra. | Lista |  | cálida; neutra; fría; no aplica | luz fría | Específico | 11 |
| FAMATR-0221 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-010 | Tensión nominal | Obligatorio | Cuando se informe o sea necesaria para compatibilidad. | Número | V |  | 12 V / 220 V | Específico | 12 |
| FAMATR-0222 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-046 | Flujo luminoso | Opcional |  | Número | lm |  |  | Específico | 13 |
| FAMATR-0223 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-047 | Formato / forma | Opcional |  | STRINGo |  |  | PAR30 / tubo / velita | Específico | 14 |
| FAMATR-0224 | ILUM | Iluminación | LAM | Lámparas y tubos | ATR-048 | Dimerizable | Opcional |  | Booleano |  | Sí; No | No | Específico | 15 |
| FAMATR-0225 | ILUM | Iluminación | LUM | Luminarias | ATR-049 | Tipo de luminaria | Obligatorio |  | Lista |  | plafón; reflector; panel; spot; aplique; farola; base de luminaria | plafón | Específico | 7 |
| FAMATR-0226 | ILUM | Iluminación | LUM | Luminarias | ATR-042 | Tecnología | Obligatorio |  | Lista |  | LED; halógena; otra | LED | Específico | 8 |
| FAMATR-0227 | ILUM | Iluminación | LUM | Luminarias | ATR-043 | Potencia | Obligatorio | Cuando la luminaria incluye fuente/luz integrada. | Número | W |  | 18 W / 55 W / 250 W | Específico | 9 |
| FAMATR-0228 | ILUM | Iluminación | LUM | Luminarias | ATR-012 | Tipo de instalación | Obligatorio |  | Lista |  | embutir; aplicar; pared; techo; exterior; riel | embutir | Específico | 10 |
| FAMATR-0229 | ILUM | Iluminación | LUM | Luminarias | ATR-050 | Forma / formato | Obligatorio | Cuando la forma define el producto. | STRINGo |  |  | cuadrado / redondo / rectangular | Específico | 11 |
| FAMATR-0230 | ILUM | Iluminación | LUM | Luminarias | ATR-036 | Dimensiones | Obligatorio | Cuando se informe o defina la reposición. | STRINGo | cm / mm |  | 60x60 / 120x36 | Específico | 12 |
| FAMATR-0231 | ILUM | Iluminación | LUM | Luminarias | ATR-045 | Temperatura de color | Obligatorio | Cuando incluye luz o LED integrado. | Lista |  | cálida; neutra; fría; no informado | luz neutra | Específico | 13 |
| FAMATR-0232 | ILUM | Iluminación | LUM | Luminarias | ATR-014 | Grado IP | Obligatorio | Para luminarias exteriores, reflectores o ambientes expuestos. | STRINGo |  |  | IP65 | Específico | 14 |
| FAMATR-0233 | ILUM | Iluminación | LUM | Luminarias | ATR-051 | Color de carcasa | Opcional |  | STRINGo |  |  | negro / blanco | Específico | 15 |
| FAMATR-0234 | ILUM | Iluminación | LUM | Luminarias | ATR-052 | Incluye base / zócalo | Opcional |  | Booleano |  | Sí; No | Sí | Específico | 16 |
| FAMATR-0235 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-053 | Tipo de elemento | Obligatorio |  | Lista |  | luz de emergencia; cartel de salida; señalización luminosa | luz de emergencia | Específico | 7 |
| FAMATR-0236 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-042 | Tecnología | Obligatorio |  | Lista |  | LED; otra | LED | Específico | 8 |
| FAMATR-0237 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-054 | Cantidad de LEDs | Obligatorio | Para luces de emergencia o carteles con LEDs declarados. | Número |  |  | 60 LEDs / 90 LEDs | Específico | 9 |
| FAMATR-0238 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-010 | Tensión nominal | Obligatorio |  | Número | V |  | 220 V / 250 V | Específico | 10 |
| FAMATR-0239 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-055 | Autonomía | Opcional |  | STRINGo | h |  |  | Específico | 11 |
| FAMATR-0240 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-056 | Tipo de montaje | Opcional |  | Lista |  | pared; techo; aplicar; colgante; otro | pared | Específico | 12 |
| FAMATR-0241 | ILUM | Iluminación | EME | Iluminación de emergencia y señalización | ATR-057 | Mensaje / señal | Obligatorio | Para carteles o señalización. | STRINGo |  |  | Salida | Específico | 13 |
| FAMATR-0242 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-058 | Tipo de sistema LED | Obligatorio |  | Lista |  | tira LED; fuente; tracklight; riel; accesorio | tira LED | Específico | 7 |
| FAMATR-0243 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-059 | Tensión de entrada | Obligatorio | Para fuentes o dispositivos alimentados desde red. | Número | V |  | 220 V | Específico | 8 |
| FAMATR-0244 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-060 | Tensión de salida | Obligatorio | Para fuentes o sistemas LED de baja tensión. | Número | V |  | 12 V / 24 V | Específico | 9 |
| FAMATR-0245 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-043 | Potencia | Obligatorio | Para fuentes, tiras LED y luminarias de riel. | Número | W |  | 60 W / 19,2 W/m | Específico | 10 |
| FAMATR-0246 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-061 | Corriente | Obligatorio | Para fuentes de alimentación. | Número | A |  | 2,1 A / 5 A | Específico | 11 |
| FAMATR-0247 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-062 | Longitud | Obligatorio | Para tiras o rieles. | Número | m |  | 1 m / 10 m | Específico | 12 |
| FAMATR-0248 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-063 | Rosca / base compatible | Obligatorio | Para tracklights o sistemas con lámpara reemplazable. | STRINGo |  |  | E27 / GU10 / AR111 | Específico | 13 |
| FAMATR-0249 | ILUM | Iluminación | SLED | Sistemas LED y rieles | ATR-064 | Color / terminación | Opcional |  | STRINGo |  |  | black / blanco | Específico | 14 |
| FAMATR-0250 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-065 | Tipo de conexión | Obligatorio |  | Lista |  | flexible; niple; unión doble; acople; conexión; junta | flexible mallado | Específico | 7 |
| FAMATR-0251 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-066 | Diámetro / rosca | Obligatorio |  | STRINGo | pulgadas / mm |  | 1/2" / 3/4" / 1 1/4" | Específico | 8 |
| FAMATR-0252 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-032 | Material | Obligatorio |  | STRINGo |  |  | PVC / polipropileno / bronce / metálico | Específico | 9 |
| FAMATR-0253 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-067 | Largo | Obligatorio | Para flexibles, niples y piezas cuyo largo define reposición. | Número | cm |  | 40 cm / 50 cm | Específico | 10 |
| FAMATR-0254 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-068 | Tipo de extremo | Opcional |  | STRINGo |  |  | HH / macho-hembra / rápido | Específico | 11 |
| FAMATR-0255 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-069 | Presión de trabajo | Opcional |  | STRINGo |  |  |  | Específico | 12 |
| FAMATR-0256 | SAN | Sanitario / Plomería | CON | Conexiones y flexibles | ATR-034 | Marca / sistema | Opcional |  | STRINGo |  |  | IPS / Duke | Específico | 13 |
| FAMATR-0257 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-007 | Tipo de pieza | Obligatorio |  | Lista |  | codo; curva; ramal; T; manguito; sifón; pileta de patio; descarga; sopapa | codo | Específico | 7 |
| FAMATR-0258 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-070 | Diámetro | Obligatorio |  | Número | mm |  | 40 mm / 50 mm / 110 mm | Específico | 8 |
| FAMATR-0259 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-032 | Material | Obligatorio |  | STRINGo |  |  | PVC / polipropileno | Específico | 9 |
| FAMATR-0260 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-071 | Ángulo | Obligatorio | Para codos y curvas. | Número | ° |  | 45° / 90° | Específico | 10 |
| FAMATR-0261 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-034 | Marca / sistema | Obligatorio | Cuando la pieza pertenece a un sistema específico. | STRINGo |  |  | Awaduct | Específico | 11 |
| FAMATR-0262 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-072 | Tipo de unión | Opcional |  | STRINGo |  |  | junta / pegar / rosca | Específico | 12 |
| FAMATR-0263 | SAN | Sanitario / Plomería | DES | Desagües y cañerías | ATR-036 | Dimensiones | Opcional |  | STRINGo | cm / mm |  | 40x63 | Específico | 13 |
| FAMATR-0264 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-073 | Tipo de grifería | Obligatorio |  | Lista |  | canilla; grifo; llave de paso; automática | canilla automática | Específico | 7 |
| FAMATR-0265 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-066 | Diámetro / rosca | Obligatorio |  | STRINGo | pulgadas |  | 1/2" / 1 1/4" | Específico | 8 |
| FAMATR-0266 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-032 | Material | Obligatorio |  | STRINGo |  |  | bronce / PVC / polipropileno | Específico | 9 |
| FAMATR-0267 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-074 | Accionamiento | Obligatorio | Cuando define la función del producto. | Lista |  | manual; automática; válvula; otro | automática | Específico | 10 |
| FAMATR-0268 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-075 | Aplicación | Obligatorio |  | STRINGo |  |  | pared / lavatorio / paso | Específico | 11 |
| FAMATR-0269 | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso | ATR-076 | Marca / modelo | Opcional |  | STRINGo |  |  | Vasser 50/1013,01 | Específico | 12 |
| FAMATR-0270 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-077 | Tipo de repuesto | Obligatorio |  | Lista |  | pistón; retenes; tornillos; resorte; manija; aro; asiento; botón; cadena; conjunto descarga; válvula | pistón | Específico | 7 |
| FAMATR-0271 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-078 | Compatible con | Obligatorio |  | STRINGo |  |  | válvula de descarga / mochila / inodoro | Específico | 8 |
| FAMATR-0272 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-079 | Marca compatible | Obligatorio | Cuando el repuesto corresponde a una marca/sistema. | STRINGo |  |  | FV / Ferrum / Capea | Específico | 9 |
| FAMATR-0273 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-080 | Modelo / código compatible | Obligatorio | Cuando exista código de repuesto o conjunto. | STRINGo |  |  | 0367,15,0-B / RC1DESUDG-LT | Específico | 10 |
| FAMATR-0274 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-032 | Material | Opcional |  | STRINGo |  |  | bronce / goma / PVC / plástico | Específico | 11 |
| FAMATR-0275 | SAN | Sanitario / Plomería | REP | Repuestos sanitarios | ATR-039 | Medida | Opcional |  | STRINGo | mm / cm |  | 86 mm | Específico | 12 |
| FAMATR-0276 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-007 | Tipo de pieza | Obligatorio |  | Lista |  | tapa ciega; rejilla; marco; accesorio de terminación | rejilla con marco | Específico | 7 |
| FAMATR-0277 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-036 | Dimensiones | Obligatorio |  | STRINGo | cm / mm |  | 14,5x14,5 cm / 11x11 cm | Específico | 8 |
| FAMATR-0278 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-032 | Material | Obligatorio | Cuando el material define reposición o compatibilidad. | STRINGo |  |  | PVC / metal / plástico | Específico | 9 |
| FAMATR-0279 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-075 | Aplicación | Obligatorio |  | STRINGo |  |  | bacha / pared / piso / sanitario | Específico | 10 |
| FAMATR-0280 | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios | ATR-013 | Color | Opcional |  | STRINGo |  |  | blanco / cromado | Específico | 11 |
| FAMATR-0281 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-081 | Tipo de refrigerante | Obligatorio |  | Lista |  | R410A; R22; otro | R410A | Específico | 7 |
| FAMATR-0282 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-031 | Presentación | Obligatorio |  | Número | kg |  | 11,35 kg / 13,5 kg | Específico | 8 |
| FAMATR-0283 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-082 | Compatibilidad / uso | Opcional |  | STRINGo |  |  | aire acondicionado / refrigeración | Específico | 9 |
| FAMATR-0284 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-083 | Tipo de envase | Opcional |  | STRINGo |  |  | garrafa / cilindro | Específico | 10 |
| FAMATR-0285 | CLIM | Climatización y ventilación | REF | Refrigerantes | ATR-084 | Observación de seguridad | Opcional |  | STRINGo largo |  |  | manejo técnico especializado | Específico | 11 |
| FAMATR-0286 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-085 | Tipo de equipo | Obligatorio |  | Lista |  | extractor; ventilador; control de aire; otro | extractor | Específico | 7 |
| FAMATR-0287 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-043 | Potencia | Obligatorio | Para extractores o ventiladores. | Número | W |  | 50 W | Específico | 8 |
| FAMATR-0288 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-010 | Tensión nominal | Obligatorio | Para equipos eléctricos. | Número | V |  | 220 V | Específico | 9 |
| FAMATR-0289 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-086 | Caudal / capacidad | Opcional |  | STRINGo |  |  |  | Específico | 10 |
| FAMATR-0290 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-087 | Diámetro / medida | Obligatorio | Para extractores o ventiladores con medida de boca. | STRINGo | mm |  | 100 / 250 | Específico | 11 |
| FAMATR-0291 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-013 | Color | Opcional |  | STRINGo |  |  | blanco | Específico | 12 |
| FAMATR-0292 | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC | ATR-088 | Modo de control / compatibilidad | Obligatorio | Para controles de aire. | STRINGo |  |  | universal / programable | Específico | 13 |
| FAMATR-0293 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-085 | Tipo de equipo | Obligatorio |  | Lista |  | bomba; flotante eléctrico; ciclador; controlador de bomba | bomba sumergible | Específico | 7 |
| FAMATR-0294 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-075 | Aplicación | Obligatorio |  | STRINGo |  |  | desagote / control de bombas | Específico | 8 |
| FAMATR-0295 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-010 | Tensión nominal | Obligatorio | Para equipos eléctricos. | Número | V |  | 220 V | Específico | 9 |
| FAMATR-0296 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-061 | Corriente | Obligatorio | Cuando se informa o define compatibilidad eléctrica. | Número | A |  | 3 A | Específico | 10 |
| FAMATR-0297 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-043 | Potencia | Opcional |  | STRINGo | W / HP |  |  | Específico | 11 |
| FAMATR-0298 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-089 | Cantidad de bombas controladas | Obligatorio | Solo si tipo de equipo = ciclador/controlador. | Número |  |  | 2 | Específico | 12 |
| FAMATR-0299 | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico | ATR-090 | Largo de cable | Obligatorio | Para flotantes u otros sensores cableados. | Número | m |  | 1,5 m | Específico | 13 |
| FAMATR-0300 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-053 | Tipo de elemento | Obligatorio |  | Lista |  | manguera; gabinete; accesorio hidrante | manguera | Específico | 7 |
| FAMATR-0301 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-032 | Material | Obligatorio | Cuando el material define resistencia o compatibilidad. | STRINGo |  |  | metal / tela / goma | Específico | 8 |
| FAMATR-0302 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-013 | Color | Obligatorio | Para gabinetes o elementos visuales de seguridad. | STRINGo |  |  | rojo | Específico | 9 |
| FAMATR-0303 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-036 | Dimensiones | Obligatorio | Cuando se informe o defina reposición. | STRINGo | cm / mm |  |  | Específico | 10 |
| FAMATR-0304 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-091 | Compatibilidad / norma | Opcional |  | STRINGo |  |  | hidrante / incendio | Específico | 11 |
| FAMATR-0305 | SEG | Seguridad edilicia | HID | Hidrantes | ATR-075 | Aplicación | Opcional |  | STRINGo |  |  | sistema contra incendio | Específico | 12 |
| FAMATR-0306 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-092 | Tipo de cerradura | Obligatorio |  | Lista |  | mueble; cilíndrica; pomo; pulso; embutir; exterior | cilíndrica | Específico | 7 |
| FAMATR-0307 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-004 | Marca | Obligatorio |  | STRINGo |  |  | Kallay / Prive / FPV | Específico | 8 |
| FAMATR-0308 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-005 | Modelo / código fabricante | Obligatorio | Cuando se indique modelo comercial. | STRINGo |  |  | 4003 / 5003 / 200 | Específico | 9 |
| FAMATR-0309 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-075 | Aplicación | Obligatorio |  | STRINGo |  |  | mueble / puerta / baño | Específico | 10 |
| FAMATR-0310 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-093 | Medida / cilindro | Opcional |  | STRINGo |  |  |  | Específico | 11 |
| FAMATR-0311 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-094 | Terminación | Opcional |  | STRINGo |  |  | cromado / bronce | Específico | 12 |
| FAMATR-0312 | CERR | Cerrajería y accesos | CER | Cerraduras | ATR-095 | Sentido de apertura | Opcional |  | Lista |  | derecha; izquierda; reversible; no aplica | reversible | Específico | 13 |
| FAMATR-0313 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-096 | Tipo de cierrapuerta | Obligatorio |  | Lista |  | hidráulico; libre/ocupado; otro | hidráulico | Específico | 7 |
| FAMATR-0314 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-004 | Marca | Opcional |  | STRINGo |  |  | OCB | Específico | 8 |
| FAMATR-0315 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-005 | Modelo / código fabricante | Opcional |  | STRINGo |  |  |  | Específico | 9 |
| FAMATR-0316 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-075 | Aplicación | Obligatorio |  | STRINGo |  |  | puerta / baño | Específico | 10 |
| FAMATR-0317 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-097 | Fuerza / tamaño | Opcional |  | STRINGo |  |  |  | Específico | 11 |
| FAMATR-0318 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-094 | Terminación | Opcional |  | STRINGo |  |  |  | Específico | 12 |
| FAMATR-0319 | CERR | Cerrajería y accesos | CIE | Cierrapuertas | ATR-056 | Tipo de montaje | Opcional |  | STRINGo |  |  | superior / embutir | Específico | 13 |
| FAMATR-0320 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-015 | Tipo de dispositivo | Obligatorio |  | Lista |  | destraba eléctrica; electroimán; lector; otro | destraba eléctrica | Específico | 7 |
| FAMATR-0321 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-021 | Tensión de bobina | Obligatorio | Para destrabas o dispositivos electromecánicos. | Número | V |  | 12 V | Específico | 8 |
| FAMATR-0322 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-043 | Potencia | Obligatorio | Para dispositivos eléctricos. | Número | W |  | 5 W | Específico | 9 |
| FAMATR-0323 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-075 | Aplicación | Obligatorio |  | STRINGo |  |  | puerta / acceso | Específico | 10 |
| FAMATR-0324 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-012 | Tipo de instalación | Obligatorio |  | STRINGo |  |  | embutir / aplicar | Específico | 11 |
| FAMATR-0325 | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas | ATR-076 | Marca / modelo | Opcional |  | STRINGo |  |  | ROA | Específico | 12 |
| FAMATR-0326 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-098 | Tipo de herramienta | Obligatorio |  | Lista |  | rodillo; mini rodillo; pincel; pinceleta | rodillo | Específico | 7 |
| FAMATR-0327 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-039 | Medida | Obligatorio |  | Número | cm / pulgadas |  | 10 cm / 22 cm / 4" | Específico | 8 |
| FAMATR-0328 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-099 | Material / pelo | Obligatorio | Cuando el material define el uso de aplicación. | STRINGo |  |  | poliéster y tela / lana / pelo corto | Específico | 9 |
| FAMATR-0329 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-100 | Uso recomendado | Obligatorio | Cuando se indica para sintético, epoxi u otro uso. | STRINGo |  |  | sintético / epoxi / obra | Específico | 10 |
| FAMATR-0330 | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación | ATR-004 | Marca | Opcional |  | STRINGo |  |  | Rosapin / Cacique | Específico | 11 |
| FAMATR-0331 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-101 | Tipo de pintura / recubrimiento | Obligatorio |  | Lista |  | látex; sintético; epoxi; barniz; convertidor; impermeabilizante; revestimiento; piso deportivo | sintético | Específico | 7 |
| FAMATR-0332 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-102 | Uso | Obligatorio |  | Lista |  | interior; exterior; interior/exterior; vial; deportivo; marino; obra | interior/exterior | Específico | 8 |
| FAMATR-0333 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-013 | Color | Obligatorio |  | STRINGo |  |  | gris espacial / blanco / rojo / azul | Específico | 9 |
| FAMATR-0334 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-094 | Terminación | Obligatorio | Cuando el tipo de pintura posee terminación declarable. | Lista |  | brillante; satinado; mate; no informado | brillante | Específico | 10 |
| FAMATR-0335 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-031 | Presentación | Obligatorio |  | Número | litros / kg |  | 1 litro / 4 litros / 20 kg | Específico | 11 |
| FAMATR-0336 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-103 | Base / composición | Opcional |  | STRINGo |  |  | acrílico / epoxi / sintético | Específico | 12 |
| FAMATR-0337 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-104 | Rendimiento | Opcional |  | STRINGo | m²/l |  |  | Específico | 13 |
| FAMATR-0338 | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos | ATR-004 | Marca | Opcional |  | STRINGo |  |  | Quimex | Específico | 14 |
| FAMATR-0339 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-105 | Tipo de producto | Obligatorio |  | Lista |  | silicona; sellador; sellarosca; adhesivo | silicona | Específico | 7 |
| FAMATR-0340 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-013 | Color | Obligatorio | Cuando el color define el producto o terminación. | STRINGo |  |  | blanco / transparente | Específico | 8 |
| FAMATR-0341 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-031 | Presentación | Obligatorio |  | STRINGo | cc / ml / unidad |  | 125 cc | Específico | 9 |
| FAMATR-0342 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-102 | Uso | Obligatorio |  | STRINGo |  |  | antihongos / roscas / juntas / sellado | Específico | 10 |
| FAMATR-0343 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-106 | Base química | Opcional |  | STRINGo |  |  | silicona / poliuretano | Específico | 11 |
| FAMATR-0344 | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos | ATR-004 | Marca | Opcional |  | STRINGo |  |  | SikaFlex | Específico | 12 |
| FAMATR-0345 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-107 | Tipo de equipamiento | Obligatorio |  | Lista |  | pizarrón; rotafolio; accesorio aula; otro | pizarrón | Específico | 7 |
| FAMATR-0346 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-032 | Material | Opcional |  | STRINGo |  |  |  | Específico | 8 |
| FAMATR-0347 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-036 | Dimensiones | Obligatorio | Cuando la medida define reposición o compra. | STRINGo | cm / mm |  |  | Específico | 9 |
| FAMATR-0348 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-108 | Movilidad | Opcional |  | Booleano |  | Sí; No | Sí | Específico | 10 |
| FAMATR-0349 | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación | ATR-102 | Uso | Opcional |  | STRINGo |  |  | aula / presentación | Específico | 11 |

## 05_Productos_Clasificados

| producto_original | unidad_original | rubro_id | rubro_sugerido | familia_id | familia_sugerida | notas_origen |
|---|---|---|---|---|---|---|
| BASTIDOR SCHNEIDER RODA | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | Obligatorios (agregar) / oPC |
| BASTIDOR SCHNEIDER  BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | Modelo |
| TAPA MARCO SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| MODULO PUNTO SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | Base = Gama del producto |
| MODULO CIEGO SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMAS DOBLES SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMAS SIMPLE  SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMAS SIMPLE ESTABILIZADA SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMAS DOBLES ESTABILIZADA SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| FICHA MACHO 10 AMP | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| FICHA MACHO 20 AMP | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| FICHA HEMBRA 10 AMP | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMAS DE 20 AMPER SCHNEIDER BASE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMA USB SCH SCHNEIDER RODA | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas | Roda = Gama del producto |
| MODULO DETECTOR DE MOVIMIENTO  SCHNEIDER RODA WDA56101 | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| ZOCALO PARA FOTOCELULA | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| FOTOCELULA | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| FICHA PARA EXTERIOR MACHO Y HEMBRA 16A SCAME IP69 | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| DISYUNTOR  4P 25A 30mA  SCHNEIDER  EZ9R36425 | Ud | ELEC | Electricidad | PROT | Protección eléctrica | EZ9R36425 = Modelo |
| TERMICA  UNIPOLAR  10A  SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| TERMICA UNIPOLAR 16A SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| TERMICA UNIPOLAR 20A SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| TERMICA TETRAPOLAR  4P 40A SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| TERMICA BIPOLAR 16A SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| CONTACTOR ESCHNEIDER  TRIFASICO 32A  BOBINA 24V NA  17899 | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| CONTACTOR MODULAR SCHNEIDER  ICT 25A 250V | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| CONTACTOR TRIFASICO  32A BOBINA 24V  SCHNEIDER LC1D32B7 | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| TIMER DIGITAL PARA RIEL DIN | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| TERMICA  TETRAPOLAR  4P 20A  SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| DISYUNTOR 4P 40A  30mA-AC SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| REFLECTOR LED  30W LUZ CALIDA AKAI ENERGY | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| REFLECTOR LED 100W LUZ CALIDA ENERGENERATION | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| ESTACION DE CARGA  TELESCOPICA DE EMBUTIR 3 TOMAS Y 2 PUERTOS USB | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| LUZ DE EMERGENCIA  60 LEDS  LCI-ME60 | Ud | ILUM | Iluminación | EME | Iluminación de emergencia y señalización |  |
| LUZ DE EMERGENCIA 90LEDS GAMASONIC | Ud | ILUM | Iluminación | EME | Iluminación de emergencia y señalización |  |
| REFLECTOR  LED 125W LEDVANCE | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| LAMPARA 80W E40 LUZ FRIA  LEDVANCE | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| PLAFON LED CUADRADO PARA APLICAR 6W LUZ FRIA LUMENAC POLO P6 | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| REFLECTOR LED 250W PROLITE | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| LAMPARA LED E27 27W | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| BOMBA SUMERGIBLE DE DESAGOTE RW DRAIN Q400F | Ud | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico |  |
| EXTRACTOR DE AIRE HY-VF 100 BLANCO | Ud | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC |  |
| AR111 LAMPARA  LED 15W AKAI ENERGY LUZ FRIA | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| AR111 12W LUZ CALIDA  OSRAM | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| AR111 11W 12VLUZ FRIA | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| LAMPARA E27 MACROLED  LUZ CALIDA  PAR 30LED | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| LAMPARA LED E27 9W | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| DICROICA LED 7W LUZ NEUTRA  MACROLED | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| DICROICA  LED 7W LUZ  CALIDA MACROLED LUZ CALIDA | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| DICROICA  ALOGENA 50W | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| PLAFON LED 6W LUZ CALIDA EMBUTIR  REDONDO | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PLAFON LED DE EMBUTIR 20W LUZ CALIDA REDONDO | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PLAFON LED DE EMBUTIR 18W LUZ NEUTRA REDONDO | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PLAFON LED APLICAR  18W LUZ  NEUTRA  CUADRADA LUCCIOLA | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PLAFON LED EMBUTIR CUADRADO  18W  LUCCIOLA | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| REFLECTOR LED  20W IP 65 POINTER PRO 20 BAEL | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| SPOT REBATIBLE CON ZOCALO  170mm x 40mm CANDIL LINEA ZAC EMBUTIDO TECHO | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| SPOT DE EMBUTIR ARR11  FIJO BLANCO  NIAN | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| ZOCALO CERAMICO PARA AR111 | Ud | ELEC | Electricidad | ACC | Accesorios eléctricos generales |  |
| TECLA DE UN PUNTO  CAMBRE 10A 220V | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| CAJA OCTOGONAL DE EMBUTIR PVC   CTR33 100mm | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| CAJA OCTOGONAL DE EMBUTIR PVC   90mm | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| CAJA OCTOGONAL DE CHAPA  PARA EMBUTIR 80mm | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| TAPA CIEGA 10X5 PVC | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| TAPA CIEGA 5X5 PVC | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| TOMA BASE INDUSTRIAL 2X16A+NEUTRO | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMA BASE INDUSTRIAL 2X32A+NEUTRO | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TAPA CIEGA REDONDA  12mm | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| TAPA CIEGA REDONDA 10mm | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| TAPA CIEGA CUADRADA DE CHAPA 12X12mm | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| PORTA BASTIDOR ZOLODA 10X5 | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| ACOPLE  TUBE ELECTRIC   0,16 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CURVA  TUBE ELECTRIC 22 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| GRAMPA  TUBE ELECTRIC  22mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CONECTOR TUBE ELECTRIC 22mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACOPLE  TUBE ELECTRIC   22 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CURVA TUBE ELECTRIC 25 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CURVA TUBE ELECTRIC 22 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CONECTOR TUBE ELECTRIC 25 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACOPLE TUBE ELECTRIC 25mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| GRAMPAS TUBE ELECTRIC 25mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CONECTOR TUBE ELECTRIC 40 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CURVA TUBE ELECTRIC 40 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| GRAMPA  TUBE ELECTRIC  40 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CONECTOR  TUBE ELECTRIC  50 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CURVA TUBE ELECTRIC 50 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| GRAMPA TUBE ELECTRIC  50 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACOPLE  TUBE ELECTRIC 50 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CAJA  OCTOGONAL C/TAPA Y CONOS DE SOBREPONER GENROD GRIS | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| CAJA EXTERIO MINI PBC KALOP | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| TUBOS LED 16W/840 LEDVANCE LUZ NEUTRA | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| PLAFON LED MOON-BL 55W REDONDO 55cm CALIDO | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PANEL  LED 60X60 | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PANEL LED  122X30 RECTANGULAR | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PORTA LAMPARA CERAMICA E40 | Ud | ELEC | Electricidad | ACC | Accesorios eléctricos generales |  |
| CAÑO DE PVC GRIS 50mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CAÑO DE PVC GRIS 25mm SISTE ELECTRIC | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| SONOFF BASIC | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| SOPORTE MODULAR PARA SONOFF  REALDIN | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| CARTEL DE SEÑALIZACION DE SALIDA  13 LED 250V ALIC | Ud | ILUM | Iluminación | EME | Iluminación de emergencia y señalización |  |
| LUZ DE EMERGENCIA  48 LED LCI  (MIKEY) | Ud | ILUM | Iluminación | EME | Iluminación de emergencia y señalización |  |
| APLIQUE DE PARED  ARTELUM  NEGRO | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| CONTROL DE AIRE PROGRAMABLE UNIVERSAL   COOLTECH | Ud | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC |  |
| TOTEM TOMA CORRIENTE GRIS | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOTEM TOMA CORRIENTE BLANCO | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| PIZARRON MOBIL | Ud | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación |  |
| ROTAFOLIO | Ud | EQUI | Equipamiento edilicio | AULA | Equipamiento de aula y presentación |  |
| MANGUERAS DE HIDRANTES | Ud | SEG | Seguridad edilicia | HID | Hidrantes |  |
| GABINETE  ROJO P/ HIDRANTE | Ud | SEG | Seguridad edilicia | HID | Hidrantes |  |
| CABLE DE 1,5mm UNIPOLAR  ROJO 100 mts | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR DE 1.5mm  CELESTE  100 mts | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR DE 1.5mm VERDE AMARILLO 100 mts | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR DE 1.5 mm ROJO  50 mts | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAE DE 1.5 mm CELESTE 50 mts | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNPOLAR DE 1.5mm VERDE AMARILLO 50 mts | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR DE  2,5mm 100mts ROJO | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR DE 2,5mm 100mts  CELESTE | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR  DE 2,5 mm  100mts VERDE AMARILLO | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR  DE 4mm  100 mts ROJO | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR DE 4 mm 100 mts  CELESTE | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| CABLE UNIPOLAR DE 6 mm 100 mts VERDE AMARILLO | Ud | ELEC | Electricidad | COND | Conductores eléctricos |  |
| FAROLAS NEGRAS  FW  ILUMINACION P/5 ARR11 | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| ESPIRAL ORGANIZADOR DE CABLES SCHNEIDER 10MTS | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| TOMA DOBLE  CAMBRE  COLOR GRIS  CODIG.7997 | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| BASTIDORES CAMBRE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| MARCO BLANCO CAMBRE | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| TOMA DOBLES CAMBRE  COLOR BLANCO | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| CAJA EXTERIOR  IP40 4 MODULOS  CAMBRE  BLANCA | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| CABLE CANAL ZOLODA 100 X50 mm | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACCESORIOS ZOLODA UNION T PLANO 100X50 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACCESORIOS ZOLODA  ESQUINERO 100X50 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACCESORIOS ZOLODA CURVA INTERIOR  O RINCONERO100X50 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACCESORIO ZOLODA  CURVA PLANA 90° 100X50 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACCESORIO ZOLODA  ESQUINERO EXTERIOR 100X50 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| ACESORIO ZOLODA  EXTREMO 100X50 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| GABINETE PVC ESTANCO 29X29 | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| GABINETE DE CHAPA IP 65 90X62X23 | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| GABINETE ESTANCO 28X24 | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| BASE PARA PLAFON LED 120X36 CHAPA | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| BASE PARA PLAFON LED 60X60 CHAPA | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| ZAPATILLAS DE TOMAS MULTIPLES | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| PLAFON LED 120X36 | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| BANDEJAS PORTACABLE DE CHAPA  5CM | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| BANDEJAS PORTACABLE  DE CHAPA 20CM | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| BANDEJAS PORTACABLES DE CHAPA  30CM | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| CONTACTOR SIEMENS  C3 9A 380V BOBINA DE 24V 3RT10 23-1AC20 | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| CONTACTOR SIEMENS AC-3 38A 400V BOBINA 220V   3RT2028-1AP00 | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| GUARDA MOTOR SIEMENS 10-16A 3RV2021-4AA10 | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| CAJA ATQ ACKERMANN | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| DISYUNTOR DIFERENCIAL  4X 25AMP. SCHNEIDER A9R71425 | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| DISYUNTOR DIFERENCIAL 4X 40AMP. SCHNEIDER A9R71440 | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| DISYUNTOR DIFERENCIAL 4X 63 AMP. SCHNEIDER A9R71463 | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| LEDVANCE TRACKLIGHT PARA 30 E27  MAX31W | Ud | ILUM | Iluminación | SLED | Sistemas LED y rieles |  |
| LEDVANCE TRACKLIGHT  AR111 GU10 MAX 14W | Ud | ILUM | Iluminación | SLED | Sistemas LED y rieles |  |
| LEDVANCE RIEL  1M BLACK  PARA APLICAR | Ud | ILUM | Iluminación | SLED | Sistemas LED y rieles |  |
| FUSIBLE NH CLASE GL 100 AMPER REPROEL | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH CLASE GL 63 AMPER REPROEL | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH 000 SEMIKRON 100 AMPER gG-GL | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH 00 SICA 16 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH NT00C: SIEMENS ,SANDERSON, REPROEL DE 80 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH  TUBIA ABB 100 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH REPROEL GL 20 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH REPROEL GL 25 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH REPROEL GL 16 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH REPROEL , ELECTROMEL 30 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FUSIBLE NH WEG GL 35 AMPER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| REPARTIDOR ESTANDAR BIPOLAR  ( BORNERA) 125AMPER  40° UN 1000V ELENT SRL | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| CAJA D ESOBRE PONER DE MODULOS DIN  12 BOCAS GENROD COD. 04512 | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| GABINETE DIN LUXURY MAX -IP 41 -24MODULOS 310X400X110mm | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| GABINETE METALICO GABEXEL  GPB1 140X140X55 | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| GABINETE METALICO GABEXEL  GPB1 140X250X55 | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| BORNERA FLEXIBLE  TORNILLO CABLE DE 2,5mm  12 unidades | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FICHA MACHO  10 AMPER COLOR GRIS RICHI DE 2 PATAS | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| PRENSA CABLE DE PVC 3/4 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| PRENSA CABLE DE 1" PVC | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| BARRA BIFF  80AMP. STEC 1M | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| CONDENSADOR TRIFASICO BLRCS250A300B40 SCHNEIDER | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| LAMPARA LED VINTAGE TRITON 4W | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| LAMPARA PARA ALTAR VELITA LLAMITA FUEGO 3W E27 FRANCE | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| DESTRABA ELECTRICO  BOBINA 12V 5W ROA | Ud | CERR | Cerrajería y accesos | ACC | Control de acceso y destrabas |  |
| FUENTE PARA TIRA DE LED 220V 2,1AMPER  A13985 | Ud | ILUM | Iluminación | SLED | Sistemas LED y rieles |  |
| FUENTE DE ALIMENT. P/TIRA DE LED   60W 220V ENTRADA/ SALIDA 12V 5A TBC | Ud | ILUM | Iluminación | SLED | Sistemas LED y rieles |  |
| TIRA LED 10MTS 24V 19,2W/M VONDERK | Ud | ILUM | Iluminación | SLED | Sistemas LED y rieles |  |
| TIMER ANALOJICO MECANICO P/RIELDIN  SCHNEIDER 15336 | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| TIMER DIGITAL PARA RIEL DIN | Ud | ELEC | Electricidad | MANI | Maniobra y control eléctrico |  |
| APLIQUE BIDERECCIONAL CURVO 706H 18034 | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| TERMICA TETRAPOLAR  4P 40A SCHNEIDER | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FLOTANTE ELECTRICO 1,5MTS  VIYILANT | Ud | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico |  |
| EXTRACTOR DE AIRE HY-VF 100 BLANCO | Ud | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC |  |
| EXTRACTOR DE AIRE  Y VENTILADOR 50W 220V HIDRA HY-VF250B | Ud | CLIM | Climatización y ventilación | VENT | Ventilación y controles HVAC |  |
| PLAFON LED BACKLIGHT 45W 120X30  LUCIOLA | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| CABLECANAL ZOLODA 100X50 | Ud | ELEC | Electricidad | CAN | Canalización eléctrica |  |
| LAMPARA COLGANTE SATURNO 165W LUMENAC USADA | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| GABINETE P/24M LUXURY-IP41 310X400X110 MM | Ud | ELEC | Electricidad | CG | Cajas y gabinetes eléctricos |  |
| PLAFON LED REDONDO DE APLICAR 6W  MACROLED | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| PLAFON LED REDONDO DE APLICAR 18W LUCCIOLA | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| JABALINA DE 1,5M | Ud | ELEC | Electricidad | ACC | Accesorios eléctricos generales |  |
| CINTA AISLADORA 19MM X 20MTS 3M | Ud | ELEC | Electricidad | ACC | Accesorios eléctricos generales |  |
| LAMPARA  E40 MH 2000 FRANCE  2000W | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| PLAFON LED RECTANGULAR  DE  130X30 LUMENAC | Ud | ILUM | Iluminación | LUM | Luminarias |  |
| TOTEMS DE TOMAS | Ud | ELEC | Electricidad | MTF | Mecanismos, tomas y fichas |  |
| LAMPARA HALOGENURO METALICO 2000W E40 LXR | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| LAMPARA LED E27 6,5W MACROLED | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| CICLADOR  RBC SITEL 220V 3 AMP. PARA 2 BOMBAS | Ud | EQEM | Equipos electromecánicos | BOM | Bombas y control hídrico |  |
| LAMAPARA LED E27 7 W LEDVANCE | Ud | ILUM | Iluminación | LAM | Lámparas y tubos |  |
| TERMICA  BIPOLAR 16 A | Ud | ELEC | Electricidad | PROT | Protección eléctrica |  |
| FLEXIBLE METALICO  3/4"  50CM LARGO | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| FLEXIBLE METALICO 1/2" 5O CM LARGO | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| TAPA CIEGA CON MARCO  14,5X14,5 CM | Ud | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios |  |
| TAPA CIEGA CON MARCO 11X11 CM | Ud | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios |  |
| REJILLA CON MARCO | Ud | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios |  |
| LLAVE DE PASO FUSION 1 1/4" | Ud | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso |  |
| PILETA DE PATIO POLIANGULAR DE DE 40X63 AWADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| CURVA DE PVC  45° 110 AWADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| SIFON PAR APILETA DE PATIO AWADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| SIFON DE BACHA PVC | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| RAMAL Y DE 40 AWADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| CODO DE 45° DIAMETRO 40 AWADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| MANGUITO DIAMETRO 40 | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| T  DE 50 AWADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| CODO DE 45° DIAMETRO 50 AWADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| T DE 50 AWUADUCT | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| MANGUITO  PVC DIAMETRO  63 | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| ACOPLE RAPIDO  PVC  DUKE 1 1/4" | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| ACOPLE RAPIDO  PVC  DUKE 1 " | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| UNION DOBLE DE POLIPROPILENO DE 1 1/4" | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| ARO BASE DE INDORO | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| ARO BASE DE INDORO  DESPLAZADO | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| GRIFO  P/PARED DECAMATIC | Ud | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso |  |
| REPUESTO FV TORNILLOS Y ESPARRAGOS  DE BRONCE PAR AVALVULA DE DESCARGA. 0367,15,0-B CONJ.N/5 | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| REPUESTO FV  RESORTE Y MANIJA REGULADORA CONJ.N°5 | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| REPUESTO FV PISTON COMPETO CON GRAMPA  PARA  VALVULA DE DESCARGA | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| REPUESTOS FV RETENES Y ORINS PARA VALVULA | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| REPUESTO FV PISTON P/VALVULA AUTOMATICA | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| REPUESTO  FV CABEZA ARMADA PAR AVALVULA DE DESCARGA | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| FERRUM REPUESTO ARO DE GOMA PAR AMOCHILA  CON 2 TORNILLOS DE FIJACION | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| CONEXION JUNTA DE APOYO POR 86MM | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| Canilla de 1/2"de bronce | Ud | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso |  |
| Canilla automatica par alavatorio VASSER 50/1013,01 | Ud | SAN | Sanitario / Plomería | GRI | Grifería y llaves de paso |  |
| SOPAPAPA REJILLA DE 40 PAR AVACHA | Ud | SAN | Sanitario / Plomería | TAP | Tapas, rejillas y accesorios sanitarios |  |
| ASIENTO PARA INODORO FERUM  AND-TP-005-BL | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| Flexible mallado de 1/2" x 40 cm | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| Flexible mallado de 3/4"  x 40 cm | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| Union doble de pp hh 1/2" IPS | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| Union doble de pp hh 3/4" IPS | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| Niple  pp 1/2" 6 cm IPS | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| Niple pp 1/2"  10 cm IPS | Ud | SAN | Sanitario / Plomería | CON | Conexiones y flexibles |  |
| Boton  plast lat bco  c/balan p/ mochila CAPEA | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| Cadena de PVC P / DEP VARIOS ferrum VP 141 | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| Conjunto  compl. desc. p/ dp ANDINA FERRUM | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| Descarga pvc extens . o 40-50 M | Ud | SAN | Sanitario / Plomería | DES | Desagües y cañerías |  |
| Sella rosca 125 cc | Ud | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos |  |
| Repuesto valvula de descarga cod. RC1DESUDG-LT | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| Boton de accionamiento superficial de descarga de deposito de inodoro  Dealer | Ud | SAN | Sanitario / Plomería | REP | Repuestos sanitarios |  |
| REFRIGERANTE FREON 410A  11,35 KG | Ud | CLIM | Climatización y ventilación | REF | Refrigerantes |  |
| REFRIGERANTE  R22 13,5 KG | Ud | CLIM | Climatización y ventilación | REF | Refrigerantes |  |
| CERRRADURA DE PULSO PARA MUEBLES | Ud | CERR | Cerrajería y accesos | CER | Cerraduras |  |
| CERRADURA PRIVE 200 | Ud | CERR | Cerrajería y accesos | CER | Cerraduras |  |
| CERRADURA KALLAY 4003 | Ud | CERR | Cerrajería y accesos | CER | Cerraduras |  |
| CERRADURA KALLAY 5003 | Ud | CERR | Cerrajería y accesos | CER | Cerraduras |  |
| CIERRAPUERTA HIDRAULICO OCB | Ud | CERR | Cerrajería y accesos | CIE | Cierrapuertas |  |
| CIERRRA PUERTA /BAÑO LIBRE OCUPADO | Ud | CERR | Cerrajería y accesos | CIE | Cierrapuertas |  |
| CERRADURA CILINDRICA CON POMO FPV | Ud | CERR | Cerrajería y accesos | CER | Cerraduras |  |
| RODILLOS PARA SINTETICO DE POLIESTER Y TELA 10 CM | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| MINI RODILLO PARA EPOXI 10CM | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| MINI RODILLO PARA EPOXI 5CM | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| RODILLO PELO CORTO  22CM ROSAPIN | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| RODILLO DE LANA  22CM  CACIQUE | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| MINI RODILLO PARA EPOXI 8CM | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| RODILLO PARA SINTETICO DE POLIESTER Y TELA 8 CM | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| PINCELETA OBRA 4"  N°40 | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| PINCEL N° 20 | Ud | PINT | Pintura y terminaciones | HERR | Herramientas de aplicación |  |
| SILICONA ANTIHONGOS  BLANCO | Ud | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos |  |
| SILICONA  TRANSPARENTE | Ud | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos |  |
| SELLADOR SICAFLEX 1A PLUS | Ud | PINT | Pintura y terminaciones | SELL | Selladores y adhesivos |  |
| IMPERMEABILIZANTE QUIMEX  X 20KG | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| SINTETICO BRILLANTE GRIS ESPACIAL X 1LITRO | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| BARNIZ MARINO X 1 LITRO | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| CONVERTIDOR OXIDO GRIS X 1 LITRO | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| SINTETICO SATINADO MARRON TOBAGO X 1 LITRO | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| SINTETICO BRILLANTE AZUL X 1 LITRO | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| LATEX ACRILICO EXTERIOR/INTERIOR X 1 LITRO | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| SINTETICO BRILLANTE BERMELLON X 4 LITROS | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| SINTETICO 3 EN 1 AMARILLO MEDIO X 4 LITROS | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| EPOXI VIAL X 4 LITROS | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| EPOXI BLANCO X 4 LITROS | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| LATEX INTERIOR/EXTERIOR GRIS CEMENTO X 4 LITROS | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| PISOS DEPORTIVOS ROJO X 4 LITROS | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| REVESTIMIENTO ACRILICOS EXTERIOR NEGRO X 4 LITROS | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |
| SINTETICO SATINADO BLANCO X 1 LITRO | Ud | PINT | Pintura y terminaciones | REC | Pinturas y recubrimientos |  |