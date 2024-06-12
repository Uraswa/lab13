using System.Diagnostics.CodeAnalysis;
using l10;
using Laba10;

namespace Lab13;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static MyObservableCollection<Game> _collection1;
    private static MyObservableCollection<Game> _collection2;

    public static void Main()
    {
        _collection1 = new MyObservableCollection<Game>("Collection 1", 0);
        _collection2 = new MyObservableCollection<Game>("Collection 2", 0);

        var journal1 = new Journal();
        var journal2 = new Journal();

        _collection1.CollectionCountChanged += journal1.AddHistoryItem;
        _collection1.CollectionReferenceChanged += journal1.AddHistoryItem;

        _collection1.CollectionReferenceChanged += journal2.AddHistoryItem;
        _collection2.CollectionReferenceChanged += journal2.AddHistoryItem;

        while (true)
        {
            PrintMenu();
            var action = Helpers.Helpers.EnterUInt("пункт меню", 1, 10);

            MyObservableCollection<Game> avlTree = null;
            switch (action)
            {
                case 1: // вывести дерево
                    avlTree = GetCollection();
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }

                    int i = 0;
                    foreach (var game in avlTree)
                    {
                        Console.WriteLine($"Элемент #{i + 1}");
                        game.ShowVirtual();
                        i++;
                    }

                    break;
                case 2: // добавить элемент в дерево
                    avlTree = GetCollection();
                    var element = GetUserGame();
                    
                    if (avlTree.Contains(element))
                    {
                        Console.WriteLine("Элемент уже существует в дереве");
                    }
                    else
                    {
                        avlTree.Add(element);
                    }
                    break;
                case 3: // добавить n случайных элементов в дерево
                    AddNRandomElementsToTree();
                    break;
                case 4: // проверить содержится ли элемент в дереве
                    avlTree = GetCollection();
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }

                    var el2Check = GetUserGame();
                    if (avlTree.Contains(el2Check))
                    {
                        Console.WriteLine("Элемент содержится в дереве");
                    }
                    else
                    {
                        Console.WriteLine("Элемент не содержится в дереве");
                    }

                    break;
                case 5: // вывести количество элементов в дереве
                    avlTree = GetCollection();
                    if (avlTree.Length == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }

                    Console.WriteLine("Количество элементов в дереве = " + avlTree.Length);
                    break;
                case 6: // очистить дерево
                    avlTree = GetCollection();
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }

                    avlTree.Clear();
                    Console.WriteLine("Дерево очищено");
                    break;
                case 7: // удаление элемента из дерева
                    avlTree = GetCollection();
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }

                    var el2Delete = GetUserGame();
                    if (avlTree.Remove(el2Delete))
                    {
                        Console.WriteLine("Элемент удален");
                    }
                    else
                    {
                        Console.WriteLine("Элемент не найден");
                    }

                    break;
                case 8: // изменить элемент по индексу
                    ChangeElementByIndex();
                    break;
                case 9: // вывести журнал
                    Console.WriteLine("Первый журнал:");
                    journal1.PrintChanges();
                    Console.WriteLine();
                    Console.WriteLine("Второй журнал:");
                    journal2.PrintChanges();
                    break;
                case 10:
                    return;
            }

            Console.WriteLine("Нажмите для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static void ChangeElementByIndex()
    {
        var collection = GetCollection();

        if (collection.Count == 0)
        {
            Console.WriteLine("Коллекция пуста");
            return;
        }

        Console.WriteLine("Введите ключевой элемент");
        var key = GetUserGame();
        if (collection.Contains(key))
        {
            Console.WriteLine("Введите новое значение");
            var val = GetUserGame();
            if (collection.Contains(val))
            {
                Console.WriteLine("Ошибка, такой элемент уже присутствует в коллекции!");
                return;
            }
            collection[key] = val;
            Console.WriteLine("Операция произведена успешно");
        }
        else
        {
            Console.WriteLine("Элемент не был найден в коллекции");
        }
    }

    private static void AddNRandomElementsToTree()
    {
        var collection = GetCollection();

        var count = Helpers.Helpers.EnterUInt("количество элементов");
        for (int i = 0; i < count; i++)
        {
            collection.Add(GetRandomGame());
        }
        
        Console.WriteLine("Элементы успешно добавлены!");
        
    }

    private static MyObservableCollection<Game> GetCollection()
    {
        uint answer = Helpers.Helpers.EnterUInt("номер коллекции: 1 - первая, 2 - вторая", 1, 2);
        if (answer == 1)
        {
            return _collection1;
        }

        return _collection2;
    }

    private static Game GetUserGame()
    {
        var gameType = Helpers.Helpers.EnterUInt("тип игры(1: игра, 2: видеоигра, 3: VR игра)", 1, 3);
        Game game = null;
        switch (gameType)
        {
            case 1:
                game = new Game();
                break;
            case 2:
                game = new VideoGame();
                break;
            case 3:
                game = new VRGame();
                break;
        }

        game.Init();
        return game;
    }
    
    private static Game GetRandomGame()
    {
        var rnd = Helpers.Helpers.GetOrCreateRandom();
        
        var gameType = rnd.Next(1, 3);
        Game game = null;
        switch (gameType)
        {
            case 1:
                game = new Game();
                break;
            case 2:
                game = new VideoGame();
                break;
            case 3:
                game = new VRGame();
                break;
        }

        game.RandomInit();
        return game;
    }

    private static void PrintMenu()
    {
        Console.WriteLine("1. Вывести элементы авл дерева");
        Console.WriteLine("2. Добавить элемент в дерево");
        Console.WriteLine("3. Добавить n случайнх элементов в дерево");
        Console.WriteLine("4. Проверить содержит ли дерево элемент");
        Console.WriteLine("5. Вывести количество элементов в дереве");
        Console.WriteLine("6. Очистить дерево");
        Console.WriteLine("7. Удалить элемент из дерева");
        Console.WriteLine("8. Изменить элемент по ключу");
        Console.WriteLine("9. Вывести журналы изменений");
        Console.WriteLine("10. Выйти");
    }
}