namespace ClinicaMedicaDeEspecialidades
{
    public class Paciente //Aqui genero la clase paciente y sus atributos.
    {
        public string Expediente { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int Edad { get; set; }
        public TipoTriage Triage { get; set; }
        public string Especialidad { get; set; } = string.Empty;
    }
    public static class RegistroPacientes // Lista en memoria para almacenar los pacientes.
    { 
        private static readonly List<Paciente> listaPacientes = new();  //Contador para generar el expediente (EXP-1, EXP-2...)
        private static int contadorExpediente = 1; 
        public static void AgregarPaciente(string nombres, string apellidos, int edad, TipoTriage triage, string especialidad)
        {
            Paciente nuevoPaciente = new()
            {
                Expediente = $"EXP-{contadorExpediente}",
                Nombres = nombres,
                Apellidos = apellidos,
                Edad = edad,
                Triage = triage,
                Especialidad = especialidad
            };
            listaPacientes.Add(nuevoPaciente);
            contadorExpediente++; // Incrementa para el siguiente paciente

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✔️ Paciente registrado con éxito. Expediente asignado: {nuevoPaciente.Expediente}");
            Console.ResetColor();
        }
        public static List<Paciente> ObtenerPacientes()
        {
            return listaPacientes;
        }
    }
}