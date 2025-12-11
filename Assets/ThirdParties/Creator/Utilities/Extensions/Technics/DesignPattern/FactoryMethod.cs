using UnityEngine;

namespace DesignPatterns
{   
    /*
    Factory Method là một mẫu thiết kế (design pattern) trong lập trình hướng đối tượng, 
    nơi bạn tạo ra một đối tượng mà không cần chỉ định chính xác lớp cụ thể của nó. 
    Thay vào đó, một phương thức được sử dụng để tạo ra đối tượng, 
    nhưng lớp con có thể thay đổi loại đối tượng được tạo.
    */

    // Interface hoặc abstract class đại diện cho các đối tượng cần tạo
    public interface IProduct
    {
        void Create();
    }

    // Các lớp cụ thể triển khai IProduct
    public class ConcreteProductA : IProduct
    {
        public void Create()
        {
            Debug.Log("ConcreteProductA created");
        }
    }

    public class ConcreteProductB : IProduct
    {
        public void Create()
        {
            Debug.Log("ConcreteProductB created");
        }
    }

    // Interface hoặc abstract class đại diện cho Factory Method
    public interface ICreator
    {
        IProduct FactoryMethod();
    }

    // Lớp con cụ thể triển khai ICreator
    public class ConcreteCreatorA : ICreator
    {
        public IProduct FactoryMethod()
        {
            return new ConcreteProductA();
        }
    }

    public class ConcreteCreatorB : ICreator
    {
        public IProduct FactoryMethod()
        {
            return new ConcreteProductB();
        }
    }

    public class ExampleFactoryMethod
    {
        public void Example()
        {
            // Sử dụng Factory Method để tạo đối tượng mà không biết lớp cụ thể của nó
            ICreator creatorA = new ConcreteCreatorA();
            IProduct productA = creatorA.FactoryMethod();
            productA.Create();

            ICreator creatorB = new ConcreteCreatorB();
            IProduct productB = creatorB.FactoryMethod();
            productB.Create();
        }
    }
}