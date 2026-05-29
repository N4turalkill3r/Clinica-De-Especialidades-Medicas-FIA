using System;
namespace ClinicaMedicaDeEspecialidades
{
    public static class GestorCobros //variable publica para que se pueda acceder desde el menu principal
    {
        public static void ProcesarCobro()
        {
            Console.Clear();
            Console.WriteLine("=== GESTIÓN DE COBROS / CANCELAR CITA / REFERIR PACIENTE ===");
            if (GestorPacientes.contadorPacientes == 0)
            {
                Console.WriteLine("Aun noo hay pacientes registrados en el sistema.");
                Console.WriteLine("\nPresione cualquier tecla para regresar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Citas Pendientes:");
            bool hayPendientes = false;
            for (int i = 0; i < GestorPacientes.contadorPacientes; i++)
            {
                if (GestorPacientes.estados[i] == 'P')
                {
                    Console.Write($"[{i}] {GestorPacientes.codigosCitas[i]} - {GestorPacientes.nombresPacientes[i]} ");
                    if (GestorPacientes.triages[i] == TipoTriage.Rojo)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("(REQUIERE REFERENCIA - TRIAGE ROJO)");
                        Console.ResetColor();
                    }
                    Console.WriteLine($" - Costo Base: ${GestorPacientes.costos[i]}");
                    hayPendientes = true;
                }
            }
            if (!hayPendientes)
            {
                Console.WriteLine("No hay citas en estado Pendiente en este momento.");
                Console.WriteLine("\nPresione cualquier tecla para regresar...");
                Console.ReadKey();
                return;
            }
            Console.Write("\nIngrese el número de índice (ej. [0], [1], [2]) del paciente a gestionar: ");
            if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 0 && indice < GestorPacientes.contadorPacientes && GestorPacientes.estados[indice] == 'P')
            {
                Console.WriteLine("\n¿Qué acción desea realizar?");
                Console.WriteLine("1. Procesar Pago (Atender Cita)");
                Console.WriteLine("2. Cancelar Cita / Referir Paciente"); 
                Console.Write("Seleccione una opción: ");
                string? accion = Console.ReadLine();
                switch (accion)//este switch es para elegir entre procesar el pago o cancelar la cita
                {   
                    case "1"://si se elige procesar el pago, se muestra un resumen de cobro con los descuentos aplicados y se pregunta si se confirma el pago para marcar la cita como atendida.
                    if (GestorPacientes.triages[indice] == TipoTriage.Rojo)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nADVERTENCIA: Este paciente tiene Triage Rojo. Se recomendaba referirlo (Opción 2).");
                        Console.ResetColor();
                    }
                    double costoBase = GestorPacientes.costos[indice];
                    double descuentoEdad = 0;
                    double descuentoSeguro = 0;
                    if (GestorPacientes.edades[indice] > 60)
                    {
                        descuentoEdad = costoBase * 0.10;
                    }
                    Console.WriteLine("\nFormas de Pago:"); //Esto es para mostrar las formas de pago que se pueden elegir
                    Console.WriteLine("1. Efectivo");
                    Console.WriteLine("2. Tarjeta");
                    Console.WriteLine("3. Seguro Médico");
                    Console.Write("Seleccione la forma de pago (1-3): ");
                    int formaPago;
                    while (!int.TryParse(Console.ReadLine(), out formaPago) || formaPago < 1 || formaPago > 3)
                    {
                        Console.Write("Opción inválida. Ingrese 1, 2 o 3: ");
                    }
                    MainMenu.formasPago[indice] = formaPago;
                    if (formaPago == 3)
                    {
                        descuentoSeguro = costoBase * 0.15; //operacion del descuento del seguro medico que es del 15% del costo base
                    }
                    double totalDescuento = descuentoEdad + descuentoSeguro;
                    double totalAPagar = costoBase - totalDescuento;
                    Console.WriteLine("\n=========================================");
                    Console.WriteLine("                RESUMEN DE COBRO            ");//Esta es la factura que se muestra en la consola.
                    Console.WriteLine("============================================");
                    Console.WriteLine($"Paciente: {GestorPacientes.nombresPacientes[indice]}");
                    Console.WriteLine($"Costo Base:         ${costoBase:F2}");
                    if (descuentoEdad > 0) Console.WriteLine($"Desc. Edad (>60):  -${descuentoEdad:F2}");
                    if (descuentoSeguro > 0) Console.WriteLine($"Desc. Seguro (15%):-${descuentoSeguro:F2}");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"TOTAL A PAGAR:      ${totalAPagar:F2}");
                    Console.ResetColor();
                    Console.Write("\n¿Confirmar pago y marcar cita como Atendida (A)? (S/N): ");
                    if (Console.ReadLine()?.Trim().ToUpper() == "S")
                    {
                        GestorPacientes.descuentosTotales[indice] = totalDescuento;//los descuentos se aplican despues de confirmar el pago, para que se pueda mostrar el resumen de cobro sin afectar el costo base hasta que se confirme el pago.
                        GestorPacientes.costos[indice] = totalAPagar; //el total a pagar se guarda en el arreglo de costos después de confirmar el pago, para que se pueda mostrar el resumen de cobro sin afectar el costo base hasta que se confirme el pago.
                        GestorPacientes.estados[indice] = 'A'; //se marca la cita como atendida después de confirmar el pago, para que se pueda mostrar el resumen de cobro sin afectar el estado de la cita hasta que se confirme el pago.
                        GestorArchivos.GuardarDatos(); // busca y guarda los datos en el archivo
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n¡Pago registrado! La cita ha sido marcada como Atendida.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("\nOperación cancelada. La cita sigue Pendiente.");
                    }
                        break;
                    case "2"://si se elige cancelar la cita o referir al paciente, se marca la cita como cancelada/referida y se eliminan los cobros asociados a esa cita.
                    GestorPacientes.estados[indice] = 'C'; 
                    GestorPacientes.costos[indice] = 0.0;  
                    GestorArchivos.GuardarDatos(); //guarda los datos en un Txt que esta dentro de la carpeta temporal bin
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nCita Cancelada/Referida exitosamente. No se generarán cobros.");
                    Console.ResetColor();
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida.");
                        Console.WriteLine("\nPresione cualquier tecla para regresar...");
                        Console.ReadKey();
                        return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nÍndice incorrecto o la cita no está en estado Pendiente.");
                Console.ResetColor();
            }
            Console.WriteLine("\nPresione cualquier tecla para regresar al menú de Reportes...");
            Console.ReadKey();
        }
        public static void GananciasPorEspecialidad()//esta función se encarga de calcular y mostrar las ganancias totales obtenidas por la clínica, sumando los cobros registrados en el arreglo de cobros, agrupados por especialidad y forma de pago.
        {
            Console.Clear();
            Console.WriteLine("=========================================================================");
            Console.WriteLine("=== GANANCIAS POR ESPECIALIDAD ===");
            Console.WriteLine("=========================================================================");
            if (GestorPacientes.contadorPacientes == 0)//Si no hay pacientes registrados, se muestra un mensaje indicando que no hay pacientes registrados y se retorna de la función.
            {
                Console.WriteLine("Aun noo hay pacientes registrados en el sistema.");
                Console.WriteLine("\nPresione cualquier tecla para regresar...");
                Console.ReadKey();
                return;
            }
            string[] especialidades = new string[GestorPacientes.contadorPacientes];//Se crea un arreglo para almacenar los nombres de las especialidades de las citas atendidas registradas.
            string[] formasPago = new string[GestorPacientes.contadorPacientes];//Se crea un arreglo para almacenar las formas de pago de las citas atendidas registradas.
            double[] gananciasPorEspecialidad = new double[GestorPacientes.contadorPacientes];//Se crea un arreglo para almacenar las ganancias totales por cada especialidad de las citas atendidas registradas.
            double[] gananciasPorFormaPago = new double[GestorPacientes.contadorPacientes];//Se crea un arreglo para almacenar las ganancias totales por cada forma de pago de las citas atendidas registradas.
            int contadorEspecialidades = 0;//Se inicializa un contador para llevar la cuenta de cuántas especialidades diferentes han sido registradas en las citas atendidas.
            int ContadorFormaPago = 0;//Se inicializa un contador para llevar la cuenta de cuántas formas de pago diferentes han sido registradas en las citas atendidas.
            for (int i = 0; i < GestorPacientes.contadorPacientes; i++)//Se recorre el arreglo de pacientes registrados para identificar a los pacientes que han sido atendidos (estado 'A') y que tienen cobros registrados, y se acumulan las ganancias por especialidad y forma de pago en los arreglos correspondientes.
            {
                if (GestorPacientes.estados[i] == 'A')//Se verifica si el paciente ha sido atendido (estado 'A').
                {
                    string nombreEspecialidad = TipoTriages.ObtenerNombreEspecialidad(GestorPacientes.especialidades[i]);//Se obtiene el nombre de la especialidad para la cita atendida utilizando el código de especialidad registrado para el paciente.
                    int indiceExistente = -1;//Se inicializa una variable para verificar si la especialidad ya ha sido registrada previamente en el arreglo de especialidades.
                    for (int j = 0; j < contadorEspecialidades; j++)//Se recorre el arreglo de especialidades para verificar si el nombre de la especialidad ya ha sido registrado.
                    {
                        if (especialidades[j] == nombreEspecialidad)//Si el nombre de la especialidad ya existe en el arreglo de especialidades, se establece indiceExistente como el índice correspondiente y se rompe el ciclo para evitar agregarlo nuevamente.
                        {
                            indiceExistente = j;
                            break;
                        }
                    }
                    if (indiceExistente != -1)//Si la especialidad ya ha sido registrada previamente en el arreglo de especialidades, se suma el cobro de la cita atendida a las ganancias acumuladas para esa especialidad en el arreglo de ganancias por especialidad.
                    {
                        gananciasPorEspecialidad[indiceExistente] += GestorPacientes.costos[i];
                    }
                    else//Si la especialidad no ha sido registrada previamente en el arreglo de especialidades, se agrega su nombre al arreglo de especialidades y se asigna el cobro de la cita atendida a las ganancias para esa nueva especialidad en el arreglo de ganancias por especialidad, y se incrementa el contador de especialidades.
                    {
                        especialidades[contadorEspecialidades] = nombreEspecialidad;
                        gananciasPorEspecialidad[contadorEspecialidades] = GestorPacientes.costos[i];
                        contadorEspecialidades++;
                    }
                    string TipoPago = TipoTriages.ObtenerFormaPago(MainMenu.formasPago[i]);//Se obtiene el nombre de la forma de pago para la cita atendida utilizando el código de forma de pago registrado para el paciente.
                    int tipoPagoExistente = -1;//Se inicializa una variable para verificar si la forma de pago ya ha sido registrada previamente en el arreglo de formas de pago.
                    for (int k = 0; k < ContadorFormaPago; k++)//Se recorre el arreglo de formas de pago para verificar si el nombre de la forma de pago ya ha sido registrado.
                    {
                        if (formasPago[k] == TipoPago)//Si el nombre de la forma de pago ya existe en el arreglo de formas de pago, se establece tipoPagoExistente como el índice correspondiente y se rompe el ciclo para evitar agregarlo nuevamente.
                        {
                            tipoPagoExistente = k;
                            break;
                        }
                    }
                    if (tipoPagoExistente != -1)//Si la forma de pago ya ha sido registrada previamente en el arreglo de formas de pago, se suma el cobro de la cita atendida a las ganancias acumuladas para esa forma de pago en el arreglo de ganancias por forma de pago.
                    {
                        gananciasPorFormaPago[tipoPagoExistente] += GestorPacientes.costos[i];
                    }
                    else//Si la forma de pago no ha sido registrada previamente en el arreglo de formas de pago, se agrega su nombre al arreglo de formas de pago y se asigna el cobro de la cita atendida a las ganancias para esa nueva forma de pago en el arreglo de ganancias por forma de pago, y se incrementa el contador de formas de pago.
                    {
                        formasPago[ContadorFormaPago] = TipoPago;
                        ContadorFormaPago++;
                    }
                }
            }
            if (contadorEspecialidades == 0)//Si no se han registrado especialidades para las citas atendidas, se muestra un mensaje indicando que no hay citas atendidas para calcular ganancias por especialidad y se retorna de la función.
            {
                Console.WriteLine("No hay citas atendidas para calcular ganancias por especialidad.");
                Console.WriteLine("\nPresione cualquier tecla para regresar...");
                Console.ReadKey();
                return;
            }
            for (int i = 0; i < contadorEspecialidades; i++)//Se recorre el arreglo de especialidades para mostrar las ganancias totales acumuladas para cada especialidad registrada, formateando la salida para una mejor visualización.
            {//Se muestra el nombre de la especialidad junto con las ganancias totales formateadas a dos decimales, utilizando PadRight para alinear los nombres de las especialidades a la izquierda y mejorar la legibilidad del reporte.
                Console.WriteLine($"Especialidad: {especialidades[i].PadRight(30)} - Ganancias Totales: ${gananciasPorEspecialidad[i]:F2}");
            }
            Console.WriteLine("=========================================================================");
            Console.WriteLine("===Ganancias por Forma de Pago===");
            Console.WriteLine("=========================================================================");
            for (int i = 0; i < ContadorFormaPago; i++)//Se recorre el arreglo de formas de pago para mostrar las ganancias totales acumuladas para cada forma de pago registrada, formateando la salida para una mejor visualización.
            {//Se muestra el nombre de la forma de pago junto con las ganancias totales formateadas a dos decimales, utilizando PadRight para alinear los nombres de las formas de pago a la izquierda y mejorar la legibilidad del reporte.
                Console.WriteLine($"Forma de Pago: {formasPago[i].PadRight(30)} - Ganancias Totales: ${gananciasPorFormaPago[i]:F2}");
            }
            Console.WriteLine("=========================================================================");
            double gananciasTotales = 0;//Se inicializa una variable para acumular las ganancias totales obtenidas por la clínica.
            for (int j = 0; j < GestorPacientes.contadorPacientes; j++)//Se recorre el arreglo de pacientes registrados para sumar los cobros correspondientes a cada paciente atendido (estado 'A') y con cobros registrados.
            {
                if (char.ToUpper(GestorPacientes.estados[j]) == 'A' && GestorPacientes.costos[j] != 0)//Se verifica si el paciente ha sido atendido (estado 'A') y tiene un cobro registrado (cobro diferente de 0).
                {
                    gananciasTotales += GestorPacientes.costos[j];//Si el paciente cumple con las condiciones anteriores, se suma su cobro al total de ganancias.
                }
            }
            Console.WriteLine($"Las ganancias totales de la clínica son: ${gananciasTotales}");//Se muestra el total de ganancias obtenidas por la clínica.
            Console.WriteLine("=========================================================================");
            Console.WriteLine("Presione cualquier tecla para volver al menú de Reportes...");
            Console.ReadKey();
        }
    }
}