using System;
using System.Diagnostics.CodeAnalysis;

namespace l10
{
    [ExcludeFromCodeCoverage]
    public class Game : IInit, IComparable, ICloneable
    {
        public class IdNumber
        {
            private static int _instancesCreated = 0;
            private int number;

            public int Number
            {
                get { return number; }
                private set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Id не может быть меньше 0");
                    }
                    else
                    {
                        number = value;
                    }
                }
            }

            public IdNumber(int number)
            {
                this.Number = number;
            }

            public IdNumber()
            {
                this.Number = ++_instancesCreated;
            }

            public override string ToString()
            {
                return Number.ToString();
            }

            public override bool Equals(object? obj)
            {
                if (obj is IdNumber n) return this.Number == n.Number;
                return false;
            }
        }

        public IdNumber Id { get; private set; }
        protected static string[] gameNamesOptions = new string[] {"Игра 1", "Игра 2", "Игра 3", "Игра 4"};
        protected const int MaximumPlayersInRandomInit = 100;

        public string Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Name));
                }
                else
                {
                    _name = value;
                }
            }
        }

        private string _name { get; set; }

        private uint _minimumPlayers;
        private uint _maximumPlayers;

        public uint MinimumPlayers
        {
            get => _minimumPlayers;
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("В игре не может быть 0 игроков. Иначе какой в ней смысл?");
                }
                else
                {
                    _minimumPlayers = value;
                }
            }
        }

        public uint MaximumPlayers
        {
            get => _maximumPlayers;
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("В игре не может быть 0 игроков. Иначе какой в ней смысл?");
                }
                else if (value < MinimumPlayers)
                {
                    throw new ArgumentException("Минимальное количество игроков не может быть больше максимального");
                }
                else
                {
                    _maximumPlayers = value;
                }
            }
        }

        //не ставлю значение 0 свойствам кол-ва игроков, потому что это логическая ошибка, игр на 0 игроков не бывает.
        // поэтому пустой конструктор создает ноу-нейм игру на 1 человека. 
        // например это может быть игра с ботами в компьютерной игре или для настолок - кубик-рубик.
        public Game()
        {
            Name = string.Empty;
            MinimumPlayers = 1;
            MaximumPlayers = 1;
            Id = new IdNumber(1);
        }

        public Game(string name, uint minimumPlayers, uint maximumPlayers, IdNumber id)
        {
            Name = name;
            MinimumPlayers = minimumPlayers;
            MaximumPlayers = maximumPlayers;
            this.Id = id;
        }

        public Game(Game game)
        {
            if (game == null)
            {
                throw new ArgumentException("Клонирование null!");
            }

            Name = game.Name;
            MinimumPlayers = game.MinimumPlayers;
            MaximumPlayers = game.MaximumPlayers;
            Id = new IdNumber(game.Id.Number);
        }

        public virtual string GetInfoAsString()
        {
            return
                $"Id игры = {Id}; Название игры = {Name}; Минимальное количество игроков = {MinimumPlayers}; Максимальное количество игроков = {MaximumPlayers}; ";
        }

        public void Show()
        {
            Console.WriteLine(
                $"Id игры = {Id}; Название игры = {Name}; Минимальное количество игроков = {MinimumPlayers}; Максимальное количество игроков = {MaximumPlayers}");
        }

        public virtual void ShowVirtual()
        {
            Show();
        }

        [ExcludeFromCodeCoverage]
        public virtual void Init()
        {
            int idNumber = (int) Helpers.Helpers.EnterUInt("Введите id игры", 1, int.MaxValue);
            Id = new IdNumber(idNumber);

            Console.WriteLine("Введите название игры");
            while (true)
            {
                Name = Console.ReadLine() ?? String.Empty;
                if (Name.Length == 0)
                {
                    Console.WriteLine("Название игры не может быть пустым. Введите его заново:");
                }
                else
                {
                    break;
                }
            }

            MinimumPlayers = Helpers.Helpers.EnterUInt("Введите минимальное количество игроков", 1);
            MaximumPlayers = Helpers.Helpers.EnterUInt("Введите максимальное количество игроков", MinimumPlayers);
        }

        public virtual void RandomInit()
        {
            Random rnd = Helpers.Helpers.GetOrCreateRandom();
            Id = new IdNumber();
            Name = "Игра " + Id.Number;
            MinimumPlayers = (uint) rnd.Next(1, MaximumPlayersInRandomInit);
            MaximumPlayers = (uint) rnd.Next((int) MinimumPlayers, MaximumPlayersInRandomInit);
        }

        public override bool Equals(object? obj)
        {
            return EqualsVirtual(obj) && (obj as Game).EqualsVirtual(this);
        }

        protected virtual bool EqualsVirtual(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Game) return false;

            Game game2compare = (Game) obj;
            return game2compare.Name == Name
                   && game2compare.Id.Number == Id.Number
                   && game2compare.MinimumPlayers == MinimumPlayers
                   && game2compare.MaximumPlayers == MaximumPlayers;
        }

        public override string ToString()
        {
            return GetInfoAsString();
        }

        public virtual int CompareTo(object? obj) // метод сравнение объектов (IComparable)
        {
            if (obj == null) return -1;
            if (obj is not Game) return -1;
            Game g = obj as Game;

            if (g.Name == null)
            {
                return -1;
            }

            if (Name == null)
            {
                return 1;
            }

            int nameComparison = this.Name.CompareTo(g.Name);
            if (nameComparison != 0) return nameComparison;

            if (MinimumPlayers != g.MinimumPlayers)
            {
                return MinimumPlayers.CompareTo(g.MinimumPlayers);
            }

            if (MaximumPlayers != g.MaximumPlayers)
            {
                return MaximumPlayers.CompareTo(g.MaximumPlayers);
            }

            if (Id.Number != g.Id.Number)
            {
                return Id.Number.CompareTo(g.Id.Number);
            }

            return Equals(obj) ? 0 : -1;
        }

        public virtual object Clone() // метод клонирования объектов (ICloneable)
        {
            return new Game(Name, MinimumPlayers, MaximumPlayers, new IdNumber(Id.Number));
        }

        public object ShallowCopy() // метод копирования объектов (ICloneable)
        {
            return this.MemberwiseClone();
        }

        public override int GetHashCode()
        {
            return Id.Number;
        }
    }
}