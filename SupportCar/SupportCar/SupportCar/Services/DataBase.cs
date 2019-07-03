using Android.Util;
using SQLite;
using SupportCar.Models;
using SupportCar.Services;
using SupportCar.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DataBase))]
namespace SupportCar.Services
{
    class DataBase : IDataBase
    {
        private IRecents Recents { get; set; }

        List<Recents> items;
        private void OnCreate()
        {
           
                var conn = this.GetConnectionAsync();
                this.CreateDatabase(conn);

                

                var itemExist = GetAllItems(conn).Result;
                var serviceExist = conn.Table<CarService>().ToListAsync().Result;
                var systemExist = conn.Table<CarSystem>().ToListAsync().Result;
                var infoExist = conn.Table<Info>().ToListAsync().Result;

                if (itemExist.Count == 0)
                    InsertItems(conn);

                if (serviceExist.Count == 0)
                    InsertCarService(conn);

                if (systemExist.Count == 0)
                    InsertCarSystem(conn);

                if (infoExist.Count == 0)
                    InsertInfo(conn);
            
        }
        public DataBase(bool isFirstCall)
        {
            if (isFirstCall)
                this.OnCreate();
        }
        public SQLiteAsyncConnection GetConnectionAsync()
        {
            try
            {
                var conn = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AutoApprende.db3"));
                

                return conn;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool CreateDatabase(SQLiteAsyncConnection connection)
        {
            try
            {
                connection.CreateTableAsync<Item>();
                connection.CreateTableAsync<Recents>();
                connection.CreateTableAsync<CarService>();
                connection.CreateTableAsync<CarSystem>();
                connection.CreateTableAsync<Info>();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #region INSERT
        private void InsertItems(SQLiteAsyncConnection conn)
        {
            try
            {
                conn.InsertAllAsync(new[]
                {
                    new Item{ Name = "Compresor (Aire Acondicionado)", Description="Tiene la función de comprimir el gas (fluido refrigerante) que permite en un ciclo de compresión/descompresión producir una transferencia de calor de una parte a otra de un circuito frigorífico.", Image = "compresor.png", Price = 1700, SharedNumber=1},
                    new Item{ Name = "Condensador (Aire Acondicionado)", Description="El condensador está localizado en la parte delantera del vehículo, entre los electroventiladores axiales y el radiador de refrigeración motor. Tiene por función evacuar el calor absorbido por el fluido frigorífico durante las fases de evaporación y compresión." , Image = "condensador.png", Price = 1800, SharedNumber=1},
                    new Item{ Name = "Evaporador (Aire Acondicionado)", Description="Tiene la función de retener el frio del gas refrigerante y enfriar el aire que es enviado al habitáculo del vehículo por medio soplador o blower." , Image = "evaporador.png", Price = 1300, SharedNumber=1},
                    new Item{ Name = "Bomba de Combustible", Description="Es la encargada de hacer que el sistema de inyección reciba de manera constante el combustible a través de los rieles de lo inyectores que mediante succión extraen el líquido del tanque." , Image = "bomba_combustible.png", Price = 2000, SharedNumber=2},
                    new Item{ Name = "Inyector", Description="Son los encargados de suministrar el combustible al conducto de admisión o a la cámara de precombustión, según si se trata de un sistema de inyección directa o indirecta respectivamente, de forma pulverizada y sin goteos para que el combustible se distribuya de la forma más homogénea posible según el régimen de funcionamiento del motor." , Image = "inyectores.png", Price = 850, SharedNumber=2},
                    new Item{ Name = "Servofreno", Description="Es el elemento que se utiliza para ayudar al conductor en la acción de frenado. La acción del servofreno se suma a la fuerza ejercida por el conductor sobre el pedal de freno, con el fin de mejorar la frenada. La fuerza de frenado aplicada al pedal de freno por el conductor tiene la propia limitación del esfuerzo humano. Para que este esfuerzo sobre el pedal de freno no tenga que ser considerable, se utilizan los servofrenos, que ayudan con su fuerza la acción sobre el pedal." , Image = "servofreno.png", Price = 1200, SharedNumber=3},
                    new Item{ Name = "Bomba de Frenos", Description="La función de la bomba de frenos, es la de convertir o transformar la fuerza mecánica de la presión ejercida por el conductor del vehículo sobre el pedal de freno, en presión hidráulica (reforzada o no por un servofreno). Por medio de canalizaciones, esta presión es transmitida a los bombines de las ruedas que accionan los frenos." , Image = "bomba_freno.png", Price = 1800, SharedNumber=3},

                    new Item{ Name = "Frenos de Tambor", Description="Es un tambor instalado sobre el buje de la rueda que gira en consonancia con ésta. La fricción es causada por un par de zapatas o pastillas que se encargan de presionar la superficie interior del tambor." , Image = "freno_tambor.png", Price = 5900, SharedNumber=3},
                    new Item{ Name = "Plato de Fijación", Description="El plato de freno está compuestopor un plato portafrenos, sobre el que se monta un cilindro de accionamiento hidráulico, las zapatas de freno y los demás elementos de fijación y regulación de las zapatas. Por otra parte, las zapatas se unen en uno de sus extremos al cilindro hidráulico y por el otro a un soporte que puede ser fijo o regulable. Al mismo tiempo se unen con el plato de freno mediante un muelle que permite su movimiento hacia el tambor manteniéndolas fijas durante su desplazamiento. Este muelle, permite que las zapatas vuelvan a su estado original un vez ha dejado de actuar el bombín." , Image = "plato_fijacion.png", Price = 102, SharedNumber=3},
                    new Item{ Name = "Balatas", Description="Elementos que, generalmente, están formadas por dos chapas de acero soldadas con forma de media luna, y recubiertas en su parte externa por los forros de freno, los cuales están unidos a la zapata mediante remaches embutidos o pegados con cola de contacto. Éstos serán los encargados de frenar mediante fricción con el tambor." , Image = "balatas.png", Price = 494, SharedNumber=3},
                    new Item{ Name = "Freno de Estacionamiento", Description="La única parte visible del freno demano es la palanca, que está situada entre los asientos delanteros, siempre accesible para el conductor. La palanca está unida a la varilla de tiro y esta a su vez está conectada mediante las tuercas de reglaje a la pieza derivadora. De este elemento es de donde parte el sistema de cables; cada uno de ellos se dirige a cada una de las ruedas de atrás. Aquí los cables se unen a la palanca de accionamiento, donde cada varilla acciona una uña que enclava el trinquete hasta que el coche se detiene por completo." , Image = "freno_estacionamiento.png", Price = 400, SharedNumber=3},
                    new Item{ Name = "Tambor", Description="Esta pieza es la parte giratoria del freno y la que se va a llevar prácticamente todo el calor generado en el frenado" , Image = "tambor.png", Price = 560, SharedNumber=3},
                    new Item{ Name = "Frenos de Disco Ventilados", Description="Un freno de Disco consiste en undisco de hierro fundido o rotor que gira con la rueda, y una pinza o mordaza (cáliper) montada en la suspensión delantera que presiona las pastillas de fricción (pastilla) contra el disco." , Image = "disco_ventilado.png", Price = 700, SharedNumber=3},
                    new Item{ Name = "Disco", Description="" , Image = "disco.png", Price = 950, SharedNumber=3},
                    new Item{ Name = "Caliper", Description="Su funcionamiento es sencillo, pues contiene dos pistones, ambos en la misma cara que presionan a una pastilla de freno, la pastilla presiona y aprieta el rotor de esta manera se detiene la rueda por efecto de la fricción." , Image = "caliper.png", Price = 3000, SharedNumber=3},
                    new Item{ Name = "Pastillas de Freno", Description="Son las que generan la mayor fricción con el disco. Su material, le hace ser bien duradera y le da una mayor potencia de frenado.Estas requieren de un reemplazo cada cierto tiempo, sin embargo dispone de un sensor que le indica al conductor cuando debe de hacerlo, puesto a que suena un chillido en aquellas que posee una pieza de metal y la misma comienza a desgastarse." , Image = "pastillas_freno.png", Price = 1450, SharedNumber=3},


                    new Item{ Name = "Filtro de Combustible", Description="Es el encargado de filtrar el combustible de impurezas que puedan ingresar al sistema. Por lo general se encuentra después de la bomba de gasolina (en la línea de suministro de gasolina)." , Image = "filtroCombustible.png", Price = 350, SharedNumber=2},
                     new Item{ Name = "ECM (Computadora del motor)", Description="Es la unidad de control electrónico o procesador, algunas marcas lo denominan: ECU, ECM, Centralita, Calculador de inyección. Es la central que recibe las señales de los sensores y enviar las señales a los actuadores a fin de controlar o automatizar el proceso de inyección. *Este componente es muy delicado por lo que se recomienda acudir directamente a una agencia." , Image = "ecm.png", Price = 000, SharedNumber=2},


                    new Item{ Name = "Culata (Motor)", Description="La culata es la parte superior del motor, aunque en ocasiones también se le denomina tapa de cilindros. Con ella se cierran los cilindros en su parte superior, y se alojan las válvulas de admisión y escape, las bujías (en motores de gasolina), el árbol de levas, los conductos de admisión de aire y combustible y los conductos de escape. Es el elemento que soporta las explosiones que se generan en los cilindros, por ello va atornillada firmemente al bloque motor. En general, la culata está construida con una doble pared que permite la circulación del líquido refrigerante (en los motores con refrigeración por aire el sistema es diferente). La culata suele estar fabricada en hierro fundido, aluminio o de una aleación ligera. Se fabrica con estos elementos, ya que son materiales que se enfrían rápidamente, que son de fácil enfriamiento y que son capaces de resistir altas presiones en su interior." , Image = "culata.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Bloque (Motor)", Description="El bloque motor, también conocido como bloque de cilindros, está construido en hierro o aluminio, en una sola pieza. Es el elemento que aloja en su interior los cilindros de un motor de combustión interna, además de los soportes de apoyo del cigüeñal. Dentro de los cilindros es donde los pistones suben y bajan, ayudados por las bielas. Los motores de refrigeración líquida, los más frecuentes, tiene una serie de conductos por los que circula el agua o líquido refrigerante y el aceite lubrique el motor. El filtro de aceite se suele ubicar en el bloque motor. Para determina la cilindrada de un motor, se hace la medida del diámetro de los cilindros, junto con la carrera que tienen los pistones." , Image = "bloque.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Cárter (Motor)", Description="El cárter es un recipiente metálico en el que se alojan los mecanismos operativos del motor. Sirve como cierre del bloque por la parte inferior, y también funciona como depósito para el aceite del motor. Además, actúa como refrigerante, puesto que el aceite que llega caliente cede parte de este calor al exterior. Normalmente, el cárter está fabricado en chapa de acero o en aleaciones de aluminio. Éstas últimas, aunque no reducen demasiado su peso, sí aportan ventajas a la hora de disipar el calor en menos tiempo. Esta pieza nos permite proteger al motor de la entrada de agua, polvo y toda la contaminación posible. Además, el cárter garantiza condiciones de seguridad. Por un lado, impide proyecciones en caso de fallo. Por otro, evita el acceso de personas o elementos externos a piezas funcionales del motor. El cárter se fija al bloque con tornillos y, al igual que ocurre con la culata, se interpone una junta estanca para su sellado. En su parte inferior, se coloca el tapón que nos permite vaciarlo a la hora de sustituir el aceite" , Image = "carter.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Árbol de Levas (Motor)", Description="El árbol de levas es un mecanismo cuya principal función es regular la apertura y el cierre de las válvulas, tanto de apertura como de cierre. Compuesto por una serie de elementos denominados levas. De tamaños y formas diversas (normalmente ovoides), aseguran el correcto funcionamiento del motor en determinado rango de revoluciones y velocidades." , Image = "arbol_levas.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Válvulas (Motor)", Description="Las válvulas son otro de los mecanismos importantes del motor de un coche. En concreto, son las encargadas de dejar fluir los gases hacia el cilindro. Las válvulas sueles ser muy robustas y están fabricadas en acero u otros materiales como titanio, ya que trabajan a temperaturas muy altas. Dependiendo del número de válvulas y de su posición, el coche presentará un comportamiento u otro. Por ejemplo, los coches de 8 válvulas funcionan mejor en pares bajos. Mientras, los de 16 válvulas, al dejar pasar mejor los gases hacia los cilindros, tiene mejor respuesta a altas revoluciones." , Image = "valvula.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Pistones (Motor)", Description="Los pistones se encuentran dentro del cilindro y son los encargados de transmitir la energía de los gases de la combustión a la biela. Es una especie de guía para el pie de biela, que luego pasa esta energía al cigüeñal. Los pistones tienen diferentes partes: Cabeza – Es la parte superior que está en contacto con el fluido durante todo el proceso Cielo – La superficie superior de la cabeza Perno – Se trata del anclaje entre el pistón y la biela Faldas – Son las que permiten el deslizamiento del pistón dentro del cilindro" , Image = "pistones.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Árbol de Levas (Motor)", Description="El árbol de levas es un mecanismo cuya principal función es regular la apertura y el cierre de las válvulas, tanto de apertura como de cierre. Compuesto por una serie de elementos denominados levas. De tamaños y formas diversas (normalmente ovoides), aseguran el correcto funcionamiento del motor en determinado rango de revoluciones y velocidades." , Image = "arbolLevas.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Cilindros (Motor)", Description="Los cilindros son las piezas por las que circulan los pistones. Acuña su nombre debido a su forma geométrica, parecida a un cilindro. Están fabricados con materiales resistentes porque son, junto a pistones y válvulas, los que crean y soportan constantes explosiones de energía que hacen funcionar el motor. Existen motores que tienen desde un cilindro a otros que tienen 12 o 14. El conjunto que forman estos cilindros en un vehículo de denomina bloque motor." , Image = "cilindros.png", Price = 000, SharedNumber=9},
                    new Item{ Name = "Cigüeñal (Motor)", Description="Por último, el cigüeñal es algo así como el eje maestro del motor. Se trata de la pieza que soporta las fuerzas y presiones que provocan las válvulas al realizar la combustión. El cigüeñal empuja a los pistones que transmiten la energía al cigüeñal a través de las bielas, convirtiendo los movimientos alternativos en fuerza circular." , Image = "ciguenal.png", Price = 000, SharedNumber=9},


                    new Item{ Name = "Batería (Sistema de Encendido)", Description="La batería del auto, podemos definir a la batería como un acumulador de electricidad, recibe energía eléctrica de una fuente exterior, la transforma en energía química y la almacena hasta que la transforma de nuevo en energía eléctrica cuando es requerida." , Image = "bateria.jpg", Price = 2000, SharedNumber=66},
                    new Item{ Name = "Bobina (Sistema de Encendido)", Description="Cumple con la función de elevar el voltaje normal que seria 6, 12, y 24 Voltios obviamente dependiendo del auto, a un valor 1000 veces mayor con el propósito de producir la chispa en la bujía, la cual con la mezcla de aire y combustible en la cámara de combustión permite la explosión que genera que el motor del auto encienda." , Image = "bobina.png", Price = 2000, SharedNumber=66},
                    new Item{ Name = "Bujías (Sistema de Encendido)", Description="Es el elemento clave para el sistema de arranque de los coches, produce el encendido de la mezcla de combustible y oxígeno en los cilindros mediante una chispa que puede ir de los 12.000 a los 40.000 voltios." , Image = "bujias.jpg", Price = 900, SharedNumber=66},
                    new Item{ Name = "Switch (Sistema de Encendido)", Description="El switch de encendido se encarga de proporcionar el paso de corriente para que pueda encender el automóvil y sus accesorios, básicamente es un interruptor de señales de tierra o corriente y lleva cuatro cables que son los que controlan, la corriente de batería, corriente de accesorios, corriente de start y corriente de ignición." , Image = "switch.jpg", Price = 1000, SharedNumber=66},


                    new Item{ Name = "Amortiguadores (Sistema de Dirección y Suspensión)", Description="Los amortiguadores del vehículo son primordiales en la seguridad activa del vehículo, ya que protegen de golpes, impactos y vibraciones tanto a los pasajeros como al resto de elementos del automóvil." , Image = "amortiguadores.png", Price = 4350, SharedNumber=55},
                    new Item{ Name = "Resortes (Sistema de Dirección y Suspensión)", Description="Un resorte amortigua los efectos de la irregularidad de la carretera y los impactos de la carretera, convirtiéndolos en vibraciones. El resorte forma un vínculo importante entre los componentes individuales de la suspensión, conectando las masas suspendidas y no suspendidas en el vehículo." , Image = "resortes.png", Price = 670, SharedNumber=55},
                    new Item{ Name = "Rótulas (Sistema de Dirección y Suspensión)", Description="Las rótulas constituyen un elemento de unión y fijación de la suspensión y de la dirección, que permite su pivotamiento y giro manteniendo la geometría de las ruedas." , Image = "rotulas.png", Price = 290, SharedNumber=55},
                    new Item{ Name = "Barra Estabilizadora (Sistema de Dirección y Suspensión)", Description="La barra estabilizadora es un componente de la suspensión que tiene como objetivo lograr que ambas ruedas de un mismo eje compartan el movimiento vertical. Con ello se logra minimizar la inclinación lateral que sufre el auto en las curvas al estar sometido a la fuerza." , Image = "barra_estabilizadora.jpg", Price = 2100, SharedNumber=55},
                    new Item{ Name = "Horquilla (Sistema de Dirección y Suspensión)", Description="Una horquilla de suspensión es una parte que conecta el chasis con el soporte de la llanta. También se conoce como brazo de suspensión o brazo de control.  Dichos brazos trabajan junto a los amortiguadores para que las llantas vayan de abajo hacia arriba. " , Image = "horquilla.png", Price = 1650, SharedNumber=55},
                    new Item{ Name = "Llantas (Sistema de Dirección y Suspensión)", Description="Las llantas son el único punto de contacto del vehículo con el suelo por lo cual es evidente que son un elemento de gran importancia e indispensable para el vehículo y que  tiene que realizar un gran número de funciones, tales como guiar, soportar la carga, amortiguar, rodar, transmitir los esfuerzos y durar. Especificaciones: 155/70 R14" , Image = "llanta.png", Price = 1200, SharedNumber=55},
                    new Item{ Name = "Bomba de Dirección Hidráulica (Sistema de Dirección y Suspensión)", Description="La bomba es la encargada de generar la presión hidráulica que nos facilita maniobrar con el coche con poco esfuerzo. Su funcionamiento es sencillo: una correa mueve la polea de la bomba de dirección moviendo el líquido hidráulico a través de un sistema totalmente estanco." , Image = "bomba_direccion.png", Price = 6350, SharedNumber=55},
                    new Item{ Name = "Terminal de Dirección (Sistema de Dirección y Suspensión)", Description="Son uniones (tipo rótula) con cierta elasticidad para absorber las irregularidades del suelo, y tiene como función principal unirse con cada una de las ruedas direccionales." , Image = "terminal.png", Price = 1190, SharedNumber=55},
                    new Item{ Name = "Columna de Dirección (Sistema de Dirección y Suspensión)", Description="La columna de dirección está normalmente compuesta por un tubo de acero (raramente de aleación ligera), fijado al bastidor o a la carrocería del vehículo y por dentro de la cual pasa el eje, que se une por una parte al volante y por otra a la caja de la dirección. El eje de la dirección gira en el interior de la columna." , Image = "columna.png", Price = 1400, SharedNumber=55},
                    new Item{ Name = "Caja de Dirección (Sistema de Dirección y Suspensión)", Description=" Recibe el movimiento del timón y la barra y lo reparte a las ruedas, mediante movimientos realizados por engranajes. Puede ser de tipo bolas recirculantes, o de cremallera. En este caso cremallera." , Image = "caja.png", Price = 5000, SharedNumber=55},


                    new Item{ Name = "Collarín (Transmisión)", Description="La función del Collarín de embrague es desacoplar el motor de la caja de transmisión al liberar el plato de presión del disco de embrague. Cuando se pisa el pedal del embrague, se presiona el collarín contra el diafragma, como consecuencia el plato de presión se separa del disco de embrague y el collarín gira junto con el plato de presión." , Image = "collarin.jpg", Price = 700, SharedNumber=934},
                    new Item{ Name = "Clutch (Transmisión)", Description="Es un sistema que permite conectar y desconectar la transmisión de una energía mecánica. Esta energía mecánica es la que proviene desde la ejecución de los pistones, pasando por las bielas, cigüeñal y rueda automotriz." , Image = "clutch.jpg", Price = 4000, SharedNumber=934},

                });


            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void InsertCarService(SQLiteAsyncConnection conn)
        {
            try
            {
                conn.InsertAllAsync(new []
                {
                    new CarService { Name = "Afinación Básica", FolderName="AfinacionBasica"},
                    new CarService { Name = "Cambio de Llantas", FolderName = "CambioLlanta" },
                    new CarService { Name = "Cambio de Parabrisas", FolderName = "Parabrisas" },
                    new CarService { Name = "Batería Descargada", FolderName = "BateriaDescargada" },
                    new CarService { Name = "Prueba de Batería", FolderName = "PruebaBateria" },
                    new CarService { Name = "Frenos", FolderName = "Frenos" },
                    new CarService { Name = "Revisión de Carretera", FolderName = "Revision" },
                    new CarService { Name = "Líquidos", FolderName = "Liquidos" },
                    new CarService { Name = "Revisión de Suspensión y Dirección", FolderName = "DireccionSuspension" },
                    new CarService { Name = "Algunos Consejos...", FolderName = "Consejos" },
                });
            }
            catch (Exception e)
            {
                throw e;
            }

        }



        private void InsertCarSystem(SQLiteAsyncConnection conn)
        {
            try
            {
                conn.InsertAllAsync(new[]
                {
                    new CarSystem { Name = "Aire Acondicionado", FolderName="AireAcondicionado"},
                    new CarSystem { Name = "Motor de Gasolina", FolderName="MotorGasolina"},
                    new CarSystem { Name = "Sistema Electrónico y OBD II", FolderName="OBD"},
                    new CarSystem { Name = "Sistema de Arranque", FolderName="Arranque"},
                    new CarSystem { Name = "Sistema de Encendido", FolderName="Encendido"},
                    new CarSystem { Name = "Sistema de Enfriamiento del Motor", FolderName="Enfriamiento"},
                    new CarSystem { Name = "Sistema de Escape", FolderName="Escape"},
                    new CarSystem { Name = "Sistema de Inyección", FolderName="Inyeccion"},
                    new CarSystem { Name = "Sistema de Transmisión", FolderName="Transmision"},
                    new CarSystem { Name = "Sistema de Frenos, Suspensiones y Dirección", FolderName="Frenos"},
                });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void InsertInfo(SQLiteAsyncConnection conn)
        {
            try
            {
                conn.InsertAllAsync(new[]
                {
                    new Info { Name = "Aceite de Motor", FolderPath="AceiteMotor"},
                    new Info { Name = "Bayoneta", FolderPath="Bayoneta"},
                    new Info { Name = "Caja de Fusibles", FolderPath="CajaFusibles"},
                });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void InsertQuery(SQLiteAsyncConnection conn, Item item)
        {
            try
            {
                conn.InsertAsync(new Recents { IdItem = item.Id });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public Item GetItem(string name, SQLiteAsyncConnection conn)
        {
            try
            {
                var item = conn.Table<Item>().Where(x => x.Name == name).FirstOrDefaultAsync();
                //conn.DeleteAllAsync<Recents>();
                if(item.Result!=null)
                    InsertQuery(conn, item.Result);

                return item.Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Info GetInfo(string name, SQLiteAsyncConnection conn)
        {
            try
            {
                var item = conn.Table<Info>().Where(x => x.Name.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();

                return item.Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Item GetItemByID(int itemID, SQLiteAsyncConnection conn)
        {
            try
            {
                var item = conn.Table<Item>().Where(x => x.Id == itemID).FirstOrDefaultAsync();

                var itemConsulted = conn.Table<Recents>().Where(q => q.IdItem == item.Id).FirstOrDefaultAsync();
                if (itemConsulted == null)
                    InsertQuery(conn, item.Result);
                else
                {
                    conn.DeleteAsync(itemConsulted);
                    InsertQuery(conn, item.Result);
                }

                return item.Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Item>> GetAllItems(SQLiteAsyncConnection conn)
        {
            try
            {
                var items = await conn.Table<Item>().ToListAsync().ConfigureAwait(false);
                
                return items;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<string> GetItemData(int id, SQLiteAsyncConnection conn)
        {
            var item = conn.Table<Item>().Where(x => x.Id == id).FirstOrDefaultAsync();
            List<string> listItems = new List<string>();
            listItems.Add(item.Result.Name);
            listItems.Add(item.Result.Image);

            IEnumerable<string> list = listItems;

            return list;
        }

        public List<Recents> GetRecentItems(SQLiteAsyncConnection conn)
        {
            this.items = new List<Recents>();

            var recentItems = conn.Table<Recents>().ToListAsync().Result;
            foreach (var item in recentItems)
            {
                items.Add(item);
            }

            return items;
        }

        public List<CarService> GetCarServices(SQLiteAsyncConnection conn)
        {
            var list = new List<CarService>();

            var carServices = conn.Table<CarService>().ToListAsync().Result;
            var lastItem = carServices[0];
            var c = 0;
            foreach (var item in carServices)
            {
                if (lastItem.Name == item.Name && c > 0)
                    break;
                else
                {
                    list.Add(item);
                    c++;
                }

            }

            return carServices;
        }

        public List<CarSystem> GetCarSystems(SQLiteAsyncConnection conn)
        {
            var list = new List<CarSystem>();

            var carSytems = conn.Table<CarSystem>().ToListAsync().Result;
            var lastItem = carSytems[0];
            var c = 0;
            foreach (var item in carSytems)
            {
                if (lastItem.Name == item.Name && c > 0)
                    break;
                else
                {
                    list.Add(item);
                    c++;
                }

            }

            return list;
        }

        public Task<List<Item>> GetRelatedItems(Item mainItem, SQLiteAsyncConnection conn)
        {
            var itemsRelated = conn.Table<Item>().Where(x => x.SharedNumber == mainItem.SharedNumber && x.Id != mainItem.Id).ToListAsync();
            return itemsRelated;
        }

        #endregion
    }
}
