using System.Collections;
using UnityEngine;

namespace DesignPatterns
{    
    /*
    Iterator Design Pattern được sử dụng để cung cấp một cách tuần tự hóa quá trình truy cập 
    các phần tử của một tập hợp mà không tiết lộ cấu trúc nội bộ của tập hợp.
    */
   
    public interface IIterator
    {
        bool HasNext();
        object Next();
    }

    // Aggregate Interface
    public interface IAggregate
    {
        IIterator CreateIterator();
    }

    // Concrete Iterator
    public class ConcreteIterator : IIterator
    {
        private ArrayList collection;
        private int currentPosition = 0;

        public ConcreteIterator(ArrayList collection)
        {
            this.collection = collection;
        }

        public bool HasNext()
        {
            return currentPosition < collection.Count;
        }

        public object Next()
        {
            object currentItem = collection[currentPosition];
            currentPosition++;
            return currentItem;
        }
    }

    // Concrete Aggregate
    public class ConcreteAggregate : IAggregate
    {
        private ArrayList collection = new ArrayList();

        public IIterator CreateIterator()
        {
            return new ConcreteIterator(collection);
        }

        public void AddItem(object item)
        {
            collection.Add(item);
        }
    }

    public class ExampleIterator
    {
        public void Example()
        {
            ConcreteAggregate aggregate = new ConcreteAggregate();
            aggregate.AddItem(1);
            aggregate.AddItem(2);
            aggregate.AddItem(3);

            IIterator iterator = aggregate.CreateIterator();

            while (iterator.HasNext())
            {
                Debug.Log(iterator.Next());
            }
        }
    }
}