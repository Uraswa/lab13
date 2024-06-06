using System.Diagnostics.CodeAnalysis;
using l10;
using LAB12_4.AvlTreeNET;
using Laba10;

namespace LAB12_4;


[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main()
    {
        AvlTreeNet<Game> avlTree = new AvlTreeNet<Game>();
        while (true)
        {
            PrintMenu();
            var action = Helpers.Helpers.EnterUInt("пункт меню", 1, 11);

            switch (action)
            {
                case 1:
                    avlTree = new AvlTreeNet<Game>();
                    Console.WriteLine("Пустое дерево успешно создано");
                    break;
                case 2:
                    uint size = Helpers.Helpers.EnterUInt("размер дерева", 0, 65536);
                    avlTree = new AvlTreeNet<Game>((int) size);
                    Console.WriteLine("Дерево успешно создано");
                    break;
                case 3:
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }
                    avlTree = new AvlTreeNet<Game>(avlTree);
                    Console.WriteLine("Новое дерево создано");
                    break;
                case 4:
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
                case 5:
                    var element = GetUserGame();
                    avlTree.Add(element);
                    break;
                case 6:
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
                case 7:
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }

                    uint elementsCount = Helpers.Helpers.EnterUInt("длину массива", (uint)avlTree.Count, int.MaxValue);
                    int startIndex = (int)Helpers.Helpers.EnterUInt("индекс массива для начала вставки", 0,
                        (uint) (elementsCount - avlTree.Count));
                    
                    var games = new Game[elementsCount];
                    avlTree.CopyTo(games, startIndex);
                    for (int j = 0; j < games.Length; j++)
                    {
                        Console.WriteLine($"Элемент #{j + 1}");
                        if (games[j] == null) Console.WriteLine("Пусто");
                        else games[j].ShowVirtual();
                    }
                    
                    break;
                case 8:
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }
                    Console.WriteLine("Количество элементов в дереве = " + avlTree.Count);
                    break;
                case 9:
                    if (avlTree.Count == 0)
                    {
                        Console.WriteLine("Дерево пустое");
                        break;
                    }
                    avlTree.Clear();
                    Console.WriteLine("Дерево очищено");
                    break;
                case 10:
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
                case 11:
                    return;
            }
            
            Console.WriteLine("Нажмите для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
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

    private static void PrintMenu()
    {
        Console.WriteLine("1. Создать пустое авл дерево");
        Console.WriteLine("2. Инициализация авл дерева с помощью ДСЧ");
        Console.WriteLine("3. Создать новое дерево на основе старого");
        Console.WriteLine("4. Вывести элементы авл дерева");
        Console.WriteLine("5. Добавить элемент в дерево");
        Console.WriteLine("6. Проверить содержит ли дерево элемент");
        Console.WriteLine("7. Копировать дерево в массив");
        Console.WriteLine("8. Вывести количество элементов в дереве");
        Console.WriteLine("9. Очистить дерево");
        Console.WriteLine("10. Удалить элемент из дерева");
        Console.WriteLine("11. Выйти");
    }
}