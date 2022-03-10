using System;
using System.Threading;

namespace RPGParte2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Primero creamos los objetos que van a existir en nuestro juego
            // En Unity se crean los GameObjects en la escena
            Heroe heroe = new Heroe("Bender", 100, 5, 10, 50);
            Villano villano = new Villano("Dr. Zoiberg", 100, 5, 10);

            // Empieza el juego, preparamos todo lo necesario
            // Esto seria el Start de Unity en una escena
            int turno = 0;
            heroe.Saludar();
            villano.Saludar();

            // Empieza el bucle de juego
            // Podemos llevar la cuenta de informacion de la partida, como los turnos
            // En Unity el bucle esta representado en el evento Update
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
                Console.WriteLine("3. Huir");
                try
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
                    }
                    // El villano realiza su accion
                    int accionVillano = new Random().Next(1, 3); // Devuelve un numero entre [1,3)
                    switch (accionVillano)
                    {
                        case 1:

                            int revisaImpar = turno % 2;
                            if (revisaImpar != 0)
                            {

                                villano.ataqueCargado = true;
                                villano.AtaqueCargado(heroe);
                            }
                            else
                            {
                                villano.Atacar(heroe);
                            }

                            break;
                        case 2:
                            villano.Defender();
                            break;
                    }
                    // Mostramos el estado de los personajes
                    // Esto representa parte del feedback del juego
                    Console.WriteLine("Heroe: " + heroe.puntosVida + " PV");
                    Console.WriteLine("Villano: " + villano.puntosVida + " PV\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("El heroe no ha escogido ninguna opcion\n");
                    Thread.Sleep(2000);
                    Console.WriteLine("El villano aprovecha la ocasion\n");
                    Thread.Sleep(2000);
                    villano.Atacar(heroe);
                }
            }
            // Al final mostramos el ganador de la batalla
            if (villano.puntosVida <= 0)
            {
                // El ganador es el heroe
                Console.WriteLine("El ganador es el heroe");
            }
            else if (heroe.puntosVida <= 0)
            {
                // Sino el ganador es el villano
                Console.WriteLine("El ganador es el villano");
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
        }

        public void Atacar(Personaje oponente)
        {
            Console.WriteLine(nombre + " ataca con fuerza\n");
            oponente.RecibirAtaque(puntosAtaque);
        }

        public void RecibirAtaque(int puntosAtaque)
        {
            int vidaRestante;
            if (defendiendo == true)
            {
                vidaRestante = puntosVida - puntosAtaque + puntosDefensa;
                puntosVida = vidaRestante;
                Console.WriteLine(nombre + " se ha defendido, le quedan " + puntosVida + "PV\n");
                Thread.Sleep(2000);
                defendiendo = false;
            }
            else
            {
                vidaRestante = puntosVida - puntosAtaque;
                puntosVida = vidaRestante;
                Console.WriteLine("A " + nombre + " le quedan " + puntosVida + "PV\n");
                Thread.Sleep(2000);
            }

        }

        public void Defender()
        {
            defendiendo = true;
            Console.WriteLine(nombre + " ha elegido usar a Fry de escudo\n" );
            Thread.Sleep(2000);
        }
    }

    public class Heroe : Personaje
    {
        public int puntosMagia; // Guarda cuantos puntos de magia tiene

        public Heroe(string nombre, int puntosVida, int puntosAtaque, int puntosDefensa, int puntosMagia) : base(nombre, puntosVida, puntosAtaque, puntosDefensa)
        {
            this.puntosMagia = puntosMagia; // Hay que asignar los puntos de magia
        }

        public void AtaqueEspecial(Personaje oponente)
        {
            Console.WriteLine(nombre + " ha lanzado una descarga\n");
            Thread.Sleep(2000);
            int magiaRestante = puntosMagia - 5;
            puntosMagia = magiaRestante;

            int ataqueMagia = puntosAtaque * 2;
            oponente.RecibirAtaque(ataqueMagia);
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
            Console.WriteLine(nombre + " ha lanzado un golpe de bisturi\n");
            Thread.Sleep(2000);
            int ataqueCarga = puntosAtaque * 3;
            oponente.RecibirAtaque(ataqueCarga);
        }
    }
}
