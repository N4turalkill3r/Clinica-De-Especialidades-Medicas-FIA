using System;
using System.Collections.Generic;
namespace ClinicaMedicaDeEspecialidades
{
    public class MainMenu   
    {
        public static readonly List<string> EspecialidadesValidas = new List<string>
        {
            "Medicina General", "Pediatría", "Ginecología", "Cardiología", "Dermatología", "Oftalmología", "Odontología", "Ortopedia"
        };
        public static int[] formasPago = new int[100];       
        public static int[] calificaciones = new int[100];
        public static void Main(string[] args) //este es el main del programa no eliminar aunque no tenga referencias :)
        {
            GestorMedicos.InicializarMedicos();
            GestorArchivos.CargarDatos();
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("===========================================");
                Console.WriteLine("    CLINICA MEDICA DE ESPECIALIDADES FIA   ");
                Console.WriteLine("===========================================");
                Console.WriteLine("1. Ingresar Nuevo Paciente");
                Console.WriteLine("2. Ver Reportes");//esta opcion lleva a un submenu de reportes.
                Console.WriteLine("3. Gestionar Cobros / Cancelar Citas");
                Console.WriteLine("4. Calificar Atención (Solo Atendidos)"); 
                Console.WriteLine("5. Buscar Paciente"); 
                Console.WriteLine("6. Salir");
                Console.WriteLine("===========================================");
                Console.Write("Seleccione una opción: ");
                string? opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1": GestorPacientes.CapturarDatosPaciente(); break;
                    case "2": MostrarReportes(); break;//MostrarReportes es una funcion del main que se encarga de mostrar los reportes de pacientes registrados y promedio de calificaciones por medico.
                    case "3": GestorCobros.ProcesarCobro(); break;
                    case "4": CalificarMedico(); break;
                    case "5": BuscarPaciente(); break;
                    case "6":
                        continuar = false;
                        Console.WriteLine("\nGracias por visitar nuestra clínica.");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOpción no válida. Presione cualquier tecla para reintentar.");
                        Console.ResetColor();
                        Console.ReadKey(); 
                        break;
                }
            }
        }
        public static void MostrarReportes()//esta funcion se encarga de mostrar los reportes
        {
            bool volver = false;//esta variable se utiliza para controlar el bucle del menú de reportes, permitiendo al usuario volver al menú principal cuando lo desee.
            while (!volver)//mientras volver sea false, el menú de reportes seguirá mostrándose, permitiendo al usuario seleccionar diferentes opciones de reporte hasta que decida volver al menú principal.
            {
                Console.Clear();//Limpia la consola
                Console.WriteLine("===================================================");
                Console.WriteLine("=== Menú de Reportes ===");
                Console.WriteLine("===================================================");
                Console.WriteLine("1. Reporte General de pacientes registrados");
                Console.WriteLine("2. Reporte de pacientes por especialidad");
                Console.WriteLine("3. Reporte de promedio de calificaciones por médico");
                Console.WriteLine("4. Reporte de Ingresos");
                Console.WriteLine("5. Volver al menú principal");
                Console.WriteLine("===================================================");
                Console.WriteLine("Seleccione una opción:");
                string? opcionReporte = Console.ReadLine();//Lee la opción seleccionada por el usuario para mostrar el reporte correspondiente o volver al menú principal.
                switch (opcionReporte)//Dependiendo de la opción seleccionada, se ejecutará el bloque de código correspondiente para mostrar el reporte deseado o volver al menú principal.
                {
                    case "1":
                        MostrarPacientes();//Llama a la función MostrarPacientes para mostrar el reporte de pacientes registrados.
                        break;
                    case "2":
                        PacientesPorEspecialidad();//Llama a la función PacientesPorEspecialidad para mostrar el reporte de pacientes por especialidad.
                        break;
                    case "3":
                        GestorMedicos.NotaPromedioMedicos();//Llama a la función NotaPromedioMedicos de la clase GestorMedicos para mostrar el reporte de promedio de calificaciones por médico.
                        break;
                    case "4":
                        GestorCobros.GananciasPorEspecialidad();//Llama a la función GananciasPorEspecialidad de la clase GestorCobros para mostrar el reporte de ganancias por especialidad.
                        break;
                    case "5"://Si el usuario selecciona la opción 5, se establece volver como true, lo que hará que el bucle del menú de reportes termine y el programa regrese al menú principal.
                        volver = true;
                        break;
                    default://Si el usuario ingresa una opción no válida, se muestra un mensaje de error y se espera a que el usuario presione una tecla para reintentar.
                        Console.WriteLine("\nOpción no válida. Intente nuevamente.");
                        Console.ReadKey();
                        break;
                }
                            
            }
            
        }
        private static void MostrarPacientes()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO DE PACIENTES REGISTRADOS ---");
            Console.WriteLine("========================================"); //Este es el inicio del output de pacientes guardados
            if (GestorPacientes.contadorPacientes == 0) //si no hay pacientes registrados, se muestra un mensaje indicando que no hay pacientes en el sistema.
            {
                Console.WriteLine("No hay pacientes registrados en este momento."); //Este es el out put que se muestra si no hay pacientes registrados en el sistema.
            }
            else
            {
                for (int i = 0; i < GestorPacientes.contadorPacientes; i++)//Aqui se recorre el arreglo de pacientes registrados y se muestra la informaccion guardada
                {
                    string nombre = GestorPacientes.nombresPacientes[i];
                    int edad = GestorPacientes.edades[i];
                    string codigo = GestorPacientes.codigosCitas[i];
                    int indexEspecialidad = GestorPacientes.especialidades[i] - 1;
                    string nombreEsp = (indexEspecialidad >= 0 && indexEspecialidad < EspecialidadesValidas.Count) 
                        ? EspecialidadesValidas[indexEspecialidad] : "Desconocida";
                    TipoTriage triage = GestorPacientes.triages[i];
                    string textoEstado = GestorPacientes.estados[i] switch {              // los estados del paciente que pide la rubrica.
                        'A' => "Atendido",
                        'C' => "Cancelada/Referido",
                        'P' => "Pendiente",
                        _ => "Desconocido"
                    };
                    string textoPago = formasPago[i] switch {
                        1 => "Efectivo", 2 => "Tarjeta", 3 => "Seguro", _ => "N/A"
                    };
                    Console.Write($"[{codigo}] {nombre.PadRight(20)} | Esp: {nombreEsp.PadRight(16)} | Est: {textoEstado.PadRight(18)} | Pago: {textoPago.PadRight(8)} | Monto: ${GestorPacientes.costos[i]:F2} |Desc: ${GestorPacientes.descuentosTotales[i]:F2} | Tr: ");
                    Console.ForegroundColor = triage switch
                    {
                        TipoTriage.Rojo => ConsoleColor.Red,
                        TipoTriage.Amarillo => ConsoleColor.Yellow,
                        _ => ConsoleColor.Green
                    };
                    Console.WriteLine(triage);
                    Console.ResetColor();
                }
            }
            Console.WriteLine("============================================================================================================");
            Console.WriteLine($"Total de espacios ocupados: {GestorPacientes.contadorPacientes} de 100.");
            Console.WriteLine("\nPresione cualquier tecla para regresar al menú de Reportes.");
            Console.ReadKey();
        }
        private static void CalificarMedico()  //esta es una funcion del main para calificar a los docotores de la clinica.
        {
            Console.Clear();
            Console.WriteLine("--- CALIFICAR ATENCIÓN MÉDICA ---");         
            bool hayAtendidos = false; //define una variable de tipo booleano.
            for (int i = 0; i < GestorPacientes.contadorPacientes; i++)
            {
                if (GestorPacientes.estados[i] == 'A' && calificaciones[i] == 0) //busca pasintes atendidos pero que la califficacion sea == 0 o no exista.
                {
                    Console.WriteLine($"[{i}] {GestorPacientes.codigosCitas[i]} - {GestorPacientes.nombresPacientes[i]}");
                    hayAtendidos = true;
                }
            }
            if (!hayAtendidos) //si esta variable es false, no hay pacientes atendidos para calificar.
            {
                Console.WriteLine("No hay pacientes en estado 'Atendido' pendientes de calificar.");
                Console.WriteLine("\nPresione cualquier tecla para regresar...");
                Console.ReadKey();
                return;
            }
            Console.Write("\nIngrese el índice [#] del paciente que desea calificar: ");
            if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 0 && indice < GestorPacientes.contadorPacientes && GestorPacientes.estados[indice] == 'A')
            {
                string nombreMedico = GestorPacientes.medicosAtencion[indice];//el medico es buscado automaticamente a partir del indice del paciente seleccionado, no se pide al usuario que lo ingrese.
                Console.WriteLine($"\nCalificando atención del médico: {nombreMedico}");//se muestra el nombre del medico que atendio al paciente seleccionado para calificar.
                Console.WriteLine("Favor de escribir un numero del 1 al 5 para calificar la atención recibida:");//se pide al usuario que ingrese una calificacion
                if (int.TryParse(Console.ReadLine(), out int nota) && nota >= 1 && nota <= 5)
                {
                    calificaciones[indice] = nota;//la calificacion se guarda en el arreglo de calificaciones en la posicion del indice del paciente seleccionado.
                    GestorArchivos.GuardarDatos(); // Guardar en el txt

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n¡Calificación guardada exitosamente!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Calificación inválida. Debe ser un número entre 1 y 5.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Índice incorrecto, el paciente no existe, o no está en estado 'Atendido'.");
                Console.ResetColor();
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        private static void BuscarPaciente()
        {
            Console.Clear();
            Console.WriteLine("--- BÚSQUEDA DE PACIENTE ---"); // Aqui buscas el paciente por du codigo o nombre completo, no acepta nombre parciales.
            string? busqueda = Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrEmpty(busqueda)) return;
            bool encontrado = false;
            for (int i = 0; i < GestorPacientes.contadorPacientes; i++)
            {
                string codigoStr = GestorPacientes.codigosCitas[i].ToLower();
                string nombreStr = GestorPacientes.nombresPacientes[i].ToLower();
                if (codigoStr == busqueda || nombreStr.Contains(busqueda))
                {
                    encontrado = true;
                    Console.WriteLine("\n========================================");
                    Console.WriteLine("          RESULTADO DE BÚSQUEDA         ");
                    Console.WriteLine("==========================================");
                    Console.WriteLine($"Nombres y Apellidos: {GestorPacientes.nombresPacientes[i]}");
                    string exp = GestorPacientes.registroDePacientes[i].Split('|')[0].Trim();
                    Console.WriteLine($"Código de Expediente: {exp}");
                    Console.Write("Triage: ");
                    Console.ForegroundColor = GestorPacientes.triages[i] == TipoTriage.Rojo ? ConsoleColor.Red : (GestorPacientes.triages[i] == TipoTriage.Amarillo ? ConsoleColor.Yellow : ConsoleColor.Green);
                    Console.WriteLine(GestorPacientes.triages[i]);
                    Console.ResetColor();
                    Console.WriteLine($"Descuento Recibido: ${GestorPacientes.descuentosTotales[i]:F2}");
                    int indexEspecialidad = GestorPacientes.especialidades[i] - 1;
                    string nombreEsp = (indexEspecialidad >= 0 && indexEspecialidad < EspecialidadesValidas.Count) ? EspecialidadesValidas[indexEspecialidad] : "Desconocida";
                    Console.WriteLine($"Especialidad: {nombreEsp}");
                    Console.WriteLine($"Médico que Atendió: {GestorPacientes.medicosAtencion[i]}");
                    string calificacionStr = calificaciones[i] > 0 ? $"{calificaciones[i]}/5" : "No calificado (o no atendido aún)";
                    Console.WriteLine($"Calificación del Paciente: {calificacionStr}");
                    Console.WriteLine("========================================");
                }
            }
            if (!encontrado)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNo se encontró ningún paciente con ese criterio.");
                Console.WriteLine("Puede revisar el registro de pacientes en la Opción 2 del menú principal para verificar el código correcto.");
                Console.ResetColor();
            }
            Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
            Console.ReadKey();
        }
        private static void PacientesPorEspecialidad()//esta funcion se encarga de mostrar los pacientes registrados por especialidad.
        {
            Console.Clear();
            if (GestorPacientes.contadorPacientes == 0)//si no hay pacientes registrados, se muestra un mensaje indicando que no hay pacientes en el sistema.
            {
                Console.WriteLine("No hay pacientes registrados en este momento.");
                Console.WriteLine("Presione cualquier tecla para regresar al menú de Reportes...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("seleccione la especialidad para mostrar los pacientes registrados:");
            Console.WriteLine(EspecialidadesValidas.Select((esp, index) => $"{index + 1}. {esp}").Aggregate((a, b) => $"{a}\n{b}"));//se muestra la lista de especialidades válidas con su respectivo número para que el usuario pueda seleccionar la especialidad deseada.
            Console.WriteLine("Ingrese el número de la especialidad:");
            if (!int.TryParse(Console.ReadLine(), out int opcionEsp) || opcionEsp < 1 || opcionEsp > EspecialidadesValidas.Count)//se valida que la opción ingresada sea un número válido dentro del rango de especialidades disponibles. Si no es válida, se muestra un mensaje de error y se regresa al menú de reportes.
            {
                Console.WriteLine("Opción no válida. Presione cualquier tecla para regresar al menú de Reportes...");
                Console.ReadKey();
                return;
            }
            string especialidadSeleccionada = EspecialidadesValidas[opcionEsp - 1];//se obtiene el nombre de la especialidad seleccionada a partir del número ingresado por el usuario, restando 1 para ajustar al índice del arreglo de especialidades válidas.
            Console.WriteLine($"\nPacientes registrados en la especialidad: {especialidadSeleccionada}");//se muestra un encabezado indicando la especialidad seleccionada para mostrar los pacientes registrados en esa especialidad.
            Console.WriteLine("=======================================================================");
            for (int i = 0; i < GestorPacientes.contadorPacientes; i++)//se recorre el arreglo de pacientes registrados y se muestra la información de aquellos pacientes cuya especialidad coincida con la especialidad seleccionada por el usuario.
            {
                if (GestorPacientes.especialidades[i] == opcionEsp)
                {
                    Console.WriteLine($"[{GestorPacientes.codigosCitas[i]}] {GestorPacientes.nombresPacientes[i]} - Triage: {GestorPacientes.triages[i]} - Estado: {GestorPacientes.estados[i]}");
                }
            }
            Console.WriteLine("=======================================================================");
            Console.WriteLine("Presione cualquier tecla para regresar al menú de Reportes...");
            Console.ReadKey();
        }
    } 
}