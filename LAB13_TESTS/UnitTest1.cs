using System.Collections.ObjectModel;
using l10;
using Lab13;

namespace LAB13_TESTS;

public class Tests
{
    [Test]
    public void TestMyObserveCollectionEmpty()
    {
        var tree = new MyObservableCollection<Game>("test", 0);
        Assert.IsTrue(tree.Length == 0);
    }
    
    [Test]
    public void TestMyObserveCollectionOneElement()
    {
        var tree = new MyObservableCollection<Game>("test", 1);
        Assert.IsTrue(tree.Length == 1);
    }
    
    [Test]
    public void TestMyObserveCollectionManyElements()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        Assert.IsTrue(tree.Length == 3);
    }

    [Test]
    public void TestMyObserveCollectionGetNotExistingElement()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        var key = new Game("test", 1, 1, new Game.IdNumber());
        Assert.Catch<ArgumentException>(() =>
        {
            var a = tree[key];
        });
    }
    
    [Test]
    public void TestMyObserveCollectionGetExistingElement()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        var item = tree.GetValues().ToArray()[0];
        
        var gotItem = tree[item];
        Assert.IsTrue(gotItem.Equals(item));
    }

    [Test]
    public void TestMyObserveCollectionSetNotExistingItem()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        var key = new Game("test", 1, 1, new Game.IdNumber());
        var value = new Game("test", 1, 1, new Game.IdNumber());
        Assert.Catch<ArgumentException>(() => tree[key] = value);
    }
    
    [Test]
    public void TestMyObserveCollectionSetNull()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        var key = tree.GetValues().ToArray()[0];
        var value = new Game("test", 1, 1, new Game.IdNumber());
        Assert.Catch<ArgumentException>(() => tree[key] = null);
    }
    
    [Test]
    public void TestMyObserveCollectionSetDuplicate()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        var key = tree.GetValues().ToArray()[0];
        var value = key;
        Assert.Catch<ArgumentException>(() => tree[key] = value);
    }
    
    [Test]
    public void MyObserveCollectionSetFine()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        var key = tree.GetValues().ToArray()[0];
        var value = new Game("rnd", 1, 1, new Game.IdNumber());
        tree[key] = value;
        
        Assert.IsTrue(tree[value].Equals(value));
    }
    
    [Test]
    public void MyObserveCollectionSetFine_CountEvents()
    {
        var tree = new MyObservableCollection<Game>("test", 3);

        int calledTimes = 0;
        tree.CollectionCountChanged += (source, args) =>
        {
            calledTimes++;
        };
        
        var key = tree.GetValues().ToArray()[0];
        var value = new Game("rnd", 1, 1, new Game.IdNumber());
        tree[key] = value;
        
        Assert.IsTrue(tree[value].Equals(value));
        Assert.IsTrue(calledTimes == 2);
    }
    
    [Test]
    public void MyObserveCollectionSetFine_ReferenceEvents()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
       
        var key = tree.GetValues().ToArray()[0];
        var value = new Game("rnd", 1, 1, new Game.IdNumber());
        
        tree.CollectionReferenceChanged += (source, args) =>
        {
            Assert.IsTrue(args.Object.Equals(key));
        };
        
        tree[key] = value;
    }

    [Test]
    public void TestMyObserveCollectionClear()
    {
        var tree = new MyObservableCollection<Game>("test", 3);
        tree.Clear();
        Assert.IsTrue(tree.Count == 0);
    }
    
    [Test]
    public void TestMyObserveCollectionClear_CountEvents()
    {
        var tree = new MyObservableCollection<Game>("test", 3);

        int calledTimes = 0;
        tree.CollectionCountChanged += (source, args) =>
        {
            calledTimes++;
        };
        tree.Clear();
        Assert.IsTrue(calledTimes == 3);
    }
    
    [Test]
    public void TestJournalEmptyConstructor()
    {
        var journal = new Journal();
    }
    
    [Test]
    public void TestJournalAddNull()
    {
        var journal = new Journal();
        Assert.Catch<ArgumentException>(() => journal.AddHistoryItem(null, null));
    }
    
    [Test]
    public void TestJournalAddNotObserveCollection()
    {
        var journal = new Journal();
        Assert.Catch<ArgumentException>(() => journal.AddHistoryItem(new(), null));
    }
    
    [Test]
    public void TestJournalAddArgsNull()
    {
        var journal = new Journal();
        var collection = new MyObservableCollection<Game>("test", 1);
        Assert.Catch<ArgumentException>(() => journal.AddHistoryItem(collection, null));
    }
    
    [Test]
    public void TestJournalAddArgsObjectNull()
    {
        var journal = new Journal();
        var collection = new MyObservableCollection<Game>("test", 1);
        Assert.Catch<ArgumentException>(() => journal.AddHistoryItem(collection, new CollectionHandlerEventArgs("test", null)));
    }
    
    [Test]
    public void TestJournalAddFine()
    {
        var journal = new Journal();
        var collection = new MyObservableCollection<Game>("test", 1);
        var args = new CollectionHandlerEventArgs("collection", new Game());
        
        journal.AddHistoryItem(collection, args);
    }

    [Test]
    public void TestJournalEntryToString()
    {
        var journalEntry = new JournalEntry("1", "2", "3");
        
        Assert.IsTrue(journalEntry.ToString() == $"Название коллекции: 1 \nТип изменения: 2 \nДанные объекта: 3");
    }
    
    
}