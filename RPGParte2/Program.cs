using System;
using System.Threading;

namespace RPGParte2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Primero creamos los objetos que van a existir en nuestro juego
            Heroe heroe = new Heroe("Bender", 100, 5, 5, 15);
            Villano villano = new Villano("Dr. Zoiberg", 100, 5, 4);

            // Empieza el juego, preparamos todo lo necesario
            int turno = 0;
            int revisaCarga = 0;
            heroe.Saludar();
            villano.Saludar();

            // Empieza el bucle de juego
            // Podemos llevar la cuenta de informacion de la partida, como los turnos
            while (heroe.puntosVida > 0 && villano.puntosVida > 0)
            {
                // Actualizamos informacion
                turno = turno + 1;
                Console.WriteLine("Es el turno " + turno);
                // Pedimos Input
                // Mostramos opciones al jugador, esto seria procesar el Input del juego
                // Se realiza y despues se realiza la del villano
                Console.WriteLine("¿Que accion quieres realizar?");
                Console.WriteLine("1. Atacar");
                Console.WriteLine("2. Defender");
                Console.WriteLine("3. Descarga");
                Console.WriteLine("4. Ionizador");
                try
                {
                    //Añadimos un if para que identifique si esta aturdido o no
                    if (heroe.aturdido == true)
                    {
                        //En caso de que si que este aturdido se saltara el turno del heroe.
                        Console.WriteLine(villano.nombre + " te ha lanzado un saco de tinta y no puedes hacer nada\n");
                        heroe.aturdido = false;
                    }
                    //Aqui se ejecutara el switch en el que ocurrirá el turno del heroe con normalidad
                    else
                    {
                        // Leemos la consola
                        string input = Console.ReadLine();
                        int eleccion = Convert.ToInt32(input);
                        // Procesamos la eleccion
                        switch (eleccion)
                        {
                            case 1:
                                heroe.Atacar(villano);
                                break;
                            case 2:
                                heroe.Defender();
                                break;
                            case 3:
                                heroe.AtaqueEspecial(villano);
                                break;
                            case 4:
                                heroe.Ionizador(villano);
                                break;
                        }
                    }
                    // El villano realiza su accion
                    int accionVillano = new Random().Next(1, 5); // Devuelve un numero entre [1,4]
                    switch (accionVillano)
                    {
                        case 1:

                            //Aqui ponemos que solo ataque cada dos turnos, uno de carga y el segundo de ataque

                            if (revisaCarga == 1)
                            {
                                villano.ataqueCargado = true;
                                villano.AtaqueCargado(heroe);
                                revisaCarga = 0;
                            }
                            else
                            {
                                Console.WriteLine(villano.nombre + " esta afilando su bisturi\n");
                                revisaCarga = revisaCarga + 1;
                            }

                            break;
                        case 2:
                            villano.Defender();
                            break;
                        case 3:
                            villano.SacoDeTinta(heroe);
                            break;
                        case 4:
                            villano.Atacar(heroe);
                            break;
                    }
                    // Mostramos el estado de los personajes
                    // Esto representa parte del feedback del juego
                    Console.WriteLine( heroe.nombre + ": " + heroe.puntosVida + " PV");
                    Console.WriteLine( villano.nombre + ": " + villano.puntosVida + " PV\n");
                }
                catch (Exception e)
                {
                    //Esta parte se ejecuta en caso de que el usuario no introduzca bien el valor.
                    Console.WriteLine(heroe.nombre + " no ha escogido ninguna opcion\n");
                    Thread.Sleep(2000);
                    Console.WriteLine(villano.nombre + " aprovecha la ocasion\n");
                    Thread.Sleep(2000);
                    villano.Atacar(heroe);
                }
            }
            // Al final mostramos el ganador de la batalla
            if (villano.puntosVida <= 0)
            {
                // El ganador es el heroe
                Console.WriteLine("El ganador es " + heroe.nombre);
            }
            else if (heroe.puntosVida <= 0)
            {
                // Sino el ganador es el villano
                Console.WriteLine("El ganador es " + villano.nombre);
            }
            // Aqui acaba la ejecucion
            // En Unity se destruyen los objetos y cargariamos otra escena o cerramos el juego
        }

    }

    public class Personaje
    {
        public string nombre;
        public int puntosVida;
        public int puntosAtaque;
        public int puntosDefensa;
        public bool defendiendo;
        public bool aturdido = false;

        public Personaje(string nombre, int puntosVida, int puntosAtaque, int puntosDefensa)
        {
            this.nombre = nombre;
            this.puntosVida = puntosVida;
            this.puntosAtaque = puntosAtaque;
            this.puntosDefensa = puntosDefensa;
            defendiendo = false; // Todos los personajes empiezan sin defender
        }

        public void Saludar()
        {
            // Se realiza el saludo por consola
            Console.WriteLine("Hola, yo soy " + this.nombre);
            Thread.Sleep(2000);
        }

        public void Atacar(Personaje oponente)
        {
            //Escribe en pantalla el ataque y llama al oponente para que reciba la funcion de RecibirAtaque
            Console.WriteLine(nombre + " ataca con fuerza\n");
            oponente.RecibirAtaque(puntosAtaque);
        }

        public void RecibirAtaque(int puntosAtaque)
        {
            //Declaramos las variables para guardar la vida restante, la probabiliad de critico y el valor añadido del golpe critico
            int vidaRestante;
            int probCritico = new Random().Next(0, 5);
            int golpeCritico;

            //Cuando la el valor sea 1 ejecutara el ataque critico, el valor puede ser entre 1 y 4
            if (probCritico == 1)
            {
                //En el caso de que se produzca el critico se sumara 5 de daño al ataque del heroe
                Console.WriteLine(nombre + " ha recibido un critico ");
                golpeCritico = 5;
            }
            else
            {
                //En caso de que no sea critico se pone el valor del daño extra en 0
                golpeCritico = 0;
            }

            if (defendiendo == true)
            {
                //En caso de que se este defendiendo se le restara al daño del atacante la defensa del atacado
                vidaRestante = puntosVida - puntosAtaque + puntosDefensa - golpeCritico;
                puntosVida = vidaRestante;
                defendiendo = false;
            }
            else
            {
                //En caso de que no se defienda el daño recibido será el valor de los puntos de ataque y el del critico.
                vidaRestante = puntosVida - puntosAtaque - golpeCritico;
                puntosVida = vidaRestante;
                Console.WriteLine("A " + nombre + " le quedan " + puntosVida + "PV\n");
                Thread.Sleep(2000);
            }

        }

        public void Defender()
        {
            //Si eligen la opción de defender, se cambia el booleano a true para el if de recibir ataque
            defendiendo = true;
            Console.WriteLine(nombre + " ha elegido usar a Fry de escudo\n" );
            Thread.Sleep(2000);
        }
    }

    public class Heroe : Personaje
    {
        public int puntosEnergia; // Guarda cuantos puntos de enegia tiene

        public Heroe(string nombre, int puntosVida, int puntosAtaque, int puntosDefensa, int puntosEnergia) : base(nombre, puntosVida, puntosAtaque, puntosDefensa)
        {
            this.puntosEnergia = puntosEnergia; // Hay que asignar los puntos de energia
        }

        public void AtaqueEspecial(Personaje oponente)
        {
            //Declaramos la energia restante, cada vez que se ejecute el ataque especial, restara energia
            Console.WriteLine(nombre + " ha lanzado una descarga\n");
            Thread.Sleep(2000);
            int energiaRestante = puntosEnergia - 5;
            puntosEnergia = energiaRestante;

            //En caso de que le quede suficiente energia, realizara el ataque
            if (puntosEnergia >= 5)
            {
                int ataqueEnergia = puntosAtaque * 5;
                oponente.RecibirAtaque(ataqueEnergia);
            }
            else
            {
                //Este será el caso en el que no le quede más energia
                Console.WriteLine("No te quedan puntos de energia\n");
                Thread.Sleep(2000);
                Console.WriteLine("Pierdes el turno\n");
                Thread.Sleep(2000);
            }
            
        }
        public void Ionizador(Personaje oponente)
        {
            //Aqui elegiremos si sale el caso del ataque de iones fuerte o debil
            int randomIones = new Random().Next(1, 3);
            switch (randomIones)
            {
                //Aqui realiza un ataque en el que suma la defensa del villano al ataque base
                case 1:
                    Console.WriteLine(nombre + " usa la defensa de " + oponente.nombre +" en su contra\n");
                    Thread.Sleep(2000);
                    int ataqueIones = puntosAtaque + oponente.puntosDefensa;
                    oponente.RecibirAtaque(ataqueIones);

                    break;
                //En este caso es la opcion en la que falla y hace la mitad de daño
                case 2:
                    Console.WriteLine(nombre + " lanza un ataque de iones, pero falla\n");
                    Thread.Sleep(2000);
                    int ataqueIonesNormal = puntosAtaque/2;
                    oponente.RecibirAtaque(ataqueIonesNormal);
                    break;
            }
        }
    }

    public class Villano : Personaje
    {
        public bool ataqueCargado; // Guarda si el ataque esta cargado

        public Villano(string nombre, int puntosVida, int puntosAtaque, int puntosDefensa) : base(nombre, puntosVida, puntosAtaque, puntosDefensa)
        {
            ataqueCargado = false; // El villano empieza el ataque sin cargar
        }

        public void AtaqueCargado(Personaje oponente)
        {
            //Aqui tenemos el ataque cargado que multiplica por 3 el daño base del villano, pero tendrá que esperar 1 turno entre cada ataque para volver a realizarlo
            Console.WriteLine(nombre + " ha lanzado un golpe de bisturi\n");
            Thread.Sleep(2000);
            int ataqueCarga = puntosAtaque * 3;
            oponente.RecibirAtaque(ataqueCarga);
        }
        public void SacoDeTinta(Personaje oponente)
        {
            //Aqui pondra el booleano de aturdido en true
            oponente.aturdido = true;
            Thread.Sleep(2000);
            
        }
    }
}
