using System;
namespace ClinicaMedicaDeEspecialidades
{
    public static class GestorPacientes //esta es una clase que gestiona la info del paciente.
    {
        public static string[] nombresPacientes = new string[100]; //Este arreglo y los demas tiene una capacidad de guardar 100 pacientes.
        public static string[] codigosCitas = new string[100];
        public static string[] fechasCitas = new string[100];
        public static int[] edades = new int[100];
        public static int[] especialidades = new int[100]; 
        public static double[] costos = new double[100];
        public static char[] estados = new char[100];       
        public static string[] registroDePacientes = new string[100];
        public static TipoTriage[] triages = new TipoTriage[100];
        public static double[] descuentosTotales = new double[100];
        public static string[] medicosAtencion = new string[100]; 
        public static int contadorPacientes = 0;
        public static void CapturarDatosPaciente()
        {
            Console.Clear(); //limpia la consola para que se vea mas ordenada.
            Console.WriteLine("--- REGISTRO DE NUEVO PACIENTE ---");
            if (contadorPacientes >= 100) //Si ya guarde 100 pacientes me va ha a avisar que ya no se pueden gusrdar mas.
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Se ha alcanzado el límite máximo de 100 pacientes en el sistema.");
                Console.ResetColor();
                return;
            }
            string nombres = "";                                        //a partir de aqui inicia el imput de los pacientes.
            while (string.IsNullOrWhiteSpace(nombres))                  //nombres.
            {
                Console.Write("Ingrese nombres (1 o 2 nombres): ");
                nombres = Console.ReadLine()?.Trim() ?? "";
            }
            string apellidos = "";                                       //apellidos.
            while (string.IsNullOrWhiteSpace(apellidos))
            {
                Console.Write("Ingrese apellidos (1 o 2 apellidos): ");
                apellidos = Console.ReadLine()?.Trim() ?? "";
            }
            int edad;                                                   //edad del paciente.
            while (true)
            {
                Console.Write("Ingrese la edad: ");
                if (int.TryParse(Console.ReadLine(), out edad) && edad >= 0 && edad <= 120) break;
                Console.WriteLine("Edad no válida (Debe ser un número entre 0 y 120). Intente de nuevo.");
            }
            TipoTriage triageSeleccionado = TipoTriage.Verde;           //Clasificacion de Triage del paciente.
            bool triageValido = false;
            while (!triageValido)
            {
                Console.WriteLine("\nSeleccione el nivel de Triage:");
                Console.WriteLine("1. Verde (Estable)");
                Console.WriteLine("2. Amarillo (Requiere atención)");
                Console.WriteLine("3. Rojo (Crítico, atención inmediata)");
                Console.Write("Opción: ");
                string? opcTriage = Console.ReadLine();
                switch (opcTriage)                                     // ejecuta el bloque segun la opcion que elija el usuario para el triage.
                {
                    case "1": triageSeleccionado = TipoTriage.Verde; triageValido = true; break;
                    case "2": triageSeleccionado = TipoTriage.Amarillo; triageValido = true; break;
                    case "3": triageSeleccionado = TipoTriage.Rojo; triageValido = true; break;
                    default: Console.WriteLine("Selección inválida."); break;
                }
            }
            int codigoEspecialidad = 1;                                 // Aqui manda a llamar la funcion para elegir 1 especialidad medica.
            bool especialidadValida = false;
            while (!especialidadValida)
            {
                Console.WriteLine("\nSeleccione la Especialidad Médica:");
                for (int i = 0; i < MainMenu.EspecialidadesValidas.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {MainMenu.EspecialidadesValidas[i]}");
                }
                Console.Write($"Opción (1 al {MainMenu.EspecialidadesValidas.Count}): ");

                if (int.TryParse(Console.ReadLine(), out int opcEsp) && opcEsp >= 1 && opcEsp <= MainMenu.EspecialidadesValidas.Count)
                {
                    codigoEspecialidad = opcEsp;
                    especialidadValida = true;
                }
                else
                {
                    Console.WriteLine("Selección de especialidad inválida.");
                }
            }
            nombresPacientes[contadorPacientes] = $"{nombres} {apellidos}";
            edades[contadorPacientes] = edad;
            triages[contadorPacientes] = triageSeleccionado;
            especialidades[contadorPacientes] = codigoEspecialidad;
            // Asignar médico según la especialidad seleccionada
            string nombreEspecialidad = MainMenu.EspecialidadesValidas[codigoEspecialidad - 1];
            string palabraClave = nombreEspecialidad switch//esto es para asignar el medico segun la especialidad que se elija.
            {//segun la especialidad que se elija, se asigna una palabra clave para buscar al medico que corresponda a esa especialidad.
                "Medicina General" => "General",
                "Pediatría" => "Pediatra",
                "Cardiología" => "Cardióloga",
                "Dermatología" => "Dermatólogo",
                "Oftalmología" => "Oftalmólogo",
                "Ortopedia" => "Ortopedista",
                "Ginecología" => "Ginecóloga",
                "Odontología" => "Odontóloga",
                _ => ""
            };
            if(palabraClave != "")//si se encontro una palabra clave valida, se busca el medico que corresponda a esa especialidad y se asigna al paciente.
            {
                string medicoAsignado = "";//variable para guardar el nombre del medico asignado al paciente.
                int menorCarga = int.MaxValue;//variable para guardar la menor carga de pacientes que tenga un medico de esa especialidad.
                for (int i = 0; i < GestorMedicos.nombresMedicos.Length; i++)//este ciclo busca en la lista de medicos el que corresponda a la especialidad elegida por el paciente.
                {
                    if(GestorMedicos.nombresMedicos[i].Contains(palabraClave))//si el nombre del medico contiene la palabra clave de la especialidad, se cuenta la cantidad de pacientes que tiene asignados ese medico.
                    {
                        int cargaActual = 0;//variable para contar la cantidad de pacientes asignados a ese medico.
                        for (int j = 0; j < contadorPacientes; j++)//este ciclo cuenta la cantidad de pacientes que ya tienen asignados a ese medico.
                        {
                            if (medicosAtencion[j] == GestorMedicos.nombresMedicos[i])//si el medico asignado a ese paciente es el mismo que el medico que se esta evaluando, se incrementa la carga actual de ese medico.
                            {
                            cargaActual++;
                            }
                        }
                        if(cargaActual < menorCarga)//si la carga de ese medico es menor que la menor carga encontrada hasta ahora y el nombre del medico contiene la palabra clave de la especialidad, se asigna ese medico al paciente.
                        {
                        menorCarga = cargaActual;//se actualiza la menor carga encontrada hasta ahora.
                        medicoAsignado = GestorMedicos.nombresMedicos[i];
                        }
                    }
                }
                if (medicoAsignado != "")
                {
                    medicosAtencion[contadorPacientes] = medicoAsignado;
                }
                else
                {
                    medicosAtencion[contadorPacientes] = "Sin Médico Asignado";
                }   
            }
            codigosCitas[contadorPacientes] = $"Pte-{(contadorPacientes + 1):D4}";
            fechasCitas[contadorPacientes] = DateTime.Now.ToString("dd/MM/yyyy");
            estados[contadorPacientes] = 'P'; 
            costos[contadorPacientes] = (codigoEspecialidad == 1) ? 10.0 : 25.0; 
            descuentosTotales[contadorPacientes] = 0.0; 
            registroDePacientes[contadorPacientes] = $"Exp: EXP-{1000 + contadorPacientes} | {nombresPacientes[contadorPacientes]} | Edad: {edad}";
            contadorPacientes++; 
            GestorArchivos.GuardarDatos(); // Para guardar los datos en el txt
            Console.ForegroundColor = ConsoleColor.Green; //esta linea hace que el siguiente string sea color verde.    |
            Console.WriteLine("\n¡Paciente almacenado correctamente en los arreglos estructurales!");                // |
            Console.ResetColor(); //esta linea hace que el texto vuelva a color por defecto despues del string anterior.|
            if (triageSeleccionado == TipoTriage.Rojo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n¡ALERTA CRÍTICA!");
                Console.WriteLine("El estado clínico del paciente supera el nivel de atención de la clínica.");
                Console.WriteLine("Presione cualquier tecla para redirigir a Cancelación/Referencia de Cita...");
                Console.ResetColor();
                Console.ReadKey();
                GestorCobros.ProcesarCobro();
            }
            else
            {
                Console.WriteLine("\nPresione cualquier tecla para regresar al menú.");
                Console.ReadKey();//pausa el codigo para que el usuario pueda leer el mensaje antes de regresar al menu principal.
            }
        }
    }
}