using System;
namespace ClinicaMedicaDeEspecialidades
{
    public static class GestorMedicos //esta clase se encarga de gestionar la información de los médicos.
    {
        public static string[] nombresMedicos = new string[10]; //arreglo para almacenar los nombres de los médicos disponibles en la clínica.
        public static void InicializarMedicos()
        {
            nombresMedicos[0] = "Dr. Carlos Orellana (General)";
            nombresMedicos[1] = "Dra. Ana Dinora (Ginecóloga)";
            nombresMedicos[2] = "Dr. Sanchez Ceren (General)";
            nombresMedicos[3] = "Dra. Laura Mendez (General)";
            nombresMedicos[4] = "Dr. Shafick Handall (Ortopedista)";
            nombresMedicos[5] = "Dra. Claudia Ortiz (Pediatra)";
            nombresMedicos[6] = "Dra. Maria Rodriguez (Cardióloga)";
            nombresMedicos[7] = "Dr. Nayib bukele (Dermatólogo)";
            nombresMedicos[8] = "Dr. Francisco Alabi (Oftalmólogo)";
            nombresMedicos[9] = "Dra. Gabriela Mendez (Odontóloga)";
        }
        public static void NotaPromedioMedicos()//esta funcion se encarga de calcular y mostrar el promedio de calificaciones por médico, considerando solo a los pacientes que han sido atendidos (estado 'A') y que tienen calificaciones registradas.
        {
            Console.Clear();
            Console.WriteLine("=== PROMEDIO DE CALIFICACIONES POR MÉDICO ===");//Se muestra un encabezado para el reporte de promedio de calificaciones por médico.
            Console.WriteLine("==========================================================");//Se muestra una línea de separación después del encabezado.
            if (GestorPacientes.contadorPacientes == 0)//Si no hay pacientes registrados, se muestra un mensaje indicando que no hay pacientes registrados y se retorna de la función.
            {
                Console.WriteLine("No se han calificado a los médicos.");
                Console.WriteLine("Presione cualquier tecla para volver al menú de Reportes...");
                Console.ReadKey();
                return;
            }
            string[] medico = new string[GestorPacientes.contadorPacientes];//Se crea un arreglo para almacenar los nombres de los médicos que han atendido a los pacientes registrados.
            int contadorMedicos = 0;//Se inicializa un contador para llevar la cuenta de cuántos médicos diferentes han atendido a los pacientes registrados.
            for (int i = 0; i < GestorPacientes.contadorPacientes; i++)//Se recorre el arreglo de pacientes registrados para identificar a los médicos que han atendido a los pacientes con estado 'A' y calificaciones registradas, y se almacenan sus nombres en el arreglo medico sin duplicados.
            {
                if (char.ToUpper(GestorPacientes.estados[i]) == 'A' && MainMenu.calificaciones[i] != 0)//Se verifica si el paciente ha sido atendido (estado 'A') y tiene una calificación registrada (calificación diferente de 0).
             {
                    string nombreMedico = GestorPacientes.medicosAtencion[i];//Se obtiene el nombre del médico que atendió al paciente.
                    bool medicoExistente = false;//Se inicializa una variable booleana para verificar si el médico ya ha sido registrado en el arreglo medico.
                    for (int j = 0; j < contadorMedicos; j++)//Se recorre el arreglo medico para verificar si el nombre del médico ya ha sido registrado.
                    {
                        if (medico[j] == nombreMedico)//Si el nombre del médico ya existe en el arreglo medico, se establece medicoExistente como true y se rompe el ciclo para evitar agregarlo nuevamente.
                        {
                            medicoExistente = true;
                            break;
                        }
                    }
                    if (!medicoExistente)//Si el médico no ha sido registrado previamente en el arreglo medico, se agrega su nombre al arreglo y se incrementa el contador de médicos.
                    {
                        medico[contadorMedicos] = nombreMedico;
                        contadorMedicos++;
                    }
                }
            }
            if (contadorMedicos == 0)//Si no se han registrado médicos que hayan atendido a pacientes con calificaciones, se muestra un mensaje indicando que no hay calificaciones disponibles para los médicos y se retorna de la función.
            {
                Console.WriteLine("No hay calificaciones disponibles para los médicos.");
                Console.WriteLine("Presione cualquier tecla para volver al menú principal...");
                Console.ReadKey();
                return;
            }
            for (int i = 0; i < contadorMedicos; i++)//Se recorre el arreglo medico para calcular y mostrar el promedio de calificaciones para cada médico registrado, considerando solo a los pacientes que han sido atendidos por ese médico y que tienen calificaciones registradas.
            {
                string nombreMedico = medico[i];//Se obtiene el nombre del médico para el cual se calculará el promedio de calificaciones.
                int sumaCalificaciones = 0;//Se inicializa una variable para acumular la suma de las calificaciones de los pacientes atendidos por el médico actual.
                int cantidadCalificaciones = 0;//Se inicializa un contador para llevar la cuenta de cuántas calificaciones se han registrado para el médico actual.
                for (int j = 0; j < GestorPacientes.contadorPacientes; j++)//Se recorre el arreglo de pacientes registrados para identificar a los pacientes que han sido atendidos por el médico actual (nombreMedico) y que tienen calificaciones registradas, y se acumulan sus calificaciones en sumaCalificaciones mientras se cuenta la cantidad de calificaciones en cantidadCalificaciones.
                {
                    if (GestorPacientes.estados[j] == 'A' && MainMenu.calificaciones[j] != 0 && GestorPacientes.medicosAtencion[j] == nombreMedico)//Se verifica si el paciente ha sido atendido (estado 'A'), tiene una calificación registrada (calificación diferente de 0) y fue atendido por el médico actual (nombreMedico).
                    {
                        sumaCalificaciones += MainMenu.calificaciones[j];//Si el paciente cumple con las condiciones anteriores, se suma su calificación a sumaCalificaciones.
                        cantidadCalificaciones++;//Se incrementa el contador de calificaciones para el médico actual.
                    }
                }
                double promedio = (double)sumaCalificaciones / cantidadCalificaciones;//Se calcula el promedio de calificaciones para el médico actual dividiendo la suma de las calificaciones entre la cantidad de calificaciones registradas.
                
                Console.WriteLine($"Médico: {nombreMedico.PadRight(35)} - Promedio de Calificación: {promedio:F2}");//Se muestra el nombre del médico junto con su promedio de calificaciones formateado a dos decimales.
            }
            Console.WriteLine("==========================================================");//Se muestra una línea de separación al final del reporte.
            Console.WriteLine("precione cualquier tecla para volver al menú de Reportes...");
            Console.ReadKey();//Se muestra un mensaje indicando que se debe presionar cualquier tecla para volver al menú principal, y se espera a que el usuario presione una tecla antes de retornar de la función.
        }
    }
}