using System;
using System.Collections.Generic;

namespace ChainOfResposibility
{

    //A interface de handlers que declara um metodo para construir
    //a corrente de handlers e também declara um método para executar a resquest
    public interface IHandler
    {
        //Construir corrente de Handlers
        IHandler SetNext(IHandler handler);

        //Executar requests
        object Handle(object request);
    }

    // O comportamneto padrão das correntes podem ser implementados na classe base de handler
    abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public virtual object Handle(object request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;

            //Retornando o handler aqui permite que podemos linkar os handlers
            //Exemplo:monkey.SetNext(squirrel).SetNext(dog);
            return handler;
        }
    }

    class MonkeyHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            if ((request as string) == "Banana")
            {
                return $"Monkey: I'll eat the {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class SquirrelHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "Nut")
            {
                return $"Squirrel: I'll eat the {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class DogHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "MeatBall")
            {
                return $"Dog: I'll eat the {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class HumanHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "Cup of coffee")
            {
                return $"Human: I'll drink the {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }


    class Client
    {
        public static void ClientCode(AbstractHandler handler)
        {
            foreach (var food in new List<string> { "Nut", "Banana", "Cup of coffee" })
            {
                Console.WriteLine($"Client: Who wants a {food}?");

                var result = handler.Handle(food);

                if (result != null)
                {
                    Console.Write($"   {result}");
                }
                else
                {
                    Console.WriteLine($"   {food} was left untouched.");
                }
            }
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            
            var monkey = new MonkeyHandler();
            var squirrel = new SquirrelHandler();
            var dog = new DogHandler();
            var human = new HumanHandler();

            monkey.SetNext(squirrel).SetNext(dog).SetNext(human);


            Console.WriteLine("Chain: Monkey > Squirrel > Dog\n");
            Client.ClientCode(monkey);
            Console.WriteLine();

            Console.WriteLine("Subchain: Squirrel > Dog\n");
            Client.ClientCode(squirrel);
        }
    }
}
