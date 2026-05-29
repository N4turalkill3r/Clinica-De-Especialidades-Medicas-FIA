namespace ClinicaMedicaDeEspecialidades
{
    public enum TipoTriage //variable de tipo enumerada para el triage del paciente, dependiendo de su estado de salud.
    {
        Verde,
        Amarillo,
        Rojo
    }

    public static class TipoTriages
    {
        public static string ObtenerNombreEspecialidad(int codigo)//esta función se encarga de obtener el nombre de la especialidad médica correspondiente al código ingresado por el usuario, utilizando el arreglo de especialidades válidas definido en la clase MainMenu.
        {
            int indice = codigo - 1;//Se calcula el índice restando 1 al código ingresado, ya que los códigos de especialidad comienzan desde 1, mientras que los índices de los arreglos comienzan desde 0.
            return (indice >= 0 && indice < MainMenu.EspecialidadesValidas.Count)//Se verifica si el índice calculado es válido (mayor o igual a 0 y menor que la cantidad de especialidades válidas). Si es válido, se retorna el nombre de la especialidad correspondiente al índice; de lo contrario, se retorna "Desconocida".
                ? MainMenu.EspecialidadesValidas[indice]
                : "Desconocida";
        }
        public static string ObtenerFormaPago(int codigo)//esta función se encarga de obtener el nombre de la forma de pago correspondiente al código ingresado por el usuario, utilizando una estructura switch para mapear los códigos a las formas de pago definidas.
        {
            return codigo switch//Se utiliza una expresión switch para retornar el nombre de la forma de pago correspondiente al código ingresado. Si el código no coincide con ninguno de los casos definidos, se retorna "Desconocida".
            {
                1 => "Efectivo",
                2 => "Tarjeta",
                3 => "Seguro Médico",
                _ => "Desconocida"
            };
        }
    }
}
